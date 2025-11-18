// Adocao.Web/Adapters/PetHttpAdapter.cs
using System.Net;
using System.Text;
using Adocao.Application;
using Adocao.Application.Features.CreatePet;
using Adocao.Application.Features.UpdatePet;
using Adocao.Application.Features.DeletePet;
using Adocao.Domain;
using Adocao.Web.Configuration;

public class PetHttpAdapter
{
    private readonly CommandDispatcher _dispatcher;
    private readonly IPetRepository _repository;

    public PetHttpAdapter(CommandDispatcher dispatcher, IPetRepository repository)
    {
        _dispatcher = dispatcher;
        _repository = repository;
    }

    public void HandleRequest(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        if (!IsAuthorized(request))
        {
            WriteJson(response, new { error = "Unauthorized" }, 401);
            response.Close();
            return;
        }

        if (request.Url.AbsolutePath == "/health" && request.HttpMethod == "GET")
        {
            bool databaseOk = true;
            try
            {
                var _ = _repository != null;
            }
            catch { databaseOk = false; }

            WriteJson(response, new { status = "ok", databaseHealth = databaseOk ? "ok" : "failing" }, 200);
        }
        else if (request.Url.AbsolutePath == "/pets" && request.HttpMethod == "GET")
        {
            var pets = _repository.GetAll();
            WriteJson(response, pets);
        }
        else if (request.Url.AbsolutePath.StartsWith("/pets/") && request.HttpMethod == "GET")
        {
            var idStr = request.Url.AbsolutePath.Replace("/pets/", "");
            if (Guid.TryParse(idStr, out var petId))
            {
                var pet = _repository.GetById(petId.ToString());
                if (pet != null) WriteJson(response, pet);
                else response.StatusCode = 404;
            }
            else response.StatusCode = 400;
        }
        else if (request.Url.AbsolutePath == "/pets" && request.HttpMethod == "POST")
        {
            var command = ReadJson<CreatePetCommand>(request);
            var createdPet = _dispatcher.Dispatch<CreatePetCommand, Pet>(command);
            WriteJson(response, createdPet, 201);
        }
        else if (request.Url.AbsolutePath.StartsWith("/pets/") && request.HttpMethod == "PUT")
        {
            var idStr = request.Url.AbsolutePath.Replace("/pets/", "");
            if (Guid.TryParse(idStr, out var petId))
            {
                var command = ReadJson<UpdatePetCommand>(request);
                command.Id = petId;
                var updatedPet = _dispatcher.Dispatch<UpdatePetCommand, Pet>(command);
                WriteJson(response, updatedPet);
            }
            else response.StatusCode = 400;
        }
        else if (request.Url.AbsolutePath.StartsWith("/pets/") && request.HttpMethod == "DELETE")
        {
            var idStr = request.Url.AbsolutePath.Replace("/pets/", "");
            if (Guid.TryParse(idStr, out var petId))
            {
                var existing = _repository.GetById(petId.ToString());
                if (existing is null)
                {
                    response.StatusCode = 404;
                    response.Close();
                    return;
                }

                var deleted = _dispatcher.Dispatch<DeletePetCommand, bool>(new DeletePetCommand { Id = petId });

                WriteJson(response, new { deleted, id = petId }, deleted ? 200 : 404);
            }
        }
        else response.StatusCode = 404;

        response.Close();
    }

    private bool IsAuthorized(HttpListenerRequest request)
    {
        var authHeader = request.Headers["Authorization"];
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic "))
            return false;

        var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
        string decodedCredentials;
        try
        {
            var credentialBytes = Convert.FromBase64String(encodedCredentials);
            decodedCredentials = Encoding.UTF8.GetString(credentialBytes);
        }
        catch
        {
            return false;
        }

        // Expected format: username:password
        var expectedUsername = "user";
        var expectedPassword = "password123";

        var parts = decodedCredentials.Split(':');
        if (parts.Length != 2)
            return false;

        var username = parts[0];
        var password = parts[1];

        return username == expectedUsername && password == expectedPassword;
    }

    private T ReadJson<T>(HttpListenerRequest request)
        => System.Text.Json.JsonSerializer.Deserialize<T>(new StreamReader(request.InputStream).ReadToEnd(),
            new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

    private void WriteJson(HttpListenerResponse response, object data, int statusCode = 200)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(data);
        var buffer = Encoding.UTF8.GetBytes(json);
        response.StatusCode = statusCode;
        response.ContentType = "application/json";
        response.OutputStream.Write(buffer, 0, buffer.Length);
    }
}