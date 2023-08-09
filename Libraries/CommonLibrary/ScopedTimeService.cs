using System;

namespace CommonLibrary
{
    public interface ITimeService
    {
        public DateTime Now { get; }

        public DateTime StartOfToday(int startHour = 0);

        public DateTime FirstDayOfWeek(int startHour = 0, DayOfWeek dow = DayOfWeek.Sunday);
    }

    public class ScopedTimeService : ITimeService
    {
        private DateTime? now;

        public DateTime Now => now ?? (now = DateTime.UtcNow).Value;

        public DateTime StartOfToday(int startHour = 0)
        {
            var current = Now;
            if (current.Hour < startHour)
            {
                current = current.AddDays(-1);
                return new(current.Year, current.Month, current.Day, startHour, 0, 0);
            }
            else
            {
                return new(current.Year, current.Month, current.Day, startHour, 0, 0);
            }
        }

        public DateTime FirstDayOfWeek(int startHour = 0, DayOfWeek dow = DayOfWeek.Sunday)
        {
            var today = StartOfToday(startHour);
            var todayDow = today.DayOfWeek;
            var diff = dow - todayDow;
            return diff switch
            {
                0 => today,
                < 0 => today.AddDays(diff),
                > 0 => today.AddDays(diff - 7),
            };
        }
    }
}