using BOL.Models;
using System.Collections.Concurrent;

namespace BLL.Interfaces.Manager.UploadAttendances
{
    public interface IUploadAttendancesManager
    {
        Task<List<TextUploadMessage>> ReadAttnTextFile_New(UploadFile model);
        Task<List<TextUploadMessage>> ReadAttnTextFile_New2(UploadFile model);
        Task<List<TextUploadMessage>> ReadAttnTextFile_New3(UploadFile model);
        Task<List<ReturnObject>> ReadAttnTextFileEmployeeWise(UploadFileEmpWise model);
        Task<List<ReturnObject>> ReadAttnTextFileEmployeeWise_new(UploadFileEmpWise model);
        Task<ReturnObject> GetAttendanceOtProcEmp(int compid, string date, int? deptid, int? secid);
        Task<List<TextUploadMessage>> SetAttendanceOtProcess(AttendanceOtProcess obj);
        Task<ReturnObject> GetTextFileFormatInfo(int compid);
    }
}
