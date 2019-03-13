using System;

namespace BlockChainBackend.Helpers
{
    public static class TimeHelper
    {
            public static decimal ToUnixTimestampSecs(this DateTime date) => ToUnixTimestampTicks(date) / (decimal) TimeSpan.TicksPerSecond;
            public static long ToUnixTimestampTicks(this DateTime date) => date.ToUniversalTime().Ticks - UnixEpochTicks;
            private static readonly long UnixEpochTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        
        public static DateTime UnixTimestampToDateTime(long unixTime)
        {
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            //long unixTimeStampInTicks = (long) (unixTime * TimeSpan.TicksPerSecond);
            return new DateTime(unixStart.Ticks + unixTime, System.DateTimeKind.Utc);
        }
    }
}