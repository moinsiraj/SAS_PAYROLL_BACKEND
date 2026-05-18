using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace BLL.Interfaces.Manager.Employees
{
    public interface IEmployeesManager : ICommonManager<Employee_DbModel>
    {
        void QR_Generator(string[] QrInfo, string[] FilePath);
        Task<string[]> Employee_active_status(int Emp_serial, int Active_status, string active_Date, string Inactive_Date, string in_active_Resion,string userName);
        Task<ReturnObject> GetActiveInactiveHistory(int empSerial);
        Task<DataSet> textfile(int nempSerial, int Emp_ID, int ngroupid, int ncompid, DateTime nDate, string ntime);
        Task<DataSet> employee_ShiftChange(int? Compid = null, int? Department = null, int? section = null, int? 
            Building = null, int? Floor = null, int? Line = null, int? Shift = null, int? Grade = null, int? 
            salcat = null, int? Newshift = null, DateTime? EffectDate = null, string user = null);
        Task<DataSet> get();
        Task<DataSet> employee_Info(int emp_serial);
        Task<DataSet> employee_Info();
        Task<DataSet> GetCompanyName();
        Task<DataSet> GetdivisionName();
        Task<DataSet> GetDistict(int di_id);
        Task<DataSet> GetThana(int th_id);
        Task<DataSet> Getpostoffice(int ThanaID);
        Task<DataSet> Getvillage(int thanaID);
        Task<DataSet> Companywiseemployee(int compid);
        Task<DataSet> Employee_ID(int compid);
        Task<DataSet> singleEmployee(int comId, int EmpId);
        Task<DataSet> formeternity(int compid);
        Task<DataSet> Proxmity_ID(int compid);
        Task<DataSet> GetDesignation();
        Task<DataSet> Department(int compid);
        Task<DataSet> GetSection(int Department);
        Task<DataSet> Getbuilding(int compid);
        Task<DataSet> GetFloor(int compid, int Building);
        Task<DataSet> GetLine(int compid, int Building, int Floor);
        Task<DataSet> Getsalarycategory(int compID);
        Task<DataSet> GetShift(int compid);
        Task<DataSet> GetGrade();
        Task<DataSet> GetBank();       
        Task<DataTable> GetCompanyAddress(int compID);
        Task<bool> EmpLeaveGenerate(DgPayEmployee obj);

        //New Version
        Task<bool> UploadEmpInfoImage(IFormFile fileEmp, IFormFile fileNominee, IFormFile fileSign, IFormFile fileEmpNid, IFormFile fileNomineeNid, string compid, int empid);
        ReturnObject viewEmpDocFile(int compid, int empid, string vFrom);
        Task<ReturnObject> UploadEmployeeImageFolder(IFormFile zipFile, int compid, string imgType);
        Task<ReturnObject> AddOrEditEmpPersonalInfo(Employee_Personal_Info obj);
        Task<Employee_Personal_Info_View> GetEmpPersonalInfo_ByEmpNo(int compid, int emp_no);
        Task<string[]> UpdateEmpOfficialInfo(Employee_Office_Info obj);
        Task<Employee_Office_Info_View> GetEmpOfficialInfo_ByEmpSerial(int emp_serial);
        Task<bool> SaveEmployeeAttenData(int emp_serial);
        Task<string> UpdateEmpNomineeInfo(Employee_Nominee_Info obj);
        Task<Employee_Nominee_Info_View> GetEmpNomineeInfo_ByEmpSerial(int emp_serial);
        Task<string> UpdateEmpEducationInfo(Employee_Education_Info obj);
        Task<Employee_Education_Info_View> GetEmpEducationInfo_ByEmpNo(int compid, int emp_no);
        Task<string> addOrEditEmpExperince_Info(Employee_Experince_Info obj);
        Task<Employee_Experince_Info_View> GetEmpExperinceInfo_ByEmpNo(int compid, int emp_no);
        Task<Employee_FullName_View> GetEmployeeFullName(int compid, int emp_no);
        Task<DataTable> GetAllDegree_Title();
        Task<DataTable> GetAllExam_Board();
        Task<DataTable> GetEmployeeNo(int compid);
        Task<ReturnObject> GetEmployeeInfoFilter(EmployeeFilterPayload obj);
        Task<ReturnObject> GetEmployeeInfoLeaveFilter(EmployeeLeaveFilterPayload obj);
        byte[] Dg_EmployeeSingleDetailedInformation(int emp_serial,string userName,string reportType);
        Task<ReturnObject> GetCompanyWiseEmployee_ForPMS(int companyID);
    }
}