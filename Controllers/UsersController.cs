using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bug_Tracker_Api.Models;

namespace Bug_Tracker_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController(AppDb db)
        {
            Db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            await Db.Connection.OpenAsync();
            var query = new UserQuery(Db);
            var result = await query.GetUsersAsync();
            return new OkObjectResult(result);
        }

        // GET api/user/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new UserQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);
        }

        // POST api/user
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User body)
        {
            await Db.Connection.OpenAsync();
            body.Db = Db;
            await body.InsertAsync();
            return new OkObjectResult(body);
        }

        // PUT api/user/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] User body)
        {
            await Db.Connection.OpenAsync();
            var query = new UserQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            result.First_Name = body.First_Name;
            result.Last_Name = body.Last_Name;
            result.Email = body.Email;
            result.Password = body.Password;

            await result.UpdateAsync();
            return new OkObjectResult(result);
        }

        // DELETE api/user/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new UserQuery(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            await result.DeleteAsync();
            return new OkResult();
        }

        // DELETE api/user
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            await Db.Connection.OpenAsync();
            var query = new UserQuery(Db);
            await query.DeleteAllAsync();
            return new OkResult();
        }

        public AppDb Db { get; }
    }
}
