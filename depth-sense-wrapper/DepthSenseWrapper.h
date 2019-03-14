#pragma once

#include "DepthSensor_Unwrapped.h"
#include <chrono>
#include "ManagedObject.h"

namespace DepthSenseWrapper
{
	using namespace std::chrono;

	typedef cli::array<short> int16Array;
	typedef cli::array<short, 2> int16Matrix;
	
	public ref class DepthSensor : ManagedObject<DepthSensor_Unwrapped>
	{
	public:
		DepthSensor();

		void Run();

		int16Matrix^ GetDepthMap();
	};
}