using Adocao.Core;
using Adocao.Domain;

namespace Adocao.Application.Features.DeletePet;

public class DeletePetCommandHandler : ICommandHandler<DeletePetCommand, bool>
{
    private readonly IPetRepository _repository;

    public DeletePetCommandHandler(IPetRepository repository)
    {
        _repository = repository;
    }

    public bool Handle(DeletePetCommand command)
    {
        _repository.Delete(command.Id.ToString());
        return true;
    }
}
