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
public class GenderController : ControllerBase
{

    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public GenderController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    // Ceci est une annotation, elle permet de définir des métadonnées sur une méthode
    // ActionResult designe le type de retour de la méthode de controller d'api
    //[Authorize]
    [HttpGet]
    public async Task<ActionResult<List<GenderDto>>> GetGenders()
    {
        var genders = await _dbContext.Genders.ToListAsync();

        var gendersDto = new List<GenderDto>();

        foreach (var gender in genders)
        {
            gendersDto.Add(_mapper.Map<GenderDto>(gender));
        }


        return Ok(gendersDto);

    }
    // POST: api/Book
    // BODY: Book (JSON)
    //[Authorize]
    //[AllowAnonymous] // permet de ne pas avoir besoin d'être authentifié pour accéder à la méthode
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Gender))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Gender>> PostAuthor([FromBody] Gender gender)
    {
        // we check if the parameter is null
        if (gender == null)
        {
            return BadRequest();
        }
        // we check if the book already exists
        Gender? addedGender = await _dbContext.Genders.FirstOrDefaultAsync(g => g.Label == gender.Label);
        if (addedGender != null)
        {
            return BadRequest("Gender already exists");
        }
        else
        {
            // we add the book to the database
            await _dbContext.Genders.AddAsync(gender);
            await _dbContext.SaveChangesAsync();

            // we return the book
            return Created("api/gender", gender);

        }
    }

    // TODO: Add PUT and DELETE methods
    // PUT: api/Book/5
    // BODY: Book (JSON)
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PutGender(int id, [FromBody] Gender gender)
    {
        if (id != gender.Id)
        {
            return BadRequest();
        }
        var genderToUpdate = await _dbContext.Genders.FirstOrDefaultAsync(g => g.Id == id);

        if (genderToUpdate == null)
        {
            return NotFound();
        }

        genderToUpdate.Label = gender.Label;

        // continuez pour les autres propriétés

        _dbContext.Entry(genderToUpdate).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("validationTest")]
    public ActionResult ValidationTest([FromBody] GenderDto gender)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Gender>> DeleteGender(int id)
    {
        var genderToDelete = await _dbContext.Genders.FindAsync(id);
        // var bookToDelete = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);

        if (genderToDelete == null)
        {
            return NotFound();
        }

        _dbContext.Genders.Remove(genderToDelete);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

}