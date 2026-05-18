using BLL.Interfaces.Manager.ExcelToDatabase;
using BLL.Utility;
using BOL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Implementation.Manager.ExcelToDatabase
{
    public class ExcelToDatabaseManager : IExcelToDatabaseManager
    {
        private readonly Dg_Common _dg_Common;
        private readonly SqlConnection _connection;
        public ExcelToDatabaseManager(Dg_Common dg_Common)
        {
            _connection = new SqlConnection(Getway.Dg_Payroll);
            _dg_Common = dg_Common;

        }

        public async Task<List<ExcelToDb>> GetExcelCountryData([FromForm] UploadExcelFile model)
        {
            var list = new List<ExcelToDb>();
            var worksheet = await _dg_Common.GetExcelWorkSheet(model.excelFile,0);
            var rowCount = worksheet.Dimension.Rows;
            for (int row = 2; row <= rowCount; row++)
            {
                list.Add(new ExcelToDb
                {
                    countryID = Convert.ToInt32(worksheet.Cells[row, 1].Value),
                    countryName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                });
            }
            return list;
        }
        public async Task<string> SaveCountry([FromForm] UploadExcelFile model)
        {
            string message = string.Empty;
            try
            {
                var worksheet = await _dg_Common.GetExcelWorkSheet(model.excelFile,0);
                var rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++)
                {
                    string countryName = worksheet.Cells[row, 1].Value.ToString().Trim();
                    SqlCommand cmd = new SqlCommand("insert into tbl_country(compId,cnt_name) values("+ model.CompanyID + ",'"+ countryName + "')",_connection);
                    await _connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    await _connection.CloseAsync();
                }
                message = "Save Successfully !!";
            }
            catch (Exception ex)
            {
                ex.ToString();
                message = "Something was worng !!";
            }
            return message;
        }
        public async Task<DataTable> GetExcelUploadType()
        {
            var data = await _dg_Common.get_InformationDataTableAsync("SELECT * FROM dg_pay_ExcelUploadTypes", _connection);
            return data;
        }
        public async Task<List<object>> SaveUploadExcelToDB([FromForm] UploadExcelFile model)
        {
            DataTable dataTable = new DataTable();
            var message = new List<object>();
            string gRemarks = string.Empty;
            try
            {
                if (model.actionTypeID == 1)
                {
                    var worksheet = await _dg_Common.GetExcelWorkSheet(model.excelFile,0);
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Text))
                        {
                            int emp_no = Convert.ToInt32(worksheet.Cells[row, 1].Value != null ? worksheet.Cells[row, 1].Value.ToString().Trim() : string.Empty);
                            string d_description = worksheet.Cells[row, 2].Value != null ? worksheet.Cells[row, 2].Value.ToString().Trim() : string.Empty;
                            decimal d_value = Convert.ToDecimal(worksheet.Cells[row, 3].Value != null ? worksheet.Cells[row, 3].Value.ToString().Trim() : string.Empty);
                            string d_date = Convert.ToDateTime(worksheet.Cells[row, 4].Value.ToString().Trim()).ToString("yyyy-MM-dd");
                            string eRemarks = worksheet.Cells[row, 5].Value != null ? worksheet.Cells[row, 5].Value.ToString().Trim() : string.Empty;
                            gRemarks = string.IsNullOrEmpty(model.remarks) ? eRemarks : model.remarks;
                            dataTable = await _dg_Common.get_InformationDataTableAsync("Dg_Pay_InsertDeductionExcel " + model.CompanyID + "," + emp_no + ",'" + d_date + "','" + d_description + "'," + d_value + ",'" + gRemarks + "','" + model.userName + "'", _connection);
                            message.Add(new
                            {
                                messageType = dataTable.Rows[0]["messageType"].ToString().Trim(),
                                showMessage = dataTable.Rows[0]["showMessage"].ToString().Trim(),
                            });
                        }                       
                    }
                }
                else if(model.actionTypeID == 2)
                {
                    var worksheet = await _dg_Common.GetExcelWorkSheet(model.excelFile, 0);
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Text))
                        {
                            int emp_no = Convert.ToInt32(worksheet.Cells[row, 1].Value != null ? worksheet.Cells[row, 1].Value.ToString().Trim() : string.Empty);
                            string al_description = worksheet.Cells[row, 2].Value != null ? worksheet.Cells[row, 2].Value.ToString().Trim() : string.Empty;
                            decimal al_value = Convert.ToDecimal(worksheet.Cells[row, 3].Value != null ? worksheet.Cells[row, 3].Value.ToString().Trim() : string.Empty);
                            string al_date = Convert.ToDateTime(worksheet.Cells[row, 4].Value != null ? worksheet.Cells[row, 4].Value.ToString().Trim() : string.Empty).ToString("yyyy-MM-dd");
                            string eRemarks = worksheet.Cells[row, 5].Value != null ? worksheet.Cells[row, 5].Value.ToString().Trim() : string.Empty;
                            gRemarks = string.IsNullOrEmpty(model.remarks) ? eRemarks : model.remarks;
                            dataTable = await _dg_Common.get_InformationDataTableAsync("Dg_Pay_InsertAllowanceExcel " + model.CompanyID + "," + emp_no + ",'" + al_date + "','" + al_description + "'," + al_value + ",'" + gRemarks + "','" + model.userName + "'", _connection);
                            message.Add(new
                            {
                                messageType = dataTable.Rows[0]["messageType"].ToString().Trim(),
                                showMessage = dataTable.Rows[0]["showMessage"].ToString().Trim(),
                            });
                        }                        
                    }
                }
                else if (model.actionTypeID == 3)
                {
                    var worksheet = await _dg_Common.GetExcelWorkSheet(model.excelFile, 0);
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Text))
                        {
                            int emp_no = Convert.ToInt32(worksheet.Cells[row, 1].Value != null ? worksheet.Cells[row, 1].Value.ToString().Trim() : string.Empty);
                            string bAccNumber = worksheet.Cells[row, 2].Value != null ? worksheet.Cells[row, 2].Value.ToString().Trim() : string.Empty;
                            string bankShortname = worksheet.Cells[row, 3].Value != null ? worksheet.Cells[row, 3].Value.ToString().Trim() : string.Empty;
                            dataTable = await _dg_Common.get_InformationDataTableAsync("Dg_Pay_UpdateBanckAccountNoExcel " + model.CompanyID + "," + emp_no + ",'" + bAccNumber + "','" + bankShortname + "'", _connection);
                            message.Add(new
                            {
                                messageType = dataTable.Rows[0]["messageType"].ToString().Trim(),
                                showMessage = dataTable.Rows[0]["showMessage"].ToString().Trim(),
                            });
                        }                       
                    }
                }
                else if(model.actionTypeID == 4)
                {
                    var worksheet = await _dg_Common.GetExcelWorkSheet(model.excelFile, 0);
                    var rowCount = worksheet.Dimension?.Rows ?? 0;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Text))
                        {
                            int emp_no = Convert.ToInt32(worksheet.Cells[row, 1].Value != null ? worksheet.Cells[row, 1].Value.ToString().Trim() : string.Empty);
                            string DependGBasic = worksheet.Cells[row, 2].Value != null ? worksheet.Cells[row, 2].Value.ToString().Trim() : string.Empty;
                            int incAmount = Convert.ToInt32(worksheet.Cells[row, 3].Value != null ? worksheet.Cells[row, 3].Value.ToString().Trim() : string.Empty);
                            string cut_of_date = Convert.ToDateTime(worksheet.Cells[row, 4].Value != null ? worksheet.Cells[row, 4].Value.ToString().Trim() : string.Empty).ToString("yyyy-MM-dd");
                            string eRemarks = worksheet.Cells[row, 5].Value != null ? worksheet.Cells[row, 5].Value.ToString().Trim() : string.Empty;
                            gRemarks = string.IsNullOrEmpty(model.remarks) ? eRemarks : model.remarks;
                            dataTable = await _dg_Common.get_InformationDataTableAsync("Dg_Pay_InsertSalIncrementExcel " + model.CompanyID + "," + emp_no + "," + incAmount + ",'" + DependGBasic + "','" + cut_of_date + "','" + model.userName + "','" + gRemarks + "'", _connection);
                            message.Add(new
                            {
                                messageType = dataTable.Rows[0]["messageType"].ToString().Trim(),
                                showMessage = dataTable.Rows[0]["showMessage"].ToString().Trim(),
                            });
                        }                       
                    }
                }
                else if (model.actionTypeID == 5)
                {
                    var worksheet = await _dg_Common.GetExcelWorkSheet(model.excelFile, 0);
                    var rowCount = worksheet.Dimension.Rows;
                    for (int row = 6; row <= rowCount; row++)
                    {
                        try
                        {
                            var savePayload = new EmployeeExcelPayload
                            {
                                comp_name = worksheet.Cells[row, 8].Value != null ? worksheet.Cells[row, 8].Value.ToString().Trim() : string.Empty,
                                emp_no = Convert.ToInt32(worksheet.Cells[row, 2].Value != null ? worksheet.Cells[row, 2].Value.ToString().Trim() : string.Empty),
                                emp_proxid = worksheet.Cells[row, 27].Value != null ? worksheet.Cells[row, 27].Value.ToString().Trim() : string.Empty,
                                pi_fullname = worksheet.Cells[row, 3].Value != null ? worksheet.Cells[row, 3].Value.ToString().Trim() : string.Empty,
                                pi_farthersname = worksheet.Cells[row, 14].Value != null ? worksheet.Cells[row, 14].Value.ToString().Trim() : string.Empty,
                                pi_mothersname = worksheet.Cells[row, 15].Value != null ? worksheet.Cells[row, 15].Value.ToString().Trim() : string.Empty,
                                pi_birthdate = Convert.ToDateTime(worksheet.Cells[row, 6].Value != null ? worksheet.Cells[row, 6].Value.ToString().Trim() : string.Empty).ToString("yyyy-MM-dd"),
                                pi_bloodgroup = worksheet.Cells[row, 23].Value != null ? worksheet.Cells[row, 23].Value.ToString().Trim() : "N/A",
                                pi_sex = worksheet.Cells[row, 16].Value != null ? worksheet.Cells[row, 16].Value.ToString().Trim() : string.Empty,
                                pi_nic = worksheet.Cells[row, 24].Value != null ? worksheet.Cells[row, 24].Value.ToString().Trim() : string.Empty,
                                pi_birth_certificate_no = worksheet.Cells[row, 25].Value != null ? worksheet.Cells[row, 25].Value.ToString().Trim() : string.Empty,
                                Pi_tin = worksheet.Cells[row, 26].Value != null ? worksheet.Cells[row, 26].Value.ToString().Trim() : string.Empty,
                                pi_religoin = worksheet.Cells[row, 21].Value != null ? worksheet.Cells[row, 21].Value.ToString().Trim() : string.Empty,
                                pi_empcontactno = worksheet.Cells[row, 22].Value != null ? worksheet.Cells[row, 22].Value.ToString().Trim() : string.Empty,
                                oi_joineddate = Convert.ToDateTime(worksheet.Cells[row, 4].Value != null ? worksheet.Cells[row, 4].Value.ToString().Trim() : string.Empty).ToString("yyyy-MM-dd"),
                                oi_ConfDate = Convert.ToDateTime(worksheet.Cells[row, 5].Value != null ? worksheet.Cells[row, 5].Value.ToString().Trim() : string.Empty).ToString("yyyy-MM-dd"),
                                oi_department_name = worksheet.Cells[row, 10].Value != null ? worksheet.Cells[row, 10].Value.ToString().Trim() : string.Empty,
                                oi_section_name = worksheet.Cells[row, 11].Value != null ? worksheet.Cells[row, 11].Value.ToString().Trim() : string.Empty,
                                oi_floor_name = worksheet.Cells[row, 9].Value != null ? worksheet.Cells[row, 9].Value.ToString().Trim() : string.Empty,
                                oi_line_name = worksheet.Cells[row, 12].Value != null ? worksheet.Cells[row, 12].Value.ToString().Trim() : string.Empty,
                                oi_shift_name = worksheet.Cells[row, 29].Value != null ? worksheet.Cells[row, 29].Value.ToString().Trim() : string.Empty,
                                oi_garde_name = worksheet.Cells[row, 28].Value != null ? worksheet.Cells[row, 28].Value.ToString().Trim() : string.Empty,
                                oi_designation_name = worksheet.Cells[row, 7].Value != null ? worksheet.Cells[row, 7].Value.ToString().Trim() : string.Empty,
                                oi_salcategory_name = worksheet.Cells[row, 13].Value != null ? worksheet.Cells[row, 13].Value.ToString().Trim() : string.Empty,
                                oi_grossalary = Convert.ToInt32(worksheet.Cells[row, 19].Value != null ? worksheet.Cells[row, 19].Value.ToString().Trim() : string.Empty),
                                oi_bank_shCode = worksheet.Cells[row, 20].Value != null ? worksheet.Cells[row, 20].Value.ToString().Trim() : string.Empty,
                                oi_bankacno = worksheet.Cells[row, 18].Value != null ? worksheet.Cells[row, 18].Value.ToString().Trim() : string.Empty,
                                oi_tiffin_bill_status = Convert.ToInt32(worksheet.Cells[row, 30].Value != null ? worksheet.Cells[row, 30].Value.ToString().Trim() : string.Empty),
                                oi_night_bill_status = Convert.ToInt32(worksheet.Cells[row, 31].Value != null ? worksheet.Cells[row, 31].Value.ToString().Trim() : string.Empty),
                                oi_ot_status = Convert.ToInt32(worksheet.Cells[row, 32].Value != null ? worksheet.Cells[row, 32].Value.ToString().Trim() : string.Empty),
                            };
                            if (savePayload.oi_garde_name == "0")
                            {
                                savePayload.oi_garde_name = "-";
                            }
                            bool isSave = await _dg_Common.saveChangesAsync("Dg_Pay_InsertEmployeeInfoExcel", _connection, savePayload);
                            if (isSave)
                            {
                                var data = await _dg_Common.get_InformationDataTableAsync("select emp_serial from dg_pay_Employee where comp_name='" + savePayload.comp_name + "' and emp_no=" + savePayload.emp_no, _connection);
                                if (data.Rows.Count > 0)
                                {
                                    int emp_Serial = Convert.ToInt32(data.Rows[0]["emp_serial"].ToString());
                                    await _dg_Common.saveChangesAsync("dg_pay_InsertMenualAttRow " + emp_Serial, _connection);
                                }
                                message.Add(new
                                {
                                    messageType = "Success",
                                    showMessage = "Save Successfully !!",
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                DataColumn[] newCol = new DataColumn[]
                {
                    new DataColumn("messageType", typeof(string)),
                    new DataColumn("showMessage", typeof(string))
                };
                dataTable.Columns.AddRange(newCol);
                dataTable.Rows[0]["messageType"] = "Error";
                dataTable.Rows[0]["showMessage"] = "Something was worng !!";
                message.Add(new
                {
                    messageType = dataTable.Rows[0]["messageType"].ToString().Trim(),
                    showMessage = dataTable.Rows[0]["showMessage"].ToString().Trim(),
                });
            }
            return message;
        }
    }
}