using Adocao.Core;
using Adocao.Domain;

namespace Adocao.Application.Features.UpdatePet;

public class UpdatePetCommand : ICommand<Pet>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Specie { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Description { get; set; } = string.Empty;
}