

using APITarefa.Models;
using Microsoft.EntityFrameworkCore;

namespace APITarefa.Context
{
    public class OrganizadorContext : DbContext
    {
       public OrganizadorContext(DbContextOptions<OrganizadorContext> options) : base(options)
        {
            
        }

        public DbSet<Tarefa> Tarefas { get; set; }
    }
}