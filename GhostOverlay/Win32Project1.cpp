// Win32Project1.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "Win32Project1.h"





// This is an example of an exported variable
WIN32PROJECT1_API int nWin32Project1=0;

// This is an example of an exported function.
WIN32PROJECT1_API int fnWin32Project1(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see Win32Project1.h for the class definition
CWin32Project1::CWin32Project1()
{
	return;
}
