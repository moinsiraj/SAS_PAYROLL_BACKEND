using BLL.Interfaces.Repository.ButtonPermission;
using BOL.Models;
using DAL.Data;
using EF.Core.Repository.Repository;

namespace DAL.Implementation.Repository.ButtonPermission
{
    public class ButtonPermissionRepository : CommonRepository<ButtonPermission_DbModel>,IButtonPermissionRepository
    {
        public ButtonPermissionRepository(dg_hrpayrollContext context) : base(context)
        {
        }
    }
}
