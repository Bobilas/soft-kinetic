#pragma once

#include "Helper.h"
#include "Locks.h"
#include <DepthSense.hxx>
#include <vector>
#include <msclr\lock.h>

using namespace System;
using namespace DepthSense;
using namespace std;
using namespace msclr;

namespace DepthSenseWrapper
{
	private class DepthSensor_Unwrapped
	{
	public:
		const static int Width = 320;
		const static int Height = 240;
		static short DepthMap[Width * Height];

		static void Setup();
		static void Dispose();

		static void Run();
		static void Stop();

		static bool GetDepthMap();
		static bool CheckDepthMap();
	private:
		static bool _getDepthMap;
		static bool _deviceFound;
		static bool _stop;
		static bool _isRunning;

		static Context _context;
		static DepthNode _depthNode;
		static ColorNode _colorNode;
		static AudioNode _audioNode;
		static StereoCameraParameters _stereoCameraParams;

		static bool CheckStop();

		static void OnNewDepthSample(DepthNode node, DepthNode::NewSampleReceivedData data);
		static void OnNewAudioSample(AudioNode node, AudioNode::NewSampleReceivedData data);
		static void OnNewColorSample(ColorNode node, ColorNode::NewSampleReceivedData data);

		static void ConfigureDepthNode();
		static void ConfigureColorNode();
		static void ConfigureAudioNode();

		static void ConfigureNode(Node node);

		static void OnNodeAdded(Device device, Device::NodeAddedData data);
		static void OnNodeRemoved(Device device, Device::NodeRemovedData data);

		static void OnDeviceAdded(Context context, Context::DeviceAddedData data);
		static void OnDeviceRemoved(Context context, Context::DeviceRemovedData data);
	};
}
