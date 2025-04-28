using APICatalogo.Contexto;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace APICatalogo.Controllers
{
    //Esse atributo indica que esse método responde a requisições HTTP GET. Ou seja, se alguém fizer um GET para essa rota (ex: https://localhost:5001/api/produtos), esse método será executado.
    [Route("api/[controller]")]// /produtos -> padrao para rotas nao nomeadas
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProdutosController(AppDbContext context)
        {
            _context = context;
        }

        //O ActionResult permite que você retorne diferentes tipos de resposta HTTP, como 200 OK, 404 NotFound, etc.
        // /api/Produtos
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = _context.Produtos.ToList();
                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados");
                }
                return produtos;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }
        [HttpGet("{valor:alpha:length(5)}")]
        public async Task<ActionResult<IEnumerable<Produto>>> Get2(string valor) {
            var teste = valor;
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }
        //Busca pelo ID informado
        // /api/Produtos/id
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id) {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound("Produto não encontrado");
                }
                return produto;
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }
        // /api/Produtos
        [HttpPost]
        public ActionResult Post(Produto produto) {
            try
            {
                if (produto is null)
                {
                    return BadRequest();
                }
                _context.Produtos.Add(produto);//cria um contexto com o objeto criado
                _context.SaveChanges();//Persiste os dados na tabela

                //é usada normalmente em um endpoint POST, e ela está fazendo algo muito legal e RESTful: depois de criar um recurso,
                //ela retorna um HTTP 201 Created com o link para acessar esse novo recurso.
                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }
        // /api/Produtos/id
        [HttpPut("{id:int}")]
        public ActionResult Put(int id,Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest();
                }
                _context.Entry(produto).State = EntityState.Modified;//Diz para o EF Core que o objeto deve ser atualizado no banco
                _context.SaveChanges();

                return Ok(produto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) {
            try
            {
                var produto = _context.Produtos.FirstOrDefault(x => x.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound("Produto não localizado...");
                }
                _context.Produtos.Remove(produto);
                _context.SaveChanges();

                return Ok(produto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }
    }
}
