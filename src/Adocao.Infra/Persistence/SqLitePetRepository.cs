
using Adocao.Domain;
using Microsoft.Data.Sqlite;

namespace Adocao.Infra.Persistence;

public class SqLitePetRepository : IPetRepository
{
    private readonly string _connectionString;

    public SqLitePetRepository(string connectionString)
    {
        _connectionString = connectionString;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS pets (
                id TEXT PRIMARY KEY,
                name TEXT NOT NULL,
                species TEXT NOT NULL,
                breed TEXT NOT NULL,
                age INTEGER NOT NULL,
                description TEXT,
                status TEXT NOT NULL
            );
        ";
        cmd.ExecuteNonQuery();
        Console.WriteLine("Banco criado");
    }

    public void Create(Pet pet)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO pets (id, name, species, breed, age, description, status) 
            VALUES (@id, @name, @species, @breed, @age, @description, @status)
        ";
        cmd.Parameters.AddWithValue("@id", pet.Id.ToString());
        cmd.Parameters.AddWithValue("@name", pet.Name);
        cmd.Parameters.AddWithValue("@species", pet.Specie);
        cmd.Parameters.AddWithValue("@breed", pet.Breed);
        cmd.Parameters.AddWithValue("@age", pet.Age);
        cmd.Parameters.AddWithValue("@description", pet.Description);
        cmd.Parameters.AddWithValue("@status", pet.Status.ToString());
        cmd.ExecuteNonQuery();
    }

    public Pet? GetById(string id)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM pets WHERE id = @id";
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return MapPet(reader);
        }

        return null;
    }

    public List<Pet> GetAll()
    {
        var pets = new List<Pet>();

        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM pets";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            pets.Add(MapPet(reader));
        }

        return pets;
    }

    public void Update(Pet pet)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE pets 
            SET name=@name, species=@species, breed=@breed, age=@age, 
                description=@description, status=@status 
            WHERE id=@id
        ";
        cmd.Parameters.AddWithValue("@id", pet.Id.ToString());
        cmd.Parameters.AddWithValue("@name", pet.Name);
        cmd.Parameters.AddWithValue("@species", pet.Specie);
        cmd.Parameters.AddWithValue("@breed", pet.Breed);
        cmd.Parameters.AddWithValue("@age", pet.Age);
        cmd.Parameters.AddWithValue("@description", pet.Description);
        cmd.Parameters.AddWithValue("@status", pet.Status.ToString());
        cmd.ExecuteNonQuery();
    }

    public void Delete(string id)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM pets WHERE id = @id";
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();
    }


    private Pet MapPet(SqliteDataReader reader)
    {
        return new Pet
        {
            Id = Guid.Parse(reader["id"].ToString()!),
            Name = reader["name"].ToString()!,
            Specie = reader["species"].ToString()!,
            Breed = reader["breed"].ToString()!,
            Age = Convert.ToInt32(reader["age"]),
            Description = reader["description"].ToString() ?? "",
            Status = Enum.Parse<EStatus>(reader["status"].ToString()!)
        };
    }
}