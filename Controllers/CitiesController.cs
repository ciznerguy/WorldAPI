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

            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();
                // שאילתה שמחזירה את הערים עם אוכלוסייה מעל 2,000,000
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

        //[HttpPost("AddCity")]
        //public IActionResult AddCity([FromBody] City newCity)  
        //{
        //    using (MySqlConnection connection = _databaseService.GetConnection())
        //    {
        //        connection.Open();

        //        // שאילתת SQL להוספת עיר חדשה עם כל העמודות
        //        string query = "INSERT INTO city (Name, CountryCode, District, Population) VALUES (@Name, @CountryCode, @District, @Population)";
        //        MySqlCommand command = new MySqlCommand(query, connection);

        //        // העברת פרמטרים לשאילתה
        //        command.Parameters.AddWithValue("@Name", newCity.Name);
        //        command.Parameters.AddWithValue("@CountryCode", newCity.Id);
        //        command.Parameters.AddWithValue("@District", newCity.District);
        //        command.Parameters.AddWithValue("@Population", newCity.Population);

        //        // ביצוע השאילתה
        //        var result = command.ExecuteNonQuery();

        //        // בדיקת הצלחה והחזרת תשובה
        //        if (result > 0)
        //        {
        //            return Ok(newCity); // החזרת העיר החדשה שנוספה
        //        }
        //        else
        //        {
        //            return BadRequest("Failed to add the city.");
        //        }
        //    }
        //}


    }
}
