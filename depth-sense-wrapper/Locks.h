#pragma once

namespace DepthSenseWrapper
{
	private ref class Locks
	{
	public:
		static void Setup()
		{
			GetDepthMap = gcnew Object();
			Stop = gcnew Object();
			Run = gcnew Object();
		}

		static Object^ GetDepthMap;
		static Object^ Stop;
		static Object^ Run;
	};
}