using BLL.Interfaces.Manager.TiffinNightBillDesignation;
using BLL.Utility;
using BOL.Models;
using System.ComponentModel.Design;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.TiffinNightBillDesignation
{
    public class TiffinNightBillDesignationManager : ITiffinNightBillDesignationManager
    {
        private readonly Dg_Common _dg_Common;
        private readonly SqlConnection _payCon;
        public TiffinNightBillDesignationManager(Dg_Common dg_Common)
        {
            _dg_Common = dg_Common;
            _payCon = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<ReturnObject> GetAllDesignationByComp_ForNightAndTiffinBill(int companyID)
        {
            var result = new ReturnObject();
            try
            {
                var dtDesc = await _dg_Common.get_InformationDataTableAsync(string.Format("Dg_Pay_designationwiseTiffinAndNightBill_List {0}", companyID), _payCon);
                if (dtDesc.Rows.Count > 0)
                {
                    result.IsSuccess = true;
                    result.Message = "Data Loaded !!";
                    result.dataTable = dtDesc;
                }
                else
                {
                    result.Message = "Data Not Loaded !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
        public async Task<ReturnObject> AddOrEditDescWiseTiffinAndNightBill(DesignationWiseTiffinAndNightBill obj)
        {
            var result = new ReturnObject();
            try
            {
                obj.DesiInfos.ToList().ForEach(DesiInfo =>
                {
                    bool isSave = _dg_Common.saveChanges("Dgh_Pay_AddOrEditDesigWiseNightTiffinBill " + obj.compnayID + ",'" + obj.companyName + "'," + DesiInfo.designationID + ",'" + DesiInfo.designationName + "'," + DesiInfo.tiffinAmount + "," + DesiInfo.nightAmount + ",'" + obj.userName + "'", _payCon);
                });
                result.IsSuccess = true;
                result.Message = "Submitted Successfully !!";
                result.dataTable = await _dg_Common.get_InformationDataTableAsync(string.Format("Dg_Pay_designationwiseTiffinAndNightBill_List {0}", obj.compnayID), _payCon);
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
        public async Task<ReturnObject> GetAllDesignationForWH(int companyID)
        {
            var result = new ReturnObject();
            var dtDesig = await _dg_Common.get_InformationDataTableAsync(string.Format("Dg_Pay_designationwise_HolidayBill_List {0}", companyID), _payCon);
            if (dtDesig.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dtDesig;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> AddOrEditDesignationForWH(DesignationWiseWH obj)
        {
            var result = new ReturnObject();
            if (obj.DesiInfos.Count > 0)
            {
                obj.DesiInfos.ToList().ForEach(item =>
                {
                    _dg_Common.saveChanges(string.Format("Dgh_Pay_AddOrEditDesigWiseHolidayBill {0},'{1}',{2},'{3}',{4},'{5}'", obj.compnayID, obj.companyName, item.designationID, item.designationName, item.whAmount, obj.userName), _payCon);
                });
                result.IsSuccess = true;
                result.Message = "Submitted Successfully !!";
                result.dataTable = await _dg_Common.get_InformationDataTableAsync(string.Format("Dg_Pay_designationwise_HolidayBill_List {0}", obj.compnayID), _payCon);
                return result;
            }
            result.Message = "Designation Empty !!";
            return result;
        }
    }
}
