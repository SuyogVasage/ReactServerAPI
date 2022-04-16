using System;
using System.Collections.Generic;

namespace ReactServerAPI.Models
{
    public partial class Mark
    {
        public int RowId { get; set; }
        public string UserId { get; set; } = null!;
        public double Mathematics { get; set; }
        public double Science { get; set; }
        public double Geography { get; set; }
        public double History { get; set; }
        public DateTime Date { get; set; }
    }
}
