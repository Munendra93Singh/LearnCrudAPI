using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LearnCrudAPI.Repos.Models;

[Table("tbl_customer")]
//[Table("district_master")]
//[Table("state_master")]
//[Table("tehsil_master")]
public partial class TblCustomer
{
    [Key]
    [StringLength(50)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Phone { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Creditlimit { get; set; }

    public bool? IsActive { get; set; }

    public int? Taxcode { get; set; }





    //[Key]
    //[StringLength(50)]
    //[Unicode(false)]
    //public string lgd_district_code { get; set; } = null!;

    //[StringLength(50)]
    //[Unicode(false)]
    //public string lgd_tehsil_name_en { get; set; } = null!;

    //[StringLength(50)]
    //[Unicode(false)]
    //public string? lgd_state_code { get; set; }

    //[StringLength(50)]
    //[Unicode(false)]
    //public string? district_name_ll { get; set; }
   

    //public string? lgd_state_name_en { get; set; }

    //public string? state_short_name { get; set; }
}
