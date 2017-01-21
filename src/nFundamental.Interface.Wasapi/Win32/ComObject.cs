using System;

namespace Fundamental.Interface.Wasapi.Win32
{
    public class ComObject
    {

        public static T CreateInstance<T>(Guid clsId) where T : class
        {
            var clsType = Type.GetTypeFromCLSID(clsId, /* throwOnError */ true);
            var obj = Activator.CreateInstance(clsType);
            return QuearyInterface<T>(obj);
        }


        public static T QuearyInterface<T>(object obj) where T : class 
        {
            var target = obj as T;
            if(target == null)
                throw new InvalidCastException($"QuearyInterface failed to cast {obj.GetType().Name} to type {typeof(T).Name}");
            return target;
        }

    }
}
