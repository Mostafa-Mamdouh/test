using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RESTCountries.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace API.Controllers
{

    public class HelperController : BaseApiController
    {


        [HttpGet("countryexisit")]
        public async Task<bool> IsCountryExisit(
[FromQuery] string country)
        {
            var countryObject = await RESTCountriesAPI.GetCountriesByNameContainsAsync(country);
            return countryObject.Any(x=>x.Name.ToLower()==country.ToLower());
        }

        [HttpGet("countrylookup")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetCountriesLookup()
        {
            var all = await RESTCountriesAPI.GetAllCountriesAsync();
            return all.Select(c => c.Name).ToList();
        }
    }
}
