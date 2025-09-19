using APICatalogo.Contexto;
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
        
        private readonly IRepository<Categoria> _repository;

        // Campo somente leitura para acessar configurações definidas no appsettings.json ou outros provedores
        private readonly IConfiguration _configuration;

        private readonly ILogger _logger;

        // Construtor do controller que recebe e armazena as dependências (injeção de dependência)
        public CategoriasController(ICategoriaRepository repository, IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _repository = repository;                
            _configuration = configuration;    // Armazena a configuração da aplicação
            _logger = logger;
        }


       
        // Define uma rota HTTP GET com o nome "LerArquivosDeConfiguracao"
        [HttpGet("LerArquivosDeConfiguracao")]
        public string GetValores()
        {
            // Lê o valor da chave "chave1" do arquivo de configuração (ex: appsettings.json)
            var valor1 = _configuration["chave1"];

            // Lê o valor da chave "chave2"
            var valor2 = _configuration["chave2"];

            // Lê o valor da chave "chave2" que está dentro da seção "secao1"
            var secao1 = _configuration["secao1:chave2"];

            // Retorna os valores lidos formatados em uma string
            return $"Chave1 = {valor1} \nChave2 = {valor2} \nSecão1 => Chave2 = {secao1}";
        }
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
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _repository.GetAll();
            return Ok(categorias);


        }
        //Busca pelo ID informado
        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id)
        {
            try
            {
                var categoria = _repository.Get(x => x.CategoriaId == id);
                if (categoria is null)
                {
                    return NotFound("Produto não encontrado");
                }
                return Ok(categoria);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }

        //[HttpPost]
        //public ActionResult Post(Categoria categoria)
        //{
        //    try
        //    {
        //        if (categoria is null)
        //        {
        //            return BadRequest();
        //        }
        //        var CategoriaCriada = _repository.Create(categoria);
        //        //é usada normalmente em um endpoint POST, e ela está fazendo algo muito legal e RESTful: depois de criar um recurso,
        //        //ela retorna um HTTP 201 Created com o link para acessar esse novo recurso.
        //        return new CreatedAtRouteResult("ObterCategoria", new { id = CategoriaCriada.CategoriaId }, CategoriaCriada);
        //    }
        //    catch (Exception ex) {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
        //    }
            
        //}

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            try
            {
                if (id != categoria.CategoriaId)
                {
                    return BadRequest();
                }
               _repository.Update(categoria);

                return Ok(categoria);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var categoria = _repository.Get(x => x.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound("Produto não localizado...");
                }
                var CategoriaExcluida = _repository.Delete(categoria);

                return Ok(CategoriaExcluida);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a solicitacao");
            }
            
        }
    }
}
