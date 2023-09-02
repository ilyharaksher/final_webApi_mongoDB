using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
 
var client = new MongoClient("mongodb://localhost:27017");  // определяем клиент
var db = client.GetDatabase("test");    // определяем объект базы данных
var collectionName = "users";   // имя коллекции
 
var builder = WebApplication.CreateBuilder(args);
 
var app = builder.Build();
 
app.UseDefaultFiles();
app.UseStaticFiles();
 
app.MapGet("/api/users", () =>
    db.GetCollection<Person>(collectionName).Find("{}").ToListAsync());
 
app.MapGet("/api/users/{id}", async (string id) =>
{
    var user = await db.GetCollection<Person>(collectionName)
        .Find(p=>p.Id == id)
        .FirstOrDefaultAsync();
 
    // если не найден, отправляем статусный код и сообщение об ошибке
    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });
 
    // если пользователь найден, отправляем его
    return Results.Json(user);
});
app.MapDelete("/api/users/{id}", async (string id) =>
{
    var user = await db.GetCollection<Person>(collectionName).FindOneAndDeleteAsync(p=>p.Id==id);
    // если не найден, отправляем статусный код и сообщение об ошибке
    if (user is null) return Results.NotFound(new { message = "Пользователь не найден" });
    return Results.Json(user);
});
 
app.MapPost("/api/users", async (Person user) => {
 
    // добавляем пользователя в список
    await db.GetCollection<Person>(collectionName).InsertOneAsync(user);
    return user;
});
 
app.MapPut("/api/users", async (Person userData) => {
 
    var user = await db.GetCollection<Person>(collectionName)
        .FindOneAndReplaceAsync(p => p.Id == userData.Id, userData, new() { ReturnDocument = ReturnDocument.After });
    if (user == null) 
        return Results.NotFound(new { message = "Пользователь не найден" });
    return Results.Json(user);
});
 
app.Run();
 
public class Person
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
}