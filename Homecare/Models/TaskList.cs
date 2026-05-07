namespace Homecare.Models
{
    // Appointment-to-CareTask many-to-many relationship.
    public class TaskList
    {
        public int AppointmentId { get; set; }
        public int CareTaskId { get; set; }

        // navs
        public Appointment? Appointment { get; set; }
        public CareTask? CareTask { get; set; }
    }
}
