using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Web_API.Models;

namespace School_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolDatabaseContext _dbContext;

        public StudentsController(SchoolDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<StudentsController>
        [HttpGet]
        public List<Student> Get()
        {
            List<Student> students = _dbContext.Students
                .Join(_dbContext.Departements, student => student.DepartementId, department => department.DepartementId,
                    (student, department) => new Student
                    {
                        StudentId = student.StudentId,
                        DepartementId = student.DepartementId,
                        Name = student.Name,
                        Semester = student.Semester,
                        Age = student.Age,
                        Departement = department
                    })
                .ToList();

            return students;
        }

        // GET api/<StudentsController>/5
        [HttpGet("{id}")]
        public Student Get(int id)
        {
            Student student = _dbContext.Students
                .Where(x => x.StudentId == id)
                .Join(_dbContext.Departements, student => student.DepartementId, department => department.DepartementId,
                    (student, department) => new Student
                    {
                        StudentId = student.StudentId,
                        DepartementId = student.DepartementId,
                        Name = student.Name,
                        Semester = student.Semester,
                        Age = student.Age,
                        Departement = department
                    })
                .First();

            return student;
        }

        // POST api/<StudentsController>
        [HttpPost]
        public void Post([FromBody] Student student)
        {
            bool isDuplicate = _dbContext.Students.Any(x => x.Name == student.Name);

            if (!isDuplicate)
            {
                _dbContext.Add(student);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Departemen sudah terdaftar");
            }
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Student student)
        {
            bool isDuplicate = _dbContext.Students.Any(x => x.Name == student.Name && x.StudentId != id);

            if (!isDuplicate)
            {
                Student data = _dbContext.Students.Where(x => x.StudentId == id).AsNoTracking().First();
                data.DepartementId = student.DepartementId;
                data.Name = student.Name;
                data.Semester = student.Semester;
                data.Age = student.Age;

                _dbContext.Update(data);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Student sudah terdaftar");
            }
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Student student = _dbContext.Students.Where(x => x.StudentId == id).AsNoTracking().First();

            if (student != null)
            {
                _dbContext.Remove(student);
                _dbContext.SaveChanges();
            }
            else
            {
                throw new Exception("Student tidak ditemukan");
            }
        }
    }
}
