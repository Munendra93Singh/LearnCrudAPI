using LearnCrudAPI.Helper;
using LearnCrudAPI.Model;
using LearnCrudAPI.Repos.Models;

namespace LearnCrudAPI.Service
{
    public interface IUserRoleService
    {
        Task<APIResponse> AssignRolePermission(List<TblRolepermission> _data);
        Task<List<TblRole>> GetAllRoles();
        Task<List<TblMenu>>GetTblMenus();
        Task<List<Appmenues>> GetAllMenubyrole(string userrole);
        Task<Menupermission> GetMenupermissionbyrole(string userrole, string menucode);
    }
}
