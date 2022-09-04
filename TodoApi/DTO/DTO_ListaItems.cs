using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTO
{
    public class DTO_ListaItems
    {
        public long Id { get; set; }
        [Required]
        public string Descricao { get; set; }
        [DefaultValue("P")]
        public char Status { get; set; }
    }
}