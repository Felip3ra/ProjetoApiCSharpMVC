using Microsoft.EntityFrameworkCore;
using APICatalogo.Contexto;
using System.Text.Json.Serialization;
using APICatalogo.Services;
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

builder.Services.AddTransient<IMeuServico, MeuServico>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
