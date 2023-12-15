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
public class EditorController : ControllerBase
{

    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public EditorController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    // Ceci est une annotation, elle permet de définir des métadonnées sur une méthode
    // ActionResult designe le type de retour de la méthode de controller d'api
    //[Authorize]
    [HttpGet]
    public async Task<ActionResult<List<EditorDto>>> GetEditors()
    {
        var editors = await _dbContext.Editors.ToListAsync();

        var editorsDto = new List<EditorDto>();

        foreach (var editor in editors)
        {
            editorsDto.Add(_mapper.Map<EditorDto>(editor));
        }


        return Ok(editorsDto);

    }
    // POST: api/Book
    // BODY: Book (JSON)
    //[Authorize]
    //[AllowAnonymous] // permet de ne pas avoir besoin d'être authentifié pour accéder à la méthode
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Editor))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Editor>> PostAuthor([FromBody] Editor editor)
    {
        // we check if the parameter is null
        if (editor == null)
        {
            return BadRequest();
        }
        // we check if the book already exists
        Editor? addedEditor = await _dbContext.Editors.FirstOrDefaultAsync(e => e.Firstname == editor.Firstname);
        if (addedEditor != null)
        {
            return BadRequest("Editor already exists");
        }
        else
        {
            // we add the book to the database
            await _dbContext.Editors.AddAsync(editor);
            await _dbContext.SaveChangesAsync();

            // we return the book
            return Created("api/editor", editor);

        }
    }

    // TODO: Add PUT and DELETE methods
    // PUT: api/Book/5
    // BODY: Book (JSON)
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PutEditor(int id, [FromBody] Editor editor)
    {
        if (id != editor.Id)
        {
            return BadRequest();
        }
        var editorToUpdate = await _dbContext.Editors.FirstOrDefaultAsync(e => e.Id == id);

        if (editorToUpdate == null)
        {
            return NotFound();
        }

        editorToUpdate.Firstname = editor.Firstname;
        editorToUpdate.Lastname = editor.Lastname;
        // continuez pour les autres propriétés

        _dbContext.Entry(editorToUpdate).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("validationTest")]
    public ActionResult ValidationTest([FromBody] EditorDto editor)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Editor>> DeleteEditor(int id)
    {
        var editorToDelete = await _dbContext.Editors.FindAsync(id);
        // var bookToDelete = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);

        if (editorToDelete == null)
        {
            return NotFound();
        }

        _dbContext.Editors.Remove(editorToDelete);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

}