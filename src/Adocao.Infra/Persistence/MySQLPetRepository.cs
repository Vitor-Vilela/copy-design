using Adocao.Domain;
using MySql.Data.MySqlClient; // Importar o namespace do MySQL
using System;
using System.Collections.Generic;

namespace Adocao.Infra.Persistence;

public class MySQLPetRepository : IPetRepository
{
    private readonly string _connectionString;

    public MySQLPetRepository(string connectionString)
    {
        _connectionString = connectionString;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS pets (
                id CHAR(36) PRIMARY KEY,
                name VARCHAR(100) NOT NULL,
                species VARCHAR(100) NOT NULL,
                breed VARCHAR(100) NOT NULL,
                age INT NOT NULL,
                description TEXT,
                status VARCHAR(20) NOT NULL
            );
        ";
        cmd.ExecuteNonQuery();
        Console.WriteLine("Banco de dados MySQL e tabela 'pets' verificados/criados.");
    }

    public void Create(Pet pet)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO pets (id, name, species, breed, age, description, status) 
            VALUES (@id, @name, @species, @breed, @age, @description, @status);
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
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM pets WHERE id = @id;";
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
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM pets;";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            pets.Add(MapPet(reader));
        }

        return pets;
    }

    public void Update(Pet pet)
    {
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE pets 
            SET name=@name, species=@species, breed=@breed, age=@age, 
                description=@description, status=@status 
            WHERE id=@id;
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
        using var conn = new MySqlConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM pets WHERE id = @id;";
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();
    }


    private Pet MapPet(MySqlDataReader reader)
    {
        var pet = new Pet();

        pet.GetType().GetProperty("Id")?.SetValue(pet, Guid.Parse(reader["id"].ToString()!));
        pet.GetType().GetProperty("Name")?.SetValue(pet, reader["name"].ToString());
        pet.GetType().GetProperty("Specie")?.SetValue(pet, reader["species"].ToString());
        pet.GetType().GetProperty("Breed")?.SetValue(pet, reader["breed"].ToString());
        pet.GetType().GetProperty("Age")?.SetValue(pet, Convert.ToInt32(reader["age"]));
        pet.GetType().GetProperty("Description")?.SetValue(pet, reader["description"].ToString());
        pet.GetType().GetProperty("Status")?.SetValue(pet, Enum.Parse<EStatus>(reader["status"].ToString()!));

        return pet;
    }
}