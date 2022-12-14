using Microsoft.AspNetCore.Mvc;
using CustomerCrud.Core;
using CustomerCrud.Requests;
using CustomerCrud.Repositories;

namespace CustomerCrud.Controllers;

[ApiController]
[Route("/controller")]

public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _db;

    public  CustomerController(ICustomerRepository db)
    {
        _db = db;
    }

    [HttpGet]
    public ActionResult GetAll()
    {
        IEnumerable<Customer> CustomerList = _db.GetAll().AsQueryable();
        
        return Ok(CustomerList);
    }

    [HttpGet("{id}", Name = "GetById")]
    public ActionResult GetById(int id)
    {
        var CustomerOne = _db.GetById(id);

        if (CustomerOne == null) return NotFound("Customer Not Found");

        return Ok(CustomerOne);
    }

   [HttpPost]
    public ActionResult Create(CustomerRequest request)
    {
        // Não consegui testar no Postman...Desativar SSL
        var customer = new Customer(_db.GetNextIdValue(), request);
        _db.Create(customer);
        return CreatedAtAction("GetById", new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]

    public ActionResult Update(int id, CustomerRequest request)
    {

        var CustomerUpdated =_db.Update(id, request);

        if (!CustomerUpdated) return NotFound("Customer Not Found");

        return Ok($"Customer {id} updated");
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var customerDeleted = _db.Delete(id);

        if (!customerDeleted) 
            return NotFound("Customer Not Found");

        return Ok($"Customer {id} deleted");
    }
}
