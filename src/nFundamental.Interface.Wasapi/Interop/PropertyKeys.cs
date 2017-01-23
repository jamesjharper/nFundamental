using System;
using Fundamental.Interface.Wasapi.Internal;

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

        public static readonly PropertyKey Name                              = new PropertyKey( new Guid(0xb725f130, 0x47ef, 0x101a, 0xa5, 0xf1, 0x02, 0x60, 0x8c, 0x9e, 0xeb, 0xac), 10); // DEVPROP_TYPE_STRING

        //
        // Device properties
        // These s correspond to the old setupapi SPDRP_XXX properties
        //
        public static readonly PropertyKey DeviceDeviceDesc                  = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 2);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceHardwareIds                 = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 3);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceCompatibleIds               = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 4);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceService                     = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 6);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClass                       = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 9);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassGuid                   = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 10); // DEVPROP_TYPE_GUID
        public static readonly PropertyKey DeviceDriver                      = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 11); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceConfigFlags                 = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 12); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceManufacturer                = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 13); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceFriendlyName                = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 14); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceLocationInfo                = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 15); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DevicePdoName                     = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 16); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceCapabilities                = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 17); // DEVPROP_TYPE_UNINT32
        public static readonly PropertyKey DeviceUiNumber                    = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 18); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceUpperFilters                = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 19); // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceLowerFilters                = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 20); // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceBusTypeGuid                 = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 21); // DEVPROP_TYPE_GUID
        public static readonly PropertyKey DeviceLegacyBusType               = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 22); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceBusNumber                   = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 23); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceEnumeratorName              = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 24); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceSecurity                    = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 25); // DEVPROP_TYPE_SECURITY_DESCRIPTOR
        public static readonly PropertyKey DeviceSecuritySds                 = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 26); // DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        public static readonly PropertyKey DeviceDevType                     = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 27); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceExclusive                   = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 28); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceCharacteristics             = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 29); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceAddress                     = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 30); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceUiNumberDescFormat          = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 31); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DevicePowerData                   = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 32); // DEVPROP_TYPE_BINARY
        public static readonly PropertyKey DeviceRemovalPolicy               = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 33); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceRemovalPolicyDefault        = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 34); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceRemovalPolicyOverride       = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 35); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceInstallState                = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 36); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceLocationPaths               = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 37); // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceBaseContainerId             = new PropertyKey( new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 38); // DEVPROP_TYPE_GUID

        //
        // Device properties
        // These s correspond to a device's status and problem code
        //
        public static readonly PropertyKey DeviceDevNodeStatus               = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 2); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceProblemCode                 = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 3); // DEVPROP_TYPE_UINT32

        //
        // Device properties
        // These s correspond to device relations
        //
        public static readonly PropertyKey DeviceEjectionRelations           = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 4);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceRemovalRelations            = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 5);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DevicePowerRelations              = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 6);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceBusRelationsn               = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 7);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceParent                      = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 8);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceChildren                    = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 9);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceSiblings                    = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 10); // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceTransportRelations          = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 11); // DEVPROP_TYPE_STRING_LIST

        //
        // Other Device properties
        //
        public static readonly PropertyKey DeviceReported                    = new PropertyKey(new Guid(0x80497100, 0x8c73, 0x48b9, 0xaa, 0xd9, 0xce, 0x38, 0x7e, 0x19, 0xc5, 0x6e), 2);    // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceLegacy                      = new PropertyKey(new Guid(0x80497100, 0x8c73, 0x48b9, 0xaa, 0xd9, 0xce, 0x38, 0x7e, 0x19, 0xc5, 0x6e), 3);    // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceInstanceId                  = new PropertyKey(new Guid(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57), 256);  // DEVPROP_TYPE_STRING

        public static readonly PropertyKey DeviceContainerId                 = new PropertyKey( new Guid(0x8c7ed206, 0x3f8a, 0x4827, 0xb3, 0xab, 0xae, 0x9e, 0x1f, 0xae, 0xfc, 0x6c), 2); // DEVPROP_TYPE_GUID

        public static readonly PropertyKey DeviceModelId                     = new PropertyKey( new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), 2);// DEVPROP_TYPE_GUID
        public static readonly PropertyKey DeviceFriendlyNameAttributes      = new PropertyKey( new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), 3); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceManufacturerAttributes      = new PropertyKey( new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), 4); // DEVPROP_TYPE_UINT32

        public static readonly PropertyKey DevicePresenceNotForDeviceb       = new PropertyKey( new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), 5); // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceSignalStrength              = new PropertyKey( new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), 6); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceIsAssociateableByUserAction = new PropertyKey( new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), 7); // DEVPROP_TYPE_BOOLEAN


        public static readonly PropertyKey NumaProximityDomain               = new PropertyKey( new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), 1);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceDhpRebalancePolicy          = new PropertyKey( new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), 2);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceNumaNode                    = new PropertyKey( new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), 3);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceBusReportedDeviceDesc       = new PropertyKey( new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), 4);     // DEVPROP_TYPE_STRING

        public static readonly PropertyKey DeviceInstallInProgress           = new PropertyKey( new Guid(0x83da6326, 0x97a6, 0x4088, 0x94, 0x53, 0xa1, 0x92, 0x3f, 0x57, 0x3b, 0x29), 9);     // DEVPROP_TYPE_BOOLEAN

        //
        // Device driver properties
        //
        public static readonly PropertyKey DeviceDriverDate                  = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 2);   // DEVPROP_TYPE_FILETIME
        public static readonly PropertyKey DeviceDriverVersion               = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 3);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverDesc                  = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 4);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverInfPath               = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 5);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverInfSection            = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 6);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverInfSectionExt         = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 7);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceMatchingDeviceId            = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 8);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverProvider              = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 9);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverPropPageProvider      = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 10);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverCoInstallers          = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 11);  // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceResourcePickerTags          = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 12);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceResourcePickerExceptions    = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 13); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceDriverRank                  = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 14); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceDriverLogoLevel             = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 15); // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceNoConnectSound              = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 17); // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceGenericDriverInstalled      = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 18); // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceAdditionalSoftwareRequested = new PropertyKey( new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 19); // DEVPROP_TYPE_BOOLEAN

        //
        // Device safe-removal properties
        //
        public static readonly PropertyKey DeviceSafeRemovalRequired         = new PropertyKey( new Guid(0xafd97640,  0x86a3, 0x4210, 0xb6, 0x7c, 0x28, 0x9c, 0x41, 0xaa, 0xbe, 0x55), 2); // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceSafeRemovalRequiredOverride = new PropertyKey( new Guid(0xafd97640,  0x86a3, 0x4210, 0xb6, 0x7c, 0x28, 0x9c, 0x41, 0xaa, 0xbe, 0x55), 3); // DEVPROP_TYPE_BOOLEAN

        //
        // Device properties that were set by the driver package that was installed
        // on the device.
        //
        public static readonly PropertyKey DrvPkgModel                       = new PropertyKey( new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), 2);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DrvPkgVendorWebSite               = new PropertyKey( new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), 3);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DrvPkgDetailedDescription         = new PropertyKey( new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), 4);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DrvPkgDocumentationLink           = new PropertyKey( new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), 5);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DrvPkgIcon                        = new PropertyKey( new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), 6);     // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DrvPkgBrandingIcon                = new PropertyKey( new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), 7);     // DEVPROP_TYPE_STRING_LIST

        //
        // Device setup class properties
        // These s correspond to the old setupapi SPCRP_XXX properties
        //
        public static readonly PropertyKey DeviceClassUpperFilters           = new PropertyKey( new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 19);    // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceClassLowerFilters           = new PropertyKey( new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 20);    // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey DeviceClassSecurity               = new PropertyKey( new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 25);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR
        public static readonly PropertyKey DeviceClassSecuritySds            = new PropertyKey( new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 26);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        public static readonly PropertyKey DeviceClassDevType                = new PropertyKey( new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 27);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceClassExclusive              = new PropertyKey( new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 28);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey DeviceClassCharacteristics        = new PropertyKey( new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 29);    // DEVPROP_TYPE_UINT32

        //
        // Device setup class properties
        // These s correspond to registry values under the device class GUID key
        //
        public static readonly PropertyKey DeviceClassName                   = new PropertyKey( new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 2);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassClassName              = new PropertyKey( new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 3);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassIcon                   = new PropertyKey( new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 4);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassClassInstaller         = new PropertyKey( new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 5);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassPropPageProvider       = new PropertyKey( new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 6);   // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassNoInstallClass         = new PropertyKey( new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 7);   // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceClassNoDisplayClass         = new PropertyKey( new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 8);   // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceClassSilentInstall          = new PropertyKey( new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 9);   // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceClassNoUseClass             = new PropertyKey( new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 10);  // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceClassDefaultService         = new PropertyKey( new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 11);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceClassIconPath               = new PropertyKey( new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 12);  // DEVPROP_TYPE_STRING_LIST

        //
        // Other Device setup class properties
        //
        public static readonly PropertyKey DeviceClassClassCoInstallers      = new PropertyKey(new Guid(0x713d1703, 0xa2e2, 0x49f5, 0x92, 0x14, 0x56, 0x47, 0x2e, 0xf3, 0xda, 0x5c), 2); // DEVPROP_TYPE_STRING_LIST

        //
        // Device interface properties
        //
        public static readonly PropertyKey DeviceInterfaceFriendlyName       = new PropertyKey( new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22), 2); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey DeviceInterfaceEnabled            = new PropertyKey( new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22), 3); // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey DeviceInterfaceClassGuid          = new PropertyKey( new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22), 4); // DEVPROP_TYPE_GUID

        //
        // Device interface class properties
        //
        public static readonly PropertyKey DeviceInterfaceClassDefaultInterface = new PropertyKey(new Guid(0x14c83a99, 0x0b3f, 0x44b7, 0xbe, 0x4c, 0xa1, 0x78, 0xd3, 0x99, 0x05, 0x64), 2);  // DEVPROP_TYPE_STRING


        public static readonly PropertyKey AudioEndpointFormFactor                = new PropertyKey( new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e), 0); 
        public static readonly PropertyKey AudioEndpointControlPanelPageProvider  = new PropertyKey( new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e), 1); 
        public static readonly PropertyKey AudioEndpointAssociation               = new PropertyKey( new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e), 2); 
        public static readonly PropertyKey AudioEndpointPhysicalSpeakers          = new PropertyKey( new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e), 3); 
        public static readonly PropertyKey AudioEndpointGuid                      = new PropertyKey( new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e), 4); 
        public static readonly PropertyKey AudioEndpointDisableSysFx              = new PropertyKey( new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e), 5); 
        public static readonly PropertyKey AudioEndpointFullRangeSpeakers         = new PropertyKey( new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e), 6); 
        public static readonly PropertyKey AudioEndpointSupportsEventDrivenMode   = new PropertyKey( new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e), 7); 
        public static readonly PropertyKey AudioEndpointJackSubType               = new PropertyKey( new Guid(0x1da5d803, 0xd492, 0x4edd, 0x8c, 0x23, 0xe0, 0xc0, 0xff, 0xee, 0x7f, 0x0e), 8);  
        public static readonly PropertyKey AudioEngineDeviceFormat                = new PropertyKey( new Guid(0xf19f064d, 0x82c, 0x4e27, 0xbc, 0x73, 0x68, 0x82, 0xa1, 0xbb, 0x8e, 0x4c), 0);
        public static readonly PropertyKey AudioEngineOemFormat                   = new PropertyKey( new Guid(0xe4870e26, 0x3cc5, 0x4cd2, 0xba, 0x46, 0xca, 0xa, 0x9a, 0x70, 0xed, 0x4), 3);
    }
}



                                                                                                 