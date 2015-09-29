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

            return reset;
        }

        public TimeSpan GetTimeUntilExpiration()
        {
            return GetExpiration() - DateTime.UtcNow;
        }

        public bool HasCompletedToday(State state)
        {
            var times = state.Data.CompletionTimes;
            var expiration = GetExpiration().AddDays(-1);
            return times.Any() && times.Last() > expiration;
        }

        public int GetStreak(State state)
        {
            int count = 0;
            var expiration = GetExpiration().AddDays(-1);
            var times = state.Data.CompletionTimes;
            var start = times.Count - 1;

            if (HasCompletedToday(state))
            {
                start--;
                count++;
            }

            for (int i = start; i >= 0; --i)
            {
                expiration = expiration.AddDays(-1);
                if (times[i] > expiration)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        public void MarkCompletion(State state)
        {
            if (HasCompletedToday(state))
            {
                return;
            }

            state.Data.CompletionTimes.Add(DateTime.UtcNow);
        }

        public bool ShouldNotify(State state)
        {
            if (HasCompletedToday(state))
            {
                return false;
            }

            var expires = GetExpiration();
            var now = DateTime.UtcNow;
            var timeLeft = expires - now;
            var timeSince = now - state.Data.LastNotification;

            if (timeLeft.Hours > 9)
            {
                return false;
            }

            if (timeLeft.Hours >= 4 && timeSince.Hours < 3)
            {
                return false;
            }

            if (timeLeft.Hours >= 2 && timeSince.Hours < 1)
            {
                return false;
            }

            return true;
        }

        public void OnNotify(State state)
        {
            state.Data.LastNotification = DateTime.UtcNow;
        }
    }
}
