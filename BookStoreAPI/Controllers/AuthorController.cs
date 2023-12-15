using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStoreAPI.Entities;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BookStoreAPI.Controllers; // BookStoreAPI est l'espace de nom racine de mon projet 


// this designe la classe dans laquelle on se trouve


// Ceci est une annotation, elle permet de définir des métadonnées sur une classe
// Dans ce contexte elle permet de définir que la classe BookController est un contrôleur d'API
// On parle aussi de decorator / décorateur
[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{

    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public AuthorController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    // Ceci est une annotation, elle permet de définir des métadonnées sur une méthode
    // ActionResult designe le type de retour de la méthode de controller d'api
    //[Authorize]
    [HttpGet]
    public async Task<ActionResult<List<AuthorDto>>> GetAuthors()
    {
        var authors = await _dbContext.Authors.ToListAsync();

        var authorsDto = new List<AuthorDto>();

        foreach (var author in authors)
        {
            authorsDto.Add(_mapper.Map<AuthorDto>(author));
        }


        return Ok(authorsDto);

    }
    // POST: api/Book
    // BODY: Book (JSON)
    //[Authorize]
    //[AllowAnonymous] // permet de ne pas avoir besoin d'être authentifié pour accéder à la méthode
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Author))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Author>> PostAuthor([FromBody] Author author)
    {
        // we check if the parameter is null
        if (author == null)
        {
            return BadRequest();
        }
        // we check if the book already exists
        Author? addedAuthor = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Firstname == author.Firstname);
        if (addedAuthor != null)
        {
            return BadRequest("Author already exists");
        }
        else
        {
            // we add the book to the database
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();

            // we return the book
            return Created("api/author", author);

        }
    }

    // TODO: Add PUT and DELETE methods
    // PUT: api/Book/5
    // BODY: Book (JSON)
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PutAuthor(int id, [FromBody] Author author)
    {
        if (id != author.Id)
        {
            return BadRequest();
        }
        var authorToUpdate = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id == id);

        if (authorToUpdate == null)
        {
            return NotFound();
        }

        authorToUpdate.Firstname = author.Firstname;
        authorToUpdate.Lastname = author.Lastname;
        // continuez pour les autres propriétés

        _dbContext.Entry(authorToUpdate).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("validationTest")]
    public ActionResult ValidationTest([FromBody] AuthorDto author)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Author>> DeleteAuthor(int id)
    {
        var authorToDelete = await _dbContext.Authors.FindAsync(id);
        // var bookToDelete = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);

        if (authorToDelete == null)
        {
            return NotFound();
        }

        _dbContext.Authors.Remove(authorToDelete);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

}