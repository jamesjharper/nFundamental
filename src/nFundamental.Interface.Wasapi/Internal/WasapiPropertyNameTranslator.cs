using System;
using System.Collections.Generic;
using System.Linq;
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
        /// The property name to Property key map.
        /// Is used to translate between property names and their GUIDs 
        /// </summary>
        public static TwoWayMap<string, PropertyKey> PropertyKeyLookUpTable = new TwoWayMap<string, PropertyKey>
        {
            {"Name",                                    PropertyKeys.Name                                 },
            {"Device.DeviceDesc",                       PropertyKeys.DeviceDeviceDesc                     },
            {"Device.HardwareIds",                      PropertyKeys.DeviceHardwareIds                    },
            {"Device.CompatibleIds",                    PropertyKeys.DeviceCompatibleIds                  },
            {"Device.Service",                          PropertyKeys.DeviceService                        },
            {"Device.Class",                            PropertyKeys.DeviceClass                          },
            {"Device.ClassGuid",                        PropertyKeys.DeviceClassGuid                      },
            {"Device.Driver",                           PropertyKeys.DeviceDriver                         },
            {"Device.ConfigFlags",                      PropertyKeys.DeviceConfigFlags                    },
            {"Device.Manufacturer",                     PropertyKeys.DeviceManufacturer                   },
            {"Device.FriendlyName",                     PropertyKeys.DeviceFriendlyName                   },
            {"Device.LocationInfo",                     PropertyKeys.DeviceLocationInfo                   },
            {"Device.PdoName",                          PropertyKeys.DevicePdoName                        },
            {"Device.Capabilities",                     PropertyKeys.DeviceCapabilities                   },
            {"Device.UiNumber",                         PropertyKeys.DeviceUiNumber                       },
            {"Device.UpperFilters",                     PropertyKeys.DeviceUpperFilters                   },
            {"Device.LowerFilters",                     PropertyKeys.DeviceLowerFilters                   },
            {"Device.BusTypeGuid",                      PropertyKeys.DeviceBusTypeGuid                    },
            {"Device.LegacyBusType",                    PropertyKeys.DeviceLegacyBusType                  },
            {"Device.BusNumber",                        PropertyKeys.DeviceBusNumber                      },
            {"Device.EnumeratorName",                   PropertyKeys.DeviceEnumeratorName                 },
            {"Device.Security",                         PropertyKeys.DeviceSecurity                       },
            {"Device.SecuritySds",                      PropertyKeys.DeviceSecuritySds                    },
            {"Device.DevType",                          PropertyKeys.DeviceDevType                        },
            {"Device.Exclusive",                        PropertyKeys.DeviceExclusive                      },
            {"Device.Characteristics",                  PropertyKeys.DeviceCharacteristics                },
            {"Device.Address",                          PropertyKeys.DeviceAddress                        },
            {"Device.UiNumberDescFormat",               PropertyKeys.DeviceUiNumberDescFormat             },
            {"Device.PowerData",                        PropertyKeys.DevicePowerData                      },
            {"Device.RemovalPolicy",                    PropertyKeys.DeviceRemovalPolicy                  },
            {"Device.RemovalPolicyDefault",             PropertyKeys.DeviceRemovalPolicyDefault           },
            {"Device.RemovalPolicyOverride ",           PropertyKeys.DeviceRemovalPolicyOverride          },
            {"Device.InstallState",                     PropertyKeys.DeviceInstallState                   },
            {"Device.LocationPaths",                    PropertyKeys.DeviceLocationPaths                  },
            {"Device.BaseContainerId",                  PropertyKeys.DeviceBaseContainerId                },
            {"Device.DevNodeStatus",                    PropertyKeys.DeviceDevNodeStatus                  },
            {"Device.ProblemCode",                      PropertyKeys.DeviceProblemCode                    },
            {"Device.EjectionRelations",                PropertyKeys.DeviceEjectionRelations              },
            {"Device.RemovalRelations",                 PropertyKeys.DeviceRemovalRelations               },
            {"Device.PowerRelations",                   PropertyKeys.DevicePowerRelations                 },
            {"Device.BusRelationsn ",                   PropertyKeys.DeviceBusRelationsn                  },
            {"Device.Parent",                           PropertyKeys.DeviceParent                         },
            {"Device.Children",                         PropertyKeys.DeviceChildren                       },
            {"Device.Siblings",                         PropertyKeys.DeviceSiblings                       },
            {"Device.TransportRelations",               PropertyKeys.DeviceTransportRelations             },
            {"Device.Reported",                         PropertyKeys.DeviceReported                       },
            {"Device.Legacy",                           PropertyKeys.DeviceLegacy                         },
            {"Device.InstanceId",                       PropertyKeys.DeviceInstanceId                     },
            {"Device.ContainerId",                      PropertyKeys.DeviceContainerId                    },
            {"Device.ModelId",                          PropertyKeys.DeviceModelId                        },
            {"Device.FriendlyNameAttributes",           PropertyKeys.DeviceFriendlyNameAttributes         },
            {"Device.ManufacturerAttributes",           PropertyKeys.DeviceManufacturerAttributes         },
            {"Device.PresenceNotForDeviceb",            PropertyKeys.DevicePresenceNotForDeviceb          },
            {"Device.SignalStrength",                   PropertyKeys.DeviceSignalStrength                 },
            {"Device.IsAssociateableByUserAction",      PropertyKeys.DeviceIsAssociateableByUserAction    },
            {"Numa.ProximityDomain",                    PropertyKeys.NumaProximityDomain                  },
            {"Device.DhpRebalancePolicy",               PropertyKeys.DeviceDhpRebalancePolicy             },
            {"Device.NumaNode",                         PropertyKeys.DeviceNumaNode                       },
            {"Device.BusReportedDeviceDesc",            PropertyKeys.DeviceBusReportedDeviceDesc          },
            {"Device.InstallInProgress",                PropertyKeys.DeviceInstallInProgress              },
            {"Device.DriverDate",                       PropertyKeys.DeviceDriverDate                     },
            {"Device.DriverVersion",                    PropertyKeys.DeviceDriverVersion                  },
            {"Device.DriverDesc",                       PropertyKeys.DeviceDriverDesc                     },
            {"Device.DriverInfPath",                    PropertyKeys.DeviceDriverInfPath                  },
            {"Device.DriverInfSection",                 PropertyKeys.DeviceDriverInfSection               },
            {"Device.DriverInfSectionExt",              PropertyKeys.DeviceDriverInfSectionExt            },
            {"Device.MatchingDeviceId",                 PropertyKeys.DeviceMatchingDeviceId               },
            {"Device.DriverProvider",                   PropertyKeys.DeviceDriverProvider                 },
            {"Device.DriverPropPageProvider",           PropertyKeys.DeviceDriverPropPageProvider         },
            {"Device.DriverCoInstallers",               PropertyKeys.DeviceDriverCoInstallers             },
            {"Device.ResourcePickerTags",               PropertyKeys.DeviceResourcePickerTags             },
            {"Device.ResourcePickerExceptions",         PropertyKeys.DeviceResourcePickerExceptions       },
            {"Device.DriverRank",                       PropertyKeys.DeviceDriverRank                     },
            {"Device.DriverLogoLevel",                  PropertyKeys.DeviceDriverLogoLevel                },
            {"Device.NoConnectSound",                   PropertyKeys.DeviceNoConnectSound                 },
            {"Device.GenericDriverInstalled",           PropertyKeys.DeviceGenericDriverInstalled         },
            {"Device.AdditionalSoftwareRequested",      PropertyKeys.DeviceAdditionalSoftwareRequested    },
            {"DeviceSafeRemoval.Required",              PropertyKeys.DeviceSafeRemovalRequired            },
            {"DeviceSafeRemoval.RequiredOverride",      PropertyKeys.DeviceSafeRemovalRequiredOverride    },
            {"DriverPackage.Model",                     PropertyKeys.DrvPkgModel                          },
            {"DriverPackage.VendorWebSite",             PropertyKeys.DrvPkgVendorWebSite                  },
            {"DriverPackage.DetailedDescription",       PropertyKeys.DrvPkgDetailedDescription            },
            {"DriverPackage.DocumentationLink",         PropertyKeys.DrvPkgDocumentationLink              },
            {"DriverPackage.BrandingIcon",              PropertyKeys.DrvPkgBrandingIcon                   },
            {"DriverPackage.Icon",                      PropertyKeys.DrvPkgIcon                           },
            {"DeviceClass.UpperFilters",                PropertyKeys.DeviceClassUpperFilters              },
            {"DeviceClass.LowerFilters ",               PropertyKeys.DeviceClassLowerFilters              },
            {"DeviceClass.Security",                    PropertyKeys.DeviceClassSecurity                  },
            {"DeviceClass.SecuritySds",                 PropertyKeys.DeviceClassSecuritySds               },
            {"DeviceClass.DevType",                     PropertyKeys.DeviceClassDevType                   },
            {"DeviceClass.Exclusive",                   PropertyKeys.DeviceClassExclusive                 },
            {"DeviceClass.Characteristics",             PropertyKeys.DeviceClassCharacteristics           },
            {"DeviceClass.Name",                        PropertyKeys.DeviceClassName                      },
            {"DeviceClass.ClassName",                   PropertyKeys.DeviceClassClassName                 },
            {"DeviceClass.Icon",                        PropertyKeys.DeviceClassIcon                      },
            {"DeviceClass.ClassInstaller",              PropertyKeys.DeviceClassClassInstaller            },
            {"DeviceClass.PropPageProvider",            PropertyKeys.DeviceClassPropPageProvider          },
            {"DeviceClass.NoInstallClass",              PropertyKeys.DeviceClassNoInstallClass            },
            {"DeviceClass.NoDisplayClass",              PropertyKeys.DeviceClassNoDisplayClass            },
            {"DeviceClass.SilentInstall",               PropertyKeys.DeviceClassSilentInstall             },
            {"DeviceClass.NoUseClass",                  PropertyKeys.DeviceClassNoUseClass                },
            {"DeviceClass.DefaultService",              PropertyKeys.DeviceClassDefaultService            },
            {"DeviceClass.IconPath",                    PropertyKeys.DeviceClassIconPath                  },
            {"DeviceClass.ClassCoInstallers",           PropertyKeys.DeviceClassClassCoInstallers         },
            {"DeviceInterface.FriendlyName",            PropertyKeys.DeviceInterfaceFriendlyName          },
            {"DeviceInterface.Enabled",                 PropertyKeys.DeviceInterfaceEnabled               },
            {"DeviceInterface.ClassGuid",               PropertyKeys.DeviceInterfaceClassGuid             },
            {"DeviceInterface.ClassDefaultInterface",   PropertyKeys.DeviceInterfaceClassDefaultInterface },
            {"AudioEndpoint.FormFactor",                PropertyKeys.AudioEndpointFormFactor              },
            {"AudioEndpoint.ControlPanelPageProvider",  PropertyKeys.AudioEndpointControlPanelPageProvider},
            {"AudioEndpoint.Association",               PropertyKeys.AudioEndpointAssociation             },
            {"AudioEndpoint.PhysicalSpeakers",          PropertyKeys.AudioEndpointPhysicalSpeakers        },
            {"AudioEndpoint.Guid",                      PropertyKeys.AudioEndpointGuid                    },
            {"AudioEndpoint.DisableSysFx",              PropertyKeys.AudioEndpointDisableSysFx            },
            {"AudioEndpoint.FullRangeSpeakers",         PropertyKeys.AudioEndpointFullRangeSpeakers       },
            {"AudioEndpoint.SupportsEventDrivenMode",   PropertyKeys.AudioEndpointSupportsEventDrivenMode },
            {"AudioEndpoint.JackSubType",               PropertyKeys.AudioEndpointJackSubType             },
            {"AudioEngine.DeviceFormat",                PropertyKeys.AudioEngineDeviceFormat              },
            {"AudioEngine.OemFormat",                   PropertyKeys.AudioEngineOemFormat                 },
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="WasapiPropertyNameTranslator"/> class.
        /// </summary>
        /// <param name="wasapiOptions">The WASAPI settings.</param>
        public WasapiPropertyNameTranslator(IOptions<WasapiOptions> wasapiOptions)
        {
            _wasapiOptions = wasapiOptions;
        }

        /// <summary>
        /// Resolves the property key from the WASAPI Property key object.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public WasapiPropertyBagKey ResolvePropertyKey(PropertyKey key)
        {
            string propertyId;
            var isKnownValue = ResolvePropertyId(key, out propertyId);

            // Omit unknown keys
            if (!isKnownValue && Options.DeviceInfo.OmitUnknownDeviceProperty)
            {
                return null;
            }

            var propertyCategory    = ResolvePropertyCategory(propertyId);
            var propertyName        = ResolvePropertyName(propertyId);

            return new WasapiPropertyBagKey(propertyId, propertyName, propertyCategory, key.PropertyId, key.FormatId, isKnownValue);
        }


        /// <summary>
        /// Resolves the property key.
        /// </summary>
        /// <param name="keyId"> The Property key Id.</param>
        /// <returns></returns>
        public PropertyKey ResolvePropertyKey(string keyId)
        {
            PropertyKey propertyKey;
            return !PropertyKeyLookUpTable.TryGetValue(keyId, out propertyKey) ? new PropertyKey(Guid.Empty, 0) : propertyKey;
        }


        /// <summary>
        /// Resolves the name of the property.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool ResolvePropertyId(PropertyKey key, out string id)
        {
            if (PropertyKeyLookUpTable.TryGetValue(key, out id))
            {
                return true;
            }

            id = $"{key.FormatId}.{key.FormatId}";
            return false;
        }

        /// <summary>
        /// Resolves the name of the property.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string ResolvePropertyCategory(string id)
        {
            var components = id.Split('.');
            return components.Length == 1 ? string.Empty : components.FirstOrDefault();
        }

        /// <summary>
        /// Resolves the name of the property.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string ResolvePropertyName(string id)
        {
            return id.Split('.').LastOrDefault();
        }
    }
}
