// Il s'agit du fichier DLL principal.
#include "VkWrapper.h"

namespace VkWrapper {

		Renderer::Renderer() {

			// Initialize debugging and validation
			_instance_layers.push_back( "VK_LAYER_LUNARG_threading" );
			_instance_layers.push_back( "VK_LAYER_LUNARG_image" );
			_instance_layers.push_back( "VK_LAYER_LUNARG_mem_tracker" );
			_instance_layers.push_back( "VK_LAYER_LUNARG_object_tracker" );
			_instance_layers.push_back( "VK_LAYER_LUNARG_param_tracker" );

			_instance_extensions.push_back( VK_EXT_DEBUG_REPORT_EXTENSION_NAME );

			_device_layers.push_back( "VK_LAYER_LUNARG_threading" );
			_device_layers.push_back( "VK_LAYER_LUNARG_image" );
			_device_layers.push_back( "VK_LAYER_LUNARG_mem_tracker" );
			_device_layers.push_back( "VK_LAYER_LUNARG_object_tracker" );
			_device_layers.push_back( "VK_LAYER_LUNARG_param_tracker" );

			_initInstance( );
			_initDebugging( );
			_initDevice( );

			vkGetDeviceQueue( _device, _queue_family_index, 0, &_queue );
		}
		Renderer::~Renderer() {
			PFN_vkDestroyDebugReportCallbackEXT fct_vkDestroyDebugReportCallbackEXT			= ( PFN_vkDestroyDebugReportCallbackEXT )vkGetInstanceProcAddr( _instance, "vkDestroyDebugReportCallbackEXT" );
			if ( fct_vkDestroyDebugReportCallbackEXT == nullptr ) {
				assert( 0 && "Unable to find the destroying debug function pointer" );
			}
			fct_vkDestroyDebugReportCallbackEXT( _instance, _debug_report_callback, nullptr );

			vkDestroyInstance(_instance, nullptr);
			_instance = VK_NULL_HANDLE;

			vkDestroyDevice(_device, nullptr);
			_device = VK_NULL_HANDLE;
		}

		void Renderer::loadTexture( ) {

		}

		void Renderer::_initDebugging( ) {
			PFN_vkCreateDebugReportCallbackEXT	fct_vkCreateDebugReportCallbackEXT			= ( PFN_vkCreateDebugReportCallbackEXT )vkGetInstanceProcAddr( _instance, "vkCreateDebugReportCallbackEXT" );
			if ( fct_vkCreateDebugReportCallbackEXT == nullptr ) {
				assert( 0 && "Unable to find the creating debug function pointers" );
			}

			VkDebugReportCallbackCreateInfoEXT debug_report_callback_create_info{ };
			debug_report_callback_create_info.sType				= VK_STRUCTURE_TYPE_DEBUG_REPORT_CREATE_INFO_EXT;
			debug_report_callback_create_info.flags				= VK_DEBUG_REPORT_ERROR_BIT_EXT | VK_DEBUG_REPORT_WARNING_BIT_EXT | VK_DEBUG_REPORT_PERFORMANCE_WARNING_BIT_EXT | VK_DEBUG_REPORT_DEBUG_BIT_EXT | VK_DEBUG_REPORT_INFORMATION_BIT_EXT;
			debug_report_callback_create_info.pfnCallback		= (PFN_vkDebugReportCallbackEXT)&shared::VulkanDebugCallback;

			fct_vkCreateDebugReportCallbackEXT( _instance, &debug_report_callback_create_info, nullptr, &_debug_report_callback);
		}

		void Renderer::_initInstance( ) {
			VkApplicationInfo application_info{};
			application_info.sType					= VK_STRUCTURE_TYPE_APPLICATION_INFO;
			application_info.apiVersion				= VK_API_VERSION;
			application_info.applicationVersion		= VK_MAKE_VERSION( 1, 0, 0 );
			application_info.pApplicationName		= "SkyView";
			application_info.engineVersion			= VK_MAKE_VERSION( 1, 0, 0 );
			application_info.pEngineName			= "RenderEngine";

			VkInstanceCreateInfo instance_create_info{};
			instance_create_info.sType						= VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
			instance_create_info.pApplicationInfo			= &application_info;
			instance_create_info.enabledLayerCount			= _instance_layers.size( );
			instance_create_info.ppEnabledLayerNames		= _instance_layers.data( );
			instance_create_info.enabledExtensionCount		= _instance_extensions.size( );
			instance_create_info.ppEnabledExtensionNames	= _instance_extensions.data( );

			_ASSERT( !vkCreateInstance( &instance_create_info, nullptr, &_instance ) );
		}

		void Renderer::_initDevice( ) {

			// Get gpu and properties
			uint32_t gpu_count = 0;
			vkEnumeratePhysicalDevices( _instance, &gpu_count, VK_NULL_HANDLE );
			std::vector<VkPhysicalDevice> gpus( gpu_count );
			vkEnumeratePhysicalDevices( _instance, &gpu_count, gpus.data( ) );
			_gpu = gpus[ 0 ];

			vkGetPhysicalDeviceProperties( _gpu, &_gpu_properties );

			// Get a compute queue
			uint32_t queue_family_count = 0;
			vkGetPhysicalDeviceQueueFamilyProperties( _gpu, &queue_family_count, VK_NULL_HANDLE );
			std::vector<VkQueueFamilyProperties> queue_family_properties( queue_family_count );
			vkGetPhysicalDeviceQueueFamilyProperties( _gpu, &queue_family_count, queue_family_properties.data( ) );
			bool IsQueueFound = false;
			for ( uint32_t i = 0; i < queue_family_count; ++i ) {
				if ( queue_family_properties[ i ].queueFlags & VK_QUEUE_COMPUTE_BIT ) {
					IsQueueFound = true;
					_queue_family_index = i;
				}
			}
			if ( !IsQueueFound ) {
				assert( 0 && "Unable to find a queue family supporting computing" );
			}

			// Creating the device
			float queue_priorities[]{ 1.0f };
			VkDeviceQueueCreateInfo device_queue_create_info{ };
			device_queue_create_info.sType				= VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO;
			device_queue_create_info.queueFamilyIndex	= _queue_family_index;
			device_queue_create_info.queueCount			= 1;
			device_queue_create_info.pQueuePriorities	= queue_priorities;

			VkDeviceCreateInfo device_create_info{};
			device_create_info.sType					= VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO;
			device_create_info.queueCreateInfoCount		= 1;
			device_create_info.pQueueCreateInfos		= &device_queue_create_info;
			device_create_info.enabledLayerCount		= _device_layers.size( );
			device_create_info.ppEnabledLayerNames		= _device_layers.data( );
			device_create_info.enabledExtensionCount	= _device_extensions.size( );
			device_create_info.ppEnabledExtensionNames	= _device_extensions.data( );

			_ASSERT( !vkCreateDevice( _gpu, &device_create_info, nullptr, &_device ) );
		}

}

