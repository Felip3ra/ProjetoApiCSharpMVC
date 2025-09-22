using APICatalogo.Contexto;
using APICatalogo.Models;

namespace APICatalogo.Repositories;

public class CategoriaRepository : Repository<CategoriaDTO> , ICategoriaRepository
{
    
    public CategoriaRepository(AppDbContext context) : base(context)
    {
        
    }
    

    

    
}
