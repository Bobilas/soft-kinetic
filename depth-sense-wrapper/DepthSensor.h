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

		static void Setup()
		{
			Locks::Setup();

			{
				lock l(Locks::Run);
				if (_isRunning)
				{
					Stop();
					l.release();
					Setup();
				}
				l.release();
			}

			_isRunning = false;
			_stop = true;
			_deviceFound = false;
			_getDepthMap = false;

			_context = Context::create("localhost");

			_context.deviceAddedEvent().connect(&OnDeviceAdded);
			_context.deviceRemovedEvent().connect(&OnDeviceRemoved);

			vector<Device> devices = _context.getDevices();

			if (devices.size() >= 1)
			{
				_deviceFound = true;

				devices[0].nodeAddedEvent().connect(&OnNodeAdded);
				devices[0].nodeRemovedEvent().connect(&OnNodeRemoved);

				vector<Node> nodes = devices[0].getNodes();

				for (int i = 0; i < nodes.size(); i++)
				{
					ConfigureNode(nodes[i]);
				}
			}
		}
		static void Dispose()
		{
			_context.stopNodes();

			if (_depthNode.isSet()) _context.unregisterNode(_depthNode);
			//if (_colorNode.isSet()) _context.unregisterNode(_colorNode);
			//if (_audioNode.isSet()) _context.unregisterNode(_audioNode);
		}

		static void Run()
		{
			if (_deviceFound)
			{
				{
					lock stopLock(Locks::Stop);
					lock runLock(Locks::Run);
					_stop = false;
					_isRunning = true;
				}
				_context.startNodes();
				_context.run();
			}
		}
		static void Stop()
		{
			{
				lock stopLock(Locks::Stop);
				_stop = true;
			}
		}

		static bool GetDepthMap()
		{
			{
				lock l(Locks::GetDepthMap);
				_getDepthMap = true;
			}

			return true;
		}
		static bool CheckDepthMap()
		{
			{
				lock l(Locks::GetDepthMap);
				return !_getDepthMap;
			}
		}
	private:
		static bool _getDepthMap;
		static bool _deviceFound;
		static bool _stop;
		static bool _isRunning;

		static Context _context;
		static DepthNode _depthNode;
		//static ColorNode _colorNode;
		//static AudioNode _audioNode;
		static StereoCameraParameters _stereoCameraParams;

		static bool CheckStop()
		{
			bool result = false;

			{
				lock runLock(Locks::Run);
				lock stopLock(Locks::Stop);

				if (_stop)
				{
					_context.quit();
					_stop = false;
					_isRunning = false;
					result = true;
				}

				result = !_isRunning;
			}

			return result;
		}

		static void OnNewDepthSample(DepthNode node, DepthNode::NewSampleReceivedData data)
		{
			if (CheckStop())
			{
				return;
			}

			{
				lock depthLock(Locks::GetDepthMap);
				if (!_getDepthMap)
				{
					return;
				}

				short* depthMap_out = DepthMap;
				const int16_t* depthMap_in = data.depthMap;

				for (int row = 0; row < Height; row++)
				{
					for (int col = 0; col < Width; col++)
					{
						//DepthMap[row][col] = data.depthMap[row * Width + col];
						*(depthMap_out++) = *(depthMap_in++);
					}
				}

				_getDepthMap = false;
			}
		}
		static void OnNewAudioSample(AudioNode node, AudioNode::NewSampleReceivedData data) 
		{
			if (CheckStop())
			{
				return;
			}
		}
		static void OnNewColorSample(ColorNode node, ColorNode::NewSampleReceivedData data) 
		{
			if (CheckStop())
			{
				return;
			}
		}

		static void ConfigureDepthNode()
		{
			_depthNode.newSampleReceivedEvent().connect(&OnNewDepthSample);

			DepthNode::Configuration config = _depthNode.getConfiguration();
			config.frameFormat = FRAME_FORMAT_QVGA;
			config.framerate = 25;
			config.mode = DepthNode::CAMERA_MODE_CLOSE_MODE;
			config.saturation = true;

			_depthNode.setEnableDepthMap(true);

			try
			{
				_context.requestControl(_depthNode, 0);
				_depthNode.setConfiguration(config);
			}
			catch (DepthSense::Exception ex) {}
		}
		//static void ConfigureColorNode()
		//{
		//	_colorNode.newSampleReceivedEvent().connect(&OnNewColorSample);

		//	ColorNode::Configuration config = _colorNode.getConfiguration();
		//	config.frameFormat = FRAME_FORMAT_VGA;
		//	config.compression = COMPRESSION_TYPE_MJPEG;
		//	config.powerLineFrequency = POWER_LINE_FREQUENCY_50HZ;
		//	config.framerate = 25;

		//	_colorNode.setEnableColorMap(true);

		//	try
		//	{
		//		_context.requestControl(_colorNode, 0);
		//		_colorNode.setConfiguration(config);
		//	}
		//	catch (DepthSense::Exception ex) {}
		//}
		//static void ConfigureAudioNode()
		//{
		//	_audioNode.newSampleReceivedEvent().connect(&OnNewAudioSample);

		//	AudioNode::Configuration config = _audioNode.getConfiguration();
		//	config.sampleRate = 44100;

		//	try
		//	{
		//		_context.requestControl(_audioNode, 0);
		//		_audioNode.setConfiguration(config);
		//		_audioNode.setInputMixerLevel(0.5f);
		//	}
		//	catch (DepthSense::Exception ex) {}
		//}

		static void ConfigureNode(Node node)
		{
			if (node.is<DepthNode>() && !_depthNode.isSet())
			{
				_depthNode = node.as<DepthNode>();
				ConfigureDepthNode();
				_context.registerNode(node);
			}
			//if (node.is<ColorNode>() && !_colorNode.isSet())
			//{
			//	_colorNode = node.as<ColorNode>();
			//	ConfigureColorNode();
			//	_context.registerNode(node);
			//}
			//if (node.is<AudioNode>() && !_audioNode.isSet())
			//{
			//	_audioNode = node.as<AudioNode>();
			//	ConfigureAudioNode();
			//	_context.registerNode(node);
			//}
		}

		static void OnNodeAdded(Device device, Device::NodeAddedData data)
		{
			ConfigureNode(data.node);
		}
		static void OnNodeRemoved(Device device, Device::NodeRemovedData data)
		{
			if (data.node.is<DepthNode>() && data.node.as<DepthNode>() == _depthNode)
			{
				_depthNode.unset();
			}
			//if (data.node.is<ColorNode>() && data.node.as<ColorNode>() == _colorNode)
			//{
			//	_colorNode.unset();
			//}
			//if (data.node.is<AudioNode>() && data.node.as<AudioNode>() == _audioNode)
			//{
			//	_audioNode.unset();
			//}
		}

		static void OnDeviceAdded(Context context, Context::DeviceAddedData data)
		{
			if (!_deviceFound)
			{
				data.device.nodeAddedEvent().connect(&OnNodeAdded);
				data.device.nodeRemovedEvent().connect(&OnNodeRemoved);
				_deviceFound = true;
			}
		}
		static void OnDeviceRemoved(Context context, Context::DeviceRemovedData data)
		{
			if (_context.getDevices().size() == 0)
			{
				_deviceFound = false;
			}
		}
	};
	
	bool DepthSensor_Unwrapped::_getDepthMap = false;
	bool DepthSensor_Unwrapped::_deviceFound = false;
	bool DepthSensor_Unwrapped::_stop = true;
	bool DepthSensor_Unwrapped::_isRunning = false;
	Context DepthSensor_Unwrapped::_context = Context();
	DepthNode DepthSensor_Unwrapped::_depthNode = DepthNode();
	//ColorNode DepthSensor_Unwrapped::_colorNode = ColorNode();
	//AudioNode DepthSensor_Unwrapped::_audioNode = AudioNode();
	StereoCameraParameters DepthSensor_Unwrapped::_stereoCameraParams = StereoCameraParameters();
	short DepthSensor_Unwrapped::DepthMap[DepthSensor_Unwrapped::Width * DepthSensor_Unwrapped::Height] = {};

}
