using System;

namespace Fundamental.Interface
{
    public class ErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorEventArgs"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public ErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
