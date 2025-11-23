using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace Lab02.WinUI;

public partial class App : MauiWinUIApplication
{
    private static WndProc? _wndProc;
    private static IntPtr _prevWndProc;
    private const int GWL_WNDPROC = -4;
    private const uint WM_CLOSE = 0x0010;
    private const uint MB_YESNO = 0x00000004;
    private const uint MB_ICONQUESTION = 0x00000020;
    private const int IDYES = 6;

    public App()
    {
        this.InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        var window = Microsoft.Maui.Controls.Application.Current.Windows.First().Handler.PlatformView as Microsoft.UI.Xaml.Window;

        if (window is not null)
        {
            var hwnd = WindowNative.GetWindowHandle(window);

            _wndProc = WndProcImpl;
            var newProcPtr = Marshal.GetFunctionPointerForDelegate(_wndProc);

            if (IntPtr.Size == 8)
            {
                _prevWndProc = SetWindowLongPtr(hwnd, GWL_WNDPROC, newProcPtr);
            }
            else
            {
                _prevWndProc = SetWindowLongPtr32(hwnd, GWL_WNDPROC, newProcPtr);
            }
        }
    }

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    private static IntPtr WndProcImpl(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg == WM_CLOSE)
        {
            int result = MessageBoxW(hWnd, "Ви впевнені, що хочете закрити програму?", "Підтвердити вихід", MB_YESNO | MB_ICONQUESTION);
            if (result == IDYES)
            {
                return CallWindowProc(_prevWndProc, hWnd, msg, wParam, lParam);
            }
            else
            {
                return IntPtr.Zero;
            }
        }

        return CallWindowProc(_prevWndProc, hWnd, msg, wParam, lParam);
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW", SetLastError = true)]
    private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr newProc);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongW", SetLastError = true)]
    private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr newProc);

    [DllImport("user32.dll", EntryPoint = "CallWindowProcW")]
    private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int MessageBoxW(IntPtr hWnd, string lpText, string lpCaption, uint uType);
}
