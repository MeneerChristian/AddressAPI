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
        public ActionResult<IEnumerable<Address>> Get(string city, string zip, string country, string street, string number, bool exact)
        {
            var addresses = _context.Addresses.ToList();

            if (!string.IsNullOrEmpty(city))
            {
                if (exact)
                    addresses = addresses.Where(a => a.City.ToLower().Equals(city.ToLower())).ToList();
                else
                    addresses = addresses.Where(a => a.City.ToLower().Contains(city.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(zip))
            {
                if (exact)
                    addresses = addresses.Where(a => a.Zip.ToLower().Equals(zip.ToLower())).ToList();
                else
                    addresses = addresses.Where(a => a.Zip.ToLower().Contains(zip.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(country))
            {
                if (exact)
                    addresses = addresses.Where(a => a.Country.ToLower().Equals(country.ToLower())).ToList();
                else
                    addresses = addresses.Where(a => a.Country.ToLower().Contains(country.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(street))
            {
                if (exact)
                    addresses = addresses.Where(a => a.Street.ToLower().Equals(street.ToLower())).ToList();
                else
                    addresses = addresses.Where(a => a.Street.ToLower().Contains(street.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(number))
            {
                if (exact)
                    addresses = addresses.Where(a => a.HouseNumber.ToLower().Equals(number.ToLower())).ToList();
                else
                    addresses = addresses.Where(a => a.HouseNumber.ToLower().Contains(number.ToLower())).ToList();
            }

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