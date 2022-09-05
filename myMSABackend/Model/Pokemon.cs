using System.ComponentModel.DataAnnotations;

namespace myMSABackend.Model
{
    public class Pokemon
    {
        [Key]
        [Required]
        public string Name { get; set; }
        public int Power { get; set; }
        public string Nickname { get; set; }
    }
}