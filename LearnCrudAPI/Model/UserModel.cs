
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnCrudAPI.Modal
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Isactive { get; set; }
        public string Statusname { get; set; }
        public string Role { get; set; }

        //public string UserName {  get; set; }
        //public string Name {  get; set; }
        //public string Email { get; set; }
        //public string Phone { get; set; }
        //public string Password { get; set; }
    }
}
