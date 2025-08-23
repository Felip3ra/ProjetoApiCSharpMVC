using Microsoft.EntityFrameworkCore;
using APICatalogo.Contexto;
using System.Text.Json.Serialization;
using APICatalogo.Services;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Logging;
using APICatalogo.Repositories;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//serve pra resolver um problema comum com serialização JSON em APIs ASP.NET Core que usam relacionamentos entre entidades
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Pega a string de conexao
string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

// Lê o valor da chave "chave1" diretamente do arquivo de configuração (por exemplo, appsettings.json)
var valor1 = builder.Configuration["chave1"];

// Lê o valor da chave "chave2" que está dentro da seção "secao1" no arquivo de configuração
var valor2 = builder.Configuration["secao1:chave2"];


//configura a conexao com o provedor do banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(mySqlConnection);
});

builder.Services.AddScoped<ApiLoggingFilter>();

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();

builder.Services.AddTransient<IMeuServico, MeuServico>();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information,
}));

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();//Middleware swagger
    app.UseSwaggerUI();//Middleware swagger UI
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Adiciona um middleware customizado na pipeline de requisições
app.Use(async (context, next) =>
{
    // Código aqui é executado **antes** de passar para o próximo middleware (por exemplo, log de request)

    await next(context); // Chama o próximo middleware na pipeline

    // Código aqui é executado **depois** que o próximo middleware terminou (por exemplo, log de resposta)
});

// Mapeia os controllers da aplicação (faz com que rotas como [HttpGet] funcionem)
app.MapControllers();

// Middleware final — será chamado **somente se nenhuma rota anterior for correspondida**
//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Middleware final");
//});

// Inicia a aplicação ASP.NET (não se deve chamar `app.Run()` duas vezes)
app.Run();

