using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.DTO;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TodoItemsController : ControllerBase
    {
        private readonly ListaContext _context;

        public TodoItemsController(ListaContext context)
        {
            _context = context;
        }

        
        /// <summary>
        /// Busca todas as tarefas existentes.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTO_ListaItems>>> GetListaItems()
        {
          return await  _context.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();

        }

        
        /// <summary>
        /// Busca uma tarefa selecionada pelo Codigo.
        /// </summary>
        [HttpGet("{codigo}")]
        public async Task<ActionResult<DTO_ListaItems>> GetListaItem(long codigo)
        {
          
            var todoItem = await _context.TodoItems.FindAsync(codigo);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }


        //De acordo com a especificação HTTP,
        //uma solicitação PUT requer que o cliente envie toda a entidade atualizada, 
        //não apenas as alterações.Para oferecer suporte a atualizações parciais, 
        //use HTTP PATCH.

        /// <summary>
        /// Altera a tarefa selecionada pelo Codigo.
        /// </summary>
        [HttpPut("{codigo}")]
        public async Task<IActionResult> UpdateListaItem(long codigo, DTO_ListaItems todoItemDTO)
        {
            if (codigo != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.FindAsync(codigo);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Descricao = todoItemDTO.Descricao;
            todoItem.Status = todoItemDTO.Status;

            if (
                todoItem.Status != 'P' && 
                todoItem.Status != 'C' && 
                todoItem.Status != 'p' && 
                todoItem.Status != 'c'
                )
            {
                return NotFound();
            }
            if ( string.IsNullOrEmpty(todoItem.Descricao))
            {
                return NotFound();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ListaItemExists(codigo))
            {
                return NotFound();
            }

            return NoContent();
        }
        


        /// <summary>
        /// Criação da tarefa para ser feita.
        /// </summary>
        /// <param name="todoItemDTO"></param>
        /// <returns>A nova Tarefa foi criada com sucesso!</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "descricao": "Comprar um mouse",
        ///        "status": P
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Retorna o novo item criado</response>
        /// <response code="400">Se o item estiver nulo</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<DTO_ListaItems>> CreateListaItem(DTO_ListaItems todoItemDTO)
        {
            var todoItem = new ListaItem
            {
                Status = todoItemDTO.Status,
                Descricao = todoItemDTO.Descricao
            };
            if (todoItem.Status != 'P' && todoItem.Status != 'C')
            {
                todoItem.Status = 'P';                
            }
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetListaItem),
                new { codigo = todoItem.Id },
                ItemToDTO(todoItem));
        }


        /// <summary>
        /// Deleta uma tarefa especifica de acordo com o código.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> DeleteListaItem(long codigo)
        {
            var todoItem = await _context.TodoItems.FindAsync(codigo);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ListaItemExists(long codigo)
        {
            return (_context.TodoItems?.Any(e => e.Id == codigo)).GetValueOrDefault();
        }

        private static DTO_ListaItems ItemToDTO(ListaItem listaItem) =>
            new DTO_ListaItems
            {
                Id = listaItem.Id,
                Descricao= listaItem.Descricao,
                Status = listaItem.Status
            };
    }
}
