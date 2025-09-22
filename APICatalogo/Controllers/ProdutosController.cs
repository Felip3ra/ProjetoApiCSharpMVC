using APICatalogo.Contexto;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repositories;
using AutoMapper;
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
        private readonly IUnityOfWork _unityOfWork;
        private readonly IMapper _mapper;
        public ProdutosController(IUnityOfWork unityOfWork, IMapper mapper)
        {
            _unityOfWork = unityOfWork;
            _mapper = mapper;
        }

        [HttpGet("produtos/{id}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorCategoria(int id)
        {
            try
            {
                var produtos = _unityOfWork.ProdutoRepository.GetProdutosPorCategoria(id);
                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados");
                }
                //var destino = _mapper.Map<ProdutoDTO>(origem);
                var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
                return Ok(produtosDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
        }
        //O ActionResult permite que você retorne diferentes tipos de resposta HTTP, como 200 OK, 404 NotFound, etc.
        // /api/Produtos
        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            try
            {
                var produtos = _unityOfWork.ProdutoRepository.GetAll();
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
        public ActionResult<ProdutoDTO> Get(int id) {
            try
            {
                var produto = _unityOfWork.ProdutoRepository.Get(x => x.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound("Produto não encontrado");
                }
                var produtoDto = _mapper.Map<ProdutoDTO>(produto);
                return Ok(produtoDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }
        //// /api/Produtos
        [HttpPost]
        public ActionResult<ProdutoDTO> Post(ProdutoDTO produtoDto)
        {
            try
            {
                if (produtoDto is null)
                {
                    return BadRequest();
                }
                var produto = _mapper.Map<Produto>(produtoDto);//mapeia o DTO para a entidade

                var NovoProduto = _unityOfWork.ProdutoRepository.Add(produto);//cria um contexto com o objeto criado
                _unityOfWork.Commit();

                var novoProdutoDto = _mapper.Map<ProdutoDTO>(NovoProduto);//mapeia a entidade para o DTO
                //é usada normalmente em um endpoint POST, e ela está fazendo algo muito legal e RESTful: depois de criar um recurso,
                //ela retorna um HTTP 201 Created com o link para acessar esse novo recurso.
                return new CreatedAtRouteResult("ObterProduto", new { id = NovoProduto.ProdutoId }, NovoProduto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }

        }
        // /api/Produtos/id
        [HttpPut("{id:int}")]
        public ActionResult<ProdutoDTO> Put(int id, ProdutoDTO produtoDto)
        {
            try
            {
                if (id != produtoDto.ProdutoId)
                {
                    return BadRequest();
                }
                var produto = _mapper.Map<Produto>(produtoDto);

                var produtoAtualizado = _unityOfWork.ProdutoRepository.Update(produto);
                _unityOfWork.Commit();

                var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produtoAtualizado);
                return Ok(produtoAtualizadoDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id) {
            try
            {
                var produto = _unityOfWork.ProdutoRepository.Get(x => x.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound("Produto não encontrado");
                }
                var produtoDeletado = _unityOfWork.ProdutoRepository.Delete(produto);
                _unityOfWork.Commit();
                var produtoDeletadoDto = _mapper.Map<ProdutoDTO>(produto);
                return Ok(produtoDeletadoDto);

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }
    }
}
