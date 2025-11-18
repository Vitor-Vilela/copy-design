using Adocao.Core;

namespace Adocao.Application.Features.DeletePet;

public class DeletePetCommand : ICommand<bool>
{
    public Guid Id { get; set; }
}
