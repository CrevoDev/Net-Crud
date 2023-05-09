using UserStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

namespace UserStoreApi.Services;

public class UsersService
{
    private readonly IMongoCollection<User> _usersCollection;

    public UsersService(
        IOptions<UserStoreDatabaseSettings> storeDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            storeDatabaseSettings.Value.ConnectionString);
        
        var mongoDatabase = mongoClient.GetDatabase(
            storeDatabaseSettings.Value.DatabaseName);
        
        _usersCollection = mongoDatabase.GetCollection<User>("Users");
    }

    private ProjectionDefinition<User, UserDTO> simpleProjection = Builders<User>.Projection
            .Exclude("Password");

    public async Task<List<UserDTO>> GetASync() => 
        await _usersCollection
            .Find(_ => true)
            .Project(simpleProjection)
            .ToListAsync();
    
    public async Task<UserDTO?> GetAsync(string id) =>
        await _usersCollection
            .Find(u => u.Id == id)
            .Project(simpleProjection)
            .FirstOrDefaultAsync();

    public async Task<string?> LoginAsync(LoginDTO loginUser)
    {
        User user = await _usersCollection
            .Find(u => u.Email == loginUser.Email)
            .FirstOrDefaultAsync();

        if (user is null) return null;

        var Token = TokenService.GenerateToken(user);

        return Token;
    }

    public async Task CreateAsync(User newUser) {
        newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
        
        await _usersCollection
            .InsertOneAsync(newUser);

        return;
    }

    public async Task UpdateAsync(string id, User updatedUser) =>
        await _usersCollection
            .ReplaceOneAsync(u => u.Id == id, updatedUser);

    public async Task DeleteAsync(string id) =>
        await _usersCollection
            .DeleteOneAsync(u => u.Id == id);
}