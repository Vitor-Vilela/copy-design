using Adocao.Application.Features.CreatePet;
using Adocao.Core;
using Adocao.Domain;

namespace Adocao.Application.Features.UpdatePet;

public class UpdatePetCommandHandler : ICommandHandler<UpdatePetCommand, Pet>
{
    private readonly IPetRepository _repository;

    public UpdatePetCommandHandler(IPetRepository repository)
    {
        _repository = repository;
    }

    public Pet Handle(UpdatePetCommand command)
    {
        var pet = new Pet
        {
            Id = command.Id,
            Name = command.Name,
            Specie = command.Specie,
            Breed = command.Breed,
            Age = command.Age,
            Description = command.Description
        };

        _repository.Update(pet);
        return pet;
    }
}