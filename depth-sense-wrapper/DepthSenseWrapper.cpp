#include "DepthSenseWrapper.h"

namespace DepthSenseWrapper
{
	DepthSensor::DepthSensor() : ManagedObject<DepthSensor_Unwrapped>(new DepthSensor_Unwrapped())
	{
		_instance->Setup();
	}

	void DepthSensor::Run()
	{
		_instance->Run();
		_instance->Stop();
	}

	int16Matrix^ DepthSensor::GetDepthMap()
	{
		_instance->GetDepthMap();
		int width = _instance->Width;
		int height = _instance->Height;
		auto start = high_resolution_clock::now();
		auto end = high_resolution_clock::now();
		while (duration_cast<milliseconds>(end - start).count() < 500)
		{
			if (_instance->CheckDepthMap())
			{
				int16Matrix^ result = gcnew int16Matrix(height, width);
				for (int row = 0; row < height; row++)
				{
					for (int col = 0; col < width; col++)
					{
						result[row, col] = _instance->DepthMap[row * width + col];
					}
				}
				return result;
			}
			end = high_resolution_clock::now();
		}

		return gcnew int16Matrix(0, 0);
	}
}