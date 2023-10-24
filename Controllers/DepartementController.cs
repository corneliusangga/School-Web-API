using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Web_API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace School_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DepartementController : ControllerBase
    {
        private readonly SchoolDatabaseContext _dbContext;

        public DepartementController(SchoolDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<DepartementController>
        [HttpGet]
        public List<Departement> Get()
        {
            List<Departement> departements = _dbContext.Departements.AsNoTracking().ToList();

            return departements;
        }

        // GET api/<DepartementController>/5
        [HttpGet("{id}")]
        public Departement Get(int id)
        {
            Departement departement = _dbContext.Departements.Where(x => x.DepartementId == id).AsNoTracking().First();

            return departement;
        }

        // POST api/<DepartementController>
        [HttpPost]
        public void Post([FromBody] Departement departement)
        {
            bool isDuplicate = _dbContext.Departements.Any(x => x.Name == departement.Name);

            if (!isDuplicate)
            {
                _dbContext.Add(departement);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Departemen sudah terdaftar");
            }
        }

        // PUT api/<DepartementController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Departement departement)
        {
            bool isDuplicate = _dbContext.Departements.Any(x => x.Name == departement.Name && x.DepartementId != id);

            if (!isDuplicate)
            {
                Departement data = _dbContext.Departements.Where(x => x.DepartementId == id).AsNoTracking().First();
                data.Name = departement.Name;

                _dbContext.Update(data);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Departemen sudah terdaftar");
            }
        }

        // DELETE api/<DepartementController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Departement departement = _dbContext.Departements.Where(x => x.DepartementId == id).First();

            if (departement != null)
            {
                _dbContext.Remove(departement);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Departemen tidak ditemukan");
            }
        }
    }
}
