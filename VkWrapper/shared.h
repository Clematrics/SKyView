#pragma once

#include <iostream>
#include <sstream>
#ifdef _WIN32
#include <Windows.h>
#endif


#include "Lib\vulkan.h"

namespace VkWrapper {

	static class shared
	{
	public:
		shared( );
		static VkBool32 VulkanDebugCallback( 
			VkDebugReportFlagsEXT		flags,
			VkDebugReportObjectTypeEXT	obj_type,
			uint64_t					src_obj,
			size_t						location,
			int32_t						msg_code,
			const char *				layer_prefix,
			const char *				msg,
			void *						user_data );
	};

}
