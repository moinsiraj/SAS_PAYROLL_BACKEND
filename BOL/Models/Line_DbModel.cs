using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class Line_DbModel
    {
        [Key]
        public int Line_Code { get; set; }
        public string Line_No { get; set; } = null;
        public string Line_Name_Bangla { get; set; } = null;
        public int? block_id { get; set; } = null;
        public string block_name { get; set; } = null;
        public int? CompanyID { get; set; } = null;
        public string CompanyName { get; set; } = null;
        public int? DepartmentID { get; set; } = null;
        public string DepartmentName { get; set; } = null;
        public int? SectionID { get; set; } = null;
        public string SectionName { get; set; } = null;
        public int? BuildingUnitID { get; set; } = null;
        public string BuildingUnitName { get; set; } = null;
        public int? FloorID { get; set; } = null;
        public string FloorName { get; set; } = null;
        public int? nMachineOp { get; set; } = null;
        public int? nHelper { get; set; } = null;
        public DateTime? Createdt { get; set; } = null;
        public string CreatedBy { get; set; } = null;
        public string ProdPlan { get; set; } = null;
        public int? Line_ID { get; set; } = null;
        public int? TargetInHrs { get; set; } = null;
        public string Line_No_Bangla { get; set; } = null;
        public string UpdateBy { get; set; } = null;
        public DateTime? Updatedt { get; set; } = null;
    }
    public class LinePayload
    {
        [Required(ErrorMessage = "Line ID Is Required !!")]
        public int LineID { get; set; }
        [Required(ErrorMessage = "Company ID Is Required !!")]
        public int companyID { get; set; }
        [Required(ErrorMessage = "Department ID Is Required !!")]
        public int departmentID { get; set; }
        [Required(ErrorMessage = "Section ID Is Required !!")]
        public int sectionID { get; set; }
        [Required(ErrorMessage = "Building ID Is Required !!")]
        public int buildingID { get; set; }
        [Required(ErrorMessage = "Floor ID Is Required !!")]
        public int floorID { get; set; }
        [Required(ErrorMessage = "Line Name Is Required !!")]
        public string lineName { get; set; }
        [Required(ErrorMessage = "Line Name Bangla Is Required !!")]
        public string lineNameBD { get; set; }
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public int block_id { get; set; }
        public string block_name { get; set; }
        public string userName { get; set; }

        public static Line_DbModel PayloadToLineDb_Obj(LinePayload obj, Line_DbModel uObj=null)
        {
            var dbModel = new Line_DbModel
            {
                Line_Code = obj.LineID,
                block_id = obj.block_id,
                block_name = obj.block_name,
                CompanyID = obj.companyID,
                DepartmentID = obj.departmentID,
                SectionID = obj.sectionID,
                BuildingUnitID = obj.buildingID,
                FloorID = obj.floorID,
                Line_No = obj.lineName,
                Line_Name_Bangla = obj.lineNameBD,
                Line_No_Bangla = "-",
                ProdPlan = uObj == null ? null : uObj.ProdPlan,
                CreatedBy = uObj == null ? obj.userName : uObj.CreatedBy,
                Createdt = uObj == null ? DateTime.Now : uObj.Createdt,
                UpdateBy = uObj == null ? null : obj.userName,
                Updatedt = uObj == null ? null : DateTime.Now,
            };
            return dbModel;
        }
    }
    public class LineList
    {
        public int LineID { get; set; }
        public string LineName { get; set; }
        public string LineNameBangla { get; set; }
        public int block_id { get; set; }
        public string block_name { get; set; }
        public int? CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int? DepartmentID { get; set;}
        public string DepartmentName { get; set; }
        public int? SectionID { get; set; }
        public string SectionName { get; set; }
        public int? BuildingID { get; set; }
        public string BuildingName { get; set; }
        public int? FloorID { get; set; }
        public string FloorName { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class DgPayLine
    {
        public int LineCode { get; set; }
        public string LineNo { get; set; } = null;
        public string LineNameBangla { get; set; } = null;
        public int? CompanyId { get; set; } = null;
        public string CompanyName { get; set; } = null;
        public int? DepartmentId { get; set; } = null;
        public string DepartmentName { get; set; } = null;
        public int? SectionId { get; set; } = null;
        public string SectionName { get; set; } = null;
        public int? BuildingUnitId { get; set; } = null;
        public string BuildingUnitName { get; set; } = null;
        public int? FloorId { get; set; } = null;
        public string FloorName { get; set; } = null;
        public int? NMachineOp { get; set; } = null;
        public int? NHelper { get; set; } = null;
        public DateTime? Createdt { get; set; } = null;
        public string CreatedBy { get; set; } = null;
        public string ProdPlan { get; set; } = null;
        public int? LineId { get; set; } = null;
        public int? TargetInHrs { get; set; } = null;
        public string LineNoBangla { get; set; } = null;

        public static Line_DbModel CustomToDbModel(DgPayLine obj)
        {
            try
            {
                var dbModel = new Line_DbModel
                {
                    Line_Code = obj.LineCode,
                    Line_No = obj.LineNo,
                    Line_Name_Bangla = obj.LineNameBangla,
                    CompanyID = obj.CompanyId,
                    CompanyName = obj.CompanyName,
                    DepartmentID = obj.DepartmentId,
                    DepartmentName = obj.DepartmentName,
                    SectionID = obj.SectionId,
                    SectionName = obj.SectionName,
                    BuildingUnitID = obj.BuildingUnitId,
                    BuildingUnitName = obj.BuildingUnitName,
                    FloorID = obj.FloorId,
                    FloorName = obj.FloorName,
                    nMachineOp = obj.NMachineOp,
                    nHelper = obj.NHelper,
                    Createdt = obj.Createdt,
                    CreatedBy = obj.CreatedBy,
                    ProdPlan = obj.ProdPlan,
                    Line_ID = obj.LineId,
                    TargetInHrs = obj.TargetInHrs,
                    Line_No_Bangla = obj.LineNoBangla
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayLine DbToCustomModel(Line_DbModel obj)
        {
            try
            {
                var customModel = new DgPayLine
                {
                    LineCode = obj.Line_Code,
                    LineNo = obj.Line_No,
                    LineNameBangla = obj.Line_Name_Bangla,
                    CompanyId = obj.CompanyID,
                    CompanyName = obj.CompanyName,
                    DepartmentId = obj.DepartmentID,
                    DepartmentName = obj.DepartmentName,
                    SectionId = obj.SectionID,
                    SectionName = obj.SectionName,
                    BuildingUnitId = obj.BuildingUnitID,
                    BuildingUnitName = obj.BuildingUnitName,
                    FloorId = obj.FloorID,
                    FloorName = obj.FloorName,
                    NMachineOp = obj.nMachineOp,
                    NHelper = obj.nHelper,
                    Createdt = obj.Createdt,
                    CreatedBy = obj.CreatedBy,
                    ProdPlan = obj.ProdPlan,
                    LineId = obj.Line_ID,
                    TargetInHrs = obj.TargetInHrs,
                    LineNoBangla = obj.Line_No_Bangla
                };
                return customModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
    }
    public class CompanyDbModel
    {
        [Key]
        public int nCompanyID { get; set; }
        public string cCmpName { get; set; }
        public string cAdd1 { get; set; }
        public string cAdd2 { get; set; }
        public string cEntUSer { get; set; }
        public DateTime? dEntdt { get; set; }
        public bool? Display_AS_Header { get; set; }
        public string lgo { get; set; }
        public bool? cmpt { get; set; }
        public bool? com_group { get; set; }
        public DateTime? Expdt { get; set; }
        public string nComNameBd { get; set; }
        public string nAddBd { get; set; }
        public string nAddBdt { get; set; }
        public bool? Active_Com { get; set; }
        public string cCompShortNm { get; set; }
        public int? comp_Grp_id { get; set; }
    }
    public class LineBlockPayload
    {
        public int blk_id { get; set; }
        public int blk_compid { get; set; }
        public int blk_floorid { get; set; }
        public string blk_name { get; set; }
        public string blk_name_bd { get; set; }
        public string userName { get; set; }
    }

    public class LineBgtPayload
    {
        public int companyId { get; set; }
        public string companyName { get; set; }
        public string userName { get; set; }
        public List<LineBgtInfo> lineBgtInfos { get; set; }
    }
    public class LineBgtInfo
    {
        public int lineId { get; set; }
        public string lineName { get; set; }
        public int bgtQty { get; set; }
    }
}