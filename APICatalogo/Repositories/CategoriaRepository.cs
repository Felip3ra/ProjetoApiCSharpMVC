using APICatalogo.Contexto;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    // Campo somente leitura para o contexto do banco de dados (injeção de dependência do Entity Framework)
    private readonly AppDbContext _context;
    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }
    public Categoria GetCategoria(int id)
    {
        return _context.Categorias.FirstOrDefault(x => x.CategoriaId == id);
    }

    public IEnumerable<Categoria> GetCategorias()
    {
        return _context.Categorias.ToList();
    }


    public Categoria Create(Categoria categoria)
    {
        if (categoria is null)
        {
            throw new ArgumentNullException(nameof(categoria));
        }

        _context.Categorias.Add(categoria);
        _context.SaveChanges();
        return categoria;
    }
    public Categoria Update(Categoria categoria)
    {
        if (categoria is null)
        {
            throw new ArgumentNullException(nameof(categoria));
        }

        _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _context.SaveChanges();
        return categoria;
    }

    public Categoria Delete(int id)
    {
        var Categoria = _context.Categorias.Find(id);

        if (Categoria is null)
        {
            throw new ArgumentNullException(nameof(Categoria));
        }

        _context.Categorias.Remove(Categoria);
        _context.SaveChanges();
        return Categoria;
    }

    

    
}
