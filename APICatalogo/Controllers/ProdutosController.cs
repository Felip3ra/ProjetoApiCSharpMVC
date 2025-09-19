using APICatalogo.Contexto;
using APICatalogo.Models;
using APICatalogo.Repositories;
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
        private readonly IProdutoRepository _produtoRepository;
        private readonly IRepository<Produto> _repository;
        public ProdutosController(IProdutoRepository produtoRepository, IRepository<Produto> repository)
        {
            _repository = repository;
            _produtoRepository = produtoRepository; 
        }
        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPorCategoria(int id)
        {
            try
            {
                var produtos = _produtoRepository.GetProdutosPorCategoria(id).ToList();
                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados");
                }
                return Ok(produtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
        }
        //O ActionResult permite que você retorne diferentes tipos de resposta HTTP, como 200 OK, 404 NotFound, etc.
        // /api/Produtos
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = _repository.GetAll();
                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados");
                }
                return Ok(produtos);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }
        //[HttpGet("{valor:alpha:length(5)}")]
        //public async Task<ActionResult<IEnumerable<Produto>>> Get2(string valor) {
        //    var teste = valor;
        //    return await _context.Produtos.AsNoTracking().ToListAsync();
        //}
        //Busca pelo ID informado
        // /api/Produtos/id
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id) {
            try
            {
                var produto = _repository.Get(x => x.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound("Produto não encontrado");
                }
                return Ok(produto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }
        //// /api/Produtos
        //[HttpPost]
        //public ActionResult Post(Produto produto) {
        //    try
        //    {
        //        if (produto is null)
        //        {
        //            return BadRequest();
        //        }
        //        var NovoProduto = _repository.Create(produto);//cria um contexto com o objeto criado
               

        //        //é usada normalmente em um endpoint POST, e ela está fazendo algo muito legal e RESTful: depois de criar um recurso,
        //        //ela retorna um HTTP 201 Created com o link para acessar esse novo recurso.
        //        return new CreatedAtRouteResult("ObterProduto", new { id = NovoProduto.ProdutoId }, NovoProduto);
        //    }
        //    catch (Exception)
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
        //    }
            
        //}
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
                var produtoAtualizado = _repository.Update(produto);
                return Ok(produtoAtualizado);
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
                var produto = _repository.Get(x => x.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound("Produto não encontrado");
                }
                return Ok(_repository.Delete(produto));

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }
    }
}
