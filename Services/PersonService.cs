using metanit.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace metanit.Services
{
    public class PersonService
    {
        private readonly IMongoCollection<Person> _personCollection;

        public PersonService(
            IOptions<PersonDatabaseSettings> personDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                personDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                personDatabaseSettings.Value.DatabaseName);

            _personCollection = mongoDatabase.GetCollection<Person>(
                personDatabaseSettings.Value.GetCollection);
        }

        public async Task<List<Person>> Find() =>
            await _personCollection.Find(_ => true).ToListAsync();

        public async Task<Person?> Find(string id) =>
            await _personCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task InsertOneAsync(Person newPerson) =>
            await _personCollection.InsertOneAsync(newPerson);

        public async Task ReplaceOneAsync(string id, Person updatedPerson) =>
            await _personCollection.ReplaceOneAsync(x => x.Id == id, updatedPerson);

        public async Task DeleteOneAsync(string id) =>
            await _personCollection.DeleteOneAsync(x => x.Id == id);
    }
}
