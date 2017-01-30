using System;

namespace Fundamental.Interface
{
    public class UnsupportedTokenTypeException : TokenException
    {

        /// <summary>
        /// Gets or sets the type of the supported.
        /// </summary>
        /// <value>
        /// The type of the supported.
        /// </value>
        public Type[] SupportedType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedTokenTypeException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="types">The types.</param>
        public UnsupportedTokenTypeException(string message, params Type[] types) : base(message)
        {
            SupportedType = types;
        }
    }
}
