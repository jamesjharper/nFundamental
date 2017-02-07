using System;

namespace Fundamental.Core
{
    public interface IFormatChangable
    {
        /// <summary>
        /// Raised when source format changes.
        /// </summary>
        event EventHandler<EventArgs> FormatChanged;
    }
}
