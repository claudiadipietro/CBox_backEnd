using CountryItem.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Country.Services;

public class MongoDBService {

    private readonly IMongoCollection<Countries> _countriesCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _countriesCollection = database.GetCollection<Countries>(mongoDBSettings.Value.CollectionName);
    }

    public async Task<List<Countries>> GetAsync() { 
        return await _countriesCollection.Find(new BsonDocument()).ToListAsync();
    }
    public async Task CreateAsync(Countries country) { 
        await _countriesCollection.InsertOneAsync(country);
        return;
    }
    public async Task AddToCountriesAsync(string id, string countryId) {
        FilterDefinition<Countries> filter = Builders<Countries>.Filter.Eq("Id", id);
        UpdateDefinition<Countries> update = Builders<Countries>.Update.AddToSet<string>("countriesIds", countryId);
        await _countriesCollection.UpdateOneAsync(filter, update);
        return;
    }
    public async Task DeleteAsync(string id) { 
        FilterDefinition<Countries> filter = Builders<Countries>.Filter.Eq("Id", id);
        await _countriesCollection.DeleteOneAsync(filter);
        return;
    }

}