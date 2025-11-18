using Adocao.Core;

namespace Adocao.Domain;

public class Pet : Entity
{
    public string Name { get; set; } = string.Empty;
    public string Specie { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Description { get; set; } = string.Empty;
    public EStatus Status { get; set; } = EStatus.Available;
}