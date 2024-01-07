using System.ComponentModel.DataAnnotations;

namespace Harjoitus1.Models
{
    public class User
    {
        public long Id { get; set; }


        [Required]
        [MinLength(3)]
        [MaxLength(100)]

        public string userName { get; set; }


        [Required]
        [MinLength(8)]
        [MaxLength(255)]
        public string Password { get; set; }
        public byte[]? Salt { get; set; }

      
        [MaxLength(50)]
        public string? firstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? lastName { get; set; }

        public DateTime? joinDate  { get; set; }
        public DateTime? lastLogin { get; set; }
    }

    public class UserDTO
    {
        public long Id { get; set; }


        [Required]
        [MinLength(3)]
        [MaxLength(100)]

        public string? userName { get; set; }


       


        [MaxLength(50)]
        public string? firstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string? lastName { get; set; }

        public DateTime? joinDate { get; set; }
        public DateTime? lastLogin { get; set; }







    }


}
