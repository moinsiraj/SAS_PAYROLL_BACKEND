using BLL.Interfaces;
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
using BLL.Utility;
using BOL.Models.Hubs;
using DAL.Data;
using DAL.Implementation.Manager.Activedate;
using DAL.Implementation.Manager.Allowance;
using DAL.Implementation.Manager.Allowancesde;
using DAL.Implementation.Manager.AllProcess;
using DAL.Implementation.Manager.AnnualLeaveAllucation;
using DAL.Implementation.Manager.App;
using DAL.Implementation.Manager.AttbonusRule;
using DAL.Implementation.Manager.AttBouns;
using DAL.Implementation.Manager.AttcoveringDay;
using DAL.Implementation.Manager.BankInfos;
using DAL.Implementation.Manager.BuildingUnit;
using DAL.Implementation.Manager.ButtonPermission;
using DAL.Implementation.Manager.Companies;
using DAL.Implementation.Manager.CompanyAccess;
using DAL.Implementation.Manager.Deduction;
using DAL.Implementation.Manager.Departments;
using DAL.Implementation.Manager.Designations;
using DAL.Implementation.Manager.DgPayDeducations;
using DAL.Implementation.Manager.Districts;
using DAL.Implementation.Manager.Divisions;
using DAL.Implementation.Manager.ECards;
using DAL.Implementation.Manager.Education;
using DAL.Implementation.Manager.EidBonusSetups;
using DAL.Implementation.Manager.Employees;
using DAL.Implementation.Manager.EmployeeTransfer;
using DAL.Implementation.Manager.ExcelToDatabase;
using DAL.Implementation.Manager.Experince;
using DAL.Implementation.Manager.Floors;
using DAL.Implementation.Manager.Grades;
using DAL.Implementation.Manager.IncrementInfoes;
using DAL.Implementation.Manager.LeaveInfor;
using DAL.Implementation.Manager.Leavetransactions;
using DAL.Implementation.Manager.LevTransFdtTdt;
using DAL.Implementation.Manager.Lines;
using DAL.Implementation.Manager.LoanCategories;
using DAL.Implementation.Manager.Loans;
using DAL.Implementation.Manager.Login;
using DAL.Implementation.Manager.LunchInoutSetups;
using DAL.Implementation.Manager.ManualAttendances;
using DAL.Implementation.Manager.MasterSetup;
using DAL.Implementation.Manager.MaternityLeaveInfo;
using DAL.Implementation.Manager.Menurights;
using DAL.Implementation.Manager.PostOffice;
using DAL.Implementation.Manager.QuestPdf;
using DAL.Implementation.Manager.Report;
using DAL.Implementation.Manager.ReportFilter;
using DAL.Implementation.Manager.ReportPermission;
using DAL.Implementation.Manager.SalaryAdvanceLogs;
using DAL.Implementation.Manager.SalaryCategories;
using DAL.Implementation.Manager.Sections;
using DAL.Implementation.Manager.ShiftChanges;
using DAL.Implementation.Manager.Shifts;
using DAL.Implementation.Manager.Specialholidays;
using DAL.Implementation.Manager.Stampcharges;
using DAL.Implementation.Manager.Tax;
using DAL.Implementation.Manager.Thanas;
using DAL.Implementation.Manager.Tiffinbillrules;
using DAL.Implementation.Manager.TiffinNightBillDesignation;
using DAL.Implementation.Manager.UploadAttendances;
using DAL.Implementation.Manager.Users;
using DAL.Implementation.Manager.Village;
using DAL.Implementation.Manager.WeeklyHolidaySetup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace DAL.Implementation
{
    public class GlobalMaster : IGlobalMaster
    {
        private dg_hrpayrollContext _db;
        private dg_SpecFoContext _dbSpecFo;
        private Dg_Common _dgCommon;
        private readonly IWebHostEnvironment _webHost;
        private readonly IHubContext<CustomHubs> _hubContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        public GlobalMaster(dg_hrpayrollContext db, dg_SpecFoContext dbSpecFo, Dg_Common dgCommon, IWebHostEnvironment webHost, IHubContext<CustomHubs> hubContext, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _db = db;
            _dbSpecFo = dbSpecFo;
            _dgCommon = dgCommon;
            _webHost = webHost;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            _config = config;

            activedate = new ActiveDateManager(_db);
            allowance = new AllowanceManager(_db, _dgCommon);
            allowancesde = new AllowancesdeManager(_db);
            allProcess = new AllProcessManager(_db, _dgCommon, _hubContext);
            annualLeaveAllo = new AnnualLeaveAllucationManager(_db, _dgCommon);
            appManager = new AppManager(_db, _dgCommon);
            attbonusRule = new AttbonusRuleManager(_db);
            attBonus = new AttBounsManager(_db);
            attcoveringDay = new AttCoveringDayManager(_db,_dgCommon);
            bankInfos = new BankInfosManager(_db);
            buildingUnit = new BuildingUnitManager(_dbSpecFo,_db,_dgCommon);
            buttonPermission = new ButtonPermissionManager(_db, _dgCommon);
            companies = new CompaniesManager(_db);
            companyAccess = new CompanyAccessManager(_db, _dgCommon);
            deduction = new DeductionManager(_db);
            departments = new DepartmentsManager(_dbSpecFo,_db,_dgCommon);
            designations = new DesignationsManager(_db,_dgCommon);
            dgPayDeducations = new DgPayDeducationsManager(_db, _dgCommon);
            districts = new DistrictsManager(_db);
            divisions = new DivisionsManager(_db,_dgCommon);
            eCards = new ECardsManager(_db, _dgCommon);
            eidBonusSetups = new EidBonusSetupsManager(_db,_dgCommon);
            incrementInfoes = new IncrementInfoesManager(_db, _dgCommon);
            employees = new EmployeesManager(_db, _dgCommon, _webHost);
            employeeTransfer = new EmployeeTransferManager(_dgCommon, _webHost);
            education = new EducationManager(_db, _dgCommon);
            experince = new ExperinceManager(_db, _dgCommon);
            floors = new FloorsManager(_dbSpecFo,_db,_dgCommon);
            grades = new GradesManager(_db);
            leaveInfor = new LeaveInforManager(_db, _dgCommon);
            leavetransactions = new LeavetransactionsManager(_db,_dgCommon);
            levTransFdtTdt = new LevTransFdtTdtManager(_db);
            lines = new LinesManager(_dbSpecFo,_db,_dgCommon);
            loanCategories = new LoanCategoriesManager(_db);
            loans = new LoansManager(_db);
            login = new loginManager(_db,_dgCommon, _config);
            lunchInoutSetups = new LunchInoutSetupsManager(_db,_dgCommon);
            manualAttendances = new ManualAttendancesManager(_db,_dgCommon);
            maternityLeaveInfo = new MaternityLeaveInfoManager(_db,_dgCommon);
            menurights = new MenurightsManager(_db, _dgCommon);
            postOffice = new PostOfficeManager(_db);
            reportManager = new ReportManager(_dgCommon,_webHost);
            reportFilterManager = new ReportFilterManager(_dgCommon);
            reportPermission = new ReportParmissionManager(_dgCommon);
            salaryAdvanceLogs = new SalaryAdvanceLogsManager(_db,_dgCommon);
            salaryCategories = new SalaryCategoriesManager(_db);
            sections = new SectionsManager(_dbSpecFo,_db,_dgCommon);
            shiftChanges = new ShiftChangesManager(_db,_dgCommon);
            shifts = new ShiftsManager(_db);
            specialholidays = new SpecialholidaysManager(_db,_dgCommon);
            stampcharges = new StampchargesManager(_db);
            thanas = new ThanasManager(_db);
            tiffinbillrules = new TiffinbillrulesManager(_db,_dgCommon);
            uploadAttendances = new UploadAttendancesManager(_dgCommon, _webHost, _hubContext);
            users = new UsersManager(_db);
            village = new VillageManager(_db);
            weeklyHolidaySetup = new WeeklyHolidaySetupManager(_dgCommon);
            tiffinNightDescWise = new TiffinNightBillDesignationManager(_dgCommon);
            masterSetup = new MasterSetupManager(_dgCommon, _config);
            employeeTax = new EmployeeTaxManager(_db, _dgCommon, _webHost, _httpContextAccessor);

            questPdfManager = new QuestPdfManager(_dgCommon);

            //Test Excel
            excelText = new ExcelToDatabaseManager(_dgCommon);
        }
        public IActiveDateManager activedate { get; private set; }
        public IAllowanceManager allowance { get; private set; }
        public IAllowancesdeManager allowancesde { get; private set; }
        public IAllProcessManager allProcess { get; private set; }
        public IAnnualLeaveAllucationManager annualLeaveAllo { get; private set; }
        public IAppManager appManager { get; private set; }
        public IAttBounsManager attBonus { get; private set; }
        public IAttbonusRuleManager attbonusRule { get; private set; }
        public IAttCoveringDayManager attcoveringDay { get; private set; }
        public IBankInfosManager bankInfos { get; private set; }
        public IBuildingUnitManager buildingUnit { get; private set; }
        public IButtonPermissionManager buttonPermission { get; private set; }
        public ICompaniesManager companies { get; private set; }
        public ICompanyAccessManager companyAccess { get; private set; }
        public IDeductionManager deduction { get; private set; }
        public IDepartmentsManager departments { get; private set; }
        public IDesignationsManager designations { get; private set; }
        public IDgPayDeducationsManager dgPayDeducations { get; private set; }
        public IDistrictsManager districts { get; private set; }
        public IDivisionsManager divisions { get; private set; }
        public IECardsManager eCards { get; private set; }
        public IEidBonusSetupsManager eidBonusSetups { get; private set; }
        public IIncrementInfoesManager incrementInfoes { get; private set; }
        public IEmployeesManager employees { get; private set; }
        public IEmployeeTransferManager employeeTransfer { get; private set; }
        public IEducationManager education { get; private set; }
        public IExperinceManager experince { get; private set; }
        public IFloorsManager floors { get; private set; }
        public IGradesManager grades { get; private set; }
        public ILeaveInforManager leaveInfor { get; private set; }
        public ILeavetransactionsManager leavetransactions { get; private set; }
        public ILevTransFdtTdtManager levTransFdtTdt { get; private set; }
        public ILinesManager lines { get; private set; }
        public ILoanCategoriesManager loanCategories { get; private set; }
        public ILoansManager loans { get; private set; }
        public ILoginManager login { get; private set; }
        public ILunchInoutSetupsManager lunchInoutSetups { get; private set; }
        public IManualAttendancesManager manualAttendances { get; private set; }
        public IMaternityLeaveInfoManager maternityLeaveInfo { get; private set; }
        public IMenurightsManager menurights { get; private set; }
        public IPostOfficeManager postOffice { get; private set; }
        public IReportManager reportManager { get; private set; }
        public IReportFilterManager reportFilterManager { get; private set; }
        public IReportPermissionManager reportPermission { get; private set; }
        public ISalaryAdvanceLogsManager salaryAdvanceLogs { get; private set; }
        public ISalaryCategoriesManager salaryCategories { get; private set; }
        public ISectionsManager sections { get; private set; }
        public IShiftChangesManager shiftChanges { get; private set; }
        public IShiftsManager shifts { get; private set; }
        public ISpecialholidaysManager specialholidays { get; private set; }
        public IStampchargesManager stampcharges { get; private set; }
        public IThanasManager thanas { get; private set; }
        public ITiffinbillrulesManager tiffinbillrules { get; private set; }
        public IUploadAttendancesManager uploadAttendances { get; private set; }
        public IUsersManager users { get; private set; }
        public IVillageManager village { get; private set; }
        public IWeeklyHolidaySetupManager weeklyHolidaySetup { get; private set; }
        public ITiffinNightBillDesignationManager tiffinNightDescWise { get; private set; }
        public IMasterSetupManager masterSetup { get; private set; }
        public IEmployeeTaxManager employeeTax { get; private set; }

        public IQuestPdfManager questPdfManager { get; private set; }

        //Test Excel
        public IExcelToDatabaseManager excelText { get; private set; }
    }
}
