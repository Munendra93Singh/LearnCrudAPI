using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LearnCrudAPI.Repos.Models
{
    public class TblCountry
    {
        [Key]
        [StringLength(50)]
        [Unicode(false)]
        public string lgd_tehsil_code { get; set; } = null!;

        [StringLength(50)]
        [Unicode(false)]
        public string lgd_tehsil_name_en { get; set; } = null!;

        [StringLength(50)]
        [Unicode(false)]
        public string? lgd_district_code { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string? district_name_ll { get; set; }


        public string? lgd_state_code { get; set; }

        public string? lgd_state_name_en { get; set; }

        public string? state_short_name { get; set; }
    }
}
