using Adocao.Domain;
using Adocao.Infra.Persistence;
using System;

namespace Adocao.Infra;

// [Design Pattern: Simple Factory / Factory Method]
// Define uma interface para criar um objeto, mas deixa as subclasses ou método decidirem
// qual classe instanciar. Aqui decidimos qual Repositório (MySQL ou SQLite) criar
// com base na configuração, sem que a Main saiba os detalhes.
public static class DatabaseFactory
{
    public static IPetRepository CreateRepository(string? dbType, string? connectionString)
    {
        if (string.IsNullOrEmpty(dbType) || string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException("Tipo de banco de dados e string de conexão devem ser fornecidos.");
        }

        return dbType.ToLowerInvariant() switch
        {
            "sqlite" => new SqLitePetRepository(connectionString),
            "mysql" => new MySQLPetRepository(connectionString),
            _ => throw new NotSupportedException($"O tipo de banco de dados '{dbType}' não é suportado.")
        };
    }
}