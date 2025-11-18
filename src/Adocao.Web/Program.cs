using System.Net;
using Adocao.Application.Features.CreatePet;
using Adocao.Application.Features.UpdatePet;
using Adocao.Application.Features.DeletePet;
using Adocao.Domain;
using Adocao.Infra;
using Adocao.Web.Configuration;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

Console.Write("Escolha o tipo de banco (sqlite/mysql): ");
var inputDb = Console.ReadLine()?.Trim().ToLower();

var dbType = string.IsNullOrEmpty(inputDb)
    ? (config["Database:Type"] ?? "sqlite")
    : inputDb;

var connectionString = config["Database:ConnectionString"];

if (string.IsNullOrEmpty(connectionString))
{
    if (dbType.Equals("sqlite", StringComparison.OrdinalIgnoreCase))
    {
        connectionString = "Data Source=adoption.db";
    }
    else if (dbType.Equals("mysql", StringComparison.OrdinalIgnoreCase))
    {
        connectionString = "Server=localhost;Port=3307;Database=adocao;User Id=root;Password=root;SslMode=None;AllowPublicKeyRetrieval=True;TreatTinyAsBoolean=false;";
    }
}

Console.WriteLine($"[INFO] Usando banco de dados: {dbType}");
Console.WriteLine($"[INFO] {connectionString}");

IPetRepository repo = DatabaseFactory.CreateRepository(dbType, connectionString);

var dispatcher = new CommandDispatcher();
dispatcher.Register(new CreatePetCommandHandler(repo));
dispatcher.Register(new UpdatePetCommandHandler(repo));
dispatcher.Register(new DeletePetCommandHandler(repo));

var adapter = new PetHttpAdapter(dispatcher, repo);

var listener = new HttpListener();
listener.Prefixes.Add("http://localhost:8080/");
listener.Start();

Console.WriteLine("[INFO] Servidor escutando em http://localhost:8080/");
Console.WriteLine("[INFO] Pressione CTRL+C para parar o servidor.");


while (true)
{
    var context = listener.GetContext();
    adapter.HandleRequest(context);
}