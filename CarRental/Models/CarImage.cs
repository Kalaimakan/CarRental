using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.Models
{
    public class CarImage
    {
        [Key]
        public Guid Id { get; set; }

        public string FileName { get; set; }

        [ForeignKey("Car")]
        public Guid CarId { get; set; }

    }
}

