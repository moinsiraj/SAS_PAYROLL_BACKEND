using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace BOL.Models
{
    public class Floor_DbModel
    {
        [Key]
        public int nFloor { get; set; }
        public string cFloor_Descriptin { get; set; } = null;
        public string cFloor_Descriptin_bangla { get; set; } = null;
        public string cEntUser { get; set; } = null;
        public DateTime? dEntdt { get; set; } = null;
        public int? BuildingUID { get; set; } = null;
        public string BuildingUIName { get; set; } = null;
        public int? CompanyID { get; set; } = null;
        public string CompanyName { get; set; } = null;
        public string cFloor_DescriptinBD { get; set; } = null;
        public string cUpdateBy { get; set; } = null;
        public DateTime? cUpdatedt { get; set; } = null;
    }
    public class FloorPayload
    {
        [Required(ErrorMessage = "Floor ID Is Required !!")]
        public int floorID { get; set; }
        [Required(ErrorMessage = "Company ID Is Required !!")]
        public int companyID { get; set; }
        [Required(ErrorMessage = "Building ID Is Required !!")]
        public int buildingID { get; set; }
        [Required(ErrorMessage = "Floor Name Is Required !!")]
        public string floorName { get; set; }
        [Required(ErrorMessage = "Floor Name Bangla Is Required !!")]
        public string floorNameBD { get; set; }
        [Required(ErrorMessage = "Create User Name Is Required !!")]
        public string userName { get; set; }

        public static Floor_DbModel PayloadToFloorDb_Obj(FloorPayload obj, Floor_DbModel uObj=null)
        {
            var dbModel = new Floor_DbModel
            {
                nFloor = obj.floorID,
                cFloor_Descriptin = obj.floorName,
                cFloor_Descriptin_bangla = obj.floorNameBD,
                CompanyID = obj.companyID,
                BuildingUID = obj.buildingID,
                cFloor_DescriptinBD = "-",
                cEntUser = uObj == null ? obj.userName : uObj.cEntUser,
                dEntdt = uObj == null ? DateTime.Now : uObj.dEntdt,
                cUpdateBy = uObj == null ? null : obj.userName,
                cUpdatedt = uObj == null ? null : DateTime.Now,
            };
            return dbModel;
        }
    }
    public class DgPayFloor
    {
        public int NFloor { get; set; }
        public string CFloorDescriptin { get; set; } = null;
        public string CFloorDescriptinBangla { get; set; } = null;
        public string CEntUser { get; set; } = null;
        public DateTime? DEntdt { get; set; } = null;
        public int? BuildingUid { get; set; } = null;
        public string BuildingUiname { get; set; } = null;
        public int? CompanyId { get; set; } = null;
        public string CompanyName { get; set; } = null;
        public string CFloorDescriptinBd { get; set; } = null;

        public static Floor_DbModel CustomToDbModel(DgPayFloor obj)
        {
            try
            {
                var DbModel = new Floor_DbModel
                {
                    nFloor = obj.NFloor,
                    cFloor_Descriptin = obj.CFloorDescriptin,
                    cFloor_Descriptin_bangla = obj.CFloorDescriptinBangla,
                    cEntUser = obj.CEntUser,
                    dEntdt = obj.DEntdt,
                    BuildingUID = obj.BuildingUid,
                    BuildingUIName = obj.BuildingUiname,
                    CompanyID = obj.CompanyId,
                    CompanyName = obj.CompanyName,
                    cFloor_DescriptinBD = obj.CFloorDescriptinBd
                };
                return DbModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }

        public static DgPayFloor DbToCustomModel(Floor_DbModel obj)
        {
            try
            {
                var CustomModel = new DgPayFloor
                {
                    NFloor = obj.nFloor,
                    CFloorDescriptin = obj.cFloor_Descriptin,
                    CFloorDescriptinBangla = obj.cFloor_Descriptin_bangla,
                    CEntUser = obj.cEntUser,
                    DEntdt = obj.dEntdt,
                    BuildingUid = obj.BuildingUID,
                    BuildingUiname = obj.BuildingUIName,
                    CompanyId = obj.CompanyID,
                    CompanyName = obj.CompanyName,
                    CFloorDescriptinBd = obj.cFloor_DescriptinBD
                };
                return CustomModel;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return null;
        }
    }
}