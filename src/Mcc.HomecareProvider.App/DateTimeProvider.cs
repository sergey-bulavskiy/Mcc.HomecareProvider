using System;

namespace Mcc.HomecareProvider.App
{
    public class DateTimeProvider
    {
        /// <summary>
        ///     Provides a consistent value of "now" throughout a transaction.
        /// </summary>
        public DateTimeOffset UtcNow { get; } = DateTimeOffset.UtcNow;
    }
}