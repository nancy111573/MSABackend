using System.ComponentModel.DataAnnotations;

namespace myMSABackend.Model
{
    public class Pokemon
    {
        [Key]
        public string Name { get; set; }
        [Required]
        public int Power { get; set; }
        public string Nickname { get; set; }
    }
}