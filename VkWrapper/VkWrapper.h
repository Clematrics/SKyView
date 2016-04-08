// VkWrapper.h

#pragma once

using namespace System;

#include "stdafx.h"
#include <assert.h>
#include <crtdbg.h>
#include <vector>

#include "Lib\vulkan.h"
#include "shared.h"

namespace VkWrapper {

	public struct Texture 
	{
		VkSampler		_sampler;
		VkImage			_image;
		VkImageLayout	_image_layout;
		VkDeviceMemory	_device_memory;
		VkImageView		_image_view;
		uint32_t		_width, _height;
		uint32_t		_layerCount;
	};

	public class Renderer
	{
	public:
		Renderer();
		~Renderer();

		void loadTexture( );
	private:
		void			_initDebugging( );
		void			_initInstance( );
		void			_initDevice( );

		VkInstance									_instance				= VK_NULL_HANDLE;
		VkDevice									_device					= VK_NULL_HANDLE;
		VkPhysicalDevice							_gpu					= VK_NULL_HANDLE;
		VkPhysicalDeviceProperties					_gpu_properties			= { };
		uint32_t									_queue_family_index		= 0;
		VkQueue										_queue					= VK_NULL_HANDLE;

		std::vector<const char*>					_instance_layers;
		std::vector<const char*>					_instance_extensions;
		std::vector<const char*>					_device_layers;
		std::vector<const char*>					_device_extensions;

		VkDebugReportCallbackEXT					_debug_report_callback	= VK_NULL_HANDLE;

		VkCommandPool								_command_pool			= VK_NULL_HANDLE;
	};

}
