namespace Adocao.Domain;

public interface IPetRepository
{
    void Create(Pet pet);
    Pet? GetById(string id);
    List<Pet> GetAll();
    void Update(Pet pet);
    void Delete(string id);
}