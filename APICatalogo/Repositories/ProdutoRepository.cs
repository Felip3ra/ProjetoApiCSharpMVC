using APICatalogo.Contexto;
using APICatalogo.Models;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        // Campo somente leitura para o contexto do banco de dados (injeção de dependência do Entity Framework)
        private readonly AppDbContext _context;
        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }
        public Produto GetProduto(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);
            if (produto == null)
            {
                throw new InvalidOperationException("Produto é null");
            }
            return produto;
        }

        public IQueryable<Produto> GetProdutos()
        {
            return _context.Produtos;
        }


        public Produto Create(Produto produto)
        {
            if (produto is null)
            {
                throw new InvalidOperationException("Produto é null");
            }

            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return produto;
        }
        public bool Update(Produto produto)
        {
            if (produto is null)
            {
                throw new InvalidOperationException("Produto é null");
            }
            if (_context.Produtos.Any(x => x.ProdutoId == produto.ProdutoId))
            {
            _context.Produtos.Update(produto);
            _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            var produto = _context.Produtos.Find(id);
            if (produto is null)
            {
                throw new InvalidOperationException("Produto é null");
            }
            if (_context.Produtos.Any(x => x.ProdutoId == produto.ProdutoId))
            {
                _context.Produtos.Remove(produto);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
