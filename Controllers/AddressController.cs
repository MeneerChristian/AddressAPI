using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AddressAPI.Data;
using AddressAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using Newtonsoft.Json;
using AddressAPI.Models.Request;

namespace AddressAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : Controller
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly DatabaseContext _context;

        public AddressController(DatabaseContext context)
        {
            _context = context;
        }

        // GET api/address
        [HttpGet]
        public ActionResult<IEnumerable<Address>> Get([FromForm] AddressFilter data)
        {
            var addresses = _context.Addresses.AsQueryable();

            if (data != null) {
                var exact = data.Exact;
                
                // Check if variable is set in data and if it is not null filter addresses by that variable dynamically
                if (exact)
                    addresses = addresses.Where(a => a.Street == (data.Street ?? a.Street))
                                        .Where(a => a.City == (data.City ?? a.City))
                                        .Where(a => a.Zip == (data.Zip ?? a.Zip))
                                        .Where(a => a.Country == (data.Country ?? a.Country))
                                        .Where(a => a.HouseNumber == (data.HouseNumber ?? a.HouseNumber));
                else
                    addresses = addresses.Where(a => a.Street.Contains(data.Street ?? a.Street))
                                        .Where(a => a.City.Contains(data.City ?? a.City))
                                        .Where(a => a.Zip.Contains(data.Zip ?? a.Zip))
                                        .Where(a => a.Country.Contains(data.Country ?? a.Country))
                                        .Where(a => a.HouseNumber.Contains(data.HouseNumber ?? a.HouseNumber));
                

                // Sort addresses by SortBy with SortOrder 
                if (String.IsNullOrEmpty(data.SortBy))
                {
                    if (!String.IsNullOrEmpty(data.SortOrder) && data.SortOrder == "desc")
                        addresses = addresses.OrderByDescending(d => typeof(Address).GetProperty(data.SortBy).GetValue(d, null));
                    else if(data.SortOrder == "asc")
                        addresses = addresses.OrderBy(d => typeof(Address).GetProperty(data.SortBy).GetValue(d, null));
                }
            }

            return addresses.ToList();
        }

        // GET api/address/5
        [HttpGet("{id}")]
        public ActionResult<Address> Get(int id)
        {
            var address = _context.Addresses.FirstOrDefault(a => a.Id == id);

            if (address == null)
                return NotFound();

            return address;
        }

        // PUT api/address
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
        public ActionResult Delete(int id)
        {
            var addressToDelete = _context.Addresses.FirstOrDefault(a => a.Id == id);
            _context.Addresses.Remove(addressToDelete);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("measure/{id1}/{id2}")]
        public async Task<ActionResult> Measure(int id1, int id2) 
        {
            var address1 = _context.Addresses.FirstOrDefault(a => a.Id == id1);
            var address2 = _context.Addresses.FirstOrDefault(a => a.Id == id2);

            var geometry1 = await GetGeometry(address1);
            var geometry2 = await GetGeometry(address2);

            var distance = GetDistance(geometry1, geometry2);

            return Ok(distance);
        }

        private double GetDistance((double lat1, double lng1) geometry1, (double lat2, double lng2) geometry2)
        {
            return GetDistance(geometry1.lat1, geometry1.lng1, geometry2.lat2, geometry2.lng2);
        }

        private double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            var R = 6371;
            var dLat = deg2rad(lat2-lat1);
            var dLon = deg2rad(lng2-lng1); 
            var a = 
                Math.Sin(dLat/2) * Math.Sin(dLat/2) +
                Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * 
                Math.Sin(dLon/2) * Math.Sin(dLon/2)
                ; 
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a)); 
            var d = R * c;
            return d;
        }

        private double deg2rad(double deg)
        {
            return deg * (Math.PI/180);
        }

        private async Task<(double, double)> GetGeometry(Address address)
        {
            var key = "35bea045c16d488ca20c7081c6f05efc";
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.opencagedata.com/geocode/v1/json?q={AddressToString(address)}&key={key}&language=en&pretty=1&no_annotations=1");

            var content = (await response.Content.ReadAsStringAsync());
            dynamic json = JsonConvert.DeserializeObject(content);

            Console.WriteLine(json);

            var geometry = json.results[0].geometry;

            return (geometry.lat, geometry.lng);
        }

        private string AddressToString(Address address)
        {
            return $"{address.Street} {address.HouseNumber}, {address.Zip} {address.City}, {address.Country}";
        }
    }
}