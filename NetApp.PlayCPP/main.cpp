#include "pch.h"
#include <cstring>
#include "hello.h"

extern "C"
{
	__declspec(dllexport) int HelloWorld(char* rntStr)
	{
		Hello^ a = gcnew Hello();
		const char* hi = "yayay";
		strcpy_s(rntStr, strlen(hi)+1, hi);
		//delete a;
		return 0;
	}

	__declspec(dllexport) int Release() 
	{
		return 0;
	}
}