using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CountryItem.Models;
using Country.Services;

    [Controller]
    [Route("api/[controller]")]
    public class CountryController: Controller {
        
        private readonly MongoDBService _mongoDBService;

        public CountryController(MongoDBService mongoDBService) {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<List<Countries>> Get() {
            return await _mongoDBService.GetAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Countries countries) {
            await _mongoDBService.CreateAsync(countries);
            return CreatedAtAction(nameof(Get), countries);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AddToCountries(string id, [FromBody] string countryId) {
            await _mongoDBService.AddToCountriesAsync(id, countryId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) {
            await _mongoDBService.DeleteAsync(id);
            return NoContent();
        }

    }