#pragma once
namespace DepthSenseWrapper
{
	static const char* str2char(System::String^ string)
	{
		const char* str = (const char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(string)).ToPointer();
		return str;
	}
}