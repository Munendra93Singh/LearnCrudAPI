using LearnCrudAPI.Helper;
using LearnCrudAPI.Model;
using LearnCrudAPI.Repos.Models;

namespace LearnCrudAPI.Service
{
    public interface ICustomerService 
    {
        Task<List<CustomerModal>> Getall();
        Task<CustomerModal> Getbycode(string code);
        Task<APIResponse> Remove(string code);
        Task<APIResponse> Create(CustomerModal data);
        Task<APIResponse> Update(CustomerModal data, string code);
    }
}
