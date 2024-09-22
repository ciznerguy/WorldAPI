using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WorldAPI.Services;
using WorldAPI.Models; // שימוש במודל City
using System.Collections.Generic;

namespace WorldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

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

            return Ok(cities);
        }

        // GET: api/Cities/LargeCities
        [HttpGet("LargeCities")]
        public IActionResult GetLargeCities()
        {
            var largeCities = new List<City>();

            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();
                // שאילתה שמחזירה את הערים עם אוכלוסייה מעל 2,000,000
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

            return Ok(largeCities);
        }
        [HttpGet("GetCitiesAbovePopulation/{minPopulation}")]
        public IActionResult GetCitiesAbovePopulation(int minPopulation)
        {
            var largeCities = new List<City>(); // רשימה שתכיל את הערים

            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();

                // שאילתה שמחזירה ערים עם אוכלוסייה מעל המספר המינימלי
                string query = "SELECT Id, Name, Population FROM city WHERE Population > @minPopulation";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@minPopulation", minPopulation); // שימוש בפרמטר

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var city = new City
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Population = reader.GetInt32(2)
                    };
                    largeCities.Add(city); // הוספת העיר לרשימה
                }

                reader.Close();
            }

            return Ok(largeCities); // החזרת התוצאות בפורמט JSON
        }

    }
}
