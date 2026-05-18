using BOL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BLL.Interfaces.Manager.ExcelToDatabase
{
    public interface IExcelToDatabaseManager
    {
        Task<List<ExcelToDb>> GetExcelCountryData([FromForm] UploadExcelFile model);
        Task<string> SaveCountry([FromForm] UploadExcelFile model);

        Task<DataTable> GetExcelUploadType();
        Task<List<object>> SaveUploadExcelToDB([FromForm] UploadExcelFile model);
    }
}