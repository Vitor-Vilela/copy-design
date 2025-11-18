namespace Adocao.Core;

public abstract class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();
}