#if (NET45 || NET40)
#define SUPPORTS_WASAPI 
#endif

#if (NET45 || NET40)
#define SUPPORTS_WINMM 
#endif

using System;
using System.Runtime.InteropServices;
using Fundamental.Core;


#if SUPPORTS_WASAPI
using Fundamental.Interface.Wasapi;
using Fundamental.Interface.Wasapi.Options;
#endif

namespace Fundamental
{
    public class AudioInterfaceProvider : InterfaceProvider,
#if SUPPORTS_WASAPI
            IInterfaceOptions<WasapiOptions>,
            ISupportsInterface<WasapiInterfaceProvider>,
#endif
            // Default provider for the running system
            ISupportsInterface<InterfaceProvider>
    {

        #region Singleton Access 

        /// <summary>
        /// The underlying singleton instance
        /// </summary>
        private static AudioInterfaceProvider _singleton;

        /// <summary>
        /// Gets the singleton.
        /// </summary>
        /// <value>
        /// The singleton.
        /// </value>
        private static AudioInterfaceProvider Singleton => _singleton ?? (_singleton = new AudioInterfaceProvider());

        /// <summary>
        /// Gets the interface.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInterface<T>() => Singleton.Get<T>();

        /// <summary>
        /// Determines whether [is interface supported].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        ///   <c>true</c> if [is interface supported]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInterfaceSupported<T>() => Singleton.IsSupported<T>();

        /// <summary>
        /// Gets the default provider for the target system.
        /// </summary>
        /// <returns></returns>
        public static InterfaceProvider GetProvider()
        {
            return GetInterface<InterfaceProvider>();
        }

        #endregion 

        #region Default provider

        /// <summary>
        /// The underlying default interface provider
        /// </summary>
        private InterfaceProvider _defaultProvider;

        /// <summary>
        /// Gets the Singleton WASAPI interface provider.
        /// </summary>
        /// <value>
        /// The WASAPI interface provider.
        /// </value>
        private InterfaceProvider DefaultProvider => _defaultProvider ?? (_defaultProvider = ResolveDefaultAudioInterface());

        /// <summary>
        /// Gets default audio interface for the current running operating system.
        /// </summary>
        /// <returns></returns>
        InterfaceProvider ISupportsInterface<InterfaceProvider>.GetAudioInterface()
        {
            return DefaultProvider;
        }

        /// <summary>
        /// Resolves the default audio interface.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">
        /// Windows XP is not currently supported.
        /// or
        /// Non windows operating currently require Mono platform to run.
        /// or
        /// Operating system is not currently supported.
        /// </exception>
        private InterfaceProvider ResolveDefaultAudioInterface()
        {

            // TODO: Detect windows universal Application

#if SUPPORTS_WINMM || SUPPORTS_WASAPI
            // Check if windows XP environment
            if (IsWindowsXpEnvironment())
            {
                // TODO: add Win MM support for windows XP
                throw new NotSupportedException("Windows XP is not currently supported.");
            }

            // This will only work in Linux and OSx environments if mono is installed
            return Get<WasapiInterfaceProvider>();

#elif (NETSTANDARD1_0 || NETSTANDARD1_1) 

            if (IsWindowEnvironment())
                 throw new NotSupportedException("Dotnet Core is not supported when running in windows environment. Please run as .net 4.x application.");

             // TODO: add native support for Linux and OSx
            if (IsNonWindowEnvironment())
                throw new NotSupportedException("Non windows operating systems currently require Mono platform to run.");

            throw new NotSupportedException("Operating system currently supported.");
#else
            throw new NotSupportedException("Target build output is not currently supported.");
#endif
        }


        #endregion

        #region WASAPI Support

#if SUPPORTS_WASAPI

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
        void IInterfaceOptions<WasapiOptions>.Configure(Action<WasapiOptions> configure)
        {
            WasapiInterfaceProvider.Configure(configure);
        }

        /// <summary>
        /// Gets the WASAPI audio interface.
        /// </summary>
        /// <returns></returns>
        WasapiInterfaceProvider ISupportsInterface<WasapiInterfaceProvider>.GetAudioInterface()
        {
            return WasapiInterfaceProvider;
        }

#endif


        #endregion

        // Private Methods 

        private static bool IsWindowsXpEnvironment()
        {
#if (NET45 || NET40)
            return IsWindowEnvironment() && Environment.OSVersion.Version.Major < 6;
#else
            return false;
#endif
        }


        private static bool IsWindowEnvironment()
        {
#if (NET40 || NETSTANDARD1_0)
            // TODO: detect mono
            return true;
#else
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
        }

        private static bool IsNonWindowEnvironment()
        {
            return !IsWindowEnvironment();
        }
    }
}
