using System;

namespace Entity.Model
{
    public class Notification
    {
        public int Id { get; set; }
        public int CitationId { get; set; }
        public string Message { get; set; }
        public DateTime DateShipment { get; set; }
        public string StateNotification { get; set; }
        public string TypeNotification { get; set; }
        public int CitationId1 { get; set; } 

    }
}
