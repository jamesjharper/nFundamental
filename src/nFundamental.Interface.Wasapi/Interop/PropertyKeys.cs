using System;

namespace Fundamental.Interface.Wasapi.Interop
{
    

    /// <summary>
    /// Property Keys
    /// </summary>
    public static class PropertyKeys
    {

        //#include <devpropdef.h>

        //
        // _NAME
        //

        public static readonly PropertyKey Name                              = new PropertyKey(PropertyCategories.Name, 10); // DEVPROP_TYPE_STRING

        //
        // Device properties
        // These s correspond to the old setupapi SPDRP_XXX properties
        //

        public static readonly PropertyKey DeviceDeviceDesc                  = new PropertyKey(PropertyCategories.Device1, 2);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceHardwareIds                 = new PropertyKey(PropertyCategories.Device1, 3);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceCompatibleIds               = new PropertyKey(PropertyCategories.Device1, 4);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceService                     = new PropertyKey(PropertyCategories.Device1, 6);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClass                       = new PropertyKey(PropertyCategories.Device1, 9);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassGuid                   = new PropertyKey(PropertyCategories.Device1, 10); // DEVPROP_TYPE_GUID
        public static readonly PropertyKey DeviceDriver                      = new PropertyKey(PropertyCategories.Device1, 11); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceConfigFlags                 = new PropertyKey(PropertyCategories.Device1, 12); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceManufacturer                = new PropertyKey(PropertyCategories.Device1, 13); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceFriendlyName                = new PropertyKey(PropertyCategories.Device1, 14); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceLocationInfo                = new PropertyKey(PropertyCategories.Device1, 15); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DevicePdoName                     = new PropertyKey(PropertyCategories.Device1, 16); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceCapabilities                = new PropertyKey(PropertyCategories.Device1, 17); // DEVPROP_TYPE_UNINT32
        public static readonly PropertyKey DeviceUiNumber                    = new PropertyKey(PropertyCategories.Device1, 18); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceUpperFilters                = new PropertyKey(PropertyCategories.Device1, 19); // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceLowerFilters                = new PropertyKey(PropertyCategories.Device1, 20); // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceBusTypeGuid                 = new PropertyKey(PropertyCategories.Device1, 21); // DEVPROP_TYPE_GUID
        public static readonly PropertyKey DeviceLegacyBusType               = new PropertyKey(PropertyCategories.Device1, 22); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceBusNumber                   = new PropertyKey(PropertyCategories.Device1, 23); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceEnumeratorName              = new PropertyKey(PropertyCategories.Device1, 24); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceSecurity                    = new PropertyKey(PropertyCategories.Device1, 25); // DEVPROP_TYPE_SECURITY_DESCRIPTOR
        public static readonly PropertyKey DeviceSecuritySds                 = new PropertyKey(PropertyCategories.Device1, 26); // DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        public static readonly PropertyKey DeviceDevType                     = new PropertyKey(PropertyCategories.Device1, 27); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceExclusive                   = new PropertyKey(PropertyCategories.Device1, 28); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceCharacteristics             = new PropertyKey(PropertyCategories.Device1, 29); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceAddress                     = new PropertyKey(PropertyCategories.Device1, 30); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceUiNumberDescFormat          = new PropertyKey(PropertyCategories.Device1, 31); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DevicePowerData                   = new PropertyKey(PropertyCategories.Device1, 32); // DEVPROP_TYPE_BINARY
        public static readonly PropertyKey DeviceRemovalPolicy               = new PropertyKey(PropertyCategories.Device1, 33); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceRemovalPolicyDefault        = new PropertyKey(PropertyCategories.Device1, 34); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceRemovalPolicyOverride       = new PropertyKey(PropertyCategories.Device1, 35); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceInstallState                = new PropertyKey(PropertyCategories.Device1, 36); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceLocationPaths               = new PropertyKey(PropertyCategories.Device1, 37); // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceBaseContainerId             = new PropertyKey(PropertyCategories.Device1, 38); // DEVPROP_TYPE_GUID

        //
        // Device properties
        // These s correspond to a device's status and problem code
        //


        public static readonly PropertyKey DeviceDevNodeStatus               = new PropertyKey(PropertyCategories.Device2, 2); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceProblemCode                 = new PropertyKey(PropertyCategories.Device2, 3); // DEVPROP_TYPE_UINT32

        //
        // Device properties
        // These s correspond to device relations
        //

        public static readonly PropertyKey DeviceEjectionRelations           = new PropertyKey(PropertyCategories.Device2, 4);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceRemovalRelations            = new PropertyKey(PropertyCategories.Device2, 5);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DevicePowerRelations              = new PropertyKey(PropertyCategories.Device2, 6);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceBusRelationsn               = new PropertyKey(PropertyCategories.Device2, 7);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceParent                      = new PropertyKey(PropertyCategories.Device2, 8);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceChildren                    = new PropertyKey(PropertyCategories.Device2, 9);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceSiblings                    = new PropertyKey(PropertyCategories.Device2, 10); // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceTransportRelations          = new PropertyKey(PropertyCategories.Device2, 11); // DEVPROP_TYPE_STRING_LIST

        //
        // Other Device properties
        //


        public static readonly PropertyKey DeviceReported                    = new PropertyKey(PropertyCategories.Device3, 2);    // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceLegacy                      = new PropertyKey(PropertyCategories.Device3, 3);    // DEVPROP_TYPE_BOOLEAN

        public static readonly PropertyKey DeviceInstanceId                  = new PropertyKey(PropertyCategories.Device4, 256);  // DEVPROP_TYPE_STRING

        public static readonly PropertyKey DeviceContainerId                 = new PropertyKey(PropertyCategories.Device5, 2); // DEVPROP_TYPE_GUID

        public static readonly PropertyKey DeviceModelId                     = new PropertyKey(PropertyCategories.Device6, 2);// DEVPROP_TYPE_GUID
        public static readonly PropertyKey DeviceFriendlyNameAttributes      = new PropertyKey(PropertyCategories.Device6, 3); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceManufacturerAttributes      = new PropertyKey(PropertyCategories.Device6, 4); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DevicePresenceNotForDeviceb       = new PropertyKey(PropertyCategories.Device6, 5); // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceSignalStrength              = new PropertyKey(PropertyCategories.Device6, 6); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceIsAssociateableByUserAction = new PropertyKey(PropertyCategories.Device6, 7); // DEVPROP_TYPE_BOOLEAN


        public static readonly PropertyKey NumaProximityDomain               = new PropertyKey(PropertyCategories.Numa, 1);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceDhpRebalancePolicy          = new PropertyKey(PropertyCategories.Numa, 2);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceNumaNode                    = new PropertyKey(PropertyCategories.Numa, 3);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceBusReportedDeviceDesc       = new PropertyKey(PropertyCategories.Numa, 4);     // DEVPROP_TYPE_STRING

        public static readonly PropertyKey DeviceInstallInProgress           = new PropertyKey(PropertyCategories.Device7, 9);     // DEVPROP_TYPE_BOOLEAN

        //
        // Device driver properties
        //

        public static readonly PropertyKey DeviceDriverDate                  = new PropertyKey(PropertyCategories.Device8, 2);   // DEVPROP_TYPE_FILETIME
        public static readonly PropertyKey DeviceDriverVersion               = new PropertyKey(PropertyCategories.Device8, 3);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverDesc                  = new PropertyKey(PropertyCategories.Device8, 4);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverInfPath               = new PropertyKey(PropertyCategories.Device8, 5);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverInfSection            = new PropertyKey(PropertyCategories.Device8, 6);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverInfSectionExt         = new PropertyKey(PropertyCategories.Device8, 7);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceMatchingDeviceId            = new PropertyKey(PropertyCategories.Device8, 8);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverProvider              = new PropertyKey(PropertyCategories.Device8, 9);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverPropPageProvider      = new PropertyKey(PropertyCategories.Device8, 10);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverCoInstallers          = new PropertyKey(PropertyCategories.Device8, 11);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceResourcePickerTags          = new PropertyKey(PropertyCategories.Device8, 12);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceResourcePickerExceptions    = new PropertyKey(PropertyCategories.Device8, 13); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverRank                  = new PropertyKey(PropertyCategories.Device8, 14); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceDriverLogoLevel             = new PropertyKey(PropertyCategories.Device8, 15); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceNoConnectSound              = new PropertyKey(PropertyCategories.Device8, 17); // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceGenericDriverInstalled      = new PropertyKey(PropertyCategories.Device8, 18); // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceAdditionalSoftwareRequested = new PropertyKey(PropertyCategories.Device8, 19); // DEVPROP_TYPE_BOOLEAN

        //
        // Device safe-removal properties
        //
        public static readonly PropertyKey DeviceSafeRemovalRequired         = new PropertyKey(PropertyCategories.DeviceSafeRemovel, 2); // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceSafeRemovalRequiredOverride = new PropertyKey(PropertyCategories.DeviceSafeRemovel, 3); // DEVPROP_TYPE_BOOLEAN

        //
        // Device properties that were set by the driver package that was installed
        // on the device.
        //
        public static readonly PropertyKey DrvPkgModel                       = new PropertyKey(PropertyCategories.DriverPackage, 2);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DrvPkgVendorWebSite               = new PropertyKey(PropertyCategories.DriverPackage, 3);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DrvPkgDetailedDescription         = new PropertyKey(PropertyCategories.DriverPackage, 4);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DrvPkgDocumentationLink           = new PropertyKey(PropertyCategories.DriverPackage, 5);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DrvPkgIcon                        = new PropertyKey(PropertyCategories.DriverPackage, 6);     // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DrvPkgBrandingIcon                = new PropertyKey(PropertyCategories.DriverPackage, 7);     // DEVPROP_TYPE_STRING_LIST

        //
        // Device setup class properties
        // These s correspond to the old setupapi SPCRP_XXX properties
        //
        public static readonly PropertyKey DeviceClassUpperFilters           = new PropertyKey(PropertyCategories.DeviceClass1, 19);    // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceClassLowerFilters           = new PropertyKey(PropertyCategories.DeviceClass1, 20);    // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceClassSecurity               = new PropertyKey(PropertyCategories.DeviceClass1, 25);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR
        public static readonly PropertyKey DeviceClassSecuritySds            = new PropertyKey(PropertyCategories.DeviceClass1, 26);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        public static readonly PropertyKey DeviceClassDevType                = new PropertyKey(PropertyCategories.DeviceClass1, 27);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceClassExclusive              = new PropertyKey(PropertyCategories.DeviceClass1, 28);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceClassCharacteristics        = new PropertyKey(PropertyCategories.DeviceClass1, 29);    // DEVPROP_TYPE_UINT32

        //
        // Device setup class properties
        // These s correspond to registry values under the device class GUID key
        //
        public static readonly PropertyKey DeviceClassName                   = new PropertyKey(PropertyCategories.DeviceClass2, 2);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassClassName              = new PropertyKey(PropertyCategories.DeviceClass2, 3);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassIcon                   = new PropertyKey(PropertyCategories.DeviceClass2, 4);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassClassInstaller         = new PropertyKey(PropertyCategories.DeviceClass2, 5);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassPropPageProvider       = new PropertyKey(PropertyCategories.DeviceClass2, 6);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassNoInstallClass         = new PropertyKey(PropertyCategories.DeviceClass2, 7);   // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceClassNoDisplayClass         = new PropertyKey(PropertyCategories.DeviceClass2, 8);   // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceClassSilentInstall          = new PropertyKey(PropertyCategories.DeviceClass2, 9);   // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceClassNoUseClass             = new PropertyKey(PropertyCategories.DeviceClass2, 10);  // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceClassDefaultService         = new PropertyKey(PropertyCategories.DeviceClass2, 11);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassIconPath               = new PropertyKey(PropertyCategories.DeviceClass2, 12);  // DEVPROP_TYPE_STRING_LIST

        //
        // Other Device setup class properties
        //

        public static readonly PropertyKey DeviceClassClassCoInstallers      = new PropertyKey(PropertyCategories.DeviceClass3, 2); // DEVPROP_TYPE_STRING_LIST

        //
        // Device interface properties
        //

        public static readonly PropertyKey DeviceInterfaceFriendlyName       = new PropertyKey(PropertyCategories.DeviceInterface, 2); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceInterfaceEnabled            = new PropertyKey(PropertyCategories.DeviceInterface, 3); // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceInterfaceClassGuid          = new PropertyKey(PropertyCategories.DeviceInterface, 4); // DEVPROP_TYPE_GUID

        //
        // Device interface class properties
        //

        public static readonly PropertyKey DeviceInterfaceClassDefaultInterface = new PropertyKey(PropertyCategories.DeviceIterfaceClass, 2);  // DEVPROP_TYPE_STRING

        public static readonly PropertyKey AudioEndpointFormFactor                = new PropertyKey(PropertyCategories.AudioEndpoint, 0); 
        public static readonly PropertyKey AudioEndpointControlPanelPageProvider  = new PropertyKey(PropertyCategories.AudioEndpoint, 1); 
        public static readonly PropertyKey AudioEndpointAssociation               = new PropertyKey(PropertyCategories.AudioEndpoint, 2); 
        public static readonly PropertyKey AudioEndpointPhysicalSpeakers          = new PropertyKey(PropertyCategories.AudioEndpoint, 3); 
        public static readonly PropertyKey AudioEndpointGuid                      = new PropertyKey(PropertyCategories.AudioEndpoint, 4); 
        public static readonly PropertyKey AudioEndpointDisableSysFx              = new PropertyKey(PropertyCategories.AudioEndpoint, 5); 
        public static readonly PropertyKey AudioEndpointFullRangeSpeakers         = new PropertyKey(PropertyCategories.AudioEndpoint, 6); 
        public static readonly PropertyKey AudioEndpointSupportsEventDrivenMode   = new PropertyKey(PropertyCategories.AudioEndpoint, 7); 
        public static readonly PropertyKey AudioEndpointJackSubType               = new PropertyKey(PropertyCategories.AudioEndpoint, 8);


        public static readonly PropertyKey AudioEngineDeviceFormat                = new PropertyKey(PropertyCategories.AudioEngine1, 0);

        public static readonly PropertyKey AudioEngineOemFormat                   = new PropertyKey(PropertyCategories.AudioEngine2, 3);
    }
}



                                                                                                 