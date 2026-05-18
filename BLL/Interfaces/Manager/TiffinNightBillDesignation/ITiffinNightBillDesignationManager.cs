using BOL.Models;

namespace BLL.Interfaces.Manager.TiffinNightBillDesignation
{
    public interface ITiffinNightBillDesignationManager
    {
        Task<ReturnObject> GetAllDesignationByComp_ForNightAndTiffinBill(int companyID);
        Task<ReturnObject> AddOrEditDescWiseTiffinAndNightBill(DesignationWiseTiffinAndNightBill obj);
        Task<ReturnObject> GetAllDesignationForWH(int companyID);
        Task<ReturnObject> AddOrEditDesignationForWH(DesignationWiseWH obj);
    }
}
