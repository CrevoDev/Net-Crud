using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserStoreApi.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("Id")]
    public string? Id { get; set; }

    [BsonElement("Name")]
    [JsonPropertyName("Nome")]
    public string UserName { get; set; } = null!;

    [BsonElement("Email")]
    [JsonPropertyName("Email")]
    public string Email { get; set; } = null!;

    [BsonElement("Password")]
    [JsonPropertyName("Senha")]
    public string Password { get; set; } = null!;
}

public class UserDTO
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("Id")]
    public string? Id { get; set; }

    [BsonElement("Name")]
    [JsonPropertyName("Nome")]
    public string UserName { get; set; } = null!;

    [BsonElement("Email")]
    [JsonPropertyName("Email")]
    public string Email { get; set; } = null!;
}

public class LoginDTO
{
    [BsonElement("Email")]
    [JsonPropertyName("Email")]
    public string Email { get; set; } = null!;

    [BsonElement("Password")]
    [JsonPropertyName("Senha")]
    public string Password { get; set; } = null!;
}