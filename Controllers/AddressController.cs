using System.Collections.Generic;
using System.Linq;
using AddressAPI.Data;
using AddressAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AddressAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly DatabaseContext _context;

        public AddressController(DatabaseContext context)
        {
            _context = context;
        }

        // GET api/address?
        [HttpGet]
        public ActionResult<IEnumerable<Address>> Get()
        {
            var addresses = _context.Addresses.ToList();

            return addresses;
        }

        // GET api/address/5
        [HttpGet("{id}")]
        public ActionResult<Address> Get(int id)
        {
            return _context.Addresses.FirstOrDefault(a => a.Id == id);
        }

        // POST api/address
        [HttpPut]
        public ActionResult Put([FromBody] Address address)
        {
            _context.Addresses.Add(address);
            _context.SaveChanges();

            return Ok();
        }

        // PUT api/address/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Address address)
        {
            var addressToUpdate = _context.Addresses.FirstOrDefault(a => a.Id == id);
            addressToUpdate.Street = address.Street;
            addressToUpdate.HouseNumber = address.HouseNumber;
            addressToUpdate.City = address.City;
            addressToUpdate.Zip = address.Zip;
            addressToUpdate.Country = address.Country;
            _context.SaveChanges();

            return Ok();
        }

        // DELETE api/address/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var addressToDelete = _context.Addresses.FirstOrDefault(a => a.Id == id);
            _context.Addresses.Remove(addressToDelete);
            _context.SaveChanges();
        }
    }
}