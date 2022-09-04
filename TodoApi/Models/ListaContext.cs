using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace TodoApi.Models
{
#pragma warning disable CS1591
    public class ListaContext : DbContext
    {
        public ListaContext(DbContextOptions<ListaContext> options)
            : base(options)
        {
        }

        public DbSet<ListaItem> TodoItems { get; set; } = null!;
    }
}
#pragma warning restore CS1591