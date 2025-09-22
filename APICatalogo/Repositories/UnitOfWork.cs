using APICatalogo.Contexto;

namespace APICatalogo.Repositories
{
    public class UnitOfWork : IUnityOfWork
    {
        private IProdutoRepository? _ProdutoRepo;

        private ICategoriaRepository? _CategoriaRepo;

        public AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _ProdutoRepo = _ProdutoRepo ?? new ProdutoRepository(_context);
            }
        }
        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _CategoriaRepo = _CategoriaRepo ?? new CategoriaRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
