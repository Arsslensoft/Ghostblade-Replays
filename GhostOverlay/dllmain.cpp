// dllmain.cpp : Defines the entry point for the DLL application.
#undef UNICODE
#include "stdafx.h"
#include "D3D9.h"
#include <D3dx9core.h>
#include <d3dx9.h>
#include <cstdio>
#include <windows.h>

#include "detours.h"
#include <windowsx.h>
#include "stdio.h"
#include <string>
#include <list>

#define EXTERN_DLL_EXPORT extern "C" __declspec(dllexport)

//Macros to read the keyboard asynchronously
#define KEY_DOWN(vk_code) ((GetAsyncKeyState(vk_code) & 0x8000) ? 1 : 0)
#define KEY_UP(vk_code) ((GetAsyncKeyState(vk_code) & 0x8000) ? 1 : 0)


LRESULT CALLBACK TempWndProc(HWND,UINT,WPARAM,LPARAM);
LRESULT CALLBACK TempWndProc (HWND hwnd,UINT msg,WPARAM wParam, LPARAM lParam) {
	return(DefWindowProc(hwnd,msg,wParam,lParam));
}
//Just some typedefs:
typedef HRESULT (WINAPI* oEndScene) (LPDIRECT3DDEVICE9 D3DDevice);
static oEndScene EndScene;

typedef HRESULT (WINAPI* oReset) (LPDIRECT3DDEVICE9 *D3DDevice, D3DPRESENT_PARAMETERS* pPresentationParameters);
static oReset Reset;

typedef HRESULT (WINAPI* oPresent) (LPDIRECT3DDEVICE9 D3DDevice, const RECT    *pSourceRect,   const RECT    *pDestRect,      HWND    hDestWindowOverride, const RGNDATA *pDirtyRegions);
static oPresent Present;

LPDIRECT3D9 pD3D;
LPD3DXFONT m_pFont;
LPD3DXFONT m_pFont_M;
LPD3DXFONT m_pFont_B;
int attach_called = 0;
BOOL capture;



EXTERN_DLL_EXPORT void WriteLogFile(const char* szString)
{
 
 //
 //FILE* pFile = fopen("C:\\logFile.txt", "a");
 // fprintf(pFile, "%s\n",szString);
 // fclose(pFile);
 
 
}

typedef struct OverlayText
{
	//int id;
	RECT rect;
	int Length;
	int FontSize;
	char text[100];


} OverlayText;

std::list<OverlayText> ovtl;

void AddOverlayText(char* ot, int top, int bot, int left, int right,int len,int fsize)
{

	OverlayText *t = (OverlayText*)malloc(sizeof(OverlayText));

	strncpy(t->text, ot, len);
	t->text[len] = '\0';
		
		t->Length =len;
		t->rect.bottom = bot;
		t->rect.right = right;
		t->rect.top = top;
		t->rect.left = left;
		t->FontSize =fsize;

		ovtl.push_back(*t);
	

}

DWORD WINAPI ThreadProc();
HANDLE hPipe1, hPipe2;

void SendMsg(char* buffer)
{
	if (!(hPipe1 == NULL || hPipe1 == INVALID_HANDLE_VALUE))
	{
		BOOL Write_St = TRUE;
		DWORD cbWritten;
		DWORD dwBytesToWrite = (DWORD)strlen(buffer);
		WriteFile(hPipe1, buffer, dwBytesToWrite, &cbWritten, NULL);
		memset(buffer, 0xCC, dwBytesToWrite);

	}
}

DWORD WINAPI NET_RvThr(LPVOID) {
	BOOL fSuccess;
	char msg[100];
	char chBuf[100];
	DWORD dwBytesToWrite = (DWORD)strlen(chBuf);
	DWORD cbRead;
	int i;

	while (1)
	{
		fSuccess = ReadFile(hPipe2, chBuf, dwBytesToWrite, &cbRead, NULL);
		if (fSuccess)
		{
			
			strncpy(msg, chBuf, cbRead);
			msg[cbRead ] = '\0';



	
			int top, bot, rg, lf, len,fsize;
			char t[100];
			sscanf(msg, "%d|%d|%d|%d|%d|%d|%[^\n]", &top, &bot, &rg, &lf,&len,&fsize,t);
			t[len] = '\0';
			AddOverlayText(t, top, bot, lf, rg,len,fsize);
	/*	
		char s[100];	
			OverlayText *p = ovt;
	

			sprintf(s, "%d|%d|%s",p->Length,p->FontSize , p->text);

			SendMsg(s);
	*/

		msg[0] = '\0';
		t[0] = '\0';

			/*printf("%c", chBuf[i]);
			printf("\n");*/
		}
	/*	if (!fSuccess && GetLastError() != ERROR_MORE_DATA)
		{
			printf("Can't Read\n");
			if (Finished)
				break;
		}*/
	}

}

int StartPipe()
{
	//Pipe Init Data
	char buf[100];

	LPTSTR lpszPipename1 = TEXT("\\\\.\\pipe\\GOVERLAYPIPE");
	LPTSTR lpszPipename2 = TEXT("\\\\.\\pipe\\GOVERLAYPIPESERV");

	DWORD cbWritten;
	DWORD dwBytesToWrite = (DWORD)strlen(buf);

	//Thread Init Data
	DWORD threadId;
	HANDLE hThread = NULL;

	BOOL Write_St = TRUE;





	hPipe1 = CreateFile(lpszPipename1, GENERIC_WRITE, 0, NULL, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, NULL);
	hPipe2 = CreateFile(lpszPipename2, GENERIC_READ, 0, NULL, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, NULL);


	if ((hPipe1 == NULL || hPipe1 == INVALID_HANDLE_VALUE) || (hPipe2 == NULL || hPipe2 == INVALID_HANDLE_VALUE))
		return	GetLastError();
	else
	{

		hThread = CreateThread(NULL, 0, NET_RvThr, NULL, 0, NULL);
		//do
		//{
		//	printf("Enter your message: ");
		//	scanf("%s", buf);
		//	if (strcmp(buf, "quit") == 0)
		//		Write_St = FALSE;
		//	else
		//	{
		//		WriteFile(hPipe1, buf, dwBytesToWrite, &cbWritten, NULL);
		//		memset(buf, 0xCC, 100);

		//	}

		//} while (Write_St);

		/*	CloseHandle(hPipe1);
		CloseHandle(hPipe2);*/
	
	}

}

bool displaysent = false;
// Function for font loading
HRESULT LoadFont(LPDIRECT3DDEVICE9 pDevice, int size)
{
		if(displaysent == false)
		{
		 D3DDISPLAYMODE mode;
  pDevice->GetDisplayMode(0, &mode); 
  // Send Display mode to Ghostblade
  char bf[50];
  sprintf(bf, "%d|%d|", mode.Width, mode.Height);
	  SendMsg(bf);
	  displaysent = true;
		}

	if(size == 30)
	{
	// clear if not empty <no memory leaks!!>
		if(m_pFont)
			return S_OK;
if(FAILED(AddFontResourceEx("TRANS.ttf",FR_PRIVATE,0)))
		return E_FAIL;

if(FAILED(	D3DXCreateFont(pDevice,size,0,FW_NORMAL,1,false,DEFAULT_CHARSET,
     OUT_DEFAULT_PRECIS,ANTIALIASED_QUALITY,DEFAULT_PITCH|FF_DONTCARE,
	 "Transformers Movie",&m_pFont)))
		return E_FAIL;
	




	return S_OK;
	}
	else if(size == 50)
	{
	// clear if not empty <no memory leaks!!>
		if(m_pFont_M)
			return S_OK;
if(FAILED(AddFontResourceEx("TRANS.ttf",FR_PRIVATE,0)))
		return E_FAIL;

if(FAILED(	D3DXCreateFont(pDevice,size,0,FW_NORMAL,1,false,DEFAULT_CHARSET,
     OUT_DEFAULT_PRECIS,ANTIALIASED_QUALITY,DEFAULT_PITCH|FF_DONTCARE,
	 "Transformers Movie",&m_pFont_M)))
		return E_FAIL;
	




	return S_OK;
	}
	else if(size == 70)
	{
	// clear if not empty <no memory leaks!!>
		if(m_pFont_B)
			return S_OK;
if(FAILED(AddFontResourceEx("TRANS.ttf",FR_PRIVATE,0)))
		return E_FAIL;

if(FAILED(	D3DXCreateFont(pDevice,size,0,FW_NORMAL,1,false,DEFAULT_CHARSET,
     OUT_DEFAULT_PRECIS,ANTIALIASED_QUALITY,DEFAULT_PITCH|FF_DONTCARE,
	 "Transformers Movie",&m_pFont_B)))
		return E_FAIL;
	




	return S_OK;
	}
	return E_FAIL;
	// Other options
	//DT_NOCLIP | DT_WORDBREAK
}

// Interceptors
HRESULT WINAPI newEndScene(LPDIRECT3DDEVICE9 pDevice)
{   

	if(displaysent == false)
	{
	LoadFont( pDevice, 30);
	LoadFont( pDevice, 70);
	LoadFont( pDevice, 50);
	}

		for (std::list<OverlayText>::iterator it = ovtl.begin(); it != ovtl.end(); it++)
		{
			OverlayText p = *it;
			if(m_pFont && p.FontSize == 30)
			   m_pFont->DrawTextA(0, p.text, p.Length, &(p.rect), DT_NOCLIP, D3DCOLOR_ARGB(255, 180, 190, 199));
			else 	if(m_pFont_M && p.FontSize == 50)
				m_pFont_M->DrawTextA(0, p.text, p.Length, &(p.rect), DT_NOCLIP, D3DCOLOR_ARGB(255, 180, 190, 199));
			else 	if(m_pFont_B && p.FontSize == 70)
			   m_pFont_B->DrawTextA(0, p.text, p.Length, &(p.rect), DT_NOCLIP, D3DCOLOR_ARGB(255, 180, 190, 199));
		}



		
	
				
			

		return EndScene(pDevice);
	
	

	
}
HRESULT WINAPI newReset(LPDIRECT3DDEVICE9 *pDevice, D3DPRESENT_PARAMETERS* pPresentationParameters)
{   

	    __asm pushad
 
		if( m_pFont ){
        m_pFont->OnLostDevice();
    } 

		if( m_pFont_B ){
        m_pFont_B->OnLostDevice();
    }   

		if( m_pFont_M ){
        m_pFont_M->OnLostDevice();
    }   

		HRESULT hRetn = (HRESULT)Reset( pDevice, pPresentationParameters );
 
    if( SUCCEEDED( hRetn ) ){
        if(m_pFont)
            m_pFont->OnResetDevice();

		if(m_pFont_M)
            m_pFont_M->OnResetDevice();

		if(m_pFont_B)
            m_pFont_B->OnResetDevice();
    }
 
    __asm popad
 
    return hRetn;


}
HRESULT WINAPI newPresent(LPDIRECT3DDEVICE9 pDevice, const RECT    *pSourceRect,   const RECT    *pDestRect,      HWND    hDestWindowOverride, const RGNDATA *pDirtyRegions)
{   
//			if(capture && KEY_DOWN(VK_MULTIPLY))
//		{
//		
//			   IDirect3DSurface9* pRenderTarget=NULL;
//			IDirect3DSurface9* surface;
		/*	  D3DDISPLAYMODE mode;
  pDevice->GetDisplayMode(0, &mode); */
//
//    pDevice->GetRenderTarget(0, &pRenderTarget);
//
//   // create a destination surface.
//   pDevice->CreateOffscreenPlainSurface(mode.Width,  mode.Height,
//                         mode.Format,
//                         D3DPOOL_SYSTEMMEM,
//						 &surface,
//                         NULL);
//   //copy the render target to the destination surface.
//   pDevice->GetRenderTargetData(pRenderTarget, surface);
//   
//
//   D3DXSaveSurfaceToFile(   GetName(), D3DXIFF_BMP, surface, NULL, NULL);
//surface->Release();
// pRenderTarget->Release();
// frame++;
//capture = false;
//		}
//			else if(!KEY_DOWN(VK_MULTIPLY))
//				frame = 0;


	return Present(pDevice,pSourceRect, pDestRect, hDestWindowOverride,pDirtyRegions);
}
EXTERN_DLL_EXPORT bool RegisterD3D() {


	IDirect3DDevice9* ppReturnedDeviceInterface;
	unsigned long* pInterface;
	// Init Overlay
	//ot.nxt = NULL;
	//ot.id = 0;

 // End Init Overlay
	HMODULE hDLL=GetModuleHandleA("d3d9");
LPDIRECT3D9(__stdcall*pDirect3DCreate9)(UINT) = (LPDIRECT3D9(__stdcall*)(UINT))GetProcAddress( hDLL, "Direct3DCreate9");

pD3D = pDirect3DCreate9(D3D_SDK_VERSION);

D3DDISPLAYMODE d3ddm;
HRESULT hRes = pD3D->GetAdapterDisplayMode(D3DADAPTER_DEFAULT, &d3ddm );
D3DPRESENT_PARAMETERS d3dpp; 
ZeroMemory( &d3dpp, sizeof(d3dpp));
d3dpp.Windowed = true;
d3dpp.SwapEffect = D3DSWAPEFFECT_DISCARD;
d3dpp.BackBufferFormat = d3ddm.Format;

WNDCLASSEX wc = { sizeof(WNDCLASSEX),CS_CLASSDC,TempWndProc,0L,0L,GetModuleHandle(NULL),NULL,NULL,NULL,NULL,LPCSTR("1"),NULL};
RegisterClassEx(&wc);
HWND hWnd = CreateWindow(LPCSTR("1"),NULL,WS_OVERLAPPEDWINDOW,100,100,300,300,GetDesktopWindow(),NULL,wc.hInstance,NULL);

hRes = pD3D->CreateDevice( 
    D3DADAPTER_DEFAULT,
    D3DDEVTYPE_HAL,
    hWnd,
    D3DCREATE_SOFTWARE_VERTEXPROCESSING | D3DCREATE_DISABLE_DRIVER_MANAGEMENT,
    &d3dpp, &ppReturnedDeviceInterface);


pD3D->Release();
DestroyWindow(hWnd);

if(pD3D == NULL){
    //printf ("WARNING: D3D FAILED");
    return false;
}
pInterface = (unsigned long*)*((unsigned long*)ppReturnedDeviceInterface);

Reset = (oReset) (DWORD) pInterface[16];
EndScene = (oEndScene) (DWORD) pInterface[42];
Present = (oPresent) (DWORD) pInterface[17];

DetourTransactionBegin();
DetourUpdateThread(GetCurrentThread());

DetourAttach(&(PVOID&)EndScene, newEndScene);
DetourTransactionCommit();
// RESET
DetourTransactionBegin();
DetourUpdateThread(GetCurrentThread());

DetourAttach(&(PVOID&)Reset, newReset);
DetourTransactionCommit();

// Present
DetourTransactionBegin();
DetourUpdateThread(GetCurrentThread());

DetourAttach(&(PVOID&)Present, newPresent);
DetourTransactionCommit();


return true;
}

DWORD WINAPI T_HkThread(LPVOID)
{
	  char buffer[MAX_PATH];

    //Loading CLR INTO PROCESS

	RegisterD3D();


	//	WriteLogFile("Registred D3D  ");


	// font path
//	WriteLogFile("Starting timer  ");
	//CreateThread( NULL, NULL, TimerMtd, NULL, NULL, NULL );
    return 0;
} 

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{

	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
			if(attach_called == 0)
			{
				 attach_called = 1;
		       CreateThread( NULL, NULL, T_HkThread, NULL, NULL, NULL );
			   StartPipe();
			  
			}
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:

	
	case DLL_PROCESS_DETACH:

	/*	CloseHandle(hPipe1);
		CloseHandle(hPipe2);*/
		break;
	}
	return TRUE;
}