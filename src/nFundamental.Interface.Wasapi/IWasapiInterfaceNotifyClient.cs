namespace Fundamental.Interface.Wasapi
{
    public interface IWasapiInterfaceNotifyClient :
       IDeviceStatusNotifier,
       IDefaultDeviceStatusNotifier,
       IDeviceAvailabilityNotifier
    {
    }
}
