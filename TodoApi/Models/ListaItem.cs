using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class ListaItem
    {
        public long Id { get; set; }
        [Required]
        public string Descricao { get; set; }
        [DefaultValue("P")]
        public char Status { get; set; }
    }
}