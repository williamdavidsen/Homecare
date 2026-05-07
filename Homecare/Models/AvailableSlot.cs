using System.ComponentModel.DataAnnotations;

namespace Homecare.Models
{
    // Personnel open concrete time ranges for a day using preset periods.
    public class AvailableSlot
    {
        public int AvailableSlotId { get; set; }

        [Required] public int PersonnelId { get; set; }    // FK -> User (Personnel)
        [Required] public DateOnly Day { get; set; }
        [Required] public TimeOnly StartTime { get; set; }
        [Required] public TimeOnly EndTime { get; set; }

        // navs
        public User? Personnel { get; set; }
        public Appointment? Appointment { get; set; }      // 1 slot = 0..1 appointment
    }
}
