using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalAPIs.Data;
using MinimalAPIs.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connDb = builder.Configuration["ConnectionStrings:BookDatabase"];
builder.Services.AddDbContext<BookDbContext>(opt => opt.UseMySql(connDb, ServerVersion.AutoDetect(connDb)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/book", (BookDbContext context) =>
{
    return context.Books;
});

app.MapGet("/book/{id}", async (BookDbContext context, int id) =>
{
    var findBook = await context.Books.FindAsync(id);

    if (findBook is null)
        return Results.NotFound("Sorry, this book doesn´t exist.");

    return Results.Ok(findBook);
});

app.MapPost("/createBook", async (BookDbContext context, [FromBody] Book book) =>
{
    context.Books.Add(book);
    await context.SaveChangesAsync();

    return Results.Ok(book);
});

app.MapPut("/updateBook/{id}", async (BookDbContext context, int id, [FromBody] Book book) =>
{
    var oldBook = await context.Books.FindAsync(id);

    if (oldBook is null)
        return Results.NotFound("Sorry, this book doesn´t exist.");

    oldBook.Author = book.Author;
    oldBook.Title = book.Title;
    oldBook.Description = book.Description;

    await context.SaveChangesAsync();

    return Results.Ok(oldBook);
});

app.MapDelete("/book/remove/{id}", async (BookDbContext context, int id) =>
{
    var book = await context.Books.FindAsync(id);

    if (book is null)
        return Results.NotFound("Sorry, this book doesn´t exist.");

    context.Books.Remove(book);
    await context.SaveChangesAsync();

    return Results.Ok(context.Books);
});

app.Run();