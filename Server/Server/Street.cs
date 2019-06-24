using System;

namespace Server
{
    public class Street
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? DeletedDate { get; set; }

        public string Index { get; set; }

        public string Address { get; set; }
    }
}
