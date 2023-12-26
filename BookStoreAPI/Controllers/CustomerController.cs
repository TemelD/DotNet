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
public class CustomerController : ControllerBase
{

    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CustomerController(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }


    // Ceci est une annotation, elle permet de définir des métadonnées sur une méthode
    // ActionResult designe le type de retour de la méthode de controller d'api
    //[Authorize]
    [HttpGet]
    public async Task<ActionResult<List<CustomerDto>>> GetCustomers()
    {
        var customers = await _dbContext.Customers.ToListAsync();

        var customersDto = new List<CustomerDto>();

        foreach (var customer in customers)
        {
            customersDto.Add(_mapper.Map<CustomerDto>(customer));
        }


        return Ok(customersDto);

    }
    // POST: api/Book
    // BODY: Book (JSON)
    //[Authorize]
    //[AllowAnonymous] // permet de ne pas avoir besoin d'être authentifié pour accéder à la méthode
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Customer))]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Customer>> PostAuthor([FromBody] Customer customer)
    {
        // we check if the parameter is null
        if (customer == null)
        {
            return BadRequest();
        }
        // we check if the book already exists
        Customer? addedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Firstname == customer.Firstname);
        if (addedCustomer != null)
        {
            return BadRequest("Customer already exists");
        }
        else
        {
            // we add the book to the database
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();

            // we return the book
            return Created("api/customer", customer);

        }
    }

    // TODO: Add PUT and DELETE methods
    // PUT: api/Book/5
    // BODY: Book (JSON)
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PutCustomer(int id, [FromBody] Customer customer)
    {
        if (id != customer.Id)
        {
            return BadRequest();
        }
        var customerToUpdate = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);

        if (customerToUpdate == null)
        {
            return NotFound();
        }

        customerToUpdate.Firstname = customer.Identifiant;
        customerToUpdate.Firstname = customer.Firstname;
        customerToUpdate.Firstname = customer.Lastname;
        customerToUpdate.Firstname = customer.MailAdress;

        _dbContext.Entry(customerToUpdate).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("validationTest")]
    public ActionResult ValidationTest([FromBody] CustomerDto customer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Customer>> DeleteCustomer(int id)
    {
        var customerToDelete = await _dbContext.Customers.FindAsync(id);
        // var bookToDelete = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);

        if (customerToDelete == null)
        {
            return NotFound();
        }

        _dbContext.Customers.Remove(customerToDelete);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

}