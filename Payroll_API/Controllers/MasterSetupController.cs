using BLL.Interfaces;
using BOL.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Payroll_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterSetupController : BaseController
    {
        private readonly IGlobalMaster _globalMaster;
        private readonly ILogger<MasterSetupController> _logger;
        public MasterSetupController(IGlobalMaster globalMaster,ILogger<MasterSetupController> logger)
        {
            _globalMaster = globalMaster;
            _logger = logger;
        }


        //Payroll Setup
        #region"Divisions"
        [HttpPost("AddOrEditDivision")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveOrUpdateDivision(DivisionPayload obj)
        {
            try
            {
                var response = await _globalMaster.divisions.AddOrEditDivision(obj);
                if (response.IsSuccess == true)
                {
                    return CustomResult(response.Message,HttpStatusCode.OK);
                }
                return CustomResult(response.Message,HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveOrUpdateDivision)}");
                return CustomResult("Internal Server Error, Please Try Again Later!",HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetAllDivisions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDivisions()
        {
            try
            {
                var response = await _globalMaster.divisions.GetAllDivisions();
                if (response.IsSuccess == true)
                {
                    return CustomResult(response.Message,response.ListData, HttpStatusCode.OK);
                }
                return CustomResult(response.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllDivisions)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetAllDivisionById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDivisionById(int id)
        {
            try
            {
                var response = await _globalMaster.divisions.GetDivisionById(id);
                if (response.IsSuccess == true)
                {
                    return CustomResult(response.Message, response.SingleData, HttpStatusCode.OK);
                }
                return CustomResult(response.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllDivisionById)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"District"
        [HttpPost("AddOrEditDistrict")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditDistrict(DistrictPayload obj)
        {
            try
            {
                var response = await _globalMaster.districts.AddOrEditDistrict(obj);
                if (response.IsSuccess == true)
                {
                    return CustomResult(response.Message, HttpStatusCode.OK);
                }
                return CustomResult(response.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditDistrict)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllDistricts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDistricts()
        {
            try
            {
                var response = await _globalMaster.districts.GetAllDistrict();
                if (response.IsSuccess == true)
                {
                    return CustomResult(response.Message, response.ListData, HttpStatusCode.OK);
                }
                return CustomResult(response.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllDistricts)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion
        
        #region"Thana"
        [HttpPost("AddOrEditThana")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditThana(ThanaPayload obj)
        {
            try
            {
                var response = await _globalMaster.thanas.AddOrEditThana(obj);
                if (response.IsSuccess == true)
                {
                    return CustomResult(response.Message, HttpStatusCode.OK);
                }
                return CustomResult(response.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditThana)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllThanaList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllThanaList()
        {
            try
            {
                var response = await _globalMaster.thanas.GetAllThana();
                if (response.IsSuccess == true)
                {
                    return CustomResult(response.Message, response.ListData, HttpStatusCode.OK);
                }
                return CustomResult(response.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllThanaList)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"Designation"
        [HttpGet("GetDesignationCatList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDesignationCatList()
        {
            try
            {
                var response = await _globalMaster.designations.GetDesignationCatList();
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.dataTable });
                }
                return BadRequest(new { response.IsSuccess,response.Message, response.dataTable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetDesignationCatList)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("AddOrEditDesignation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditDesignation(DesignationPayload obj)
        {
            try
            {
                var response = await _globalMaster.designations.AddOrEditDesignation(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditDesignation)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllDesignationList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDesignationList()
        {
            try
            {
                var response = await _globalMaster.designations.GetAllDesignation();
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllDesignationList)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"Department"
        [HttpPost("AddOrEditDepartment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditDepartment(DepartmentPayload obj)
        {
            try
            {
                var response = await _globalMaster.departments.AddOrEditDepartment(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditDepartment)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllDepartmentListByCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDepartmentList(int compnayID)
        {
            try
            {
                var response = await _globalMaster.departments.GetAllDepartmentByCompany(compnayID);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllDepartmentList)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"Section"
        [HttpPost("AddOrEditSection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditSection(SectionPayload obj)
        {
            try
            {
                var response = await _globalMaster.sections.AddOrEditSection(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditSection)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllSectionListByCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSectionListByCompany(int compnayID)
        {
            try
            {
                var response = await _globalMaster.sections.GetAllSectionByCompany(compnayID);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllSectionListByCompany)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"BuildingUnit"
        [HttpPost("AddOrEditBuildingUnit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditBuildingUnit(BuildingUnitPayload obj)
        {
            try
            {
                var response = await _globalMaster.buildingUnit.AddOrEditBuildingUnit(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditBuildingUnit)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllBuildingUnitByCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllBuildingUnitByCompany(int compnayID)
        {
            try
            {
                var response = await _globalMaster.buildingUnit.GetAllBuildingUnitByCompany(compnayID);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllBuildingUnitByCompany)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"Floor"
        [HttpPost("AddOrEditFloor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditFloor(FloorPayload obj)
        {
            try
            {
                var response = await _globalMaster.floors.AddOrEditFloor(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditFloor)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllFloorByCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFloorByCompany(int compnayID)
        {
            try
            {
                var response = await _globalMaster.floors.GetAllFloorByCompany(compnayID);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllFloorByCompany)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"Line"
        [HttpPost("AddOrEditLine")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditLine(LinePayload obj)
        {
            try
            {
                var response = await _globalMaster.lines.AddOrEditLine(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditLine)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllLineByCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllLineByCompany(int compnayID)
        {
            try
            {
                var response = await _globalMaster.lines.GetAllLineByCompany(compnayID);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllLineByCompany)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetBlockByCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBlockByCompany(int compnayID, int floorID)
        {
            try
            {
                var response = await _globalMaster.lines.GetBlockByCompany(compnayID, floorID);
                if (response.IsSuccess == true)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetBlockByCompany)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpPost("AddOrEditLineBlock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditLineBlock(LineBlockPayload obj)
        {
            try
            {
                var response = await _globalMaster.lines.AddOrEditLineBlock(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, message = response.Message, data = response.dataTable });
                }
                return BadRequest(new { isSuccess = response.IsSuccess, statusCode = 400, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditLineBlock)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllLineBlockByCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllLineBlockByCompany(int compnayID)
        {
            try
            {
                var response = await _globalMaster.lines.GetAllLineBlockByCompany(compnayID);
                if (response.IsSuccess == true)
                {
                    return Ok(new { isSuccess = response.IsSuccess, statusCode = 200, data = response.dataTable });
                }
                return NotFound(new { isSuccess = response.IsSuccess, statusCode = 404, message = response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllLineBlockByCompany)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"Allowance"
        [HttpPost("AddOrEditAllowance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditAllowance(AllowancesdepPayload obj)
        {
            try
            {
                var response = await _globalMaster.allowancesde.AddOrEditAllowance(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditAllowance)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllAllowances")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAllowances()
        {
            try
            {
                var response = await _globalMaster.allowancesde.GetAllAllowances();
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllAllowances)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"SalaryCategory"
        [HttpPost("AddOrEditSalaryCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditSalaryCategory(SalarycategoryPayload obj)
        {
            try
            {
                var response = await _globalMaster.salaryCategories.AddOrEditSalaryCategory(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditSalaryCategory)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllSalaryCategoryByCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSalaryCategoryByCompany(int companyID)
        {
            try
            {
                var response = await _globalMaster.salaryCategories.GetAllSalaryCategoryByCompany(companyID);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllSalaryCategoryByCompany)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"Grade"
        [HttpPost("AddOrEditGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditGrade(GradePayload obj)
        {
            try
            {
                var response = await _globalMaster.grades.AddOrEditGrade(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditGrade)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllGrade()
        {
            try
            {
                var response = await _globalMaster.grades.GetAllGrade();
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllGrade)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"Deduction"
        [HttpPost("AddOrEditDeductions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditDeductions(DeductionPayload obj)
        {
            try
            {
                var response = await _globalMaster.deduction.AddOrEditDeduction(obj);
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditDeductions)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        [HttpGet("GetAllDeductions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDeductions()
        {
            try
            {
                var response = await _globalMaster.deduction.GetAllDeduction();
                if (response.IsSuccess == true)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllDeductions)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"CompanyAccess"
        [HttpGet("GetAllCompanyAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllCompanyAccess()
        {
            try
            {
                var response = _globalMaster.companyAccess.GetAllCompanyAccess();
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message, response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllCompanyAccess)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetAccessByCompanyUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAccessByCompanyUser(int companyID, string userName)
        {
            try
            {
                var response = await _globalMaster.companyAccess.GetAccessByCompanyUser(companyID, userName);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message, response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAccessByCompanyUser)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("AddOrEditCompanyAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditCompanyAccess(DgPayCompanyAccessPayload obj)
        {
            try
            {
                var response = await _globalMaster.companyAccess.AddOrEditCompanyAccess(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message, response.ListData });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditCompanyAccess)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        #region"AllPermission"
        [HttpGet("GetMasterMenuList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMasterMenuList()
        {
            try
            {
                var response = await _globalMaster.masterSetup.GetMasterMenuList();
                if (response.Count > 0)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetMasterMenuList)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetMasterButtonList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMasterButtonList(string userName)
        {
            try
            {
                var response = await _globalMaster.masterSetup.GetMasterButtonList(userName);
                if (response.Count > 0)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetMasterButtonList)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("SaveUserWiseButton")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveUserWiseButton(ButtonPermissionSavePayload obj)
        {
            try
            {
                var response = await _globalMaster.masterSetup.SaveUserWiseButton(obj);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(SaveUserWiseButton)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetTotalReportList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTotalReportList()
        {
            try
            {
                var response = await _globalMaster.masterSetup.GetTotalReportList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetTotalReportList)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetReportUserDropdown")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetReportUserDropdown(int compid)
        {
            try
            {
                var response = await _globalMaster.masterSetup.GetReportUserDropdown(compid);
                if(response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.dataTable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetReportUserDropdown)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetPermissionReportByUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPermissionReportByUser(int compid, string userName)
        {
            try
            {
                var response = await _globalMaster.masterSetup.GetPermissionReportByUser(compid, userName);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.dataTable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetPermissionReportByUser)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("Save_Pay_ReportPermission")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Save_Pay_ReportPermission(ReportPermissionPayload obj)
        {
            try
            {
                var response = await _globalMaster.masterSetup.Save_Pay_ReportPermission(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(Save_Pay_ReportPermission)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("AddOrEditMenuGroup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditMenuGroup(MenuGroupPayload obj)
        {
            try
            {
                var response = await _globalMaster.masterSetup.AddOrEditMenuGroup(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditMenuGroup)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetAllMenuGroup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllMenuGroup()
        {
            try
            {
                var response = await _globalMaster.masterSetup.GetAllMenuGroup();
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.dataTable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetAllMenuGroup)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetMenuGroupSelect")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMenuGroupSelect(int groupId)
        {
            try
            {
                var response = await _globalMaster.masterSetup.GetMenuGroupSelect(groupId);
                return Ok(new { response.IsSuccess, response.dataTable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetMenuGroupSelect)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("AddOrEditNewUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrEditNewUser(NewUserCreate obj)
        {
            try
            {
                var response = await _globalMaster.masterSetup.AddOrEditNewUser(obj);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.Message });
                }
                return BadRequest(new { response.IsSuccess, response.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(AddOrEditNewUser)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetNewUserAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNewUserAll()
        {
            try
            {
                var response = await _globalMaster.masterSetup.GetNewUserAll();
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.dataTable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetNewUserAll)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetUserByCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserByCompany(int compid)
        {
            try
            {
                var response = await _globalMaster.masterSetup.GetUserByCompany(compid);
                if (response.IsSuccess)
                {
                    return Ok(new { response.IsSuccess, response.dataTable });
                }
                return NotFound(new { response.IsSuccess, response.dataTable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetUserByCompany)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetMenuUserSelect")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMenuUserSelect(string userName)
        {
            try
            {
                var response = await _globalMaster.masterSetup.GetMenuUserSelect(userName);
                return Ok(new { response.IsSuccess, response.dataTable });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetMenuUserSelect)}");
                return CustomResult("Internal Server Error, Please Try Again Later!", HttpStatusCode.InternalServerError);
            }
        }
        #endregion
    }
}