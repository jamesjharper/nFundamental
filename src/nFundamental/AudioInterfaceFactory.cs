
using System;
using System.Runtime.InteropServices;
using Fundamental.Core;
#if NET46
using Fundamental.Interface.Wasapi;
using Fundamental.Interface.Wasapi.Options;
#endif

namespace Fundamental
{
    public class AudioInterfaceFactory : InterfaceProvider,
            #if NET46
            IInterfaceOptions<WasapiOptions>,
            ISupportsInterface<WasapiInterfaceProvider>,
            #endif
            ISupportsInterface<IInterfaceProvider>
    {


#if NET46

        /// <summary>
        /// The underlying WASAPI interface provider
        /// </summary>
        private WasapiInterfaceProvider _wasapiInterfaceProvider;

        /// <summary>
        /// Gets the Singleton WASAPI interface provider.
        /// </summary>
        /// <value>
        /// The WASAPI interface provider.
        /// </value>
        private WasapiInterfaceProvider WasapiInterfaceProvider => _wasapiInterfaceProvider ?? (_wasapiInterfaceProvider = new WasapiInterfaceProvider());

        /// <summary>
        /// Configures the WASAPI audio interface.
        /// </summary>
        /// <param name="configure">The configure.</param>
        public void Configure(Action<WasapiOptions> configure)
        {
            WasapiInterfaceProvider.Configure(configure);
        }

        /// <summary>
        /// Gets the WASAPI audio interface.
        /// </summary>
        /// <returns></returns>
        public WasapiInterfaceProvider GetAudioInterface()
        {
            return WasapiInterfaceProvider;
        }
#endif


        /// <summary>
        /// Gets default audio interface for the current running operating system.
        /// </summary>
        /// <returns></returns>
        IInterfaceProvider ISupportsInterface<IInterfaceProvider>.GetAudioInterface()
        {
#if NET46
            // Check if windows XP environment
            if(IsWindowsXpEnvironment())
                throw new NotSupportedException("Windows XP is not currently supported.");

            return GetAudioInterface<WasapiInterfaceProvider>();
#endif

#if NETCOREAPP1_6
            if (IsWindowEnvironment())
                 throw new NotSupportedException("Dotnet Core is not supported when running in windows environment. Please run as .net 4.6 application.");

#endif


            if (IsNonWindowEnvironment())
                throw new NotSupportedException("Non windows operating currently require Mono platform to run.");

            throw new NotSupportedException("Operating system is not currently supported.");
        }


        // Private Methods 

        private static bool IsWindowsXpEnvironment()
        {
#if NET46
            return IsWindowEnvironment() && Environment.OSVersion.Version.Major < 6;
#else
            return false;
#endif
        }


        private static bool IsWindowEnvironment()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        private static bool IsNonWindowEnvironment()
        {
            return !IsWindowEnvironment();
        }
    }
}
