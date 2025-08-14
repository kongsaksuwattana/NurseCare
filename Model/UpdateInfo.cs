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
    public enum TeamName
    {
        TeamA,
        TeamB,
        TeamC,
        TeamD
    }
    public enum TurnPosture
    {
        Left,
        Right,
        Supine
    }

    [Table("UpdateBedInfo")]
    public class UpdateBedInfo
    {        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("UpdateId")]
        public int updateId { get; set; } 

        [Column("BedId")]
        public string? BedId { get; set; } = string.Empty;

        [Column("PatientId")]
        public string? PatientId { get; set; } = string.Empty;  // Using Person.Id as PatientId

        [Column("PatientHN")]
        public string? PatientHN { get; set; } = string.Empty;

        [Column("NurseId")]
        public string? NurseId { get; set; } = string.Empty;    // Using Person.Id as NurseId

        [Column("TurningTime")]
        public DateTime? TurningTime { get; set; } = DateTime.Now;

        [Column("NextTurningTime")]
        public DateTime? NextTurningTime { get; set; }

        [Column("UpdateDateTime")]
        public DateTime UpdateDateTime { get; set; }

        [Column("IsManualKeyed")]
        public bool IsManualKeyed { get; set; } = false;

        [Column("ImageUrl")]
        public string? ImageUrl { get; set; } = string.Empty; // Optional image URL for the update

        [Column("IsNotifying")]
        public bool IsNotifying { get; set; } = false;
    }

    [Table("UpdateBedInfoAndEffect")]
    public class UpdateBedInfoAndEffect : UpdateBedInfo
    {
        [Column("TeamSupport")]
        public TeamName? TeamSupport { get; set; } = TeamName.TeamA; // Default to TeamA
        public TurnPosture? Posture { get; set; } = TurnPosture.Supine;
        public bool Occiput { get; set; } = false;
        public bool Scapula { get; set; } = false;
        public bool Sacrum_Coccyx { get; set; } = false;
        public bool Heel { get; set; } = false;
    }


    [PrimaryKey("updateId")]
    [Table("UpdateTeamInfo")]
    public class UpdateTeamInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("UpdateId")]
        public int updateId { get; set; }

        [Column("TeamName")]
        public TeamName TeamName { get; set; }

        [Column("NurseId")]
        public string? NurseId { get; set; } = string.Empty;

        [Column("UpdateDateTime")]
        public DateTime UpdateDateTime { get; set; }

    }
    [Table("Bed")]
    public class Bed
    {
        [Key]
        [Column("BedId")]
        public string BedId { get; set; } = string.Empty;

        [Column("TeamSupport")]
        public TeamName? TeamSupport { get; set; } = TeamName.TeamA;

        [Column("IsOccupied")]
        public bool IsOccupied { get; set; } = false;
        
    }
}
