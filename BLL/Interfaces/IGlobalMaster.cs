using BLL.Interfaces.Manager.Activedate;
using BLL.Interfaces.Manager.Allowance;
using BLL.Interfaces.Manager.Allowancesde;
using BLL.Interfaces.Manager.AllProcess;
using BLL.Interfaces.Manager.AnnualLeaveAllucation;
using BLL.Interfaces.Manager.App;
using BLL.Interfaces.Manager.AttbonusRule;
using BLL.Interfaces.Manager.AttBouns;
using BLL.Interfaces.Manager.AttcoveringDay;
using BLL.Interfaces.Manager.BankInfos;
using BLL.Interfaces.Manager.BuildingUnit;
using BLL.Interfaces.Manager.ButtonPermission;
using BLL.Interfaces.Manager.Companies;
using BLL.Interfaces.Manager.CompanyAccess;
using BLL.Interfaces.Manager.Deduction;
using BLL.Interfaces.Manager.Departments;
using BLL.Interfaces.Manager.Designations;
using BLL.Interfaces.Manager.DgPayDeducations;
using BLL.Interfaces.Manager.Districts;
using BLL.Interfaces.Manager.Divisions;
using BLL.Interfaces.Manager.ECards;
using BLL.Interfaces.Manager.Education;
using BLL.Interfaces.Manager.EidBonusSetups;
using BLL.Interfaces.Manager.Employees;
using BLL.Interfaces.Manager.EmployeeTransfer;
using BLL.Interfaces.Manager.ExcelToDatabase;
using BLL.Interfaces.Manager.Experince;
using BLL.Interfaces.Manager.Floors;
using BLL.Interfaces.Manager.Grades;
using BLL.Interfaces.Manager.IncrementInfoes;
using BLL.Interfaces.Manager.LeaveInfor;
using BLL.Interfaces.Manager.Leavetransactions;
using BLL.Interfaces.Manager.LevTransFdtTdt;
using BLL.Interfaces.Manager.Lines;
using BLL.Interfaces.Manager.LoanCategories;
using BLL.Interfaces.Manager.Loans;
using BLL.Interfaces.Manager.Login;
using BLL.Interfaces.Manager.LunchInoutSetups;
using BLL.Interfaces.Manager.ManualAttendances;
using BLL.Interfaces.Manager.MasterSetup;
using BLL.Interfaces.Manager.MaternityLeaveInfo;
using BLL.Interfaces.Manager.Menurights;
using BLL.Interfaces.Manager.PostOffice;
using BLL.Interfaces.Manager.QuestPdf;
using BLL.Interfaces.Manager.Report;
using BLL.Interfaces.Manager.ReportFilter;
using BLL.Interfaces.Manager.ReportPermission;
using BLL.Interfaces.Manager.SalaryAdvanceLogs;
using BLL.Interfaces.Manager.SalaryCategories;
using BLL.Interfaces.Manager.Sections;
using BLL.Interfaces.Manager.ShiftChanges;
using BLL.Interfaces.Manager.Shifts;
using BLL.Interfaces.Manager.Specialholidays;
using BLL.Interfaces.Manager.Stampcharges;
using BLL.Interfaces.Manager.Tax;
using BLL.Interfaces.Manager.Thanas;
using BLL.Interfaces.Manager.Tiffinbillrules;
using BLL.Interfaces.Manager.TiffinNightBillDesignation;
using BLL.Interfaces.Manager.UploadAttendances;
using BLL.Interfaces.Manager.Users;
using BLL.Interfaces.Manager.Village;
using BLL.Interfaces.Manager.WeeklyHolidaySetup;

namespace BLL.Interfaces
{
    public interface IGlobalMaster
    {
        IActiveDateManager activedate { get; }
        IAllowanceManager allowance { get; }
        IAllowancesdeManager allowancesde { get; }
        IAllProcessManager allProcess { get; }
        IAnnualLeaveAllucationManager annualLeaveAllo { get; }
        IAppManager appManager { get; }
        IAttBounsManager attBonus { get; }
        IAttbonusRuleManager attbonusRule { get; }
        IAttCoveringDayManager attcoveringDay { get; }
        IBankInfosManager bankInfos { get; }
        IBuildingUnitManager buildingUnit { get; }
        IButtonPermissionManager buttonPermission { get; }
        ICompaniesManager companies { get; }
        ICompanyAccessManager companyAccess { get; }
        IDeductionManager deduction { get; }
        IDepartmentsManager departments { get; }
        IDesignationsManager designations { get; }
        IDgPayDeducationsManager dgPayDeducations { get; }
        IDistrictsManager districts { get; }
        IDivisionsManager divisions { get; }
        IECardsManager eCards { get; }
        IEidBonusSetupsManager eidBonusSetups { get; }
        IIncrementInfoesManager incrementInfoes { get; }
        IEmployeesManager employees { get; }
        IEmployeeTransferManager employeeTransfer {  get; }
        IEducationManager education {  get; }
        IExperinceManager experince { get; }
        IFloorsManager floors { get; }
        IGradesManager grades { get; }
        ILeaveInforManager leaveInfor { get; }
        ILeavetransactionsManager leavetransactions { get; }
        ILevTransFdtTdtManager levTransFdtTdt { get; }
        ILinesManager lines { get; }
        ILoanCategoriesManager loanCategories { get; }
        ILoansManager loans { get; }
        ILoginManager login { get; }
        ILunchInoutSetupsManager lunchInoutSetups { get; }
        IManualAttendancesManager manualAttendances { get; }
        IMaternityLeaveInfoManager maternityLeaveInfo { get; }
        IMenurightsManager menurights { get; }
        IPostOfficeManager postOffice { get; }
        IReportManager reportManager { get; }
        IReportFilterManager reportFilterManager { get; }
        IReportPermissionManager reportPermission {  get; }
        ISalaryAdvanceLogsManager salaryAdvanceLogs { get; }
        ISalaryCategoriesManager salaryCategories { get; }
        ISectionsManager sections { get; }
        IShiftChangesManager shiftChanges { get; }
        IShiftsManager shifts { get; }
        ISpecialholidaysManager specialholidays { get; }
        IStampchargesManager stampcharges { get; }
        IThanasManager thanas { get; }
        ITiffinbillrulesManager tiffinbillrules { get; }
        IUploadAttendancesManager uploadAttendances { get; }
        IUsersManager users { get; }
        IVillageManager village { get; }
        IWeeklyHolidaySetupManager weeklyHolidaySetup { get; }
        ITiffinNightBillDesignationManager tiffinNightDescWise { get; }
        IMasterSetupManager masterSetup { get; }
        IEmployeeTaxManager employeeTax { get; }

        IQuestPdfManager questPdfManager { get; }

        //Test Excel
        IExcelToDatabaseManager excelText { get; }
    }
}