﻿using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {
            _logger.LogInformation("GetStudents method called");
            //var students = new List<StudentDTO>();
            //foreach (var item in CollegeRepository.Students) {
            //    StudentDTO obj = new StudentDTO()
            //    {
            //        Id = item.Id,
            //        studentName = item.studentName,
            //        Address = item.Address,
            //        Email = item.Email,
            //    };
            //    students.Add(obj);
            //}

            var students = CollegeRepository.Students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                studentName = s.studentName,
                Address = s.Address,
                Email = s.Email,
            });
            return Ok(students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentByID")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }

            var students = CollegeRepository.Students.Where(c => c.Id == id).FirstOrDefault();
            if (students == null)
            {
                _logger.LogError($"The student with ID = {id} not found!!!");
                return NotFound($"The student with ID = {id} not found!!!");
            }
            var studentDTO = new StudentDTO
            {
                Id = students.Id,
                studentName = students.studentName,
                Address = students.Address,
                Email = students.Email,
            };
            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        public ActionResult<StudentDTO> GetStudentByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var students = CollegeRepository.Students.Where(n => n.studentName == name).FirstOrDefault();
            if (students == null)
                return NotFound($"The student name {name} not found!!!");
            var studentDTO = new StudentDTO
            {
                Id = students.Id,
                studentName = students.studentName,
                Address = students.Address,
                Email = students.Email,
            };
            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> CreateStudent([FromBody]StudentDTO model)
        {
            //if ((!ModelState.IsValid))
            //{
            //    return BadRequest(ModelState); 
            //}
            if (model == null)
                return BadRequest();

            //if (model.AdmissionDate < DateTime.Now)
            //{
            //    ModelState.AddModelError("AdmissionDate Error", "Admission date should be greater than or equal to today's date");
            //    return BadRequest(ModelState);
            //}

            int newId = CollegeRepository.Students.LastOrDefault().Id + 1;

            Student student = new Student
            {
                Id=newId,
                studentName=model.studentName,
                Address = model.Address,
                Email = model.Email,
            };
            CollegeRepository.Students.Add(student);
            model.Id = student.Id;
            return CreatedAtRoute("GetStudentByID", new { id = model.Id }, model);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
            {
                return BadRequest();
            }
            var student = CollegeRepository.Students.Where(i => i.Id == model.Id).FirstOrDefault();
            if (student == null)
            {
                return NotFound($"The student with ID = {model.Id} not found!!!");
            }
            student.studentName = model.studentName;
            student.Email = model.Email;
            student.Address = model.Address;
            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
            {
                return BadRequest();
            }
            var student = CollegeRepository.Students.Where(i => i.Id == id).FirstOrDefault();
            if (student == null)
            {
                return NotFound($"The student with ID = {id} not found!!!");
            }
            var model = new StudentDTO
            {
                Id = student.Id,
                studentName = student.studentName,
                Email = student.Email,
                Address = student.Address,
            };
            patchDocument.ApplyTo(model, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            student.studentName = model.studentName;
            student.Email = model.Email;
            student.Address = model.Address;
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteStudentByID")]
        public ActionResult<bool> DeleteStudent(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var Student = CollegeRepository.Students.Where(i => i.Id == id).FirstOrDefault();
            if (Student == null)
            {
                return NotFound($"The student with ID = {id} not found!!!");
            }
            CollegeRepository.Students.Remove(Student);
            return Ok(true) ;
        }
    }
}
