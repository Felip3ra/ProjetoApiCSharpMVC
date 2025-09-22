using APICatalogo.Contexto;
using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using APICatalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace APICatalogo.Controllers
{
    //Esse atributo indica que esse método responde a requisições HTTP GET. Ou seja, se alguém fizer um GET para essa rota (ex: https://localhost:5001/api/produtos), esse método será executado.
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        
        private readonly IUnityOfWork _unityOfWork;

        private readonly ILogger _logger;

        // Construtor do controller que recebe e armazena as dependências (injeção de dependência)
        public CategoriasController(IConfiguration configuration, ILogger<CategoriasController> logger, IUnityOfWork unityOfWork)
        {
            
            _logger = logger;
            _unityOfWork = unityOfWork;

        }



        //// Define uma rota HTTP GET com o nome "LerArquivosDeConfiguracao"
        //[HttpGet("LerArquivosDeConfiguracao")]
        //public string GetValores()
        //{
        //    // Lê o valor da chave "chave1" do arquivo de configuração (ex: appsettings.json)
        //    var valor1 = _configuration["chave1"];

        //    // Lê o valor da chave "chave2"
        //    var valor2 = _configuration["chave2"];

        //    // Lê o valor da chave "chave2" que está dentro da seção "secao1"
        //    var secao1 = _configuration["secao1:chave2"];

        //    // Retorna os valores lidos formatados em uma string
        //    return $"Chave1 = {valor1} \nChave2 = {valor2} \nSecão1 => Chave2 = {secao1}";
        //}
        [HttpGet("UsandoFromServices/{nome}")]
        public ActionResult<string> GetSaudacaoFromServices([FromServices] IMeuServico meuservico, string nome)
        {
            return meuservico.Saudacao(nome);
        }

        //[HttpGet("produtos")]
        //public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        //{
        //    try
        //    {
        //        //return _context.Categorias.Include(x => x.Produtos).ToList();
        //        _logger.LogInformation("==============GET API/CATEGORIAS/PRODUTOS ===================");
        //        return _context.Categorias.Include(x => x.Produtos).Where(x => x.CategoriaId <= 5).ToList();
        //    }
        //    catch (Exception)
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
        //    }
            
        //}
        //O ActionResult permite que você retorne diferentes tipos de resposta HTTP, como 200 OK, 404 NotFound, etc.
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<CategoriaDTO>> Get()
        {
            var categorias = _unityOfWork.CategoriaRepository.GetAll();

            if (categorias is null)
            {
                return NotFound("Categorias não encontradas");
            }
            var categoriasDTO = new List<CategoriaDTO>();
            foreach (var item in categorias)
            {
                categoriasDTO.Add(new CategoriaDTO
                {
                    CategoriaId = item.CategoriaId,
                    Nome = item.Nome,
                    ImagemUrl = item.ImagemUrl
                });
            }
            return Ok(categoriasDTO);


        }
        //Busca pelo ID informado
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            try
            {
                var categoria = _unityOfWork.CategoriaRepository.Get(x => x.CategoriaId == id);
                if (categoria is null)
                {
                    return NotFound("Produto não encontrado");
                }
                var categoriaDTO = new CategoriaDTO
                {
                    CategoriaId = categoria.CategoriaId,
                    Nome = categoria.Nome,
                    ImagemUrl = categoria.ImagemUrl
                };
                return Ok(categoriaDTO);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            try
            {
                if (categoriaDto is null)
                {
                    return BadRequest();
                }

                var categoria = new Categoria()
                {
                    CategoriaId = categoriaDto.CategoriaId,
                    Nome = categoriaDto.Nome,
                    ImagemUrl = categoriaDto.ImagemUrl
                };
                var CategoriaCriada = _unityOfWork.CategoriaRepository.Add(categoria);

                var NovacategoriaDTO = new CategoriaDTO
                {
                    CategoriaId = categoria.CategoriaId,
                    Nome = categoria.Nome,
                    ImagemUrl = categoria.ImagemUrl
                };
                
                _unityOfWork.Commit();
                //é usada normalmente em um endpoint POST, e ela está fazendo algo muito legal e RESTful: depois de criar um recurso,
                //ela retorna um HTTP 201 Created com o link para acessar esse novo recurso.
                return new CreatedAtRouteResult("ObterCategoria", new { id = NovacategoriaDTO.CategoriaId }, NovacategoriaDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }

        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    return BadRequest();
                }

                var categoria = new Categoria()
                {
                    CategoriaId = categoriaDto.CategoriaId,
                    Nome = categoriaDto.Nome,
                    ImagemUrl = categoriaDto.ImagemUrl
                };

                _unityOfWork.CategoriaRepository.Update(categoria);
                _unityOfWork.Commit();

                var NovacategoriaDTO = new CategoriaDTO
                {
                    CategoriaId = categoria.CategoriaId,
                    Nome = categoria.Nome,
                    ImagemUrl = categoria.ImagemUrl
                };
                return Ok(NovacategoriaDTO);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            try
            {
                var categoria = _unityOfWork.CategoriaRepository.Get(x => x.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound("Produto não localizado...");
                }
                var CategoriaExcluida = _unityOfWork.CategoriaRepository.Delete(categoria);
                _unityOfWork.Commit();
                return Ok(CategoriaExcluida);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }
    }
}
