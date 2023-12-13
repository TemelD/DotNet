using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStoreAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BookStoreAPI.Controllers; // BookStoreAPI est l'espace de nom racine de mon projet 


// this designe la classe dans laquelle on se trouve


// Ceci est une annotation, elle permet de définir des métadonnées sur une classe
// Dans ce contexte elle permet de définir que la classe BookController est un contrôleur d'API
// On parle aussi de decorator / décorateur
[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{

    private readonly ApplicationDbContext _dbContext;

    public BookController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Ceci est une annotation, elle permet de définir des métadonnées sur une méthode
    // ActionResult designe le type de retour de la méthode de controller d'api
    [HttpGet]
    public async Task<ActionResult<List<Book>>> GetBooks()
    {
        var books = await _dbContext.Books.ToListAsync();
        return Ok(books);
    }
    // POST: api/Book
    // BODY: Book (JSON)
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Book))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Book>> PostBook([FromBody] Book book)
    {
        // we check if the parameter is null
        if (book == null)
        {
            return BadRequest();
        }
        // we check if the book already exists
        Book? addedBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Title == book.Title);
        if (addedBook != null)
        {
            return BadRequest("Book already exists");
        }
        else
        {
            // we add the book to the database
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
 
            // we return the book
            return Created("api/book", book);
 
        }
    }

[HttpPut("{id}")]
[ProducesResponseType(200, Type = typeof(Book))]
[ProducesResponseType(400)]
[ProducesResponseType(404)]
public async Task<ActionResult<Book>> PutBook(int id, [FromBody] Book updatedBook)
{
    if (updatedBook == null || id != updatedBook.Id)
    {
        return BadRequest();
    }

    var existingBook = await _dbContext.Books.FindAsync(id);

    if (existingBook == null)
    {
        return NotFound();
    }

    // Utilisez l'initialiseur d'objet pour mettre à jour les propriétés nécessaires
    existingBook = new Book
    {
        Id = existingBook.Id,
        Title = updatedBook.Title, // Mise à jour de la propriété en lecture seule
        Author = updatedBook.Author
    };

    try
    {
        await _dbContext.SaveChangesAsync();
        return Ok(existingBook);
    }
    catch (DbUpdateException ex)
    {
        // Log the exception
        return BadRequest("Unable to save changes to the database. Please try again.");
    }
}

[HttpDelete("{id}")]
[ProducesResponseType(200, Type = typeof(Book))]
[ProducesResponseType(404)]
public async Task<ActionResult<Book>> DeleteBook(int id)
{
    var bookToDelete = await _dbContext.Books.FindAsync(id);

    if (bookToDelete == null)
    {
        return NotFound();
    }

    _dbContext.Books.Remove(bookToDelete);

    try
    {
        await _dbContext.SaveChangesAsync();
        return Ok(bookToDelete);
    }
    catch (DbUpdateException ex)
    {
        // Log the exception
        return BadRequest("Unable to save changes to the database. Please try again.");
    }
}
}