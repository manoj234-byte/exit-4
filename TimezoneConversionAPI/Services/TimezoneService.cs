using NodaTime;
using TimezoneConversionAPI.Models;
using System;

namespace TimezoneConversionAPI.Services
{
    public class TimezoneService
    {
        public TimezoneConversionResponse ConvertTimeZone(TimezoneConversionRequest request)
        {
            var istTimeZone = DateTimeZoneProviders.Tzdb["Asia/Kolkata"];
            var currentTimeInIST = SystemClock.Instance.GetCurrentInstant().InZone(istTimeZone).ToDateTimeUtc();

            var targetZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(request.TargetTimeZone);
            if (targetZone == null)
            {
                throw new ArgumentException("Invalid target time zone");
            }

            var targetTime = Instant.FromDateTimeUtc(currentTimeInIST).InZone(targetZone).ToString("yyyy-MM-dd HH:mm:ss", null);

            return new TimezoneConversionResponse
            {
                ConvertedTime = targetTime
            };
        }
    }
}
