namespace AccountingOfTraficViolation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Login { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        public byte Role { get; set; }

        [StringLength(15)]
        public string Name { get; set; }

        [StringLength(15)]
        public string Surname { get; set; }
    }
}
