using System.ComponentModel.DataAnnotations;

namespace BOL.Models
{
    public class BuildingUnit_DbModel
    {
        [Key]
        public int Unit_Code { get; set; }
        public string Unit_Name { get; set; } = null;
        public int? Company_ID { get; set; } = null;
        public string Company_Name { get; set; } = null;
        public string CreatedBy { get; set; } = null;
        public DateTime? Createddt { get; set; } = null;
        public string UpdatedBy { get; set; } = null;
        public DateTime? Updateddt { get; set; } = null;
    }
    public class BuildingUnitPayload
    {
        [Required(ErrorMessage = "Unit ID Is Required !!")]
        public int unitCode { get; set; }
        [Required(ErrorMessage = "Company ID Is Required !!")]
        public int companyID { get; set; }
        [Required(ErrorMessage = "Company Name Is Required !!")]
        public string companyName { get; set; }
        [Required(ErrorMessage = "Unit Name Is Required !!")]
        public string unitName { get; set; }
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public string userName { get; set; }

        public static BuildingUnit_DbModel PayloadToBuildingUnitDb_Obj(BuildingUnitPayload obj, BuildingUnit_DbModel uObj=null)
        {
            var dbModel = new BuildingUnit_DbModel
            {
                Unit_Code = obj.unitCode,
                Unit_Name = obj.unitName,
                Company_ID = obj.companyID,
                Company_Name = obj.companyName,
                CreatedBy = uObj == null ? obj.userName : uObj.CreatedBy,
                Createddt = uObj == null ? DateTime.Now : uObj.Createddt,
                UpdatedBy = uObj == null ? null : obj.userName,
                Updateddt = uObj == null ? null : DateTime.Now,
            };
            return dbModel;
        }
    }
    public partial class DgPayBuildingUnit
    {
        public int UnitCode { get; set; }
        public string UnitName { get; set; } = null;
        public int? CompanyId { get; set; } = null;
        public string CompanyName { get; set; } = null;
        public string CreatedBy { get; set; } = null;
        public DateTime? Createddt { get; set; } = null;

        public static BuildingUnit_DbModel CustomToDbModel(DgPayBuildingUnit obj)
        {
            try
            {
                var dbModel = new BuildingUnit_DbModel
                {
                    Unit_Code = obj.UnitCode,
                    Unit_Name = obj.UnitName,
                    Company_ID = obj.CompanyId,
                    Company_Name = obj.CompanyName,
                    CreatedBy = obj.CreatedBy,
                    Createddt = obj.Createddt,
                };
                return dbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
        public static DgPayBuildingUnit DbToCustomModel(BuildingUnit_DbModel obj)
        {
            try
            {
                var customModel = new DgPayBuildingUnit
                {
                    UnitCode = obj.Unit_Code,
                    UnitName = obj.Unit_Name,
                    CompanyId = obj.Company_ID,
                    CompanyName = obj.Company_Name,
                    CreatedBy = obj.CreatedBy,
                    Createddt = obj.Createddt
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
}
