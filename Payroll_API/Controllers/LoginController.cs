using BLL.Interfaces;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;
        public LoginController(IGlobalMaster globalMaster, IConfiguration configuration, ILogger<LoginController> logger)
        {
            _globalMaster = globalMaster;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("PostLoginDetails")]
        public async Task<IActionResult> PostLoginDetails(UserModel _userData)
        {
            try
            {
                if (_userData != null)
                {
                    //var resultLoginCheck = await _globalMaster.login.GetFirstOrDefaultAsync(e => e.FullName == _userData.FullName && e.Password == _userData.Password && Convert.ToBoolean(e.Active_status)==true);
                    var resultLoginCheck = await _globalMaster.login.GetMasterUserTableInfo(_userData.FullName, _userData.Password);
                    if (resultLoginCheck == null)
                    {
                        return BadRequest("Invalid Credentials");
                    }
                    else
                    {
                        _userData.UserMessage = "Login Success";
                        //string EmployeeId = _userData.FullName.Any(char.IsDigit) ? Regex.Replace(_userData.FullName, "[^0-9]", string.Empty) : _userData.FullName;
                        string EmployeeId = !string.IsNullOrEmpty(resultLoginCheck.Emp_ID) ? resultLoginCheck.Emp_ID : _userData.FullName;
                        var claims = new[] {
                            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("UserId", _userData.FullName),
                            new Claim("DisplayName", resultLoginCheck.UserFullname)
                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(1000000),
                            signingCredentials: signIn);
                        _userData.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
                        var accessToken = _userData.AccessToken;
                        //return Ok(new { accessToken = accessToken, UserFullName = resultLoginCheck.UserFullname, resultLoginCheck.CompId, EmpId = resultLoginCheck.Emp_ID,userName= resultLoginCheck.FullName });
                        return Ok(new { accessToken = accessToken, UserFullName = resultLoginCheck.UserFullname, resultLoginCheck.CompId, EmpId = EmployeeId, userName = resultLoginCheck.FullName });
                    }
                }
                else
                {
                    return BadRequest("No Data Posted");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostLoginDetails)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("batchUserPassUpdate")]
        public async Task<IActionResult> UpdateUserPassBatch()
        {
            var res = await _globalMaster.login.UpdateUserPassBatch();
            return Ok(res);
        }

        [HttpPost("Save_PasswordChange")]
        public async Task<IActionResult> Save_PasswordChange(int compID, string loginID, string confPassword, string newPassword)
        {
            try
            {
                var response = await _globalMaster.login.SaveUserPasswordChange(compID, loginID, confPassword, newPassword);
                if (response == "Password Change Successfully !!")
                {
                    return CustomResult(response, HttpStatusCode.OK);
                }
                return CustomResult(response,HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Save_PasswordChange)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SendPasswordResetCode")]
        public async Task<IActionResult> SendPasswordResetCode(UserPasswordResetPayload obj)
        {
            try
            {
                var response = await _globalMaster.login.SendPasswordResetCode(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message });
                }
                return BadRequest(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SendPasswordResetCode)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("CheckResetCodeValidity")]
        public async Task<IActionResult> CheckResetCodeValidity(string userId, string vCode)
        {
            try
            {
                var response = await _globalMaster.login.CheckResetCodeValidity(userId, vCode);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CheckResetCodeValidity)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("CheckUserEmailSet")]
        public async Task<IActionResult> CheckUserEmailSet(UserPasswordResetPayload obj)
        {
            try
            {
                var response = await _globalMaster.login.CheckUserEmailSet(obj);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CheckUserEmailSet)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("SetupUserEmailId")]
        public async Task<IActionResult> SetupUserEmailId(UserEmailSetPayload obj)
        {
            try
            {
                var response = await _globalMaster.login.SetupUserEmailId(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message });
                }
                return BadRequest(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SetupUserEmailId)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("userMenuParmission2")]
        public async Task<IActionResult> GetPermitedMenuList2(List<MenuList2> obj)
        {
            try
            {
                var result = await _globalMaster.login.GetPermitedMenuList2(obj);
                return CustomResult("Menu Loaded !!", result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetPermitedMenuList2)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("userMenuParmission")]
        public async Task<IActionResult> GetPermitedMenuList(List<MenuList> obj)
        {
            try
            {
                var result = await _globalMaster.login.GetPermitedMenuList(obj);
                if (result.Count > 0)
                {
                    return CustomResult("Menu Loaded !!", result, HttpStatusCode.OK);
                }
                return CustomResult("Menu Not Loaded !!", result, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetPermitedMenuList)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost("userBtnParmission")]
        public async Task<IActionResult> SetBtnPermission(List<ButtonList> obj)
        {
            try
            {
                var result = await _globalMaster.login.SetBtnPermission(obj);
                if (result.Count > 0)
                {
                    return CustomResult("Button Loaded !!", result, HttpStatusCode.OK);
                }
                return CustomResult("Button Not Loaded !!", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SetBtnPermission)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }       
    }
}
