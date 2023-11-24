using Microsoft.AspNetCore.Mvc;

namespace CourierCompanyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        // GET: api/<OffersController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<OffersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OffersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OffersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OffersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
