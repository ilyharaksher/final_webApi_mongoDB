using MongoDB.Driver;
using metanit.Models;
using metanit.Controllers;

//var client = new MongoClient("mongodb://localhost:27017");  // определяем клиент
//var db = client.GetDatabase("test");    // определяем объект базы данных
//var collectionName = "users";   // имя коллекции
 
var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<PersonDatabaseSettings>(
    builder.Configuration.GetSection("PersonDatabase"));

builder.Services.AddSingleton<PersonService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.MapGet("/api/users", () =>
//    db.GetCollection<Person>(collectionName).Find("{}").ToListAsync());

//app.MapGet("/api/users/{id}", async (string id) =>
//{
//    var user = await db.GetCollection<Person>(collectionName)
//        .Find(p=>p.Id == id)
//        .FirstOrDefaultAsync();

//    if (user == null) return Results.NotFound(new { message = "User is not found" });

//    return Results.Json(user);
//});
//app.MapDelete("/api/users/{id}", async (string id) =>
//{
//    var user = await db.GetCollection<Person>(collectionName).FindOneAndDeleteAsync(p=>p.Id==id);
//    // if not found send status code and error message
//    if (user is null) return Results.NotFound(new { message = "User is not found" });
//    return Results.Json(user);
//});

//app.MapPost("/api/users", async (Person user) => {

//    // adding user to the list
//    await db.GetCollection<Person>(collectionName).InsertOneAsync(user);
//    return user;
//});

//app.MapPut("/api/users", async (Person userData) => {

//    var user = await db.GetCollection<Person>(collectionName)
//        .FindOneAndReplaceAsync(p => p.Id == userData.Id, userData, new() { ReturnDocument = ReturnDocument.After });
//    if (user == null) 
//        return Results.NotFound(new { message = "User is not found" });
//    return Results.Json(user);
//});



app.Run();
 
