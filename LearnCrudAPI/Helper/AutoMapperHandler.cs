using AutoMapper;
using LearnCrudAPI.Model;
using LearnCrudAPI.Repos.Models;

namespace LearnCrudAPI.Helper
{
    public class AutoMapperHandler :Profile
    {
        public AutoMapperHandler() 
        {
            CreateMap<TblCustomer, CustomerModal>().ForMember(item => item.Statusname, opt => opt.MapFrom(
                item => (item.IsActive != null && item.IsActive.Value) ? "Active" : "In active")).ReverseMap();
        }
    }
}
