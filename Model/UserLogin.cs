using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NurseCare.Model
{
    public enum UserRole
    {
        Admin, 
        Nurse,        
        Doctor,
        Patient
    }

    [Table("UserLogin")]
    public class UserLogin
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [Column("Username")]
        public string Username { get; set; } = string.Empty;
        [Column("Password")]
        public string Password { get; set; } = string.Empty;
        [Column("PersonId")]
        public string PersonId { get; set; } = string.Empty; //Using Person.Id
        [Column("Role")]
        public int Role { get; set; } = (int)UserRole.Nurse; // Default to Patient role
    }
}
