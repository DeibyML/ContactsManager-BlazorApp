using BlazorCrud.Server.Helpers;
using Microsoft.AspNetCore.Mvc;
using BlazorCrud.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrud.Server
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public PeopleController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Person>>> Get([FromQuery] Pagination pagination)
        {
            var queryable = context.People.AsQueryable();
            await HttpContext.InsertPaginationParametersInResponse(queryable, pagination.RowsPerPage);
            return await queryable.Paginate(pagination).OrderBy(con => con.Id).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Person person)
        {
            context.Add(person);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("GetPerson", new { Id = person.Id }, person);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Person person)
        {
            context.Entry(person).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}", Name = "GetPerson")]
        [AllowAnonymous]
        public async Task<ActionResult<Person>> Get(int id)
        {
            return await context.People.FirstOrDefaultAsync(p => p.Id == id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Person person = new Person { Id = id };
            context.Remove(person);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

