using BOL.Models;
using EF.Core.Repository.Interface.Manager;
using System.Data;

namespace BLL.Interfaces.Manager.Menurights
{
    public interface IMenurightsManager : ICommonManager<Menuright_DbModel>
    {
        Task<DataSet> GetCompanywiseuserlist(int compid);
        Task<DataSet> Getuserwisemenulist(string m_user);
    }
}
