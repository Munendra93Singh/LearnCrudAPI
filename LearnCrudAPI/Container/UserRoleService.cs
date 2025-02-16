using LearnCrudAPI.Helper;
using LearnCrudAPI.Model;
using LearnCrudAPI.Repos;
using LearnCrudAPI.Repos.Models;
using LearnCrudAPI.Service;
using Microsoft.EntityFrameworkCore;

namespace LearnCrudAPI.Container
{
    public class UserRoleService : IUserRoleService
    {
        private readonly LearndataContext context;
        public UserRoleService(LearndataContext learndata)
        {
            this.context = learndata;
        }
        public async Task<APIResponse> AssignRolePermission(List<TblRolepermission> _data)
        {
            APIResponse response = new APIResponse();
            int processcount = 0;
            try
            {
                using (var dbtransaction = await this.context.Database.BeginTransactionAsync())
                {
                    if (_data.Count > 0)
                    {
                        _data.ForEach(item =>
                        {
                            var userdata = this.context.TblRolepermissions.FirstOrDefault(item1 => item1.Userrole == item.Userrole &&
                            item1.Menucode == item.Menucode);
                            if (userdata != null)
                            {

                                userdata.Haveview = item.Haveview;
                                userdata.Haveadd = item.Haveadd;
                                userdata.Havedelete = item.Havedelete;
                                userdata.Haveedit = item.Haveedit;
                                processcount++;
                            }
                            else
                            {
                                this.context.TblRolepermissions.Add(item);
                                processcount++;

                            }
                        });
                        if (_data.Count == processcount)
                        {
                            await this.context.SaveChangesAsync();
                            await dbtransaction.CommitAsync();
                            response.Result = "Save";
                            response.Message = "Save Successfully";
                        }
                        else
                        {
                            await dbtransaction.RollbackAsync();
                        }
                    }
                    else
                    {
                        response.Result = "fail";
                        response.Message = "failed";
                    }
                }
            }
            catch (Exception ex)
            {
                response = new APIResponse();
            }
            return response;
        }

        public async Task<List<TblRole>> GetAllRoles()
        {
           return await this.context.TblRoles.ToListAsync();
        }

        public async Task<List<TblMenu>> GetTblMenus()
        {
            return await this.context.TblMenus.ToListAsync();
        }

        public async Task<List<Appmenues>>GetAllMenubyrole(string userrole)
        {
            List<Appmenues> appmenus = new List<Appmenues>();

            var accessdata = (from menu in this.context.TblRolepermissions.Where(o => o.Userrole == userrole && o.Haveview)
                              join m in this.context.TblMenus on menu.Menucode equals m.Code into _jointable
                              from p in _jointable.DefaultIfEmpty()
                              select new { code = menu.Menucode, name = p.Name }).ToList();
            if (accessdata.Any())
            {
                accessdata.ForEach(item =>
                {
                    appmenus.Add(new Appmenues()
                    {
                        code = item.code,
                        Name = item.name,
                      
                    });
                });
            }

            return appmenus;
        }    
       
        public async Task<Menupermission> GetMenupermissionbyrole(string userrole, string menucode)
        {
            Menupermission menupermission = new Menupermission();
            var _data = await this.context.TblRolepermissions.FirstOrDefaultAsync(o => o.Userrole == userrole && o.Haveview
            && o.Menucode == menucode);
            if (_data != null)
            {
                menupermission.code = _data.Menucode;
                menupermission.Haveview = _data.Haveview;
                menupermission.Haveadd = _data.Haveadd;
                menupermission.Haveedit = _data.Haveedit;
                menupermission.Havedelete = _data.Havedelete;
            }
            return menupermission;
        }

       
    }
}