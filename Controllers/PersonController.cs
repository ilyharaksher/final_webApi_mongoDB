
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using metanit.Models;

namespace metanit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class PersonController : ControllerBase
    {

        private readonly PersonService _personService;

        public PersonController(PersonService personService) =>
            _personService = personService;

        [HttpGet]
        public async Task<List<Person>> Get() =>
            await _personService.Find();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Person>> Get(string id)
        {
            var person = await _personService.Find(id);

            if (person is null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Person newPerson)
        {
            await _personService.InsertOneAsync(newPerson);

            return CreatedAtAction(nameof(Get), new { id = newPerson.Id }, newPerson);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Person updatedPerson)
        {
            var person = await _personService.Find(id);

            if (person is null)
            {
                return NotFound();
            }

            updatedPerson.Id = person.Id;

            await _personService.ReplaceOneAsync(id, updatedPerson);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var person = await _personService.Find(id);

            if (person is null)
            {
                return NotFound();
            }

            await _personService.DeleteOneAsync(id);

            return NoContent();
        }


    }
    public class PersonDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string GetCollection { get; set; } = null!;
    }
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
