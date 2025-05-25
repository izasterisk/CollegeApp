using System.Net;
using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using CollegeApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;
        private readonly IUserService _userService;
        public UserController(ILogger<UserController> logger, IMapper mapper, IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _apiResponse = new();
            _userService = userService;
        }
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateUserAsync(UserDTO dto)
        {
            try
            {
                var userCreated = await _userService.CreateUserAsync(dto);
                _apiResponse.Data = userCreated;
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
        [Route("All", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userService.GetUserAsync();
                _apiResponse.Data = users;
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
        [Route("{id:int}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetUserByIdAsync(int id)
        {
            try
            {
                if(id <= 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Status = false;
                    _apiResponse.Errors.Add("Invalid user ID provided.");
                    return BadRequest(_apiResponse);
                }
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Status = false;
                    _apiResponse.Errors.Add($"User with ID {id} not found.");
                    return NotFound(_apiResponse);
                }
                _apiResponse.Data = user;
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
        [Route("{username}", Name = "GetUserByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetUserByUsernameAsync(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Status = false;
                    _apiResponse.Errors.Add("Invalid username provided.");
                    return BadRequest(_apiResponse);
                }
                var user = await _userService.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Status = false;
                    _apiResponse.Errors.Add($"User with username {username} not found.");
                    return NotFound(_apiResponse);
                }
                _apiResponse.Data = user;
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
    }
}
