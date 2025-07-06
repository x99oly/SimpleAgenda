using SimpleAgenda.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAgenda.DTOS.Publics
{
    public class ScheduleOutDto
    {
        public Guid? Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public RecurrenceTypeEnum? RecurrenceType { get; set; }
        public int? RecurrenceInterval { get; set; }

        public int? Hour { get; set; }
        public int? Minute { get; set; }

        public List<DayOfWeek>? RecurrenceWeekDays { get; set; }

        public List<DateOnly>? ExcludedDates { get; set; }
    }
}
