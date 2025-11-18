using Adocao.Domain;
using Adocao.Infra.Persistence;
using System;

namespace Adocao.Infra;

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