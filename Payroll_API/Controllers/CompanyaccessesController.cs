using BLL.Interfaces;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyaccessesController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<CompanyaccessesController> _logger;
        public CompanyaccessesController(IGlobalMaster globalMaster, ILogger<CompanyaccessesController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }

        [HttpGet("get-Payroll_User_List")]
        public async Task<IActionResult> User_List()
        {
            try
            {
                var data = await _globalMaster.companyAccess.User_List_New();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(User_List)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        //[HttpGet("get-Payroll_User_List")]
        //public async Task<IActionResult> User_List_New()
        //{
        //    try
        //    {
        //        var data = await _globalMaster.companyAccess.User_List_New();
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Something went wrong in the {nameof(User_List_New)}");
        //        return StatusCode(500, "Internal Server Error, Please Try Again Later!");
        //    }
        //}

        [HttpGet("get-Payroll_User_Listfrom_ACCESS")]
        public async Task<IActionResult> User_Listfrom_ACCESS(string user)
        {
            try
            {
                var data = await _globalMaster.companyAccess.User_Listfrom_ACCESS(user);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(User_Listfrom_ACCESS)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet]
        public IActionResult GetDgPayCompanyaccesses()
        {
            try
            {
                var data = _globalMaster.companyAccess.GetAll().Select(data => DgPayCompanyaccess.DbToCustomModel(data)).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayCompanyaccesses)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDgPayCompanyaccess(int id)
        {
            try
            {
                var data = await _globalMaster.companyAccess.GetFirstOrDefaultAsync(x => x.ca_serial == id);
                var nData = DgPayCompanyaccess.DbToCustomModel(data);
                return Ok(nData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDgPayCompanyaccess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDgPayCompanyaccess(int id, DgPayCompanyaccess obj)
        {
            try
            {
                var nData = DgPayCompanyaccess.CustonToDbModel(obj);
                await _globalMaster.companyAccess.UpdateAsync(nData);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PutDgPayCompanyaccess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostDgPayCompanyaccess(DgPayCompanyaccess obj)
        {
            try
            {
                var isExists = _globalMaster.companyAccess.GetAll().Where(x => x.ca_compid==obj.CaCompid && x.ca_accessuser==obj.CaAccessuser).FirstOrDefault();
                if (isExists == null)
                {
                    var nData = DgPayCompanyaccess.CustonToDbModel(obj);
                    await _globalMaster.companyAccess.AddAsync(nData);
                    return CustomResult("Save Successfully !!", new { Company = obj.CaCompName, UserName = obj.CaAccessuser }, System.Net.HttpStatusCode.Created);
                    //return CreatedAtAction("GetDgPayCompanyaccess", new { ca_serial = nData.ca_serial }, nData);
                }
                return CustomResult("Already Exists !!", new {Company=obj.CaCompName,UserName=obj.CaAccessuser},HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(PostDgPayCompanyaccess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDgPayCompanyaccess(int id)
        {
            try
            {
                var obj = await _globalMaster.companyAccess.GetFirstOrDefaultAsync(c => c.ca_serial == id);
                if (obj == null)
                {
                    _logger.LogError($"Invalid Delete Attempt In {nameof(DeleteDgPayCompanyaccess)}");
                    return BadRequest("Submitted Data Is Invalid");
                }
                await _globalMaster.companyAccess.DeleteAsync(obj);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteDgPayCompanyaccess)}");
                return StatusCode(500, "Internal Server Error, Please Try Again Later!");
            }
        }
    }
}
