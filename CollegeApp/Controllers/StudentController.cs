using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors(PolicyName = "AllowOnlyGoogle")]
    //[DisableCors]
    [Authorize(AuthenticationSchemes = "LoginforLocaluser", Roles = "Superadmin, Admin")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;        
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private APIResponse _apiResponse;
        //private readonly ICollegeRepository<Student> _studentRepository;


        //public StudentController(ILogger<StudentController> logger, IMapper mapper, IStudentRepository studentRepository)
        public StudentController(ILogger<StudentController> logger, IMapper mapper,
            IStudentRepository studentRepository)
        {
            _logger = logger;
            _studentRepository = studentRepository;
            _mapper = mapper;
            _apiResponse = new();
        }
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[EnableCors(PolicyName = "AllowOnlyGoogle")]        
        //[AllowAnonymous]

        //public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsAsync()
        public async Task<ActionResult<APIResponse>> GetStudentsAsync()
        {
            try
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
                //var students = await _dbContext.Students.ToListAsync();

                var students = await _studentRepository.GetAllAsync();
                //return Ok(students);

                //var studentDTOData = _mapper.Map<List<StudentDTO>>(students);
                //return Ok(studentDTOData);

                //79. Chuyển về chung 1 response type cho dễ quản lý
                _apiResponse.Data = _mapper.Map<List<StudentDTO>>(students);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                //Ok - 200
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }            
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentByID")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetStudentByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Bad Request");
                    return BadRequest();
                }

                //var students = await _dbContext.Students.Where(c => c.Id == id).FirstOrDefaultAsync();

                //var students = await _studentRepository.GetByIdAsync(id);
                var students = await _studentRepository.GetAsync(students => students.Id == id);
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

                _apiResponse.Data = _mapper.Map<StudentDTO>(students);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }            
        }

        [HttpGet("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetStudentByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return BadRequest();

                //var students = await _dbContext.Students.Where(n => n.studentName == name).FirstOrDefaultAsync();
                //var students = await _studentRepository.GetByNameAsync(name);
                var students = await _studentRepository.GetAsync(students => students.studentName.ToLower().Contains(name));

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

                _apiResponse.Data = _mapper.Map<StudentDTO>(students);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateStudentAsync([FromBody]StudentDTO dto)
        {
            try
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

                //await _dbContext.Students.AddAsync(student);
                //await _dbContext.SaveChangesAsync();

                //var id = await _studentRepository.CreateAsync(student);
                //dto.Id = id;

                var studentAfterCreated = await _studentRepository.CreateAsync(student);
                dto.Id = studentAfterCreated.Id;
                _apiResponse.Data = dto;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return CreatedAtRoute("GetStudentByID", new { id = dto.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }            
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateStudentAsync([FromBody] StudentDTO dto)
        {
            try
            {
                if (dto == null || dto.Id <= 0)
                {
                    return BadRequest();
                }
                var student = await _studentRepository.GetAsync(s => s.Id == dto.Id, true);
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
                //_dbContext.Students.Update(newRecord);
                await _studentRepository.UpdateAsync(newRecord);
                //student.studentName = model.studentName;
                //student.Email = model.Email;
                //student.Address = model.Address;
                //student.DOB = Convert.ToDateTime(model.DOB);

                //await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }            
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            try
            {
                //Example: "path": "/studentName", "op": "replace", "value": "New Student Name"
                if (patchDocument == null || id <= 0)
                {
                    return BadRequest();
                }
                var student = await _studentRepository.GetAsync(s => s.Id == id, true);
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
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //student.studentName = studentDTO.studentName;
                //student.Email = studentDTO.Email;
                //student.Address = studentDTO.Address;
                //student.DOB = Convert.ToDateTime(studentDTO.DOB);

                student = _mapper.Map<Student>(studentDTO);
                //_dbContext.Students.Update(student);
                //await _dbContext.SaveChangesAsync();
                await _studentRepository.UpdateAsync(student);
                return NoContent();
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }            
        }

        [HttpDelete("{id}", Name = "DeleteStudentByID")]
        public async Task<ActionResult<APIResponse>> DeleteStudentAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }
                var Student = await _studentRepository.GetAsync(s => s.Id == id);
                if (Student == null)
                {
                    return NotFound($"The student with ID = {id} not found!!!");
                }
                await _studentRepository.DeleteAsync(Student);

                _apiResponse.Data = true;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }            
        }
    }
}
