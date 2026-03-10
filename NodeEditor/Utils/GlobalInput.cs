using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NodeEditor
{
    public partial class GlobalInput/* : SingletonMono<GlobalInput>*/
    {
        private static Action<GlobalInputKeyCode, bool> onKeyCallback;
        public static event Action<GlobalInputKeyCode, bool> OnKeyCallback
        {
            add
            {
                onKeyCallback -= value;
                onKeyCallback += value;
            }
            remove { onKeyCallback -= value; }
        }

        private static bool isHooking = false;
        // Set the hook
        public static bool SetHook()
        {
            //Log.Info("GlobalInput.SetHook");
            isHooking = false;
            // If no process linked, link hook process
            if (llKbProc == null)
                llKbProc = HookCallback;

            // If already hooked, unhook
            if (hookID != IntPtr.Zero)
                UnsetHook();

            // Hook new process
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                windowHandle = GetModuleHandle(curModule.ModuleName);
                hookID = SetWindowsHookEx((int)HookType.WH_KEYBOARD_LL, llKbProc, windowHandle, 0);
            }

            // If hooking failed, return false
            if (hookID == IntPtr.Zero)
            {
                Log.Error("GlobalInput Failed to hook");
                return isHooking;
            }

            // Hooked successfully
            isHooking = true;
            return isHooking;
        }

        // Unhook process
        public static void UnsetHook()
        {
            //Log.Info("GlobalInput.UnsetHook");
            UnhookWindowsHookEx(hookID);
            hookID = IntPtr.Zero;
        }

        // Hook function for windows to call on keystroke events
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && isHooking)
            {
                // Read vkCode from lParam
                int vkCode = Marshal.ReadInt32(lParam);

                // If vkCode key is down, set the key state to true
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    SetKeyState(vkCode, true);
                    onKeyCallback?.Invoke((GlobalInputKeyCode)nCode, true);
                }

                // If vkCode key is up, set the key state to false
                if (wParam == (IntPtr)WM_KEYUP)
                {
                    SetKeyState(vkCode, false);
                    onKeyCallback?.Invoke((GlobalInputKeyCode)nCode, false);
                }

            }

            // Call next hook
            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        static void SetKeyState(int key, bool state)
        {
            if (!keyStates[key])
                keyDownStates[key] = state;

            keyStates[key] = state;
            keyUpStates[key] = !state;
        }

        public static bool GetKey(GlobalInputKeyCode key)
        {
            return keyStates[(int)key];
        }

        public static bool GetKeyDown(GlobalInputKeyCode key)
        {
            if (keyDownStates[(int)key])
            {
                keyDownStates[(int)key] = false;
                return true;
            }

            return false;
        }

        public static bool GetKeyUp(GlobalInputKeyCode key)
        {
            if (keyUpStates[(int)key])
            {
                keyUpStates[(int)key] = false;
                return true;
            }

            return false;
        }

        //static GlobalInput()
        //{
        //    // Hook process
        //    SetHook();
        //}

        ~GlobalInput()
        {
            UnsetHook();
        }
    }
}