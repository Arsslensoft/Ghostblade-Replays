using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GhostLib
{
        public enum DllInjectionResult
    {
        DllNotFound,
        GameProcessNotFound,
        InjectionFailed,
        Success
    }


  public  class GhostOverlay
    {
        static readonly IntPtr INTPTR_ZERO = (IntPtr)0;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttribute, IntPtr dwStackSize, IntPtr lpStartAddress,
            IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        static GhostOverlay _instance;

        public static GhostOverlay GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GhostOverlay();
                }
                return _instance;
            }
        }

        GhostOverlay() { }

        public DllInjectionResult Inject(string sProcName, string sDllPath)
        {
            if (!File.Exists(sDllPath))
            {
                return DllInjectionResult.DllNotFound;
            }

            uint _procId = 0;

            Process[] _procs = Process.GetProcesses();
            for (int i = 0; i < _procs.Length; i++)
            {
                if (_procs[i].ProcessName == sProcName)
                {
                    _procId = (uint)_procs[i].Id;
                    break;
                }
            }

            if (_procId == 0)
            {
                return DllInjectionResult.GameProcessNotFound;
            }

            if (!bInject(_procId, sDllPath))
            {
                return DllInjectionResult.InjectionFailed;
            }

            return DllInjectionResult.Success;
        }

        bool bInject(uint pToBeInjected, string sDllPath)
        {
            IntPtr hndProc = OpenProcess((0x2 | 0x8 | 0x10 | 0x20 | 0x400), 1, pToBeInjected);

            if (hndProc == INTPTR_ZERO)
            {
                return false;
            }

            IntPtr lpLLAddress = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            if (lpLLAddress == INTPTR_ZERO)
            {
                return false;
            }

            IntPtr lpAddress = VirtualAllocEx(hndProc, (IntPtr)null, (IntPtr)sDllPath.Length, (0x1000 | 0x2000), 0X40);

            if (lpAddress == INTPTR_ZERO)
            {
                return false;
            }

            byte[] bytes = Encoding.ASCII.GetBytes(sDllPath);

            if (WriteProcessMemory(hndProc, lpAddress, bytes, (uint)bytes.Length, 0) == 0)
            {
                return false;
            }

            if (CreateRemoteThread(hndProc, (IntPtr)null, INTPTR_ZERO, lpLLAddress, lpAddress, 0, (IntPtr)null) == INTPTR_ZERO)
            {
                return false;
            }

            CloseHandle(hndProc);

            return true;
        }

      public static bool IsSafeToInject()
      {
          return (Process.GetProcessesByName("LolClient").Length == 0);
      }
        public static bool IsDirectXAvailable(string lol)
        {
            return File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + @"\D3DX9_43.dll") || File.Exists(lol + @"\D3DX9_43.dll"); 
        }
      public static sbyte ShowOverlay(string loldir)
      {
          try{
              if(SettingsManager.Settings.GhostOverlayEnabled)
              {
                  if(!IsSafeToInject())
                      return -2;
                  if (!File.Exists(loldir + @"\TRANS.ttf"))
                      File.Copy(Application.StartupPath + @"\TRANS.ttf", loldir + @"\TRANS.ttf");


                    if (!IsDirectXAvailable(loldir))
                        File.Copy(Application.StartupPath + @"\D3DX9_43.dll", loldir + @"\D3DX9_43.dll");


                    if (GetInstance.Inject("League of Legends", Application.StartupPath + @"\GhostOverlay.dll") == DllInjectionResult.Success)
                        return 1;

                    
              }
              return 0;
          }
          catch(Exception ex)
          {
              RiotTool.Instance.Log.Log.Error("[SHOW OVERLAY]",ex);
             
          }
          return -1;
      }
        static NamedPipeServer PServer1 = null;
        static NamedPipeServer PServer2 = null;

        static void PServer1_OnModeReceived(object sender, EventArgs e)
            {
            try
            {
                string[] w = sender.ToString().Split('|');
                int wd = int.Parse(w[0]);
                int h = int.Parse(w[1]);

                if (SettingsManager.Settings.Overlays == null)
                    return;


                foreach (GOverlay gov in SettingsManager.Settings.Overlays)
                {
                  if(gov.OverlayText == "GHOSTBLADE REPLAYS")
                    {
                        gov.Left = wd - 240;
                        gov.Bottom = h - 5;
                    }
                }
                SettingsManager.Save();

                SendOverlays();
            }
            catch
            {

            }
        }
        public static void StartServer()
        {
            try {
                PServer1 = new NamedPipeServer(@"\\.\pipe\GOVERLAYPIPE", 0);
                PServer2 = new NamedPipeServer(@"\\.\pipe\GOVERLAYPIPESERV", 1);
                PServer1.OnModeReceived += PServer1_OnModeReceived;
                PServer2.OnModeReceived += PServer1_OnModeReceived;
                PServer1.Start();
                PServer2.Start();

         
                 
             
            }
            catch { }
      
        }

        public static void SendOverlay(string overlay, int top, int bot, int left, int right,int fsize)
        {
            try
            {
                PServer2.SendMessage(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", top, bot, right, left, overlay.Length,fsize, overlay), PServer2.clientse);
            }
            catch { }
        }
        public static void Stop()
        {
            try
            {
                PServer1.StopServer();
                PServer2.StopServer();
            }
            catch { }
        }

        public static void SendOverlays()
        {
            if (SettingsManager.Settings.Overlays == null)
                return;


            foreach(GOverlay gov in SettingsManager.Settings.Overlays)
                SendOverlay(gov.OverlayText, gov.Top, gov.Bottom, gov.Left, gov.Right,gov.Size);
            

        }
    }

    public class GOverlay
    {

        public string OverlayText;
        public int Top;
        public int Bottom;
        public int Right;
        public int Left;
        public int Size;

    }

    public class NamedPipeServer
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle CreateNamedPipe(
           String pipeName,
           uint dwOpenMode,
           uint dwPipeMode,
           uint nMaxInstances,
           uint nOutBufferSize,
           uint nInBufferSize,
           uint nDefaultTimeOut,
           IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int ConnectNamedPipe(
           SafeFileHandle hNamedPipe,
           IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int DisconnectNamedPipe(
           SafeFileHandle hNamedPipe);

        public const uint DUPLEX = (0x00000003);
        public const uint FILE_FLAG_OVERLAPPED = (0x40000000);

        public class Client
        {
            public SafeFileHandle handle;
            public FileStream stream;
        }

        public const int BUFFER_SIZE = 100;
        public Client clientse = null;

        public string pipeName;
        Thread listenThread;
        SafeFileHandle clientHandle;
        public int ClientType;

        public NamedPipeServer(string PName, int Mode)
        {
            pipeName = PName;
            ClientType = Mode;//0 Reading Pipe, 1 Writing Pipe

        }

        public void Start()
        {
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }
        private void ListenForClients()
        {
            while (true)
            {

                clientHandle = CreateNamedPipe(this.pipeName, DUPLEX | FILE_FLAG_OVERLAPPED, 0, 255, BUFFER_SIZE, BUFFER_SIZE, 0, IntPtr.Zero);

                //could not create named pipe
                if (clientHandle.IsInvalid)
                    return;

                int success = ConnectNamedPipe(clientHandle, IntPtr.Zero);

                //could not connect client
                if (success == 0)
                    return;

                clientse = new Client();
                clientse.handle = clientHandle;
                clientse.stream = new FileStream(clientse.handle, FileAccess.ReadWrite, BUFFER_SIZE, true);

                if (ClientType == 0)
                {
                    Thread readThread = new Thread(new ThreadStart(Read));
                    readThread.Start();
                }
            }
        }
        public event EventHandler OnModeReceived;
        private void Read()
        {
            //Client client = (Client)clientObj;
            //clientse.stream = new FileStream(clientse.handle, FileAccess.ReadWrite, BUFFER_SIZE, true);
            byte[] buffer = null;
            ASCIIEncoding encoder = new ASCIIEncoding();

            while (true)
            {

                int bytesRead = 0;

                try
                {
                    buffer = new byte[BUFFER_SIZE];
                    bytesRead = clientse.stream.Read(buffer, 0, BUFFER_SIZE);
                }
                catch
                {
                    //read error has occurred
                    break;
                }

                //client has disconnected
                if (bytesRead == 0)
                    break;

                //fire message received event
                //if (this.MessageReceived != null)
                //    this.MessageReceived(clientse, encoder.GetString(buffer, 0, bytesRead));

                int ReadLength = 0;
                for (int i = 0; i < BUFFER_SIZE; i++)
                {
                    if (buffer[i].ToString("x2") != "cc")
                    {
                        ReadLength++;
                    }
                    else
                        break;
                }
                if (ReadLength > 0)
                {
                    byte[] Rc = new byte[ReadLength];
                    Buffer.BlockCopy(buffer, 0, Rc, 0, ReadLength);
                    OnModeReceived(encoder.GetString(Rc, 0, ReadLength), EventArgs.Empty);
                  
                    buffer.Initialize();
                }

            }

            //clean up resources
            clientse.stream.Close();
            clientse.handle.Close();

        }
        public void SendMessage(string message, Client client)
        {

            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] messageBuffer = encoder.GetBytes(message);

            if (client.stream.CanWrite)
            {
                client.stream.Write(messageBuffer, 0, messageBuffer.Length);
                client.stream.Flush();
            }


        }
        public void StopServer()
        {
            //clean up resources

            DisconnectNamedPipe(this.clientHandle);


            this.listenThread.Abort();
        }

    }
}
