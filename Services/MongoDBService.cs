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
        var filter = Builders<Countries>.Filter.Eq(country => country.available, true);
        return await _countriesCollection.Find(filter).ToListAsync();
    }

    public async Task<List<Countries>> GetCountriesSelected() { 
        return await _countriesCollection.Find(country => country.available == true).ToListAsync();
    }

    public async Task ModifyRandom() { 
        await setAllFalse();
        Random random = new Random();
        var countriesCollection = _countriesCollection.Find(new BsonDocument());
        var size = unchecked((int)countriesCollection.CountDocuments());
        var country = await countriesCollection.ToListAsync();
        for (int i = 1; i < 11; i++) {
            var randomIndex = random.Next(0, size); 
            var countryToUpdate = country[randomIndex];
            FilterDefinition<Countries> filter = Builders<Countries>.Filter.Eq(country => country.Id, countryToUpdate.Id);
            UpdateDefinition<Countries> update = Builders<Countries>.Update.Set(country => country.available, countryToUpdate.available = true);
            await _countriesCollection.UpdateOneAsync(filter, update);    
        }
        return;
    }

    public async Task setAllFalse() { 
        var countriesCollection = _countriesCollection.Find(new BsonDocument());
        var size = unchecked((int)countriesCollection.CountDocuments());
        var country = await countriesCollection.ToListAsync();
        for (int i = 1; i < size; i++) {
            var countryToUpdate = country[i];
        FilterDefinition<Countries> filter = Builders<Countries>.Filter.Eq(country => country.Id, countryToUpdate.Id);
        UpdateDefinition<Countries> update = Builders<Countries>.Update.Set(country => country.available, countryToUpdate.available = false);
        await _countriesCollection.UpdateOneAsync(filter, update);
        }
        return;
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