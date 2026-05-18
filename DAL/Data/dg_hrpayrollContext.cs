using BOL.Models;
using Microsoft.EntityFrameworkCore;
namespace DAL.Data
{
    public class dg_hrpayrollContext : DbContext
    {
        public dg_hrpayrollContext(DbContextOptions<dg_hrpayrollContext> options): base(options)
        {
        }
        public DbSet<Activedate_DbModel> dg_pay_activedate { get; set; }
        public DbSet<Allowance_DbModel> dg_pay_allowances { get; set; }
        public DbSet<Allowancesde_DbModel> dg_pay_allowancesdes { get; set; }
        public DbSet<AllProcess_DbModel> All_process { get; set; }
        public DbSet<AnnualLeaveAllucation_DbModel> dg_annual_leave_allucation { get; set; }
        public DbSet<AttBonus_DbModel> dg_pay_attbonus { get; set; }
        public DbSet<AttbonusRule_DbModel> dg_pay_attbonus_rules { get; set; }
        public DbSet<AttcoveringDay_DbModel> dg_pay_attcovering_days { get; set; }
        public DbSet<BankInfos_DbModel> dg_pay_BankInfo { get; set; }       
        public DbSet<ButtonPermission_DbModel> dg_button_permissions { get; set; }
        public DbSet<Company_DbModel> dg_pay_company { get; set; }
        public DbSet<CompanyAccess_DbModel> dg_pay_companyaccess { get; set; }
        public DbSet<Deduction_DbModel> dg_pay_deductionsdes { get; set; }
        public DbSet<Department_DbModel> dg_pay_Department { get; set; }
        public DbSet<Designation_DbModel> dg_pay_designation { get; set; }
        public DbSet<DesignationCat_DbModel> dg_pay_designation_category { get; set; }
        public DbSet<DgPayDeducation_DbModel> dg_pay_deducations { get; set; }
        public DbSet<District_DbModel> dg_pay_district { get; set; }
        public DbSet<Division_DbModel> dg_pay_division { get; set; }
        public DbSet<ECard_DbModel> E_card { get; set; }
        public DbSet<EidBonusSetup_DbModel> dg_pay_eid_bonus_setup { get; set; }
        public DbSet<IncrementInfo_DbModel> dg_emp_increment_info { get; set; }
        public DbSet<Employee_DbModel> dg_pay_Employee { get; set; }
        public DbSet<EmpEducation> dg_Pay_EmployeeEducation { get; set; }
        public DbSet<EmpExperince> dg_Pay_Emp_Experience_info { get;set; }        
        public DbSet<Grade_DbModel> dg_pay_grade { get; set; }
        public DbSet<LeaveInfor_DbModel> dg_pay_leaveInfor { get; set; }
        public DbSet<Leavetransaction_DbModel> dg_pay_leavetransaction { get; set; }
        public DbSet<LevTransFdateTdate_DbModel> dg_pay_leave_info_forfromdatetodate { get; set; }        
        public DbSet<LoanCategory_DbModel> dg_pay_loancategory { get; set; }
        public DbSet<Loan_DbModel> dg_pay_loan { get; set; }
        public DbSet<LunchInoutSetup_DbModel> dg_lunch_inout_setup { get; set; }
        public DbSet<Attendance_DbModel> dg_pay_attendance { get; set; }
        public DbSet<MaternityLeaveInfo_DbModel> dg_Pay_Maternity_leave_info { get; set; }
        public DbSet<Menuright_DbModel> dg_menurights { get; set; }
        public DbSet<PostOffice_DbModel> dg_pay_postoffice { get; set; }
        public DbSet<SalaryAdvanceLog_DbModel> dg_Pay_Salary_Advance_Log { get; set; }
        public DbSet<Salarycategory_DbModel> dg_pay_salarycategory { get; set; }        
        public DbSet<ShiftChange_DbModel> dg_pay_shift_change { get; set; }
        public DbSet<Shift_DbModel> dg_pay_shift { get; set; }
        public DbSet<Specialholiday_DbModel> dg_pay_specialholidays { get; set; }
        public DbSet<Stampcharge_DbModel> dg_stampcharge { get; set; }
        public DbSet<Thana_DbModel> dg_pay_thana { get; set; }
        public DbSet<Tiffinbillrule_DbModel> dg_pay_tiffinbillrules { get; set; }
        public DbSet<User_DbModel> Tbl_User { get; set; }
        public DbSet<Village_DbModel> dg_pay_village { get; set; }
        //public DbSet<ReportPermission_DbModel> Dg_Pay_Report_permission { get; set; }
        public DbSet<BankBranch> dg_pay_BankInfo_branch { get; set; }
    }

    public class dg_SpecFoContext : DbContext
    {
        public dg_SpecFoContext(DbContextOptions<dg_SpecFoContext> options):base(options)
        {
        }
        public DbSet<Department_DbModel> Smt_Department { get; set; }
        public DbSet<Section_DbModel> Smt_Section { get; set; }
        public DbSet<BuildingUnit_DbModel> Smt_BuildingUnit { get; set; }
        public DbSet<Floor_DbModel> Smt_Floor { get; set; }
        public DbSet<Line_DbModel> Smt_Line { get; set; }
        public DbSet<CompanyDbModel> Smt_Company { get; set; }
    }
}