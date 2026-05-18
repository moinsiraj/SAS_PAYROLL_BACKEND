using BLL.Interfaces.Manager.Designations;
using BLL.Utility;
using BOL.Models;
using DAL.Data;
using DAL.Implementation.Repository.Designations;
using EF.Core.Repository.Manager;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.Design;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.Designations
{
    public class DesignationsManager : CommonManager<Designation_DbModel>,IDesignationsManager
    {
        private readonly dg_hrpayrollContext _context;
        private readonly Dg_Common _dg_Common;
        private readonly SqlConnection _payCon;
        public DesignationsManager(dg_hrpayrollContext context, Dg_Common dg_Common) : base(new DesignationsRepository(context))
        {
            _context = context;
            _dg_Common = dg_Common;
            _payCon = new SqlConnection(Getway.Dg_Payroll);
        }

        public async Task<ReturnObject> GetDesignationCatList()
        {
            var result = new ReturnObject();
            var dt = await _dg_Common.get_InformationDataTableAsync("select desig_cat_id,desig_cat_name from dg_pay_designation_category order by desig_cat_name", _payCon);
            if (dt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dt;
                return result;
            }
            result.Message = "Data Not Found !!";
            return result;
        }
        public async Task<ReturnObject> AddOrEditDesignation(DesignationPayload obj)
        {
            var result = new ReturnObject();
            try
            {
                var dbData = new Designation_DbModel();
                if (obj.designationID == 0)
                {
                    var isExists = GetFirstOrDefault(x => x.dec_name == obj.designationName && x.dec_name_bangla == obj.designationNameBN);
                    if (isExists == null)
                    {
                        dbData = DesignationPayload.PayloadToDesignationDb_Obj(obj);
                        bool isSave = await AddAsync(dbData);
                        if (isSave)
                        {
                            result.IsSuccess = true;
                            result.Message = "Designation Save Successfully !!";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Can Not Saved Designation !!";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Designation Already Exists !!";
                    }
                }
                else
                {
                    var dbObj = GetFirstOrDefault(x => x.dec_id == obj.designationID);
                    dbData = DesignationPayload.PayloadToDesignationDb_Obj(obj, dbObj);
                    bool isUpdate = await UpdateAsync(dbData);
                    if (isUpdate)
                    {
                        result.IsSuccess = true;
                        result.Message = "Designation Update Successfully !!";
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Can Not Update Designation !!";
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.IsSuccess = false;
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }
        public async Task<ReturnObject<DesignationJoinList>> GetAllDesignation()
        {
            var result = new ReturnObject<DesignationJoinList>();
            try
            {
                var designationLs = await GetAllAsync();
                var userLs = await _context.Tbl_User.ToListAsync();
                var descCat = await _context.dg_pay_designation_category.ToListAsync();
                var mainList = from a in designationLs
                join b in userLs on (!string.IsNullOrEmpty(a?.dec_user) ? a?.dec_user.Trim():string.Empty) equals b?.FullName.Trim() into userFullName
                from a1 in userFullName.DefaultIfEmpty()
                join c in userLs on a.dec_updateBy equals c.FullName.Trim() into userFullName2
                from a2 in userFullName2.DefaultIfEmpty()
                join desig in descCat on a.dec_cat_id equals desig.desig_cat_id into desigNm
                from fDesig in desigNm.DefaultIfEmpty()
                select (new DesignationJoinList
                {
                    dec_id = a.dec_id,
                    dec_groupid = (a?.dec_groupid),
                    dec_name = a?.dec_name,
                    dec_name_bangla = a?.dec_name_bangla,
                    desig_cat_id = ((int)(a?.dec_cat_id)),
                    desig_cat_name = fDesig?.desig_cat_name.Trim(),
                    dec_user = a1?.UserFullname.Trim(),
                    dec_udate = (a?.dec_udate),
                    dec_updateBy = a2?.UserFullname.Trim(),
                    dec_update = (a?.dec_update)
                });
                if (mainList.ToList() != null)
                {
                    result.IsSuccess = true;
                    result.Message = "Data Loaded Successfully !!";
                    result.ListData = mainList.ToList();
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Data Loaded Fail !!";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                result.IsSuccess = false;
                result.Message = "Something Went Wrong !!";
            }
            return result;
        }

        public async Task<ReturnObject> GetDesigWiseBgtList(int companyId)
        {
            var result = new ReturnObject();
            var dtbgtLs = await _dg_Common.get_InformationDataTableAsync("dg_pay_designationwiseBgtList", _payCon);
            if (dtbgtLs.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.Message = "Data Loaded !!";
                result.dataTable = dtbgtLs;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> AddOrEditDesignationBgt(DesignationBgtPayload obj)
        {
            var result = new ReturnObject();
            obj.designationBgtInfos.ToList().ForEach(info =>
            {
                _dg_Common.saveChanges("Dg_Pay_AddOrEditDesigWiseBgt " + obj.companyId + ",'" + obj.companyName + "'," + info.designationId + ",'" + info.designationName + "'," + info.bgtQty + ",'" + obj.userName + "'", _payCon);
            });
            result.IsSuccess = true;
            result.Message = "Submitted Successfully !!";
            result.dataTable = await _dg_Common.get_InformationDataTableAsync("dg_pay_designationwiseBgtList", _payCon);
            return result;
        }
        public async Task<ReturnObject> GetDesignationWiseBudgetAll(int companyID)
        {
            var result = new ReturnObject();
            var dtBgt = await _dg_Common.get_InformationDataTableAsync("dg_pay_Employee_BudgetList " + companyID, _payCon);
            if(dtBgt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.Message = "Data Loaded !!";
                result.dataTable = dtBgt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetSelectedDesignationWiseBudget(int bgtId)
        {
            var result = new ReturnObject();
            var dtBgt = await _dg_Common.get_InformationDataTableAsync("dg_pay_Employee_BudgetSelectList " + bgtId, _payCon);
            if (dtBgt.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.Message = "Data Loaded !!";
                result.dataTable = dtBgt;
                return result;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> AddOrEditDesignationWiseBudget(DesignationBgt_Payload obj)
        {
            var result = new ReturnObject();
            var dtBgtMsg = await _dg_Common.get_InformationDataTableAsync("dg_pay_EmployeeBgt_AddOrEdit " + obj.bgt_id + "," + obj.companyId + "," + obj.departmentId + "," + obj.sectionId + "," + obj.buildingId + "," + obj.floorId + "," + obj.lineId + "," + obj.designationId + "," + obj.manpower + "," + obj.bgtAmount + ",'" + obj.userName + "'", _payCon);
            if (dtBgtMsg.Rows[0]["msgType"].ToString() == "success")
            {
                result.IsSuccess = true;
                result.Message = dtBgtMsg.Rows[0]["msg"].ToString();
                result.dataTable = await _dg_Common.get_InformationDataTableAsync("dg_pay_Employee_BudgetList " + obj.companyId, _payCon);
                return result;
            }
            result.Message = dtBgtMsg.Rows[0]["msg"].ToString();
            return result;
        }
    }
}