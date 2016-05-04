using SkyView;
using System;
using System.Runtime.InteropServices;

namespace SkyView.RenderEngine {

    public class RenderEngine {

        public RenderEngine() {
            //application_info.sType = Vk.StructureType.VK_STRUCTURE_TYPE_APPLICATION_INFO;
            //application_info.apiVersion = Vk.API_VERSION;
            //application_info.applicationVersion = Vk.MAKE_VERSION(0, 1, 0);
            //application_info.pApplicationName = "SkyView";
            //application_info.engineVersion = Vk.MAKE_VERSION(0, 0, 1);
            //application_info.pEngineName = "SyView Renderer";

            //Vk.InstanceCreateInfo instance_create_info = new Vk.InstanceCreateInfo();
            //instance_create_info.sType = Vk.StructureType.VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
            //instance_create_info.pApplicationInfo = application_info;
            //instance_create_info.enabledLayerCount = 0;
            //instance_create_info.ppEnabledExtensionNames = null;
            //instance_create_info.enabledExtensionCount = 0;
            //instance_create_info.ppEnabledExtensionNames = null;

            //IntPtr instance = Marshal.AllocHGlobal(Marshal.SizeOf<Vk.Instance>());
            //Vk.Result vr = Vk.CreateInstance(ref instance_create_info, IntPtr.Zero, out instance);
        }

        ~RenderEngine() {
            //Vk.DestroyInstance(instance, Vk.NULL_HANDLE);
            //Vk.DestroyDevice(device, Vk.NULL_HANDLE);
        }

        Vk.Instance             instance;
        Vk.Device               device;
        Vk.ApplicationInfo      application_info = new Vk.ApplicationInfo();
    }

}
