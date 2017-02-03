using System;
using Fundamental.Core;
using Fundamental.Interface.Wasapi.Interop;
using Fundamental.Interface.Wasapi.Options;

namespace Fundamental.Interface.Wasapi.Internal
{
    public class WasapiPropertyNameTranslator : IWasapiPropertyNameTranslator
    {
        /// <summary>
        /// The WASAPI options
        /// </summary>
        private readonly IOptions<WasapiOptions> _wasapiOptions;

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        private WasapiOptions Options => _wasapiOptions.Value;

        /// <summary>
        /// The property name to unique identifier map which is used
        /// to translate between property names and their GUIDs 
        /// </summary>
        public static TwoWayMap<string, PropertyKey> PropertyKeyLookUpTable = new TwoWayMap<string, PropertyKey>
        {
            {"Name",                                   PropertyKeys.Name                                 },
            {"DeviceDeviceDesc",                       PropertyKeys.DeviceDeviceDesc                     },
            {"DeviceHardwareIds",                      PropertyKeys.DeviceHardwareIds                    },
            {"DeviceCompatibleIds",                    PropertyKeys.DeviceCompatibleIds                  },
            {"DeviceService",                          PropertyKeys.DeviceService                        },
            {"DeviceClass",                            PropertyKeys.DeviceClass                          },
            {"DeviceClassGuid",                        PropertyKeys.DeviceClassGuid                      },
            {"DeviceDriver",                           PropertyKeys.DeviceDriver                         },
            {"DeviceConfigFlags",                      PropertyKeys.DeviceConfigFlags                    },
            {"DeviceManufacturer",                     PropertyKeys.DeviceManufacturer                   },
            {"DeviceFriendlyName",                     PropertyKeys.DeviceFriendlyName                   },
            {"DeviceLocationInfo",                     PropertyKeys.DeviceLocationInfo                   },
            {"DevicePdoName",                          PropertyKeys.DevicePdoName                        },
            {"DeviceCapabilities",                     PropertyKeys.DeviceCapabilities                   },
            {"DeviceUiNumber",                         PropertyKeys.DeviceUiNumber                       },
            {"DeviceUpperFilters",                     PropertyKeys.DeviceUpperFilters                   },
            {"DeviceLowerFilters",                     PropertyKeys.DeviceLowerFilters                   },
            {"DeviceBusTypeGuid",                      PropertyKeys.DeviceBusTypeGuid                    },
            {"DeviceLegacyBusType",                    PropertyKeys.DeviceLegacyBusType                  },
            {"DeviceBusNumber",                        PropertyKeys.DeviceBusNumber                      },
            {"DeviceEnumeratorName",                   PropertyKeys.DeviceEnumeratorName                 },
            {"DeviceSecurity",                         PropertyKeys.DeviceSecurity                       },
            {"DeviceSecuritySds",                      PropertyKeys.DeviceSecuritySds                    },
            {"DeviceDevType",                          PropertyKeys.DeviceDevType                        },
            {"DeviceExclusive",                        PropertyKeys.DeviceExclusive                      },
            {"DeviceCharacteristics",                  PropertyKeys.DeviceCharacteristics                },
            {"DeviceAddress",                          PropertyKeys.DeviceAddress                        },
            {"DeviceUiNumberDescFormat",               PropertyKeys.DeviceUiNumberDescFormat             },
            {"DevicePowerData",                        PropertyKeys.DevicePowerData                      },
            {"DeviceRemovalPolicy",                    PropertyKeys.DeviceRemovalPolicy                  },
            {"DeviceRemovalPolicyDefault",             PropertyKeys.DeviceRemovalPolicyDefault           },
            {"DeviceRemovalPolicyOverride ",           PropertyKeys.DeviceRemovalPolicyOverride          },
            {"DeviceInstallState",                     PropertyKeys.DeviceInstallState                   },
            {"DeviceLocationPaths",                    PropertyKeys.DeviceLocationPaths                  },
            {"DeviceBaseContainerId",                  PropertyKeys.DeviceBaseContainerId                },
            {"DeviceDevNodeStatus",                    PropertyKeys.DeviceDevNodeStatus                  },
            {"DeviceProblemCode",                      PropertyKeys.DeviceProblemCode                    },
            {"DeviceEjectionRelations",                PropertyKeys.DeviceEjectionRelations              },
            {"DeviceRemovalRelations",                 PropertyKeys.DeviceRemovalRelations               },
            {"DevicePowerRelations",                   PropertyKeys.DevicePowerRelations                 },
            {"DeviceBusRelationsn ",                   PropertyKeys.DeviceBusRelationsn                  },
            {"DeviceParent",                           PropertyKeys.DeviceParent                         },
            {"DeviceChildren",                         PropertyKeys.DeviceChildren                       },
            {"DeviceSiblings",                         PropertyKeys.DeviceSiblings                       },
            {"DeviceTransportRelations",               PropertyKeys.DeviceTransportRelations             },
            {"DeviceReported",                         PropertyKeys.DeviceReported                       },
            {"DeviceLegacy",                           PropertyKeys.DeviceLegacy                         },
            {"DeviceInstanceId",                       PropertyKeys.DeviceInstanceId                     },
            {"DeviceContainerId",                      PropertyKeys.DeviceContainerId                    },
            {"DeviceModelId",                          PropertyKeys.DeviceModelId                        },
            {"DeviceFriendlyNameAttributes",           PropertyKeys.DeviceFriendlyNameAttributes         },
            {"DeviceManufacturerAttributes",           PropertyKeys.DeviceManufacturerAttributes         },
            {"DevicePresenceNotForDeviceb",            PropertyKeys.DevicePresenceNotForDeviceb          },
            {"DeviceSignalStrength",                   PropertyKeys.DeviceSignalStrength                 },
            {"DeviceIsAssociateableByUserAction",      PropertyKeys.DeviceIsAssociateableByUserAction    },
            {"NumaProximityDomain",                    PropertyKeys.NumaProximityDomain                  },
            {"DeviceDhpRebalancePolicy",               PropertyKeys.DeviceDhpRebalancePolicy             },
            {"DeviceNumaNode",                         PropertyKeys.DeviceNumaNode                       },
            {"DeviceBusReportedDeviceDesc",            PropertyKeys.DeviceBusReportedDeviceDesc          },
            {"DeviceInstallInProgress",                PropertyKeys.DeviceInstallInProgress              },
            {"DeviceDriverDate",                       PropertyKeys.DeviceDriverDate                     },
            {"DeviceDriverVersion",                    PropertyKeys.DeviceDriverVersion                  },
            {"DeviceDriverDesc",                       PropertyKeys.DeviceDriverDesc                     },
            {"DeviceDriverInfPath",                    PropertyKeys.DeviceDriverInfPath                  },
            {"DeviceDriverInfSection",                 PropertyKeys.DeviceDriverInfSection               },
            {"DeviceDriverInfSectionExt",              PropertyKeys.DeviceDriverInfSectionExt            },
            {"DeviceMatchingDeviceId",                 PropertyKeys.DeviceMatchingDeviceId               },
            {"DeviceDriverProvider",                   PropertyKeys.DeviceDriverProvider                 },
            {"DeviceDriverPropPageProvider",           PropertyKeys.DeviceDriverPropPageProvider         },
            {"DeviceDriverCoInstallers",               PropertyKeys.DeviceDriverCoInstallers             },
            {"DeviceResourcePickerTags",               PropertyKeys.DeviceResourcePickerTags             },
            {"DeviceResourcePickerExceptions",         PropertyKeys.DeviceResourcePickerExceptions       },
            {"DeviceDriverRank",                       PropertyKeys.DeviceDriverRank                     },
            {"DeviceDriverLogoLevel",                  PropertyKeys.DeviceDriverLogoLevel                },
            {"DeviceNoConnectSound",                   PropertyKeys.DeviceNoConnectSound                 },
            {"DeviceGenericDriverInstalled",           PropertyKeys.DeviceGenericDriverInstalled         },
            {"DeviceAdditionalSoftwareRequested",      PropertyKeys.DeviceAdditionalSoftwareRequested    },
            {"DeviceSafeRemovalRequired",              PropertyKeys.DeviceSafeRemovalRequired            },
            {"DeviceSafeRemovalRequiredOverride",      PropertyKeys.DeviceSafeRemovalRequiredOverride    },
            {"DrvPkgModel",                            PropertyKeys.DrvPkgModel                          },
            {"DrvPkgVendorWebSite",                    PropertyKeys.DrvPkgVendorWebSite                  },
            {"DrvPkgDetailedDescription",              PropertyKeys.DrvPkgDetailedDescription            },
            {"DrvPkgDocumentationLink",                PropertyKeys.DrvPkgDocumentationLink              },
            {"DrvPkgBrandingIcon",                     PropertyKeys.DrvPkgBrandingIcon                   },
            {"DrvPkgIcon",                             PropertyKeys.DrvPkgIcon                           },
            {"DeviceClassUpperFilters",                PropertyKeys.DeviceClassUpperFilters              },
            {"DeviceClassLowerFilters ",               PropertyKeys.DeviceClassLowerFilters              },
            {"DeviceClassSecurity",                    PropertyKeys.DeviceClassSecurity                  },
            {"DeviceClassSecuritySds",                 PropertyKeys.DeviceClassSecuritySds               },
            {"DeviceClassDevType",                     PropertyKeys.DeviceClassDevType                   },
            {"DeviceClassExclusive",                   PropertyKeys.DeviceClassExclusive                 },
            {"DeviceClassCharacteristics",             PropertyKeys.DeviceClassCharacteristics           },
            {"DeviceClassName",                        PropertyKeys.DeviceClassName                      },
            {"DeviceClassClassName",                   PropertyKeys.DeviceClassClassName                 },
            {"DeviceClassIcon",                        PropertyKeys.DeviceClassIcon                      },
            {"DeviceClassClassInstaller",              PropertyKeys.DeviceClassClassInstaller            },
            {"DeviceClassPropPageProvider",            PropertyKeys.DeviceClassPropPageProvider          },
            {"DeviceClassNoInstallClass",              PropertyKeys.DeviceClassNoInstallClass            },
            {"DeviceClassNoDisplayClass",              PropertyKeys.DeviceClassNoDisplayClass            },
            {"DeviceClassSilentInstall",               PropertyKeys.DeviceClassSilentInstall             },
            {"DeviceClassNoUseClass",                  PropertyKeys.DeviceClassNoUseClass                },
            {"DeviceClassDefaultService",              PropertyKeys.DeviceClassDefaultService            },
            {"DeviceClassIconPath",                    PropertyKeys.DeviceClassIconPath                  },
            {"DeviceClassClassCoInstallers",           PropertyKeys.DeviceClassClassCoInstallers         },
            {"DeviceInterfaceFriendlyName",            PropertyKeys.DeviceInterfaceFriendlyName          },
            {"DeviceInterfaceEnabled",                 PropertyKeys.DeviceInterfaceEnabled               },
            {"DeviceInterfaceClassGuid",               PropertyKeys.DeviceInterfaceClassGuid             },
            {"DeviceInterfaceClassDefaultInterface",   PropertyKeys.DeviceInterfaceClassDefaultInterface },
            {"AudioEndpointFormFactor",                PropertyKeys.AudioEndpointFormFactor              },
            {"AudioEndpointControlPanelPageProvider",  PropertyKeys.AudioEndpointControlPanelPageProvider},
            {"AudioEndpointAssociation",               PropertyKeys.AudioEndpointAssociation             },
            {"AudioEndpointPhysicalSpeakers",          PropertyKeys.AudioEndpointPhysicalSpeakers        },
            {"AudioEndpointGuid",                      PropertyKeys.AudioEndpointGuid                    },
            {"AudioEndpointDisableSysFx",              PropertyKeys.AudioEndpointDisableSysFx            },
            {"AudioEndpointFullRangeSpeakers",         PropertyKeys.AudioEndpointFullRangeSpeakers       },
            {"AudioEndpointSupportsEventDrivenMode",   PropertyKeys.AudioEndpointSupportsEventDrivenMode },
            {"AudioEndpointJackSubType",               PropertyKeys.AudioEndpointJackSubType             },
            {"AudioEngineDeviceFormat",                PropertyKeys.AudioEngineDeviceFormat              },
            {"AudioEngineOemFormat",                   PropertyKeys.AudioEngineOemFormat                 },
        };
     
        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiPropertyNameTranslator"/> class.
        /// </summary>C:\Projects\Personal\nFundamental\tests\nFundamental.Interface.Wasapi.Tests\WasapiInterfaceNotifyClientTests.cs
        /// <param name="wasapiOptions">The WASAPI settings.</param>
        public WasapiPropertyNameTranslator(IOptions<WasapiOptions> wasapiOptions)
        {
            _wasapiOptions = wasapiOptions;
        }

        /// <summary>
        /// Resolves the name of the property.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string ResolvePropertyName(PropertyKey key)
        {
            string propertyName;
            if(PropertyKeyLookUpTable.TryGetValue(key, out propertyName))
                return propertyName;
            
            return Options.DeviceInfo.OmitUnknownDeviceProperty ? string.Empty : key.ToString();
        }

        /// <summary>
        /// Resolves the property key.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public PropertyKey ResolvePropertyKey(string propertyName)
        {
            PropertyKey propertyKey;
            return !PropertyKeyLookUpTable.TryGetValue(propertyName, out propertyKey) ? new PropertyKey(Guid.Empty, 0) : propertyKey;
        }
    }
}
