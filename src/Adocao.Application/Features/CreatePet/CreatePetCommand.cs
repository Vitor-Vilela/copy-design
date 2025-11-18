using System.Text.Json.Serialization;
using Adocao.Core;
using Adocao.Domain;

public class CreatePetCommand : ICommand<Pet>
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("specie")]
    public string Specie { get; set; } = string.Empty;

    [JsonPropertyName("breed")]
    public string Breed { get; set; } = string.Empty;

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}