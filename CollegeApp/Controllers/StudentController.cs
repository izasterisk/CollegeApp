using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        
        private readonly IMapper _mapper;
        public StudentController(ILogger<StudentController> logger, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsAsync()
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

            //Cách cơ bản
            //var students = await _dbContext.Students.Select(s => new StudentDTO()
            //{
            //    Id = s.Id,
            //    studentName = s.studentName,
            //    Address = s.Address,
            //    Email = s.Email,
            //    DOB = s.DOB.ToShortDateString(),
            //}).ToListAsync();

            //Lấy toàn bộ thì viết như dưới.
            var students = await _dbContext.Students.ToListAsync();
            //return Ok(students);

            var studentDTOData = _mapper.Map<List<StudentDTO>>(students);
            return Ok(studentDTOData);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentByID")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StudentDTO>> GetStudentByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad Request");
                return BadRequest();
            }

            var students = await _dbContext.Students.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (students == null)
            {
                _logger.LogError($"The student with ID = {id} not found!!!");
                return NotFound($"The student with ID = {id} not found!!!");
            }
            //var studentDTO = new StudentDTO
            //{
            //    Id = students.Id,
            //    studentName = students.studentName,
            //    Address = students.Address,
            //    Email = students.Email,
            //    DOB = students.DOB.ToShortDateString(),
            //};

            var studentDTO =_mapper.Map<StudentDTO>(students);
            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        public async Task<ActionResult<StudentDTO>> GetStudentByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var students = await _dbContext.Students.Where(n => n.studentName == name).FirstOrDefaultAsync();
            if (students == null)
                return NotFound($"The student name {name} not found!!!");
            //var studentDTO = new StudentDTO
            //{
            //    Id = students.Id,
            //    studentName = students.studentName,
            //    Address = students.Address,
            //    Email = students.Email,
            //    DOB = students.DOB.ToShortDateString(),
            //};

            var studentDTO = _mapper.Map<StudentDTO>(students);
            return Ok(studentDTO);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> CreateStudentAsync([FromBody]StudentDTO dto)
        {
            //if ((!ModelState.IsValid))
            //{
            //    return BadRequest(ModelState); 
            //}
            if (dto == null)
                return BadRequest();

            //if (model.AdmissionDate < DateTime.Now)
            //{
            //    ModelState.AddModelError("AdmissionDate Error", "Admission date should be greater than or equal to today's date");
            //    return BadRequest(ModelState);
            //}


            //Student student = new Student
            //{
            //    studentName = dto.studentName,
            //    Address = dto.Address,
            //    Email = dto.Email,
            //    DOB = Convert.ToDateTime(dto.DOB),
            //};

            var student = _mapper.Map<Student>(dto);
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            dto.Id = student.Id;
            return CreatedAtRoute("GetStudentByID", new { id = dto.Id }, dto);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                return BadRequest();
            }
            var student = await _dbContext.Students.AsNoTracking().Where(i => i.Id == dto.Id).FirstOrDefaultAsync();
            if (student == null)
            {
                return NotFound($"The student with ID = {dto.Id} not found!!!");
            }

            //var newRecord = new Student
            //{
            //    Id = student.Id,
            //    studentName = model.studentName,
            //    Email = model.Email,
            //    Address = model.Address,
            //    DOB = Convert.ToDateTime(model.DOB),
            //};

            var newRecord = _mapper.Map<Student>(dto);
            _dbContext.Students.Update(newRecord);
            //student.studentName = model.studentName;
            //student.Email = model.Email;
            //student.Address = model.Address;
            //student.DOB = Convert.ToDateTime(model.DOB);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            //Example: "path": "/studentName", "op": "replace", "value": "New Student Name"
            if (patchDocument == null || id <= 0)
            {
                return BadRequest();
            }
            var student = await _dbContext.Students.AsNoTracking().Where(i => i.Id == id).FirstOrDefaultAsync();
            if (student == null)
            {
                return NotFound($"The student with ID = {id} not found!!!");
            }
            //var model = new StudentDTO
            //{
            //    Id = student.Id,
            //    studentName = student.studentName,
            //    Email = student.Email,
            //    Address = student.Address,
            //};
            var studentDTO = _mapper.Map<StudentDTO>(student);
            patchDocument.ApplyTo(studentDTO, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //student.studentName = studentDTO.studentName;
            //student.Email = studentDTO.Email;
            //student.Address = studentDTO.Address;
            //student.DOB = Convert.ToDateTime(studentDTO.DOB);

            student = _mapper.Map<Student>(studentDTO);
            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteStudentByID")]
        public async Task<ActionResult<bool>> DeleteStudentAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var Student = await _dbContext.Students.Where(i => i.Id == id).FirstOrDefaultAsync();
            if (Student == null)
            {
                return NotFound($"The student with ID = {id} not found!!!");
            }
            _dbContext.Students.Remove(Student);
            await _dbContext.SaveChangesAsync();
            return Ok(true) ;
        }
    }
}
