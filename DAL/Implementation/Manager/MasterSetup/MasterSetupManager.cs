using BLL.Interfaces.Manager.MasterSetup;
using BLL.Utility;
using BOL.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace DAL.Implementation.Manager.MasterSetup
{
    public class MasterSetupManager : IMasterSetupManager
    {
        private readonly Dg_Common _dgCommon;
        private readonly SqlConnection _payCon;
        private readonly SqlConnection _specFoCon;
        private readonly string _key;
        public MasterSetupManager(Dg_Common dgCommon, IConfiguration config)
        {
            _dgCommon = dgCommon;
            _payCon = new SqlConnection(Getway.Dg_Payroll);
            _specFoCon = new SqlConnection(Getway.SpecFoCon);
            _key = config["DgEncryptKey:Key"];
        }

        public async Task<List<MasterMenuTree>> GetMasterMenuList()
        {
            var mTree = new List<MasterMenuTree>();
            var dtAppName = await _dgCommon.get_InformationDataTableAsync("select Apps_Name from SpecFo.dbo.Smt_Userpermission group by Apps_Name", _payCon);
            for (int i = 0; i < dtAppName.Rows.Count; i++)
            {
                string appName = dtAppName.Rows[i]["Apps_Name"].ToString();
                var dtAppChaid = await _dgCommon.get_InformationDataTableAsync("select Module_Name from SpecFo.dbo.Smt_Userpermission where Apps_Name='"+ appName + "' group by Module_Name", _payCon);
                mTree.Add(new MasterMenuTree
                {
                    id = i + 1,
                    moduleName = appName,
                    mainMenu = dtAppChaid.AsEnumerable().Select(row => new MenuModuleChild
                    {
                        mainMenuName = row["Module_Name"].ToString(),
                        subMenu = _dgCommon.get_InformationDataTable("select Form_Name,Url from SpecFo.dbo.Smt_Userpermission where Module_Name='"+ row["Module_Name"].ToString() + "' and Apps_Name='"+ appName.Trim() + "'", _payCon).AsEnumerable().Select(row => new MainMenuChild
                        {
                            subMenuName = row["Form_Name"].ToString(),
                            url = row["Url"].ToString()
                        }).ToList(),
                    }).ToList(),
                });
            }
            return mTree;
        }
        public async Task<List<MasterButtonTree>> GetMasterButtonList(string userName)
        {
            var mTree = new List<MasterButtonTree>();
            var dtParent = await _dgCommon.get_InformationDataTableAsync("Sp_Smt_GetFormNameForBtnPermission '" + userName + "'", _specFoCon);

            for (int i = 0; i < dtParent.Rows.Count; i++)
            {
                string parentName = dtParent.Rows[i]["Form_Name"].ToString();
                var dtChild = await _dgCommon.get_InformationDataTableAsync("Sp_Smt_GetUserPermitedFormForButton '" + userName + "','" + parentName + "'", _specFoCon);

                var childMenus = dtChild.AsEnumerable().Select(row => new MasterMainButtonMenu
                {
                    menuName = row["Form_Name"].ToString(),
                    url = row["Url"].ToString(),
                    menuDesc = row["Form_Desc"].ToString(),
                    masterButtons = _dgCommon.get_InformationDataTable("Sp_Smt_Get_UserPermitterBtn '" + row["Url"].ToString() + "','" + userName + "'", _specFoCon)
                        .AsEnumerable()
                        .Where(btn => !string.IsNullOrEmpty(btn["Btn_Text"].ToString()))
                        .Select(btn => new MasterButton
                        {
                            buttonID = btn["Btn_Name"].ToString(),
                            buttonText = btn["Btn_Text"].ToString(),
                            isPermission = Convert.ToBoolean(btn["Permission"])
                        }).ToList(),
                }).Where(menu => menu.masterButtons.Any()).ToList();

                if (childMenus.Any())
                {
                    mTree.Add(new MasterButtonTree
                    {
                        id = i + 1,
                        parentMenu = parentName,
                        childMenu = childMenus,
                    });
                }
            }
            return mTree;

            /*var mTree = new List<MasterButtonTree>();
            var dtParent = await _dgCommon.get_InformationDataTableAsync("Sp_Smt_GetFormNameForBtnPermission '" + userName + "'", _specFoCon);
            for (int i = 0; i < dtParent.Rows.Count; i++)
            {
                string parentName = dtParent.Rows[i]["Form_Name"].ToString();
                var dtChild = await _dgCommon.get_InformationDataTableAsync("Sp_Smt_GetUserPermitedFormForButton '"+ userName + "','"+ parentName + "'", _specFoCon);
                mTree.Add(new MasterButtonTree
                {
                    id = i + 1,
                    parentMenu = parentName,
                    childMenu = dtChild.AsEnumerable().Select(row => new MasterMainButtonMenu
                    {
                        menuName = row["Form_Name"].ToString(),
                        url = row["Url"].ToString(),
                        menuDesc = row["Form_Desc"].ToString(),
                        masterButtons = _dgCommon.get_InformationDataTable("Sp_Smt_Get_UserPermitterBtn '" + row["Url"].ToString() + "','"+ userName + "'", _specFoCon).AsEnumerable().Select(btn => new MasterButton
                        {
                            buttonID = btn["Btn_Name"].ToString(),
                            buttonText = btn["Btn_Text"].ToString(),
                            isPermission = Convert.ToBoolean(btn["Permission"])
                        }).ToList(),
                    }).ToList(),
                });
            }
            var filteredTree = mTree.Where(x => x.childMenu.Any(y => y.masterButtons.Any(z => !string.IsNullOrEmpty(z.buttonText)))).ToList();
            return filteredTree;*/

        }

        public async Task<ReturnObject> SaveUserWiseButton(ButtonPermissionSavePayload obj)
        {
            var result = new ReturnObject();
            await _dgCommon.saveChangesAsync("delete SpecFo.dbo.tst_permitterbtn where UserName='" + obj.userName + "'", _payCon);
            if (obj.saveChild.Count > 0)
            {
                obj.saveChild.ToList().ForEach(row =>
                {
                    _dgCommon.saveChanges("Dg_Pay_SaveUserWiseButtonPermission '" + obj.userName + "','" + row.url + "','" + row.buttonID + "','" + row.parentMenu + "'", _payCon);
                });
                result.IsSuccess = true;
                result.Message = "Save Successfully !!";
                return result;
            }
            result.IsSuccess = true;
            result.Message = "Save Successfully !!";
            return result;
        }
        public async Task<List<TreeListReport>> GetTotalReportList()
        {
            List<TreeListReport> listObjTree = new List<TreeListReport>();
            List<Dg_ReportPermission> listObj = new List<Dg_ReportPermission>();
            var rptListType = await _dgCommon.get_InformationDataTableAsync("select distinct rep_cat from dg_pay_totalReportList where rep_IsShowReport=1", _payCon);
            for (int i = 0; i < rptListType.Rows.Count; i++)
            {
                string reportCat = rptListType.Rows[i]["rep_cat"].ToString();
                var childData = await _dgCommon.get_InformationDataTableAsync("select * from dg_pay_totalReportList where rep_cat='" + reportCat + "' and rep_IsShowReport=1", _payCon);
                foreach (DataRow row in childData.Rows)
                {
                    listObj.Add(new Dg_ReportPermission
                    {
                        report_id = Convert.ToInt32(row["rep_id"]),
                        rep_show_Sl = Convert.ToInt32(row["rep_show_Sl"]),
                        report_type = row["rep_cat"].ToString(),
                        report_name = row["rep_name"].ToString(),
                        report_url = row["rep_url"].ToString(),
                        report_for = row["rep_for"].ToString(),
                        report_IsExcel = Convert.ToBoolean(row["rep_IsExcel"])
                    });
                }
                listObjTree.Add(new TreeListReport
                {
                    id = i + 1,
                    title = rptListType.Rows[i]["rep_cat"].ToString(),
                    reports = listObj.Where(x => x.report_type == rptListType.Rows[i]["rep_cat"].ToString()).ToList(),
                });
            }
            return listObjTree;
        }
        public async Task<ReturnObject> GetReportUserDropdown(int compid)
        {
            var result = new ReturnObject();
            var data = await _dgCommon.get_InformationDataTableAsync("select ca_serial,ca_accessuser from dg_pay_companyaccess where permission=1 and ca_compid=" + compid, _payCon);
            if (data.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = data;
                return result;
            }
            return result;
        }
        public async Task<ReturnObject> GetPermissionReportByUser(int compid, string userName)
        {
            var result = new ReturnObject();
            var data = await _dgCommon.get_InformationDataTableAsync("select report_type,report_name,report_url,report_IsExcel from Dg_Pay_Report_permission where report_compid=" + compid + " and report_user='" + userName + "'", _payCon);
            if(data.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = data;
                return result;
            }
            return result;
        }
        public async Task<ReturnObject> Save_Pay_ReportPermission(ReportPermissionPayload obj)
        {
            var result = new ReturnObject();
            if (obj.userPermissionStatus == "U")
            {
                await _dgCommon.saveChangesAsync("delete Dg_Pay_Report_permission where report_user='" + obj.report_user + "' and report_compid=" + obj.report_compid + "", _payCon);
                foreach (var item in obj.permissionForm)
                {
                    await _dgCommon.saveChangesAsync("Dg_Pay_ReportPermission_Save '" + item.report_type + "'," + item.report_serial + ",'" + item.report_name + "','" + item.report_url + "','" + obj.report_user + "','" + obj.userPermissionStatus + "'," + obj.report_compid + "," + obj.report_permission_code + ",'" + obj.permission_by + "','0','" + item.report_IsExcel + "'", _payCon);
                }
                result.IsSuccess = true;
                result.Message = "User Wise Save Successfully !!";
                return result;
            }
            await _dgCommon.saveChangesAsync("UPDATE Tbl_User SET report_permission_code=" + obj.report_permission_code + " WHERE FullName='" + obj.report_user + "'", _payCon);
            result.IsSuccess = true;
            result.Message = "Group Wise Save Successfully !!";
            return result;
        }
        public async Task<ReturnObject> AddOrEditMenuGroup(MenuGroupPayload obj)
        {
            var result = new ReturnObject();
            bool flag;
            if (obj.mainMenus.Count > 0)
            {
                if (obj.mGroupId == 0)
                {
                    var dtIsGroup = await _dgCommon.get_InformationDataTableAsync("select cGrpDescription from Smt_UserGroups where cGrpDescription='" + obj.mGroupName + "'", _specFoCon);
                    bool isGroup = dtIsGroup.Rows.Count > 0 ? true : false;
                    if (!isGroup)
                    {
                        flag = await _dgCommon.saveChangesAsync("Sp_Smt_Usergroup_Save '" + obj.mGroupName + "','" + obj.userName + "'", _specFoCon);
                        if (flag)
                        {
                            result.IsSuccess = true;
                            result.Message = "Saved Successfully !!";
                            var dtGroupId = await _dgCommon.get_InformationDataTableAsync("select nUgroup from Smt_UserGroups where cGrpDescription='" + obj.mGroupName + "'", _specFoCon);
                            int groupid = Convert.ToInt32(!string.IsNullOrEmpty(dtGroupId.Rows[0]["nUgroup"].ToString()) ? dtGroupId.Rows[0]["nUgroup"] : 0);
                            this.GroupMenuSave(obj.mainMenus, groupid, obj.userName,"G");
                            return result;
                        }
                        result.Message = "Save Fail !!";
                        return result;
                    }
                    result.Message = "Group Name Already Exists !!";
                    return result;
                }
                flag = await _dgCommon.saveChangesAsync("Sp_Smt_Usergroup_Update " + obj.mGroupId + ",'" + obj.mGroupName + "','" + obj.userName + "'", _specFoCon);
                if (flag)
                {
                    result.IsSuccess = true;
                    result.Message = "Update Successfully !!";
                    this.GroupMenuSave(obj.mainMenus, obj.mGroupId, obj.userName,"G");
                    return result;
                }
                result.Message = "Update Fail !!";
                return result;
            }
            result.Message = "You Can Not Chaek Any Menu !!";
            return result;
        }
        public async Task<ReturnObject> GetAllMenuGroup()
        {
            var result = new ReturnObject();
            var dtGroup = await _dgCommon.get_InformationDataTableAsync("select nUgroup,cGrpDescription,cEntUser,dEntdt from Smt_UserGroups order by nUgroup desc", _specFoCon);
            if (dtGroup.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.dataTable = dtGroup;
            }
            return result;
        }
        public async Task<ReturnObject> GetMenuGroupSelect(int groupId)
        {
            var result = new ReturnObject();
            if (groupId != 0)
            {
                var dtGrpSelect = await _dgCommon.get_InformationDataTableAsync("select Form_Name as subMenuName,Url from Smt_UserPermittedform where nUgroup=" + groupId + " and Url<>''", _specFoCon);
                result.IsSuccess = true;
                result.dataTable = dtGrpSelect;
                return result;
            }
            return result;
        }
        public async Task<ReturnObject> AddOrEditNewUser(NewUserCreate obj)
        {
            var result = new ReturnObject();
            bool flag;
            if(obj.Permission_status == "U" && obj.mainMenus.Count <= 0)
            {
                result.Message = "You Can Not Check Any Menu !!";
                return result;
            }
            //obj.nUgroup = obj.nUgroup == 0 ? 99 : obj.nUgroup;
            string enctPass = _dgCommon.EncryptText(_key, obj.cPassWord);
            var payload = NewUserCreate.GetUserDbPayload(obj, enctPass);
            if (obj.userSerial == 0)
            {
                var dtIsUser = await _dgCommon.get_InformationDataTableAsync("select cUserName from Smt_Users where cUserName='" + obj.cUserName + "'", _specFoCon);
                bool isUser = dtIsUser.Rows.Count > 0 ? true : false;
                if (!isUser)
                {
                    //flag = _dgCommon.saveChanges("Sp_Smt_Users_Save", _specFoCon, obj);
                    flag = _dgCommon.saveChanges("Sp_Smt_Users_Save", _specFoCon, payload);
                    if (flag)
                    {
                        result.IsSuccess = true;
                        result.Message = "Saved Successfully !!";
                        if(obj.Permission_status == "U")
                        {
                            GroupMenuSave(obj.mainMenus, obj.nUgroup, obj.cUserName, obj.Permission_status);
                        }
                        return result;
                    }
                    result.Message = "Save Fail !!";
                    return result;
                }
                result.Message = "User Already Exists !!";
                return result;
            }
            //flag = await _dgCommon.saveChangesAsync("Sp_Smt_Users_Save", _specFoCon, obj);
            flag = await _dgCommon.saveChangesAsync("Sp_Smt_Users_Save", _specFoCon, payload);
            if (flag)
            {
                result.IsSuccess = true;
                result.Message = "Update Successfully !!";
                if (obj.Permission_status == "U")
                {
                    GroupMenuSave(obj.mainMenus, obj.nUgroup, obj.cUserName, obj.Permission_status);
                }
                return result;
            }
            result.Message = "Update Fail !!";
            return result;
        }
        public async Task<ReturnObject> GetNewUserAll()
        {
            var result = new ReturnObject();
            var dtNuser = await _dgCommon.get_InformationDataTableAsync("Sp_Smt_GetNewUserInfo", _specFoCon);
            if (dtNuser.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.Message = "Data Loaded !!";
                result.dataTable = dtNuser;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetUserByCompany(int compid)
        {
            var result = new ReturnObject();
            var dtNuser = await _dgCommon.get_InformationDataTableAsync("select nUserID as ca_serial,cUserName as ca_accessuser from Smt_Users where nCompanyID="+ compid, _specFoCon);
            if (dtNuser.Rows.Count > 0)
            {
                result.IsSuccess = true;
                result.Message = "Data Loaded !!";
                result.dataTable = dtNuser;
            }
            result.Message = "No Data Available !!";
            return result;
        }
        public async Task<ReturnObject> GetMenuUserSelect(string userName)
        {
            var result = new ReturnObject();
            var dtpStatus = await _dgCommon.get_InformationDataTableAsync("select Permission_status from Smt_Users where cUserName='" + userName + "'", _specFoCon);
            string permissionStatus = !string.IsNullOrEmpty(dtpStatus.Rows[0]["Permission_status"].ToString()) ? dtpStatus.Rows[0]["Permission_status"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(userName) && permissionStatus == "U")
            {
                var dtUsrSelect = await _dgCommon.get_InformationDataTableAsync("select Form_Name as subMenuName,Url from Smt_UserPermittedform where User_ID='" + userName + "' and Permission_Status='" + permissionStatus + "' and Url<>''", _specFoCon);
                if (dtUsrSelect.Rows.Count > 0)
                {
                    result.IsSuccess = true;
                    result.dataTable = dtUsrSelect;
                    return result;
                }                
            }
            return result;
        }
        private void GroupMenuSave(List<MainMenuChild> Menus, int groupid,string userName,string parmissionSts)
        {
            string query = parmissionSts != "U" ? "Sp_Smt_UserPermittedform_Delete " + groupid + "" : 
                "delete from Smt_UserPermittedform where User_ID='" + userName + "' and Permission_Status='"+ parmissionSts + "'";
            _dgCommon.saveChanges(query, _specFoCon);
            Menus.ToList().ForEach(menu =>
            {
                _dgCommon.saveChanges("Sp_Smt_UserPermittedform_Save " + groupid + ",'" + menu.subMenuName + "','" + menu.url + "','"+ parmissionSts + "','" + userName + "'", _specFoCon);
            });
        }



        /*public async Task<List<object>> GetPermitedMenuList(List<MenuList> obj)
        {
            var lstMenu = new List<object>();
            string UserName = obj[0].UserName.Trim();
            var dt = await _sqlCommon.get_InformationDataTableAsync(string.Format("select Permission_Status,nUgroup from Smt_Users where cUserName='{0}'", UserName), _connection);
            if (dt.Rows.Count > 0)
            {
                int uGroupID = int.Parse(dt.Rows[0]["nUgroup"].ToString());
                string pStatus = dt.Rows[0]["Permission_status"].ToString().Trim();
                if (uGroupID != 1)
                {
                    if (pStatus == "U")
                    {
                        for (int iac = 0; iac < obj.Count; iac++)
                        {
                            string frmName = obj[iac].MenuText;
                            var dtgtfrmU = await _sqlCommon.get_InformationDataTableAsync(string.Format("select Form_Name from Smt_UserPermittedform where User_ID='{0}' and Form_Name='{1}' and Permission_Status='U'",UserName,frmName), _connection);
                            if (dtgtfrmU.Rows.Count < 1)
                            {                                
                                lstMenu.Add(new
                                {
                                    MenuText = obj[iac].MenuText
                                });
                            }
                        }
                    }
                    else
                    {
                        for (int iac = 0; iac < obj.Count; iac++)
                        {
                            string frmName = obj[iac].MenuText;
                            DataTable dtgtfrmU = await _sqlCommon.get_InformationDataTableAsync(string.Format("select Form_Name from Smt_UserPermittedform where nUgroup={0} and Form_Name='{1}'", uGroupID, frmName), _connection);
                            if (dtgtfrmU.Rows.Count < 1)
                            {
                                lstMenu.Add(new
                                {
                                    MenuText = obj[iac].MenuText
                                });
                            }
                        }
                    }
                }
            }           
            return lstMenu;
        }*/

    }
}
