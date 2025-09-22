using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;
namespace APICatalogo.Contexto;

//cria o mapeamento do banco de dados
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<CategoriaDTO>? Categorias { get; set; }
    public DbSet<Produto>? Produtos { get; set; }
}
