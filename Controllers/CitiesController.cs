using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WorldAPI.Services;
using WorldAPI.Models; // Usage of City model
using System.Collections.Generic;

namespace WorldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        // Constructor: Inject the DatabaseService into the controller
        public CitiesController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // GET: api/Cities
        [HttpGet]
        public IActionResult GetCities()
        {
            List<City> cities = new List<City>();

            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = "SELECT Id, Name, Population FROM city";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    City city = new City
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Population = reader.GetInt32(2)
                    };
                    cities.Add(city);
                }

                reader.Close();
            }

            return Ok(cities); // Returns the list of cities
        }

        // GET: api/Cities/LargeCities
        [HttpGet("LargeCities")]
        public IActionResult GetLargeCities()
        {
            List<City> largeCities = new List<City>();

            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = "SELECT Id, Name, Population FROM city WHERE Population > 2000000";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    City city = new City
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Population = reader.GetInt32(2)
                    };
                    largeCities.Add(city);
                }

                reader.Close();
            }

            return Ok(largeCities); // Returns cities with population over 2 million
        }

        // GET: api/Cities/GetCitiesAbovePopulation/{minPopulation}
        [HttpGet("GetCitiesAbovePopulation/{minPopulation}")]
        public IActionResult GetCitiesAbovePopulation(int minPopulation)
        {
            List<City> cities = new List<City>();

            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = "SELECT Id, Name, Population FROM city WHERE Population > @minPopulation";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@minPopulation", minPopulation);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    City city = new City
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Population = reader.GetInt32(2)
                    };
                    cities.Add(city);
                }

                reader.Close();
            }

            return Ok(cities); // Returns cities with population above minPopulation
        }

        // POST: api/Cities/AddCity
        [HttpPost("AddCity")]
        public IActionResult AddCity([FromBody] City newCity)
        {
            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();

                string query = "INSERT INTO city (Name, CountryCode, District, Population) VALUES (@Name, @CountryCode, @District, @Population)";
                MySqlCommand command = new MySqlCommand(query, connection);

                // Adding parameters to the SQL query
                command.Parameters.AddWithValue("@Name", newCity.Name);
                command.Parameters.AddWithValue("@CountryCode", newCity.CountryCode);
                command.Parameters.AddWithValue("@District", newCity.District);
                command.Parameters.AddWithValue("@Population", newCity.Population);

                // Execute query and check if city was added
                var result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    return Ok(newCity); // City successfully added
                }
                else
                {
                    return BadRequest("Failed to add the city.");
                }
            }
        }
    }
}
