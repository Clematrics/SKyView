#include "stdafx.h"
#include "shared.h"

namespace VkWrapper {

	shared::shared( )
	{
	}

	VKAPI_ATTR VkBool32 VKAPI_CALL
	shared::VulkanDebugCallback(
			VkDebugReportFlagsEXT		flags,
			VkDebugReportObjectTypeEXT	obj_type,
			uint64_t					src_obj,
			size_t						location,
			int32_t						msg_code,
			const char *				layer_prefix,
			const char *				msg,
			void *						user_data )
	{
		std::ostringstream stream;
		stream << "VKDBG: ";
		if ( flags & VK_DEBUG_REPORT_INFORMATION_BIT_EXT ) {
			stream << "INFO: ";
		}
		if ( flags & VK_DEBUG_REPORT_WARNING_BIT_EXT ) {
			stream << "WARNING: ";
		}
		if ( flags & VK_DEBUG_REPORT_PERFORMANCE_WARNING_BIT_EXT ) {
			stream << "PERFORMANCE: ";
		}
		if ( flags & VK_DEBUG_REPORT_ERROR_BIT_EXT ) {
			stream << "ERROR: ";
		}
		if ( flags & VK_DEBUG_REPORT_DEBUG_BIT_EXT ) {
			stream << "DEBUG: ";
		}
		stream << "@[" << layer_prefix << "]: ";
		stream << msg << std::endl;

		#ifdef _WIN32
		if ( flags & VK_DEBUG_REPORT_ERROR_BIT_EXT ) {
			MessageBox( NULL, (LPCTSTR)stream.str( ).c_str( ), (LPCTSTR)"Vulkan Error!", 0 );
		}
		#endif

		return false;
	}

}