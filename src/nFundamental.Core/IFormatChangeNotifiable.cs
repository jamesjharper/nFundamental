using System;

namespace Fundamental.Core
{
    public interface IFormatChangeNotifiable
    {
        /// <summary>
        /// Raised when source format changes.
        /// </summary>
        event EventHandler<EventArgs> FormatChanged;
    }
}
