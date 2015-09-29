using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtaLogic
{
    public sealed class Logic
    {
        public DateTime GetExpiration()
        {
            var reset = DateTime.UtcNow;
            if (reset.Millisecond != 0)
            {
                reset += TimeSpan.FromMilliseconds(1000 - reset.Millisecond);
            }
            if (reset.Second != 0)
            {
                reset += TimeSpan.FromSeconds(60 - reset.Second);
            }
            if (reset.Minute != 0)
            {
                reset += TimeSpan.FromMinutes(60 - reset.Minute);
            }

            if (reset.Hour > 6)
            {
                reset += TimeSpan.FromHours(24 - reset.Hour);
                reset += TimeSpan.FromHours(6);
            }
            else if (reset.Hour < 6)
            {
                reset += TimeSpan.FromHours(6 - reset.Hour);
            }

            return reset.ToLocalTime();
        }

        public TimeSpan GetTimeUntilExpiration()
        {
            return GetExpiration() - DateTime.Now;
        }

        public int GetStreak(State state)
        {
            return 0;
        }
    }
}
