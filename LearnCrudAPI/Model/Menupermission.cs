using System.ComponentModel.DataAnnotations.Schema;

namespace LearnCrudAPI.Model
{
    public class Menupermission
    {
        public  string code { get; set; }
        public string name { get; set; }       
        public bool Haveview { get; set; }     
        public bool Haveadd { get; set; }      
        public bool Haveedit { get; set; }      
        public bool Havedelete { get; set; }
    }
}
