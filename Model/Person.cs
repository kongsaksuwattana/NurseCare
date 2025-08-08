using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MySql.EntityFrameworkCore.DataAnnotations;

using Microsoft.EntityFrameworkCore;
namespace NurseCare.Model
{
    [MySQLCharset("utf8mb4")]
    [Table("Person")]    
    public class Person
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Column("FirstName")]
        [MySQLCollation("utf8mb4_general_ci")]  
        public string? FirstName { get; set; } = string.Empty;

        [Column("LastName")]
        [MySQLCollation("utf8mb4_general_ci")]
        public string? LastName { get; set; } = string.Empty;

        [Column("NickName")]
        [MySQLCollation("utf8mb4_general_ci")]
        public string? NickName { get; set; } = string.Empty;

    }

    [MySQLCharset("utf8mb4")]
    [Table("Patient")]
    public class Patient : Person
    {
        [Column("HN")]
        public string HN { get; set; } = string.Empty;

        [Column("DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("Disease")]
        [MySQLCollation("utf8mb4_general_ci")]
        public string? Disease { get; set; } = string.Empty;

        [Column("DateOfAdmission")]
        public DateTime? DateOfAdmission { get; set; }

        [Column("DateOfDischarge")]
        public DateTime? DateOfDischarge { get; set; }

        [Column("ContactPerson")]
        [MySQLCollation("utf8mb4_general_ci")]
        public string? ContactPerson { get; set; } = string.Empty;

        [Column("ContactPhone")]
        public string? ContactPhone { get; set; } = string.Empty;
    }

    [MySQLCharset("utf8mb4")]
    [Table("Nurse")]
    public class Nurse : Person
    {
        [Column("EmployeeId")]
        public string? EmployeeId { get; set; } = string.Empty;

        [Column("DateOfHire")]
        public DateTime? DateOfHire { get; set; } = DateTime.Now;

        [Column("Department")]
        [MySQLCollation("utf8mb4_general_ci")]
        public string? Department { get; set; } = string.Empty;

        [Column("TeamName")]        
        public TeamName TeamName { get; set; } = TeamName.TeamA;
    }

    [MySQLCharset("utf8mb4")]
    [Table("ContactAddress")]
    public class ContactAddress
    {

        [Column("Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString(); //Person.Id

        [Column("Address1")]
        [MySQLCollation("utf8mb4_general_ci")]
        public string? Address1 { get; set; }

        [Column("Address2")]
        [MySQLCollation("utf8mb4_general_ci")]
        public string? Address2 { get; set; }

        [Column("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [Column("Email")]
        public string? Email { get; set; } = string.Empty;
    }
}