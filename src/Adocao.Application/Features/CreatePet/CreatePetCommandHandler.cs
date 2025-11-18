using Adocao.Core;
using Adocao.Domain;

namespace Adocao.Application.Features.CreatePet;

public class CreatePetCommandHandler : ICommandHandler<CreatePetCommand, Pet>
{
    private readonly IPetRepository _repository;

    public CreatePetCommandHandler(IPetRepository repository)
    {
        _repository = repository;
    }

    public Pet Handle(CreatePetCommand command)
    {
        var pet = new Pet
        {
            Name = command.Name,
            Specie = command.Specie,
            Breed = command.Breed,
            Age = command.Age,
            Description = command.Description
        };

        _repository.Create(pet);
        return pet;
    }
}