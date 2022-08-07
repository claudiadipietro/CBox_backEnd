using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CountryItem.Models;
using Country.Services;

    [Controller]
    [Route("api/[controller]")]
    public class CountryController: Controller {
        
        private readonly CountryDBService _countryDBService;

        public CountryController(CountryDBService countryDBService) {
            _countryDBService = countryDBService;
        }

        [HttpGet]
        public async Task<List<Countries>> Get() {
            return await _countryDBService.GetAsync();
        }

        [HttpGet("countriesFiltered")]
        public async Task<List<Countries>> GetCountriesSelected() {
            return await _countryDBService.GetAsync();
        }

        [HttpPut("randomizeCountries")]
         public async Task<IActionResult> PutRandom(){
            await _countryDBService.ModifyRandom();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Countries countries) {
            await _countryDBService.CreateAsync(countries);
            return CreatedAtAction(nameof(Get), countries);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AddToCountries(string id, [FromBody] string countryId) {
            await _countryDBService.AddToCountriesAsync(id, countryId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id) {
            await _countryDBService.DeleteAsync(id);
            return NoContent();
        }

    }