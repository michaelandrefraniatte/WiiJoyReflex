using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Numerics;
using Microsoft.Win32.SafeHandles;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using static System.Windows.Forms.AxHost;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net;
using System.Collections;
using System.Reflection;

namespace wjr
{
    public class Class1
    {
        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(Keys vKey);
        [DllImport("hid.dll")]
        private static extern void HidD_GetHidGuid(out Guid gHid);
        [DllImport("hid.dll")]
        private extern static bool HidD_SetOutputReport(IntPtr HidDeviceObject, byte[] lpReportBuffer, uint ReportBufferLength);
        [DllImport("setupapi.dll")]
        private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, string Enumerator, IntPtr hwndParent, UInt32 Flags);
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInvo, ref Guid interfaceClassGuid, Int32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
        [DllImport("setupapi.dll")]
        private static extern Boolean SetupDiGetDeviceInterfaceDetail(IntPtr hDevInfo, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData, UInt32 deviceInterfaceDetailDataSize, out UInt32 requiredSize, IntPtr deviceInfoData);
        [DllImport("Kernel32.dll")]
        private static extern SafeFileHandle CreateFile(string fileName, [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess, [MarshalAs(UnmanagedType.U4)] FileShare fileShare, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] uint flags, IntPtr template);
        [DllImport("Kernel32.dll")]
        private static extern IntPtr CreateFile(string fileName, System.IO.FileAccess fileAccess, System.IO.FileShare fileShare, IntPtr securityAttributes, System.IO.FileMode creationDisposition, EFileAttributes flags, IntPtr template);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_read_timeout")]
        private static unsafe extern int Lhid_read_timeout(SafeFileHandle dev, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_write")]
        private static unsafe extern int Lhid_write(SafeFileHandle device, byte[] data, UIntPtr length);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_open_path")]
        private static unsafe extern SafeFileHandle Lhid_open_path(IntPtr handle);
        [DllImport("lhidread.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Lhid_close")]
        private static unsafe extern void Lhid_close(SafeFileHandle device);
        public const double REGISTER_IR = 0x04b00030, REGISTER_EXTENSION_INIT_1 = 0x04a400f0, REGISTER_EXTENSION_INIT_2 = 0x04a400fb, REGISTER_EXTENSION_TYPE = 0x04a400fa, REGISTER_EXTENSION_CALIBRATION = 0x04a40020, REGISTER_MOTIONPLUS_INIT = 0x04a600fe;
        public static double mousexpp, mouseypp, WidthS, HeightS, irx0, iry0, irx1, iry1, irx, iry, irxc, iryc, WiimoteIRSensors0X, WiimoteIRSensors0Y, WiimoteIRSensors1X, WiimoteIRSensors1Y, WiimoteRawValuesX, WiimoteRawValuesY, WiimoteRawValuesZ, calibrationinit, tickviewxinit, stickviewyinit, WiimoteNunchuckStateRawValuesX, WiimoteNunchuckStateRawValuesY, WiimoteNunchuckStateRawValuesZ, WiimoteNunchuckStateRawJoystickX, WiimoteNunchuckStateRawJoystickY, mousex, mousey, mousexp, mouseyp, WiimoteIRSensors0Xcam, WiimoteIRSensors0Ycam, WiimoteIRSensors1Xcam, WiimoteIRSensors1Ycam, WiimoteIRSensorsXcam, WiimoteIRSensorsYcam, WiimoteIR0notfound = 0, irx2, iry2, irx3, iry3;
        public static bool WiimoteIR1foundcam, WiimoteIR0foundcam, WiimoteIR1found, WiimoteIR0found, WiimoteIRswitch, WiimoteButtonStateA, WiimoteButtonStateB, WiimoteButtonStateMinus, WiimoteButtonStateHome, WiimoteButtonACC, WiimoteButtonStatePlus, WiimoteButtonStateOne, WiimoteButtonStateTwo, WiimoteButtonStateUp, WiimoteButtonStateDown, WiimoteButtonStateLeft, WiimoteButtonStateRight, WiimoteNunchuckStateC, WiimoteNunchuckStateZ, ISWIIMOTE, running, enabling, scriptenabling;
        public static byte[] buff = new byte[] { 0x55 }, mBuff = new byte[22], aBuffer = new byte[22];
        public const byte Type = 0x12, IR = 0x13, WriteMemory = 0x16, ReadMemory = 0x16, IRExtensionAccel = 0x37;
        public static FileStream mStream;
        public static SafeFileHandle handle = null;
        private static Type program;
        private static object obj;
        private static string code = @"
                using System;
                using System.Runtime.InteropServices;
                namespace StringToCode
                {
                    public class FooClass 
                    { 
                        bool SendLeftClick; bool SendRightClick; bool SendMiddleClick; bool SendWheelUp; bool SendWheelDown; bool SendLeft; bool SendRight; bool SendUp; bool SendDown; bool SendLButton; bool SendRButton; bool SendCancel; bool SendMBUTTON; bool SendXBUTTON1; bool SendXBUTTON2; bool SendBack; bool SendTab; bool SendClear; bool SendReturn; bool SendSHIFT; bool SendCONTROL; bool SendMENU; bool SendPAUSE; bool SendCAPITAL; bool SendKANA; bool SendHANGEUL; bool SendHANGUL; bool SendJUNJA; bool SendFINAL; bool SendHANJA; bool SendKANJI; bool SendEscape; bool SendCONVERT; bool SendNONCONVERT; bool SendACCEPT; bool SendMODECHANGE; bool SendSpace; bool SendPRIOR; bool SendNEXT; bool SendEND; bool SendHOME; bool SendLEFT; bool SendUP; bool SendRIGHT; bool SendDOWN; bool SendSELECT; bool SendPRINT; bool SendEXECUTE; bool SendSNAPSHOT; bool SendINSERT; bool SendDELETE; bool SendHELP; bool SendAPOSTROPHE; bool Send0; bool Send1; bool Send2; bool Send3; bool Send4; bool Send5; bool Send6; bool Send7; bool Send8; bool Send9; bool SendA; bool SendB; bool SendC; bool SendD; bool SendE; bool SendF; bool SendG; bool SendH; bool SendI; bool SendJ; bool SendK; bool SendL; bool SendM; bool SendN; bool SendO; bool SendP; bool SendQ; bool SendR; bool SendS; bool SendT; bool SendU; bool SendV; bool SendW; bool SendX; bool SendY; bool SendZ; bool SendLWIN; bool SendRWIN; bool SendAPPS; bool SendSLEEP; bool SendNUMPAD0; bool SendNUMPAD1; bool SendNUMPAD2; bool SendNUMPAD3; bool SendNUMPAD4; bool SendNUMPAD5; bool SendNUMPAD6; bool SendNUMPAD7; bool SendNUMPAD8; bool SendNUMPAD9; bool SendMULTIPLY; bool SendADD; bool SendSEPARATOR; bool SendSUBTRACT; bool SendDECIMAL; bool SendDIVIDE; bool SendF1; bool SendF2; bool SendF3; bool SendF4; bool SendF5; bool SendF6; bool SendF7; bool SendF8; bool SendF9; bool SendF10; bool SendF11; bool SendF12; bool SendF13; bool SendF14; bool SendF15; bool SendF16; bool SendF17; bool SendF18; bool SendF19; bool SendF20; bool SendF21; bool SendF22; bool SendF23; bool SendF24; bool SendNUMLOCK; bool SendSCROLL; bool SendLeftShift; bool SendRightShift; bool SendLeftControl; bool SendRightControl; bool SendLMENU; bool SendRMENU;
                        public bool[] getstate = new bool[36];
                        public static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
                        public static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
                        public static void valchanged(int n, bool val)
                        {
                            if (val)
                            {
                                if (wd[n] <= 1)
                                {
                                    wd[n] = wd[n] + 1;
                                }
                                wu[n] = 0;
                            }
                            else
                            {
                                if (wu[n] <= 1)
                                {
                                    wu[n] = wu[n] + 1;
                                }
                                wd[n] = 0;
                            }
                        }
                        public object[] Main(bool englishkeyboard, bool adsswitch, bool JoyconLeftButtonDPAD_DOWN, bool JoyconLeftButtonDPAD_LEFT, bool JoyconLeftButtonDPAD_RIGHT, bool JoyconLeftButtonDPAD_UP, bool JoyconLeftButtonMINUS, bool JoyconLeftButtonACC, bool JoyconLeftButtonSHOULDER_1, bool JoyconLeftButtonSHOULDER_2, bool JoyconLeftButtonCAPTURE, bool WiimoteButtonStateOne, bool WiimoteButtonStateTwo, bool WiimoteButtonStateDown, bool WiimoteButtonStateLeft, bool WiimoteButtonStateRight, bool WiimoteButtonStateUp, bool WiimoteButtonStateHome, bool WiimoteButtonACC, bool WiimoteButtonStateA, bool WiimoteButtonStateB, bool WiimoteButtonStatePlus, bool WiimoteButtonStateMinus)
                        {
                            funct_driver
                            return new object[] { SendLeftClick, SendRightClick, SendMiddleClick, SendWheelUp, SendWheelDown, SendLeft, SendRight, SendUp, SendDown, SendLButton, SendRButton, SendCancel, SendMBUTTON, SendXBUTTON1, SendXBUTTON2, SendBack, SendTab, SendClear, SendReturn, SendSHIFT, SendCONTROL, SendMENU, SendPAUSE, SendCAPITAL, SendKANA, SendHANGEUL, SendHANGUL, SendJUNJA, SendFINAL, SendHANJA, SendKANJI, SendEscape, SendCONVERT, SendNONCONVERT, SendACCEPT, SendMODECHANGE, SendSpace, SendPRIOR, SendNEXT, SendEND, SendHOME, SendLEFT, SendUP, SendRIGHT, SendDOWN, SendSELECT, SendPRINT, SendEXECUTE, SendSNAPSHOT, SendINSERT, SendDELETE, SendHELP, SendAPOSTROPHE, Send0, Send1, Send2, Send3, Send4, Send5, Send6, Send7, Send8, Send9, SendA, SendB, SendC, SendD, SendE, SendF, SendG, SendH, SendI, SendJ, SendK, SendL, SendM, SendN, SendO, SendP, SendQ, SendR, SendS, SendT, SendU, SendV, SendW, SendX, SendY, SendZ, SendLWIN, SendRWIN, SendAPPS, SendSLEEP, SendNUMPAD0, SendNUMPAD1, SendNUMPAD2, SendNUMPAD3, SendNUMPAD4, SendNUMPAD5, SendNUMPAD6, SendNUMPAD7, SendNUMPAD8, SendNUMPAD9, SendMULTIPLY, SendADD, SendSEPARATOR, SendSUBTRACT, SendDECIMAL, SendDIVIDE, SendF1, SendF2, SendF3, SendF4, SendF5, SendF6, SendF7, SendF8, SendF9, SendF10, SendF11, SendF12, SendF13, SendF14, SendF15, SendF16, SendF17, SendF18, SendF19, SendF20, SendF21, SendF22, SendF23, SendF24, SendNUMLOCK, SendSCROLL, SendLeftShift, SendRightShift, SendLeftControl, SendRightControl, SendLMENU, SendRMENU };
                        }
                    }
                }";
        private static string finalcode = "", funct_driver = "";
        private static string KeyboardMouseDriverType; static double MouseMoveX; static double MouseMoveY; static double MouseAbsX; static double MouseAbsY; static double MouseDesktopX; static double MouseDesktopY; static bool SendLeftClick; static bool SendRightClick; static bool SendMiddleClick; static bool SendWheelUp; static bool SendWheelDown; static bool SendLeft; static bool SendRight; static bool SendUp; static bool SendDown; static bool SendLButton; static bool SendRButton; static bool SendCancel; static bool SendMBUTTON; static bool SendXBUTTON1; static bool SendXBUTTON2; static bool SendBack; static bool SendTab; static bool SendClear; static bool SendReturn; static bool SendSHIFT; static bool SendCONTROL; static bool SendMENU; static bool SendPAUSE; static bool SendCAPITAL; static bool SendKANA; static bool SendHANGEUL; static bool SendHANGUL; static bool SendJUNJA; static bool SendFINAL; static bool SendHANJA; static bool SendKANJI; static bool SendEscape; static bool SendCONVERT; static bool SendNONCONVERT; static bool SendACCEPT; static bool SendMODECHANGE; static bool SendSpace; static bool SendPRIOR; static bool SendNEXT; static bool SendEND; static bool SendHOME; static bool SendLEFT; static bool SendUP; static bool SendRIGHT; static bool SendDOWN; static bool SendSELECT; static bool SendPRINT; static bool SendEXECUTE; static bool SendSNAPSHOT; static bool SendINSERT; static bool SendDELETE; static bool SendHELP; static bool SendAPOSTROPHE; static bool Send0; static bool Send1; static bool Send2; static bool Send3; static bool Send4; static bool Send5; static bool Send6; static bool Send7; static bool Send8; static bool Send9; static bool SendA; static bool SendB; static bool SendC; static bool SendD; static bool SendE; static bool SendF; static bool SendG; static bool SendH; static bool SendI; static bool SendJ; static bool SendK; static bool SendL; static bool SendM; static bool SendN; static bool SendO; static bool SendP; static bool SendQ; static bool SendR; static bool SendS; static bool SendT; static bool SendU; static bool SendV; static bool SendW; static bool SendX; static bool SendY; static bool SendZ; static bool SendLWIN; static bool SendRWIN; static bool SendAPPS; static bool SendSLEEP; static bool SendNUMPAD0; static bool SendNUMPAD1; static bool SendNUMPAD2; static bool SendNUMPAD3; static bool SendNUMPAD4; static bool SendNUMPAD5; static bool SendNUMPAD6; static bool SendNUMPAD7; static bool SendNUMPAD8; static bool SendNUMPAD9; static bool SendMULTIPLY; static bool SendADD; static bool SendSEPARATOR; static bool SendSUBTRACT; static bool SendDECIMAL; static bool SendDIVIDE; static bool SendF1; static bool SendF2; static bool SendF3; static bool SendF4; static bool SendF5; static bool SendF6; static bool SendF7; static bool SendF8; static bool SendF9; static bool SendF10; static bool SendF11; static bool SendF12; static bool SendF13; static bool SendF14; static bool SendF15; static bool SendF16; static bool SendF17; static bool SendF18; static bool SendF19; static bool SendF20; static bool SendF21; static bool SendF22; static bool SendF23; static bool SendF24; static bool SendNUMLOCK; static bool SendSCROLL; static bool SendLeftShift; static bool SendRightShift; static bool SendLeftControl; static bool SendRightControl; static bool SendLMENU; static bool SendRMENU;
        private static string cbescape1select; private static string cbescape2select; private static string cbtab1select; private static string cbtab2select; private static string cbshift1select; private static string cbshift2select; private static string cbcontrol1select; private static string cbcontrol2select; private static string cba1select; private static string cba2select; private static string cbe1select; private static string cbe2select; private static string cbx1select; private static string cbx2select; private static string cbc1select; private static string cbc2select; private static string cbv1select; private static string cbv2select; private static string cbspace1select; private static string cbspace2select; private static string cbf1select; private static string cbf2select; private static string cbr1select; private static string cbr2select; private static string cbuproll1select; private static string cbuproll2select; private static string cbdownroll1select; private static string cbdownroll2select; private static string cbleftclick1select; private static string cbleftclick2select; private static string cbrightclick1select; private static string cbrightclick2select; private static double irmode = 1; private static double centery = 160f; private static bool sendinput; private static bool adsswitch; private static bool englishkeyboard; private static double irxsens; private static double irysens; private static double stickxsens; private static double stickysens; private static double viewpower1x; private static double viewpower2x; private static double viewpower3x; private static double viewpower1y; private static double viewpower2y; private static double viewpower3y; private static bool cursor; private static bool brink; private static bool mw3; private static bool titanfall; private static bool metro; private static bool bo3; private static bool fortnite; private static bool fake; private static bool mc; private static string cbsendescapeselect; private static string cbsendtabselect; private static string cbsendshiftselect; private static string cbsendcontrolselect; private static string cbsendaselect; private static string cbsendeselect; private static string cbsendxselect; private static string cbsendcselect; private static string cbsendvselect; private static string cbsendspaceselect; private static string cbsendfselect; private static string cbsendrselect; private static string cbsenduprollselect; private static string cbsenddownrollselect; private static string cbsendleftclickselect; private static string cbsendrightclickselect; private static double dzx; private static double dzy; private static bool cbarrows1; private static bool cbarrows2; private static bool cbnumpad;
        public static bool[] getstate = new bool[36];
        public static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static void valchanged(int n, bool val)
        {
            if (val)
            {
                if (wd[n] <= 1)
                {
                    wd[n] = wd[n] + 1;
                }
                wu[n] = 0;
            }
            else
            {
                if (wu[n] <= 1)
                {
                    wu[n] = wu[n] + 1;
                }
                wd[n] = 0;
            }
        }
        public static void Apply(string cbescape1selectapply, string cbescape2selectapply, string cbtab1selectapply, string cbtab2selectapply, string cbshift1selectapply, string cbshift2selectapply, string cbcontrol1selectapply, string cbcontrol2selectapply, string cba1selectapply, string cba2selectapply, string cbe1selectapply, string cbe2selectapply, string cbx1selectapply, string cbx2selectapply, string cbc1selectapply, string cbc2selectapply, string cbv1selectapply, string cbv2selectapply, string cbspace1selectapply, string cbspace2selectapply, string cbf1selectapply, string cbf2selectapply, string cbr1selectapply, string cbr2selectapply, string cbuproll1selectapply, string cbuproll2selectapply, string cbdownroll1selectapply, string cbdownroll2selectapply, string cbleftclick1selectapply, string cbleftclick2selectapply, string cbrightclick1selectapply, string cbrightclick2selectapply, double irmodeapply, double centeryapply, bool sendinputapply, bool adsswitchapply, bool englishkeyboardapply, double irxsensapply, double irysensapply, double stickxsensapply, double stickysensapply, double viewpower1xapply, double viewpower2xapply, double viewpower3xapply, double viewpower1yapply, double viewpower2yapply, double viewpower3yapply, bool cursorapply, bool brinkapply, bool mw3apply, bool titanfallapply, bool metroapply, bool bo3apply, bool fortniteapply, bool fakeapply, bool mcapply, string cbsendescapeselectapply, string cbsendtabselectapply, string cbsendshiftselectapply, string cbsendcontrolselectapply, string cbsendaselectapply, string cbsendeselectapply, string cbsendxselectapply, string cbsendcselectapply, string cbsendvselectapply, string cbsendspaceselectapply, string cbsendfselectapply, string cbsendrselectapply, string cbsenduprollselectapply, string cbsenddownrollselectapply, string cbsendleftclickselectapply, string cbsendrightclickselectapply, double dzxapply, double dzyapply, bool cbarrows1apply, bool cbarrows2apply, bool cbnumpadapply)
        {
            scriptenabling = false;
            cbescape1select = cbescape1selectapply;
            cbescape2select = cbescape2selectapply;
            cbtab1select = cbtab1selectapply;
            cbtab2select = cbtab2selectapply;
            cbshift1select = cbshift1selectapply;
            cbshift2select = cbshift2selectapply;
            cbcontrol1select = cbcontrol1selectapply;
            cbcontrol2select = cbcontrol2selectapply;
            cba1select = cba1selectapply;
            cba2select = cba2selectapply;
            cbe1select = cbe1selectapply;
            cbe2select = cbe2selectapply;
            cbx1select = cbx1selectapply;
            cbx2select = cbx2selectapply;
            cbc1select = cbc1selectapply;
            cbc2select = cbc2selectapply;
            cbv1select = cbv1selectapply;
            cbv2select = cbv2selectapply;
            cbspace1select = cbspace1selectapply;
            cbspace2select = cbspace2selectapply;
            cbf1select = cbf1selectapply;
            cbf2select = cbf2selectapply;
            cbr1select = cbr1selectapply;
            cbr2select = cbr2selectapply;
            cbuproll1select = cbuproll1selectapply;
            cbuproll2select = cbuproll2selectapply;
            cbdownroll1select = cbdownroll1selectapply;
            cbdownroll2select = cbdownroll2selectapply;
            cbleftclick1select = cbleftclick1selectapply;
            cbleftclick2select = cbleftclick2selectapply;
            cbrightclick1select = cbrightclick1selectapply;
            cbrightclick2select = cbrightclick2selectapply;
            irmode = irmodeapply;
            centery = centeryapply;
            sendinput = sendinputapply;
            adsswitch = adsswitchapply;
            englishkeyboard = englishkeyboardapply;
            irxsens = irxsensapply;
            irysens = irysensapply;
            stickxsens = stickxsensapply;
            stickysens = stickysensapply;
            if (viewpower1xapply == 0f & viewpower2xapply == 0f & viewpower3xapply == 0f)
            {
                viewpower1x = 0.333f;
                viewpower2x = 0.333f;
                viewpower3x = 0.333f;
            }
            else
            {
                viewpower1x = viewpower1xapply / (viewpower1xapply + viewpower2xapply + viewpower3xapply);
                viewpower2x = viewpower2xapply / (viewpower1xapply + viewpower2xapply + viewpower3xapply);
                viewpower3x = viewpower3xapply / (viewpower1xapply + viewpower2xapply + viewpower3xapply);
            }
            if (viewpower1yapply == 0f & viewpower2yapply == 0f & viewpower3yapply == 0f)
            {
                viewpower1y = 0.333f;
                viewpower2y = 0.333f;
                viewpower3y = 0.333f;
            }
            else
            {
                viewpower1y = viewpower1yapply / (viewpower1yapply + viewpower2yapply + viewpower3yapply);
                viewpower2y = viewpower2yapply / (viewpower1yapply + viewpower2yapply + viewpower3yapply);
                viewpower3y = viewpower3yapply / (viewpower1yapply + viewpower2yapply + viewpower3yapply);
            }
            cursor = cursorapply;
            brink = brinkapply;
            mw3 = mw3apply;
            titanfall = titanfallapply;
            metro = metroapply;
            bo3 = bo3apply;
            fortnite = fortniteapply;
            fake = fakeapply;
            mc = mcapply;
            cbsendescapeselect = cbsendescapeselectapply;
            cbsendtabselect = cbsendtabselectapply;
            cbsendshiftselect = cbsendshiftselectapply;
            cbsendcontrolselect = cbsendcontrolselectapply;
            cbsendaselect = cbsendaselectapply;
            cbsendeselect = cbsendeselectapply;
            cbsendxselect = cbsendxselectapply;
            cbsendcselect = cbsendcselectapply;
            cbsendvselect = cbsendvselectapply;
            cbsendspaceselect = cbsendspaceselectapply;
            cbsendfselect = cbsendfselectapply;
            cbsendrselect = cbsendrselectapply;
            cbsenduprollselect = cbsenduprollselectapply;
            cbsenddownrollselect= cbsenddownrollselectapply;
            cbsendleftclickselect = cbsendleftclickselectapply;
            cbsendrightclickselect = cbsendrightclickselectapply;
            dzx = dzxapply;
            dzy = dzyapply;
            cbarrows1 = cbarrows1apply;
            cbarrows2 = cbarrows2apply;
            cbnumpad = cbnumpadapply; 
            funct_driver = @"
                            SendEscape = escape1 | escape2;
                            SendTab = tab1 | tab2;
                            SendLeftShift = shift1 | shift2;
                            SendLeftControl = control1 | control2;
                            if (englishkeyboard)
                                SendQ = a1 | a2;
                            else
                                SendA = a1 | a2;
                            SendE = e1 | e2;
                            SendX = x1 | x2;
                            SendC = c1 | c2;
                            SendV = v1 | v2;
                            SendSpace = space1 | space2;
                            SendR = r1 | r2;
                            SendF = f1 | f2;
                            SendWheelDown = downroll1 | downroll2;
                            SendWheelUp = uproll1 | uproll2;
                            SendLeftClick = leftclick1 | leftclick2;
                            if (!adsswitch)
                            {
                                SendRightClick = rightclick1 | rightclick2;
                            }
                            else
                            {
                                valchanged(1, rightclick1 | rightclick2);
                                if (wd[1] == 1 & !getstate[1])
                                {
                                    getstate[1] = true;
                                }
                                else
                                {
                                    if (wd[1] == 1 & getstate[1])
                                    {
                                        getstate[1] = false;
                                    }
                                }
                                if (SendV | SendR | SendX | SendA | SendE | SendEscape | SendTab)
                                {
                                    getstate[1] = false;
                                }
                                SendRightClick = getstate[1];
                            }";
            if (cbescape1select == "")
                funct_driver = funct_driver.Replace(" escape1", " false");
            else
                funct_driver = funct_driver.Replace(" escape1", " " + cbescape1select);
            if (cbescape2select == "")
                funct_driver = funct_driver.Replace(" escape2", " false");
            else
                funct_driver = funct_driver.Replace(" escape2", " " + cbescape2select);
            if (cbtab1select == "")
                funct_driver = funct_driver.Replace(" tab1", " false");
            else
                funct_driver = funct_driver.Replace(" tab1", " " + cbtab1select);
            if (cbtab2select == "")
                funct_driver = funct_driver.Replace(" tab2", " false");
            else
                funct_driver = funct_driver.Replace(" tab2", " " + cbtab2select);
            if (cbshift1select == "")
                funct_driver = funct_driver.Replace(" shift1", " false");
            else
                funct_driver = funct_driver.Replace(" shift1", " " + cbshift1select);
            if (cbshift2select == "")
                funct_driver = funct_driver.Replace(" shift2", " false");
            else
                funct_driver = funct_driver.Replace(" shift2", " " + cbshift2select);
            if (cbcontrol1select == "")
                funct_driver = funct_driver.Replace(" control1", " false");
            else
                funct_driver = funct_driver.Replace(" control1", " " + cbcontrol1select);
            if (cbcontrol2select == "")
                funct_driver = funct_driver.Replace(" control2", " false");
            else
                funct_driver = funct_driver.Replace(" control2", " " + cbcontrol2select);
            if (cba1select == "")
                funct_driver = funct_driver.Replace(" a1", " false");
            else
                funct_driver = funct_driver.Replace(" a1", " " + cba1select);
            if (cba2select == "")
                funct_driver = funct_driver.Replace(" a2", " false");
            else
                funct_driver = funct_driver.Replace(" a2", " " + cba2select);
            if (cbe1select == "")
                funct_driver = funct_driver.Replace(" e1", " false");
            else
                funct_driver = funct_driver.Replace(" e1", " " + cbe1select);
            if (cbe2select == "")
                funct_driver = funct_driver.Replace(" e2", " false");
            else
                funct_driver = funct_driver.Replace(" e2", " " + cbe2select);
            if (cbx1select == "")
                funct_driver = funct_driver.Replace(" x1", " false");
            else
                funct_driver = funct_driver.Replace(" x1", " " + cbx1select);
            if (cbx2select == "")
                funct_driver = funct_driver.Replace(" x2", " false");
            else
                funct_driver = funct_driver.Replace(" x2", " " + cbx2select);
            if (cbc1select == "")
                funct_driver = funct_driver.Replace(" c1", " false");
            else
                funct_driver = funct_driver.Replace(" c1", " " + cbc1select);
            if (cbc2select == "")
                funct_driver = funct_driver.Replace(" c2", " false");
            else
                funct_driver = funct_driver.Replace(" c2", " " + cbc2select);
            if (cbv1select == "")
                funct_driver = funct_driver.Replace(" v1", " false");
            else
                funct_driver = funct_driver.Replace(" v1", " " + cbv1select);
            if (cbv2select == "")
                funct_driver = funct_driver.Replace(" v2", " false");
            else
                funct_driver = funct_driver.Replace(" v2", " " + cbv2select);
            if (cbspace1select == "")
                funct_driver = funct_driver.Replace(" space1", " false");
            else
                funct_driver = funct_driver.Replace(" space1", " " + cbspace1select);
            if (cbspace2select == "")
                funct_driver = funct_driver.Replace(" space2", " false");
            else
                funct_driver = funct_driver.Replace(" space2", " " + cbspace2select);
            if (cbf1select == "")
                funct_driver = funct_driver.Replace(" f1", " false");
            else
                funct_driver = funct_driver.Replace(" f1", " " + cbf1select);
            if (cbf2select == "")
                funct_driver = funct_driver.Replace(" f2", " false");
            else
                funct_driver = funct_driver.Replace(" f2", " " + cbf2select);
            if (cbr1select == "")
                funct_driver = funct_driver.Replace(" r1", " false");
            else
                funct_driver = funct_driver.Replace(" r1", " " + cbr1select);
            if (cbr2select == "")
                funct_driver = funct_driver.Replace(" r2", " false");
            else
                funct_driver = funct_driver.Replace(" r2", " " + cbr2select);
            if (cbuproll1select == "")
                funct_driver = funct_driver.Replace(" uproll1", " false");
            else
                funct_driver = funct_driver.Replace(" uproll1", " " + cbuproll1select);
            if (cbuproll2select == "")
                funct_driver = funct_driver.Replace(" uproll2", " false");
            else
                funct_driver = funct_driver.Replace(" uproll2", " " + cbuproll2select);
            if (cbdownroll1select == "")
                funct_driver = funct_driver.Replace(" downroll1", " false");
            else
                funct_driver = funct_driver.Replace(" downroll1", " " + cbdownroll1select);
            if (cbdownroll2select == "")
                funct_driver = funct_driver.Replace(" downroll2", " false");
            else
                funct_driver = funct_driver.Replace(" downroll2", " " + cbdownroll2select);
            if (cbleftclick1select == "")
                funct_driver = funct_driver.Replace(" leftclick1", " false");
            else
                funct_driver = funct_driver.Replace(" leftclick1", " " + cbleftclick1select);
            if (cbleftclick2select == "")
                funct_driver = funct_driver.Replace(" leftclick2", " false");
            else
                funct_driver = funct_driver.Replace(" leftclick2", " " + cbleftclick2select);
            if (cbrightclick1select == "")
                funct_driver = funct_driver.Replace(" rightclick1", " false");
            else
                funct_driver = funct_driver.Replace(" rightclick1", " " + cbrightclick1select);
            if (cbrightclick2select == "")
                funct_driver = funct_driver.Replace(" rightclick2", " false");
            else
                funct_driver = funct_driver.Replace(" rightclick2", " " + cbrightclick2select);
            if (cbsendaselect == "SendA")
                finalcode = code.Replace("funct_driver", funct_driver.Replace("SendEscape ", cbsendescapeselect + " ").Replace("SendTab ", cbsendtabselect + " ").Replace("SendLeftShift ", cbsendshiftselect + " ").Replace("SendLeftControl ", cbsendcontrolselect + " ").Replace("SendE ", cbsendeselect + " ").Replace("SendX ", cbsendxselect + " ").Replace("SendC ", cbsendcselect + " ").Replace("SendV ", cbsendvselect + " ").Replace("SendSpace ", cbsendspaceselect + " ").Replace("SendF ", cbsendfselect + " ").Replace("SendR ", cbsendrselect + " ").Replace("SendWheelUp ", cbsenduprollselect + " ").Replace("SendWheelDown ", cbsenddownrollselect + " ").Replace("SendLeftClick ", cbsendleftclickselect + " ").Replace("SendRightClick ", cbsendrightclickselect + " "));
            else
                finalcode = code.Replace("funct_driver", funct_driver.Replace("SendEscape ", cbsendescapeselect + " ").Replace("SendTab ", cbsendtabselect + " ").Replace("SendLeftShift ", cbsendshiftselect + " ").Replace("SendLeftControl ", cbsendcontrolselect + " ").Replace("SendA ", cbsendaselect + " ").Replace("SendQ ", cbsendaselect + " ").Replace("SendE ", cbsendeselect + " ").Replace("SendX ", cbsendxselect + " ").Replace("SendC ", cbsendcselect + " ").Replace("SendV ", cbsendvselect + " ").Replace("SendSpace ", cbsendspaceselect + " ").Replace("SendF ", cbsendfselect + " ").Replace("SendR ", cbsendrselect + " ").Replace("SendWheelUp ", cbsenduprollselect + " ").Replace("SendWheelDown ", cbsenddownrollselect + " ").Replace("SendLeftClick ", cbsendleftclickselect + " ").Replace("SendRightClick ", cbsendrightclickselect + " "));
            System.CodeDom.Compiler.CompilerParameters parameters = new System.CodeDom.Compiler.CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("System.Drawing.dll");
            Microsoft.CSharp.CSharpCodeProvider provider = new Microsoft.CSharp.CSharpCodeProvider();
            System.CodeDom.Compiler.CompilerResults results = provider.CompileAssemblyFromSource(parameters, finalcode);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}) : {1}", error.ErrorNumber, error.ErrorText));
                }
                MessageBox.Show("Script Error :\n\r" + sb.ToString());
                return;
            }
            Assembly assembly = results.CompiledAssembly;
            program = assembly.GetType("StringToCode.FooClass");
            obj = Activator.CreateInstance(program);
            mousex = 0;
            mousey = 0;
            mousexp = 0;
            mouseyp = 0;
            mousexpp = 0;
            mouseypp = 0;
            MouseDesktopX = 0;
            MouseDesktopY = 0;
            MouseMoveX = 0;
            MouseMoveY = 0;
            MouseAbsX = 0;
            MouseAbsY = 0;
            SendKeys.UnLoadKM();
            scriptenabling = true;
        }
        public static void Start()
        {
            running = true;
            do
                Thread.Sleep(1);
            while (!wiimotejoyconleftconnect());
            ScanScanLeft();
            Task.Run(() => taskD());
            Task.Run(() => taskDLeft());
            Thread.Sleep(1000);
            calibrationinit = -aBuffer[4] + 135f;
            stick_rawLeft[0] = report_bufLeft[6 + (ISLEFT ? 0 : 3)];
            stick_rawLeft[1] = report_bufLeft[7 + (ISLEFT ? 0 : 3)];
            stick_rawLeft[2] = report_bufLeft[8 + (ISLEFT ? 0 : 3)];
            stickCenterLeft[0] = (UInt16)(stick_rawLeft[0] | ((stick_rawLeft[1] & 0xf) << 8));
            stickCenterLeft[1] = (UInt16)((stick_rawLeft[1] >> 4) | (stick_rawLeft[2] << 4));
            acc_gcalibrationLeftX = (Int16)(report_bufLeft[13] | ((report_bufLeft[14] << 8) & 0xff00));
            acc_gcalibrationLeftY = (Int16)(report_bufLeft[15] | ((report_bufLeft[16] << 8) & 0xff00));
            acc_gcalibrationLeftZ = (Int16)(report_bufLeft[17] | ((report_bufLeft[18] << 8) & 0xff00));
            acc_gLeft.X = ((Int16)(report_bufLeft[13] | ((report_bufLeft[14] << 8) & 0xff00)) - acc_gcalibrationLeftX) * (1.0f / 4000f);
            acc_gLeft.Y = ((Int16)(report_bufLeft[15] | ((report_bufLeft[16] << 8) & 0xff00)) - acc_gcalibrationLeftY) * (1.0f / 4000f);
            acc_gLeft.Z = ((Int16)(report_bufLeft[17] | ((report_bufLeft[18] << 8) & 0xff00)) - acc_gcalibrationLeftZ) * (1.0f / 4000f);
            InitDirectAnglesLeft = acc_gLeft;
            enabling = true;
        }
        public static void taskB()
        {
            if (enabling)
            {
                if (irmode == 1)
                {
                    WiimoteIRSensors0X = aBuffer[6] | ((aBuffer[8] >> 4) & 0x03) << 8;
                    WiimoteIRSensors0Y = aBuffer[7] | ((aBuffer[8] >> 6) & 0x03) << 8;
                    WiimoteIRSensors1X = aBuffer[9] | ((aBuffer[8] >> 0) & 0x03) << 8;
                    WiimoteIRSensors1Y = aBuffer[10] | ((aBuffer[8] >> 2) & 0x03) << 8;
                    WiimoteIR0found = WiimoteIRSensors0X > 0f & WiimoteIRSensors0X <= 1024f & WiimoteIRSensors0Y > 0f & WiimoteIRSensors0Y <= 768f;
                    WiimoteIR1found = WiimoteIRSensors1X > 0f & WiimoteIRSensors1X <= 1024f & WiimoteIRSensors1Y > 0f & WiimoteIRSensors1Y <= 768f;
                    if (WiimoteIR0found)
                    {
                        WiimoteIRSensors0Xcam = WiimoteIRSensors0X - 512f;
                        WiimoteIRSensors0Ycam = WiimoteIRSensors0Y - 384f;
                    }
                    if (WiimoteIR1found)
                    {
                        WiimoteIRSensors1Xcam = WiimoteIRSensors1X - 512f;
                        WiimoteIRSensors1Ycam = WiimoteIRSensors1Y - 384f;
                    }
                    if (WiimoteIR0found & WiimoteIR1found)
                    {
                        WiimoteIRSensorsXcam = (WiimoteIRSensors0Xcam + WiimoteIRSensors1Xcam) / 2f;
                        WiimoteIRSensorsYcam = (WiimoteIRSensors0Ycam + WiimoteIRSensors1Ycam) / 2f;
                    }
                    if (WiimoteIR0found)
                    {
                        irx0 = 2 * WiimoteIRSensors0Xcam - WiimoteIRSensorsXcam;
                        iry0 = 2 * WiimoteIRSensors0Ycam - WiimoteIRSensorsYcam;
                    }
                    if (WiimoteIR1found)
                    {
                        irx1 = 2 * WiimoteIRSensors1Xcam - WiimoteIRSensorsXcam;
                        iry1 = 2 * WiimoteIRSensors1Ycam - WiimoteIRSensorsYcam;
                    }
                    irxc = irx0 + irx1;
                    iryc = iry0 + iry1;
                }
                else
                {
                    WiimoteIR0found = (aBuffer[6] | ((aBuffer[8] >> 4) & 0x03) << 8) > 1 & (aBuffer[6] | ((aBuffer[8] >> 4) & 0x03) << 8) < 1023;
                    WiimoteIR1found = (aBuffer[9] | ((aBuffer[8] >> 0) & 0x03) << 8) > 1 & (aBuffer[9] | ((aBuffer[8] >> 0) & 0x03) << 8) < 1023;
                    if (WiimoteIR0notfound == 0 & WiimoteIR1found)
                        WiimoteIR0notfound = 1;
                    if (WiimoteIR0notfound == 1 & !WiimoteIR0found & !WiimoteIR1found)
                        WiimoteIR0notfound = 2;
                    if (WiimoteIR0notfound == 2 & WiimoteIR0found)
                    {
                        WiimoteIR0notfound = 0;
                        if (!WiimoteIRswitch)
                            WiimoteIRswitch = true;
                        else
                            WiimoteIRswitch = false;
                    }
                    if (WiimoteIR0notfound == 0 & WiimoteIR0found)
                        WiimoteIR0notfound = 0;
                    if (WiimoteIR0notfound == 0 & !WiimoteIR0found & !WiimoteIR1found)
                        WiimoteIR0notfound = 0;
                    if (WiimoteIR0notfound == 1 & WiimoteIR0found)
                        WiimoteIR0notfound = 0;
                    if (WiimoteIR0found)
                    {
                        WiimoteIRSensors0X = (aBuffer[6] | ((aBuffer[8] >> 4) & 0x03) << 8);
                        WiimoteIRSensors0Y = (aBuffer[7] | ((aBuffer[8] >> 6) & 0x03) << 8);
                    }
                    if (WiimoteIR1found)
                    {
                        WiimoteIRSensors1X = (aBuffer[9] | ((aBuffer[8] >> 0) & 0x03) << 8);
                        WiimoteIRSensors1Y = (aBuffer[10] | ((aBuffer[8] >> 2) & 0x03) << 8);
                    }
                    if (WiimoteIRswitch)
                    {
                        WiimoteIR0foundcam = WiimoteIR0found;
                        WiimoteIR1foundcam = WiimoteIR1found;
                        WiimoteIRSensors0Xcam = WiimoteIRSensors0X - 512f;
                        WiimoteIRSensors0Ycam = WiimoteIRSensors0Y - 384f;
                        WiimoteIRSensors1Xcam = WiimoteIRSensors1X - 512f;
                        WiimoteIRSensors1Ycam = WiimoteIRSensors1Y - 384f;
                    }
                    else
                    {
                        WiimoteIR1foundcam = WiimoteIR0found;
                        WiimoteIR0foundcam = WiimoteIR1found;
                        WiimoteIRSensors1Xcam = WiimoteIRSensors0X - 512f;
                        WiimoteIRSensors1Ycam = WiimoteIRSensors0Y - 384f;
                        WiimoteIRSensors0Xcam = WiimoteIRSensors1X - 512f;
                        WiimoteIRSensors0Ycam = WiimoteIRSensors1Y - 384f;
                    }
                    if (WiimoteIR0foundcam & WiimoteIR1foundcam)
                    {
                        irx2 = WiimoteIRSensors0Xcam;
                        iry2 = WiimoteIRSensors0Ycam;
                        irx3 = WiimoteIRSensors1Xcam;
                        iry3 = WiimoteIRSensors1Ycam;
                        WiimoteIRSensorsXcam = WiimoteIRSensors0Xcam - WiimoteIRSensors1Xcam;
                        WiimoteIRSensorsYcam = WiimoteIRSensors0Ycam - WiimoteIRSensors1Ycam;
                    }
                    if (WiimoteIR0foundcam & !WiimoteIR1foundcam)
                    {
                        irx2 = WiimoteIRSensors0Xcam;
                        iry2 = WiimoteIRSensors0Ycam;
                        irx3 = WiimoteIRSensors0Xcam - WiimoteIRSensorsXcam;
                        iry3 = WiimoteIRSensors0Ycam - WiimoteIRSensorsYcam;
                    }
                    if (WiimoteIR1foundcam & !WiimoteIR0foundcam)
                    {
                        irx3 = WiimoteIRSensors1Xcam;
                        iry3 = WiimoteIRSensors1Ycam;
                        irx2 = WiimoteIRSensors1Xcam + WiimoteIRSensorsXcam;
                        iry2 = WiimoteIRSensors1Ycam + WiimoteIRSensorsYcam;
                    }
                    irxc = irx2 + irx3;
                    iryc = iry2 + iry3;
                }
                irx = irxc * (1024f / 1346f);
                iry = iryc + centery >= 0 ? Scale(iryc + centery, 0f, 782f + centery, 0f, 1024f) : Scale(iryc + centery, -782f + centery, 0f, -1024f, 0f);
                WiimoteButtonStateA = (aBuffer[2] & 0x08) != 0;
                WiimoteButtonStateB = (aBuffer[2] & 0x04) != 0;
                WiimoteButtonStateMinus = (aBuffer[2] & 0x10) != 0;
                WiimoteButtonStateHome = (aBuffer[2] & 0x80) != 0;
                WiimoteButtonStatePlus = (aBuffer[1] & 0x10) != 0;
                WiimoteButtonStateOne = (aBuffer[2] & 0x02) != 0;
                WiimoteButtonStateTwo = (aBuffer[2] & 0x01) != 0;
                WiimoteButtonStateUp = (aBuffer[1] & 0x08) != 0;
                WiimoteButtonStateDown = (aBuffer[1] & 0x04) != 0;
                WiimoteButtonStateLeft = (aBuffer[1] & 0x01) != 0;
                WiimoteButtonStateRight = (aBuffer[1] & 0x02) != 0;
                WiimoteRawValuesX = aBuffer[3] - 135f + calibrationinit;
                WiimoteRawValuesY = aBuffer[4] - 135f + calibrationinit;
                WiimoteRawValuesZ = aBuffer[5] - 135f + calibrationinit;
                WiimoteButtonACC = (WiimoteRawValuesZ > 0 ? WiimoteRawValuesZ : -WiimoteRawValuesZ) >= 30f & (WiimoteRawValuesY > 0 ? WiimoteRawValuesY : -WiimoteRawValuesY) >= 30f & (WiimoteRawValuesX > 0 ? WiimoteRawValuesX : -WiimoteRawValuesX) >= 30f;
                stick_rawLeft[0] = report_bufLeft[6 + (ISLEFT ? 0 : 3)];
                stick_rawLeft[1] = report_bufLeft[7 + (ISLEFT ? 0 : 3)];
                stick_rawLeft[2] = report_bufLeft[8 + (ISLEFT ? 0 : 3)];
                stickLeft[0] = ((UInt16)(stick_rawLeft[0] | ((stick_rawLeft[1] & 0xf) << 8)) - stickCenterLeft[0]) / 1440f;
                stickLeft[1] = ((UInt16)((stick_rawLeft[1] >> 4) | (stick_rawLeft[2] << 4)) - stickCenterLeft[1]) / 1440f;
                JoyconLeftStickX = stickLeft[0];
                JoyconLeftStickY = stickLeft[1];
                acc_gLeft.X = ((Int16)(report_bufLeft[13 + 0 * 12] | ((report_bufLeft[14 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[13 + 1 * 12] | ((report_bufLeft[14 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[13 + 2 * 12] | ((report_bufLeft[14 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationLeftX) * (1.0f / 12000f);
                acc_gLeft.Y = -((Int16)(report_bufLeft[15 + 0 * 12] | ((report_bufLeft[16 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[15 + 1 * 12] | ((report_bufLeft[16 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[15 + 2 * 12] | ((report_bufLeft[16 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationLeftY) * (1.0f / 12000f);
                acc_gLeft.Z = -((Int16)(report_bufLeft[17 + 0 * 12] | ((report_bufLeft[18 + 0 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[17 + 1 * 12] | ((report_bufLeft[18 + 1 * 12] << 8) & 0xff00)) + (Int16)(report_bufLeft[17 + 2 * 12] | ((report_bufLeft[18 + 2 * 12] << 8) & 0xff00)) - acc_gcalibrationLeftZ) * (1.0f / 12000f);
                JoyconLeftButtonSHOULDER_1 = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & 0x40) != 0;
                JoyconLeftButtonSHOULDER_2 = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & 0x80) != 0;
                JoyconLeftButtonSR = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & 0x10) != 0;
                JoyconLeftButtonSL = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & 0x20) != 0;
                JoyconLeftButtonDPAD_DOWN = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & (ISLEFT ? 0x01 : 0x04)) != 0;
                JoyconLeftButtonDPAD_RIGHT = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & (ISLEFT ? 0x04 : 0x08)) != 0;
                JoyconLeftButtonDPAD_UP = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & (ISLEFT ? 0x02 : 0x02)) != 0;
                JoyconLeftButtonDPAD_LEFT = (report_bufLeft[3 + (ISLEFT ? 2 : 0)] & (ISLEFT ? 0x08 : 0x01)) != 0;
                JoyconLeftButtonMINUS = (report_bufLeft[4] & 0x01) != 0;
                JoyconLeftButtonCAPTURE = (report_bufLeft[4] & 0x20) != 0;
                JoyconLeftButtonSTICK = (report_bufLeft[4] & (ISLEFT ? 0x08 : 0x04)) != 0;
                JoyconLeftButtonACC = acc_gLeft.X <= -1.13;
                JoyconLeftButtonSMA = JoyconLeftButtonSL | JoyconLeftButtonSR | JoyconLeftButtonMINUS | JoyconLeftButtonACC;
                if (LeftValListY.Count >= 50)
                {
                    LeftValListY.RemoveAt(0);
                    LeftValListY.Add(acc_gLeft.Y);
                }
                else
                    LeftValListY.Add(acc_gLeft.Y);
                JoyconLeftRollLeft = LeftValListY.Average() <= -0.75f;
                JoyconLeftRollRight = LeftValListY.Average() >= 0.75f;
                DirectAnglesLeft = acc_gLeft - InitDirectAnglesLeft;
                JoyconLeftButtonACC = acc_gLeft.X <= -1.13;
                JoyconLeftAccelX = DirectAnglesLeft.X * 1350f;
                JoyconLeftAccelY = -DirectAnglesLeft.Y * 1350f;
            }
        }
        public static void taskX()
        {
            valchanged(0, GetAsyncKeyState(Keys.F9));
            if (wd[0] == 1 & !getstate[0])
            {
                if (sendinput)
                    KeyboardMouseDriverType = "sendinput";
                else
                    KeyboardMouseDriverType = "kmevent";
                getstate[0] = true;
            }
            else
            {
                if (wd[0] == 1 & getstate[0])
                {
                    mousex = 0;
                    mousey = 0;
                    mousexp = 0;
                    mouseyp = 0;
                    mousexpp = 0;
                    mouseypp = 0;
                    MouseDesktopX = 0;
                    MouseDesktopY = 0;
                    MouseMoveX = 0;
                    MouseMoveY = 0;
                    MouseAbsX = 0;
                    MouseAbsY = 0;
                    SendKeys.SetKM(KeyboardMouseDriverType, 0, 0, 0, 0, 0, 0, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
                    getstate[0] = false;
                }
            }
            if (enabling & getstate[0])
            {
                if (irx >= 0f & irx <= 1024f)
                    mousex = Scale(irx * irx * irx / 1024f / 1024f * viewpower3x + irx * irx / 1024f * viewpower2x + irx * viewpower1x, 0f, 1024f, (dzx / 10000f) * 1024f, 1024f);
                if (irx <= 0f & irx >= -1024f)
                    mousex = Scale(-(-irx * -irx * -irx) / 1024f / 1024f * viewpower3x - (-irx * -irx) / 1024f * viewpower2x - (-irx) * viewpower1x, -1024f, 0f, -1024f, -(dzx / 10000f) * 1024f);
                if (iry >= 0f & iry <= 1024f)
                    mousey = Scale(iry * iry * iry / 1024f / 1024f * viewpower3y + iry * iry / 1024f * viewpower2y + iry * viewpower1y, 0f, 1024f, (dzy / 10000f) * 1024f, 1024f);
                if (iry <= 0f & iry >= -1024f)
                    mousey = Scale(-(-iry * -iry * -iry) / 1024f / 1024f * viewpower3y - (-iry * -iry) / 1024f * viewpower2y - (-iry) * viewpower1y, -1024f, 0f, -1024f, -(dzy / 10000f) * 1024f);
                mousexp = mousex * irxsens;
                mouseyp = mousey * irysens;
                if (cursor)
                {
                    WidthS = Screen.PrimaryScreen.Bounds.Width / 2;
                    HeightS = Screen.PrimaryScreen.Bounds.Height / 2;
                    MouseDesktopX = (int)(WidthS - mousex * WidthS / 1024f);
                    MouseDesktopY = (int)(HeightS + mousey * HeightS / 1024f);
                }
                if (brink)
                {
                    MouseMoveX = -mousexp;
                    MouseMoveY = mouseyp;
                }
                if (mw3)
                {
                    MouseAbsX = (int)(32767.5f - mousexp * 32767.5f / 1024f);
                    MouseAbsY = (int)(mouseyp * 32767.5f / 1024f + 32767.5f);
                }
                if (titanfall)
                {
                    MouseAbsX = -(int)mousexp;
                    MouseAbsY = (int)mouseyp;
                }
                if (metro)
                {
                    mousexpp += mousexp;
                    mouseypp += mouseyp;
                    MouseAbsX = (int)(32767.5f - mousexp - mousexpp);
                    MouseAbsY = (int)(mouseyp + mouseypp + 32767.5f);
                }
                if (bo3)
                {
                    MouseAbsX = -(int)mousexp;
                    MouseAbsY = (int)mouseyp;
                }
                if (fortnite)
                {
                    MouseMoveX = -mousexp;
                    MouseMoveY = mouseyp;
                    MouseAbsX = (int)(32767.5f - mousex * 32767.5f / 1024f);
                    MouseAbsY = (int)(mousey * 32767.5f / 1024f + 32767.5f);
                    WidthS = Screen.PrimaryScreen.Bounds.Width / 2;
                    HeightS = Screen.PrimaryScreen.Bounds.Height / 2;
                    MouseDesktopX = (int)(WidthS - mousex * WidthS / 1024f);
                    MouseDesktopY = (int)(HeightS + mousey * HeightS / 1024f);
                }
                if (fake)
                {
                    MouseAbsX = (int)(32767.5f - mousexp);
                    MouseAbsY = (int)(mouseyp + 32767.5f);
                }
                if (mc)
                {
                    MouseMoveX = -mousexp;
                    MouseMoveY = mouseyp;
                    WidthS = Screen.PrimaryScreen.Bounds.Width / 2;
                    HeightS = Screen.PrimaryScreen.Bounds.Height / 2;
                    MouseDesktopX = (int)(WidthS - mousex * WidthS / 1024f);
                    MouseDesktopY = (int)(HeightS + mousey * HeightS / 1024f);
                }
                if (scriptenabling)
                {
                    object[] val = (object[])program.InvokeMember("Main", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, new object[] { englishkeyboard, adsswitch, JoyconLeftButtonDPAD_DOWN, JoyconLeftButtonDPAD_LEFT, JoyconLeftButtonDPAD_RIGHT, JoyconLeftButtonDPAD_UP, JoyconLeftButtonMINUS, JoyconLeftButtonACC, JoyconLeftButtonSHOULDER_1, JoyconLeftButtonSHOULDER_2, JoyconLeftButtonCAPTURE, WiimoteButtonStateOne, WiimoteButtonStateTwo, WiimoteButtonStateDown, WiimoteButtonStateLeft, WiimoteButtonStateRight, WiimoteButtonStateUp, WiimoteButtonStateHome, WiimoteButtonACC, WiimoteButtonStateA, WiimoteButtonStateB, WiimoteButtonStatePlus, WiimoteButtonStateMinus });
                    SendLeftClick = (bool)val[0]; SendRightClick = (bool)val[1]; SendMiddleClick = (bool)val[2]; SendWheelUp = (bool)val[3]; SendWheelDown = (bool)val[4]; SendLeft = (bool)val[5]; SendRight = (bool)val[6]; SendUp = (bool)val[7]; SendDown = (bool)val[8]; SendLButton = (bool)val[9]; SendRButton = (bool)val[10]; SendCancel = (bool)val[11]; SendMBUTTON = (bool)val[12]; SendXBUTTON1 = (bool)val[12]; SendXBUTTON2 = (bool)val[14]; SendBack = (bool)val[15]; SendTab = (bool)val[16]; SendClear = (bool)val[17]; SendReturn = (bool)val[18]; SendSHIFT = (bool)val[19]; SendCONTROL = (bool)val[20]; SendMENU = (bool)val[21]; SendPAUSE = (bool)val[22]; SendCAPITAL = (bool)val[23]; SendKANA = (bool)val[24]; SendHANGEUL = (bool)val[25]; SendHANGUL = (bool)val[26]; SendJUNJA = (bool)val[27]; SendFINAL = (bool)val[28]; SendHANJA = (bool)val[29]; SendKANJI = (bool)val[30]; SendEscape = (bool)val[31]; SendCONVERT = (bool)val[32]; SendNONCONVERT = (bool)val[33]; SendACCEPT = (bool)val[34]; SendMODECHANGE = (bool)val[35]; SendSpace = (bool)val[36]; SendPRIOR = (bool)val[37]; SendNEXT = (bool)val[38]; SendEND = (bool)val[39]; SendHOME = (bool)val[40]; SendLEFT = (bool)val[41]; SendUP = (bool)val[42]; SendRIGHT = (bool)val[43]; SendDOWN = (bool)val[44]; SendSELECT = (bool)val[45]; SendPRINT = (bool)val[46]; SendEXECUTE = (bool)val[47]; SendSNAPSHOT = (bool)val[48]; SendINSERT = (bool)val[49]; SendDELETE = (bool)val[50]; SendHELP = (bool)val[51]; SendAPOSTROPHE = (bool)val[52]; Send0 = (bool)val[53]; Send1 = (bool)val[54]; Send2 = (bool)val[55]; Send3 = (bool)val[56]; Send4 = (bool)val[57]; Send5 = (bool)val[58]; Send6 = (bool)val[59]; Send7 = (bool)val[60]; Send8 = (bool)val[61]; Send9 = (bool)val[62]; SendA = (bool)val[63]; SendB = (bool)val[64]; SendC = (bool)val[65]; SendD = (bool)val[66]; SendE = (bool)val[67]; SendF = (bool)val[68]; SendG = (bool)val[69]; SendH = (bool)val[70]; SendI = (bool)val[71]; SendJ = (bool)val[72]; SendK = (bool)val[73]; SendL = (bool)val[74]; SendM = (bool)val[75]; SendN = (bool)val[76]; SendO = (bool)val[77]; SendP = (bool)val[78]; SendQ = (bool)val[79]; SendR = (bool)val[80]; SendS = (bool)val[81]; SendT = (bool)val[82]; SendU = (bool)val[83]; SendV = (bool)val[84]; SendW = (bool)val[85]; SendX = (bool)val[86]; SendY = (bool)val[87]; SendZ = (bool)val[88]; SendLWIN = (bool)val[89]; SendRWIN = (bool)val[90]; SendAPPS = (bool)val[91]; SendSLEEP = (bool)val[92]; SendNUMPAD0 = (bool)val[93]; SendNUMPAD1 = (bool)val[94]; SendNUMPAD2 = (bool)val[95]; SendNUMPAD3 = (bool)val[96]; SendNUMPAD4 = (bool)val[97]; SendNUMPAD5 = (bool)val[98]; SendNUMPAD6 = (bool)val[99]; SendNUMPAD7 = (bool)val[100]; SendNUMPAD8 = (bool)val[101]; SendNUMPAD9 = (bool)val[102]; SendMULTIPLY = (bool)val[103]; SendADD = (bool)val[104]; SendSEPARATOR = (bool)val[105]; SendSUBTRACT = (bool)val[106]; SendDECIMAL = (bool)val[107]; SendDIVIDE = (bool)val[108]; SendF1 = (bool)val[109]; SendF2 = (bool)val[110]; SendF3 = (bool)val[111]; SendF4 = (bool)val[112]; SendF5 = (bool)val[113]; SendF6 = (bool)val[114]; SendF7 = (bool)val[115]; SendF8 = (bool)val[116]; SendF9 = (bool)val[117]; SendF10 = (bool)val[118]; SendF11 = (bool)val[119]; SendF12 = (bool)val[120]; SendF13 = (bool)val[121]; SendF14 = (bool)val[122]; SendF15 = (bool)val[123]; SendF16 = (bool)val[124]; SendF17 = (bool)val[125]; SendF18 = (bool)val[126]; SendF19 = (bool)val[127]; SendF20 = (bool)val[128]; SendF21 = (bool)val[129]; SendF22 = (bool)val[130]; SendF23 = (bool)val[131]; SendF24 = (bool)val[132]; SendNUMLOCK = (bool)val[133]; SendSCROLL = (bool)val[134]; SendLeftShift = (bool)val[135]; SendRightShift = (bool)val[136]; SendLeftControl = (bool)val[137]; SendRightControl = (bool)val[138]; SendLMENU = (bool)val[139]; SendRMENU = (bool)val[140];
                }
                if (!cbarrows1 & !cbarrows2 & !cbnumpad)
                {
                    if (englishkeyboard)
                    {
                        SendD = JoyconLeftStickX > 0.35f / stickxsens;
                        SendA = JoyconLeftStickX < -0.35f / stickxsens;
                        SendW = JoyconLeftStickY > 0.35f / stickysens;
                        SendS = JoyconLeftStickY < -0.35f / stickysens;
                    }
                    else
                    {
                        SendD = JoyconLeftStickX > 0.35f / stickxsens;
                        SendQ = JoyconLeftStickX < -0.35f / stickxsens;
                        SendZ = JoyconLeftStickY > 0.35f / stickysens;
                        SendS = JoyconLeftStickY < -0.35f / stickysens;
                    }
                }
                else if (cbarrows1)
                {
                    SendRight = JoyconLeftStickX > 0.35f / stickxsens;
                    SendLeft = JoyconLeftStickX < -0.35f / stickxsens;
                    SendUp = JoyconLeftStickY > 0.35f / stickysens;
                    SendDown = JoyconLeftStickY < -0.35f / stickysens;
                }
                else if (cbarrows2)
                {
                    SendRIGHT = JoyconLeftStickX > 0.35f / stickxsens;
                    SendLEFT = JoyconLeftStickX < -0.35f / stickxsens;
                    SendUP = JoyconLeftStickY > 0.35f / stickysens;
                    SendDOWN = JoyconLeftStickY < -0.35f / stickysens;
                }
                else if (cbnumpad)
                {
                    SendNUMPAD6 = JoyconLeftStickX > 0.35f / stickxsens;
                    SendNUMPAD4 = JoyconLeftStickX < -0.35f / stickxsens;
                    SendNUMPAD8 = JoyconLeftStickY > 0.35f / stickysens;
                    SendNUMPAD2 = JoyconLeftStickY < -0.35f / stickysens;
                }
                SendKeys.SetKM(KeyboardMouseDriverType, MouseMoveX, MouseMoveY, MouseAbsX, MouseAbsY, MouseDesktopX, MouseDesktopY, SendLeftClick, SendRightClick, SendMiddleClick, SendWheelUp, SendWheelDown, SendLeft, SendRight, SendUp, SendDown, SendLButton, SendRButton, SendCancel, SendMBUTTON, SendXBUTTON1, SendXBUTTON2, SendBack, SendTab, SendClear, SendReturn, SendSHIFT, SendCONTROL, SendMENU, SendPAUSE, SendCAPITAL, SendKANA, SendHANGEUL, SendHANGUL, SendJUNJA, SendFINAL, SendHANJA, SendKANJI, SendEscape, SendCONVERT, SendNONCONVERT, SendACCEPT, SendMODECHANGE, SendSpace, SendPRIOR, SendNEXT, SendEND, SendHOME, SendLEFT, SendUP, SendRIGHT, SendDOWN, SendSELECT, SendPRINT, SendEXECUTE, SendSNAPSHOT, SendINSERT, SendDELETE, SendHELP, SendAPOSTROPHE, Send0, Send1, Send2, Send3, Send4, Send5, Send6, Send7, Send8, Send9, SendA, SendB, SendC, SendD, SendE, SendF, SendG, SendH, SendI, SendJ, SendK, SendL, SendM, SendN, SendO, SendP, SendQ, SendR, SendS, SendT, SendU, SendV, SendW, SendX, SendY, SendZ, SendLWIN, SendRWIN, SendAPPS, SendSLEEP, SendNUMPAD0, SendNUMPAD1, SendNUMPAD2, SendNUMPAD3, SendNUMPAD4, SendNUMPAD5, SendNUMPAD6, SendNUMPAD7, SendNUMPAD8, SendNUMPAD9, SendMULTIPLY, SendADD, SendSEPARATOR, SendSUBTRACT, SendDECIMAL, SendDIVIDE, SendF1, SendF2, SendF3, SendF4, SendF5, SendF6, SendF7, SendF8, SendF9, SendF10, SendF11, SendF12, SendF13, SendF14, SendF15, SendF16, SendF17, SendF18, SendF19, SendF20, SendF21, SendF22, SendF23, SendF24, SendNUMLOCK, SendSCROLL, SendLeftShift, SendRightShift, SendLeftControl, SendRightControl, SendLMENU, SendRMENU);
                if (brink | bo3 | fortnite | mc)
                    Thread.Sleep(20);
            }
        }
        private static double Scale(double value, double min, double max, double minScale, double maxScale)
        {
            double scaled = minScale + (double)(value - min) / (max - min) * (maxScale - minScale);
            return scaled;
        }
        private static void taskD()
        {
            while (running)
            {
                try
                {
                    mStream.Read(aBuffer, 0, 22);
                }
                catch { }
            }
        }
        private static void taskDLeft()
        {
            while (running)
            {
                try
                {
                    Lhid_read_timeout(handleLeft, report_bufLeft, (UIntPtr)report_lenLeft);
                }
                catch { }
            }
        }
        public static void Close()
        {
            try
            {
                running = false;
                Thread.Sleep(100);
                SendKeys.SetKM(KeyboardMouseDriverType, 0, 0, 0, 0, 0, 0, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
                mStream.Close();
                handle.Close();
                Subcommand3Left(0x06, new byte[] { 0x01 }, 1);
                Lhid_close(handleLeft);
                handleLeft.Close();
                wiimotedisconnect();
                joyconleftdisconnect();
            }
            catch { }
        }
        private const string vendor_id = "57e", vendor_id_ = "057e", product_r1 = "0330", product_r2 = "0306", product_l = "2006";
        private enum EFileAttributes : uint
        {
            Overlapped = 0x40000000,
            Normal = 0x80
        };
        struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid InterfaceClassGuid;
            public int Flags;
            public IntPtr RESERVED;
        }
        struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public UInt32 cbSize;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 256)]
            public string DevicePath;
        }
        [DllImport("MotionInputPairing.dll", EntryPoint = "wiimotejoyconleftconnect")]
        public static extern bool wiimotejoyconleftconnect();
        [DllImport("MotionInputPairing.dll", EntryPoint = "joyconleftdisconnect")]
        public static extern bool joyconleftdisconnect();
        [DllImport("MotionInputPairing.dll", EntryPoint = "wiimotedisconnect")]
        public static extern bool wiimotedisconnect();
        private static bool ScanScanLeft()
        {
            ISWIIMOTE = false;
            ISLEFT = false;
            int index = 0;
            System.Guid guid;
            HidD_GetHidGuid(out guid);
            System.IntPtr hDevInfo = SetupDiGetClassDevs(ref guid, null, new System.IntPtr(), 0x00000010);
            SP_DEVICE_INTERFACE_DATA diData = new SP_DEVICE_INTERFACE_DATA();
            diData.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(diData);
            while (SetupDiEnumDeviceInterfaces(hDevInfo, new System.IntPtr(), ref guid, index, ref diData))
            {
                System.UInt32 size;
                SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, new System.IntPtr(), 0, out size, new System.IntPtr());
                SP_DEVICE_INTERFACE_DETAIL_DATA diDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                diDetail.cbSize = 5;
                if (SetupDiGetDeviceInterfaceDetail(hDevInfo, ref diData, ref diDetail, size, out size, new System.IntPtr()))
                {
                    if ((diDetail.DevicePath.Contains(vendor_id) | diDetail.DevicePath.Contains(vendor_id_)) & (diDetail.DevicePath.Contains(product_r1) | diDetail.DevicePath.Contains(product_r2)))
                    {
                        ISWIIMOTE = true;
                        WiimoteFound(diDetail.DevicePath);
                    }
                    if ((diDetail.DevicePath.Contains(vendor_id) | diDetail.DevicePath.Contains(vendor_id_)) & diDetail.DevicePath.Contains(product_l))
                    {
                        ISLEFT = true;
                        AttachJoyLeft(diDetail.DevicePath);
                    }
                    if (ISWIIMOTE & ISLEFT)
                        return true;
                }
                index++;
            }
            return false;
        }
        private static void WiimoteFound(string path)
        {
            do
            {
                handle = CreateFile(path, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, (uint)EFileAttributes.Overlapped, IntPtr.Zero);
                WriteData(handle, IR, (int)REGISTER_IR, new byte[] { 0x08 }, 1);
                WriteData(handle, Type, (int)REGISTER_EXTENSION_INIT_1, new byte[] { 0x55 }, 1);
                WriteData(handle, Type, (int)REGISTER_EXTENSION_INIT_2, new byte[] { 0x00 }, 1);
                WriteData(handle, Type, (int)REGISTER_MOTIONPLUS_INIT, new byte[] { 0x04 }, 1);
                ReadData(handle, 0x0016, 7);
                ReadData(handle, (int)REGISTER_EXTENSION_TYPE, 6);
                ReadData(handle, (int)REGISTER_EXTENSION_CALIBRATION, 16);
                ReadData(handle, (int)REGISTER_EXTENSION_CALIBRATION, 32);
            }
            while (handle.IsInvalid);
            mStream = new FileStream(handle, FileAccess.ReadWrite, 22, true);
        }
        private static void ReadData(SafeFileHandle _hFile, int address, short size)
        {
            mBuff[0] = (byte)ReadMemory;
            mBuff[1] = (byte)((address & 0xff000000) >> 24);
            mBuff[2] = (byte)((address & 0x00ff0000) >> 16);
            mBuff[3] = (byte)((address & 0x0000ff00) >> 8);
            mBuff[4] = (byte)(address & 0x000000ff);
            mBuff[5] = (byte)((size & 0xff00) >> 8);
            mBuff[6] = (byte)(size & 0xff);
            HidD_SetOutputReport(_hFile.DangerousGetHandle(), mBuff, 22);
        }
        private static void WriteData(SafeFileHandle _hFile, byte mbuff, int address, byte[] buff, short size)
        {
            mBuff[0] = (byte)mbuff;
            mBuff[1] = (byte)(0x04);
            mBuff[2] = (byte)IRExtensionAccel;
            Array.Copy(buff, 0, mBuff, 3, 1);
            HidD_SetOutputReport(_hFile.DangerousGetHandle(), mBuff, 22);
            mBuff[0] = (byte)WriteMemory;
            mBuff[1] = (byte)(((address & 0xff000000) >> 24));
            mBuff[2] = (byte)((address & 0x00ff0000) >> 16);
            mBuff[3] = (byte)((address & 0x0000ff00) >> 8);
            mBuff[4] = (byte)((address & 0x000000ff) >> 0);
            mBuff[5] = (byte)size;
            Array.Copy(buff, 0, mBuff, 6, 1);
            HidD_SetOutputReport(_hFile.DangerousGetHandle(), mBuff, 22);
        }
        public static bool JoyconLeftButtonSMA, JoyconLeftButtonACC, JoyconLeftRollLeft, JoyconLeftRollRight;
        private static double JoyconLeftStickX, JoyconLeftStickY;
        public static System.Collections.Generic.List<double> LeftValListX = new System.Collections.Generic.List<double>(), LeftValListY = new System.Collections.Generic.List<double>();
        public static bool JoyconLeftGyroCenter, JoyconLeftAccelCenter;
        public static double JoyconLeftAccelX, JoyconLeftAccelY, JoyconLeftGyroX, JoyconLeftGyroY;
        public static Vector3 InitEulerAnglesaLeft, EulerAnglesaLeft, InitEulerAnglesbLeft, EulerAnglesbLeft;
        public static Vector3 gyr_gLeft = new Vector3();
        public static Vector3 i_aLeft = new Vector3(1, 0, 0);
        public static Vector3 j_aLeft = new Vector3(0, 1, 0);
        public static Vector3 k_aLeft = new Vector3(0, 0, 1);
        public static Vector3 k_aCrossLeft = new Vector3(0, 0, 1);
        public static Vector3 i_bLeft = new Vector3(1, 0, 0);
        public static Vector3 j_bLeft = new Vector3(0, 1, 0);
        public static Vector3 j_bCrossLeft = new Vector3(0, 1, 0);
        public static Vector3 k_bLeft = new Vector3(0, 0, 1);
        private static double[] stickLeft = { 0, 0 };
        private static double[] stickCenterLeft = { 0, 0 };
        private static byte[] stick_rawLeft = { 0, 0, 0 };
        public static SafeFileHandle handleLeft;
        public static Vector3 acc_gLeft = new Vector3();
        public const uint report_lenLeft = 49;
        public static Vector3 InitDirectAnglesLeft, DirectAnglesLeft;
        public static bool JoyconLeftButtonSHOULDER_1, JoyconLeftButtonSHOULDER_2, JoyconLeftButtonSR, JoyconLeftButtonSL, JoyconLeftButtonDPAD_DOWN, JoyconLeftButtonDPAD_RIGHT, JoyconLeftButtonDPAD_UP, JoyconLeftButtonDPAD_LEFT, JoyconLeftButtonMINUS, JoyconLeftButtonSTICK, JoyconLeftButtonCAPTURE, ISLEFT;
        public static byte[] report_bufLeft = new byte[report_lenLeft];
        public static float acc_gcalibrationLeftX, acc_gcalibrationLeftY, acc_gcalibrationLeftZ;
        public static void AttachJoyLeft(string path)
        {
            do
            {
                IntPtr handle = CreateFile(path, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, new System.IntPtr(), System.IO.FileMode.Open, EFileAttributes.Normal, new System.IntPtr());
                handleLeft = Lhid_open_path(handle);
                Subcommand2Left(0x40, new byte[] { 0x1 }, 1);
                Subcommand2Left(0x3, new byte[] { 0x30 }, 1);
            }
            while (handleLeft.IsInvalid);
        }
        private static void Subcommand2Left(byte sc, byte[] buf, uint len)
        {
            byte[] buf_Left = new byte[report_lenLeft];
            System.Array.Copy(buf, 0, buf_Left, 11, len);
            buf_Left[10] = sc;
            buf_Left[1] = 0;
            buf_Left[0] = 0x1;
            Lhid_write(handleLeft, buf_Left, (UIntPtr)(len + 11));
            Lhid_read_timeout(handleLeft, buf_Left, (UIntPtr)report_lenLeft);
        }
        private static void Subcommand3Left(byte sc, byte[] buf, uint len)
        {
            byte[] buf_Left = new byte[report_lenLeft];
            System.Array.Copy(buf, 0, buf_Left, 11, len);
            buf_Left[10] = sc;
            buf_Left[1] = 0x5;
            buf_Left[0] = 0x80;
            Lhid_write(handleLeft, buf_Left, new UIntPtr(2));
            buf_Left[1] = 0x6;
            buf_Left[0] = 0x80;
            Lhid_write(handleLeft, buf_Left, new UIntPtr(2));
        }
    }
    [System.Security.SuppressUnmanagedCodeSecurity]
    public class SendKeys
    {
        [DllImport("mouse.dll", EntryPoint = "MoveMouseTo", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MoveMouseTo(int x, int y);
        [DllImport("mouse.dll", EntryPoint = "MoveMouseBy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MoveMouseBy(int x, int y);
        [DllImport("keyboard.dll", EntryPoint = "SendKey", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendKey(UInt16 bVk, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SendKeyF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendKeyF(UInt16 bVk, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SendKeyArrows", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendKeyArrows(UInt16 bVk, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SendKeyArrowsF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendKeyArrowsF(UInt16 bVk, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonLeft", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendMouseEventButtonLeft();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonLeftF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendMouseEventButtonLeftF();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonRight", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendMouseEventButtonRight();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonRightF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendMouseEventButtonRightF();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonMiddle", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendMouseEventButtonMiddle();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonMiddleF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendMouseEventButtonMiddleF();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonWheelUp", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendMouseEventButtonWheelUp();
        [DllImport("keyboard.dll", EntryPoint = "SendMouseEventButtonWheelDown", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendMouseEventButtonWheelDown();
        [DllImport("mouse.dll", EntryPoint = "MouseMW3", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MouseMW3(int x, int y);
        [DllImport("mouse.dll", EntryPoint = "MouseBrink", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MouseBrink(int x, int y);
        [DllImport("keyboard.dll", EntryPoint = "SimulateKeyDown", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SimulateKeyDown(UInt16 keyCode, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SimulateKeyUp", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SimulateKeyUp(UInt16 keyCode, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SimulateKeyDownArrows", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SimulateKeyDownArrows(UInt16 keyCode, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "SimulateKeyUpArrows", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SimulateKeyUpArrows(UInt16 keyCode, UInt16 bScan);
        [DllImport("keyboard.dll", EntryPoint = "LeftClick", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LeftClick();
        [DllImport("keyboard.dll", EntryPoint = "LeftClickF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LeftClickF();
        [DllImport("keyboard.dll", EntryPoint = "RightClick", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RightClick();
        [DllImport("keyboard.dll", EntryPoint = "RightClickF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RightClickF();
        [DllImport("keyboard.dll", EntryPoint = "MiddleClick", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiddleClick();
        [DllImport("keyboard.dll", EntryPoint = "MiddleClickF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MiddleClickF();
        [DllImport("keyboard.dll", EntryPoint = "WheelDownF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WheelDownF();
        [DllImport("keyboard.dll", EntryPoint = "WheelUpF", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WheelUpF();
        [DllImport("user32.dll")]
        public static extern void SetPhysicalCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern void SetCaretPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int X, int Y);
        public const ushort VK_LBUTTON = (ushort)0x01;
        public const ushort VK_RBUTTON = (ushort)0x02;
        public const ushort VK_CANCEL = (ushort)0x03;
        public const ushort VK_MBUTTON = (ushort)0x04;
        public const ushort VK_XBUTTON1 = (ushort)0x05;
        public const ushort VK_XBUTTON2 = (ushort)0x06;
        public const ushort VK_BACK = (ushort)0x08;
        public const ushort VK_Tab = (ushort)0x09;
        public const ushort VK_CLEAR = (ushort)0x0C;
        public const ushort VK_Return = (ushort)0x0D;
        public const ushort VK_SHIFT = (ushort)0x10;
        public const ushort VK_CONTROL = (ushort)0x11;
        public const ushort VK_MENU = (ushort)0x12;
        public const ushort VK_PAUSE = (ushort)0x13;
        public const ushort VK_CAPITAL = (ushort)0x14;
        public const ushort VK_KANA = (ushort)0x15;
        public const ushort VK_HANGEUL = (ushort)0x15;
        public const ushort VK_HANGUL = (ushort)0x15;
        public const ushort VK_JUNJA = (ushort)0x17;
        public const ushort VK_FINAL = (ushort)0x18;
        public const ushort VK_HANJA = (ushort)0x19;
        public const ushort VK_KANJI = (ushort)0x19;
        public const ushort VK_Escape = (ushort)0x1B;
        public const ushort VK_CONVERT = (ushort)0x1C;
        public const ushort VK_NONCONVERT = (ushort)0x1D;
        public const ushort VK_ACCEPT = (ushort)0x1E;
        public const ushort VK_MODECHANGE = (ushort)0x1F;
        public const ushort VK_Space = (ushort)0x20;
        public const ushort VK_PRIOR = (ushort)0x21;
        public const ushort VK_NEXT = (ushort)0x22;
        public const ushort VK_END = (ushort)0x23;
        public const ushort VK_HOME = (ushort)0x24;
        public const ushort VK_LEFT = (ushort)0x25;
        public const ushort VK_UP = (ushort)0x26;
        public const ushort VK_RIGHT = (ushort)0x27;
        public const ushort VK_DOWN = (ushort)0x28;
        public const ushort VK_SELECT = (ushort)0x29;
        public const ushort VK_PRINT = (ushort)0x2A;
        public const ushort VK_EXECUTE = (ushort)0x2B;
        public const ushort VK_SNAPSHOT = (ushort)0x2C;
        public const ushort VK_INSERT = (ushort)0x2D;
        public const ushort VK_DELETE = (ushort)0x2E;
        public const ushort VK_HELP = (ushort)0x2F;
        public const ushort VK_APOSTROPHE = (ushort)0xDE;
        public const ushort VK_0 = (ushort)0x30;
        public const ushort VK_1 = (ushort)0x31;
        public const ushort VK_2 = (ushort)0x32;
        public const ushort VK_3 = (ushort)0x33;
        public const ushort VK_4 = (ushort)0x34;
        public const ushort VK_5 = (ushort)0x35;
        public const ushort VK_6 = (ushort)0x36;
        public const ushort VK_7 = (ushort)0x37;
        public const ushort VK_8 = (ushort)0x38;
        public const ushort VK_9 = (ushort)0x39;
        public const ushort VK_A = (ushort)0x41;
        public const ushort VK_B = (ushort)0x42;
        public const ushort VK_C = (ushort)0x43;
        public const ushort VK_D = (ushort)0x44;
        public const ushort VK_E = (ushort)0x45;
        public const ushort VK_F = (ushort)0x46;
        public const ushort VK_G = (ushort)0x47;
        public const ushort VK_H = (ushort)0x48;
        public const ushort VK_I = (ushort)0x49;
        public const ushort VK_J = (ushort)0x4A;
        public const ushort VK_K = (ushort)0x4B;
        public const ushort VK_L = (ushort)0x4C;
        public const ushort VK_M = (ushort)0x4D;
        public const ushort VK_N = (ushort)0x4E;
        public const ushort VK_O = (ushort)0x4F;
        public const ushort VK_P = (ushort)0x50;
        public const ushort VK_Q = (ushort)0x51;
        public const ushort VK_R = (ushort)0x52;
        public const ushort VK_S = (ushort)0x53;
        public const ushort VK_T = (ushort)0x54;
        public const ushort VK_U = (ushort)0x55;
        public const ushort VK_V = (ushort)0x56;
        public const ushort VK_W = (ushort)0x57;
        public const ushort VK_X = (ushort)0x58;
        public const ushort VK_Y = (ushort)0x59;
        public const ushort VK_Z = (ushort)0x5A;
        public const ushort VK_LWIN = (ushort)0x5B;
        public const ushort VK_RWIN = (ushort)0x5C;
        public const ushort VK_APPS = (ushort)0x5D;
        public const ushort VK_SLEEP = (ushort)0x5F;
        public const ushort VK_NUMPAD0 = (ushort)0x60;
        public const ushort VK_NUMPAD1 = (ushort)0x61;
        public const ushort VK_NUMPAD2 = (ushort)0x62;
        public const ushort VK_NUMPAD3 = (ushort)0x63;
        public const ushort VK_NUMPAD4 = (ushort)0x64;
        public const ushort VK_NUMPAD5 = (ushort)0x65;
        public const ushort VK_NUMPAD6 = (ushort)0x66;
        public const ushort VK_NUMPAD7 = (ushort)0x67;
        public const ushort VK_NUMPAD8 = (ushort)0x68;
        public const ushort VK_NUMPAD9 = (ushort)0x69;
        public const ushort VK_MULTIPLY = (ushort)0x6A;
        public const ushort VK_ADD = (ushort)0x6B;
        public const ushort VK_SEPARATOR = (ushort)0x6C;
        public const ushort VK_SUBTRACT = (ushort)0x6D;
        public const ushort VK_DECIMAL = (ushort)0x6E;
        public const ushort VK_DIVIDE = (ushort)0x6F;
        public const ushort VK_F1 = (ushort)0x70;
        public const ushort VK_F2 = (ushort)0x71;
        public const ushort VK_F3 = (ushort)0x72;
        public const ushort VK_F4 = (ushort)0x73;
        public const ushort VK_F5 = (ushort)0x74;
        public const ushort VK_F6 = (ushort)0x75;
        public const ushort VK_F7 = (ushort)0x76;
        public const ushort VK_F8 = (ushort)0x77;
        public const ushort VK_F9 = (ushort)0x78;
        public const ushort VK_F10 = (ushort)0x79;
        public const ushort VK_F11 = (ushort)0x7A;
        public const ushort VK_F12 = (ushort)0x7B;
        public const ushort VK_F13 = (ushort)0x7C;
        public const ushort VK_F14 = (ushort)0x7D;
        public const ushort VK_F15 = (ushort)0x7E;
        public const ushort VK_F16 = (ushort)0x7F;
        public const ushort VK_F17 = (ushort)0x80;
        public const ushort VK_F18 = (ushort)0x81;
        public const ushort VK_F19 = (ushort)0x82;
        public const ushort VK_F20 = (ushort)0x83;
        public const ushort VK_F21 = (ushort)0x84;
        public const ushort VK_F22 = (ushort)0x85;
        public const ushort VK_F23 = (ushort)0x86;
        public const ushort VK_F24 = (ushort)0x87;
        public const ushort VK_NUMLOCK = (ushort)0x90;
        public const ushort VK_SCROLL = (ushort)0x91;
        public const ushort VK_LeftShift = (ushort)0xA0;
        public const ushort VK_RightShift = (ushort)0xA1;
        public const ushort VK_LeftControl = (ushort)0xA2;
        public const ushort VK_RightControl = (ushort)0xA3;
        public const ushort VK_LMENU = (ushort)0xA4;
        public const ushort VK_RMENU = (ushort)0xA5;
        public const ushort VK_BROWSER_BACK = (ushort)0xA6;
        public const ushort VK_BROWSER_FORWARD = (ushort)0xA7;
        public const ushort VK_BROWSER_REFRESH = (ushort)0xA8;
        public const ushort VK_BROWSER_STOP = (ushort)0xA9;
        public const ushort VK_BROWSER_SEARCH = (ushort)0xAA;
        public const ushort VK_BROWSER_FAVORITES = (ushort)0xAB;
        public const ushort VK_BROWSER_HOME = (ushort)0xAC;
        public const ushort VK_VOLUME_MUTE = (ushort)0xAD;
        public const ushort VK_VOLUME_DOWN = (ushort)0xAE;
        public const ushort VK_VOLUME_UP = (ushort)0xAF;
        public const ushort VK_MEDIA_NEXT_TRACK = (ushort)0xB0;
        public const ushort VK_MEDIA_PREV_TRACK = (ushort)0xB1;
        public const ushort VK_MEDIA_STOP = (ushort)0xB2;
        public const ushort VK_MEDIA_PLAY_PAUSE = (ushort)0xB3;
        public const ushort VK_LAUNCH_MAIL = (ushort)0xB4;
        public const ushort VK_LAUNCH_MEDIA_SELECT = (ushort)0xB5;
        public const ushort VK_LAUNCH_APP1 = (ushort)0xB6;
        public const ushort VK_LAUNCH_APP2 = (ushort)0xB7;
        public const ushort VK_OEM_1 = (ushort)0xBA;
        public const ushort VK_OEM_PLUS = (ushort)0xBB;
        public const ushort VK_OEM_COMMA = (ushort)0xBC;
        public const ushort VK_OEM_MINUS = (ushort)0xBD;
        public const ushort VK_OEM_PERIOD = (ushort)0xBE;
        public const ushort VK_OEM_2 = (ushort)0xBF;
        public const ushort VK_OEM_3 = (ushort)0xC0;
        public const ushort VK_OEM_4 = (ushort)0xDB;
        public const ushort VK_OEM_5 = (ushort)0xDC;
        public const ushort VK_OEM_6 = (ushort)0xDD;
        public const ushort VK_OEM_7 = (ushort)0xDE;
        public const ushort VK_OEM_8 = (ushort)0xDF;
        public const ushort VK_OEM_102 = (ushort)0xE2;
        public const ushort VK_PROCESSKEY = (ushort)0xE5;
        public const ushort VK_PACKET = (ushort)0xE7;
        public const ushort VK_ATTN = (ushort)0xF6;
        public const ushort VK_CRSEL = (ushort)0xF7;
        public const ushort VK_EXSEL = (ushort)0xF8;
        public const ushort VK_EREOF = (ushort)0xF9;
        public const ushort VK_PLAY = (ushort)0xFA;
        public const ushort VK_ZOOM = (ushort)0xFB;
        public const ushort VK_NONAME = (ushort)0xFC;
        public const ushort VK_PA1 = (ushort)0xFD;
        public const ushort VK_OEM_CLEAR = (ushort)0xFE;
        public const ushort S_LBUTTON = (ushort)0x0;
        public const ushort S_RBUTTON = 0;
        public const ushort S_CANCEL = 70;
        public const ushort S_MBUTTON = 0;
        public const ushort S_XBUTTON1 = 0;
        public const ushort S_XBUTTON2 = 0;
        public const ushort S_BACK = 14;
        public const ushort S_Tab = 15;
        public const ushort S_CLEAR = 76;
        public const ushort S_Return = 28;
        public const ushort S_SHIFT = 42;
        public const ushort S_CONTROL = 29;
        public const ushort S_MENU = 56;
        public const ushort S_PAUSE = 0;
        public const ushort S_CAPITAL = 58;
        public const ushort S_KANA = 0;
        public const ushort S_HANGEUL = 0;
        public const ushort S_HANGUL = 0;
        public const ushort S_JUNJA = 0;
        public const ushort S_FINAL = 0;
        public const ushort S_HANJA = 0;
        public const ushort S_KANJI = 0;
        public const ushort S_Escape = 1;
        public const ushort S_CONVERT = 0;
        public const ushort S_NONCONVERT = 0;
        public const ushort S_ACCEPT = 0;
        public const ushort S_MODECHANGE = 0;
        public const ushort S_Space = 57;
        public const ushort S_PRIOR = 73;
        public const ushort S_NEXT = 81;
        public const ushort S_END = 79;
        public const ushort S_HOME = 71;
        public const ushort S_LEFT = 75;
        public const ushort S_UP = 72;
        public const ushort S_RIGHT = 77;
        public const ushort S_DOWN = 80;
        public const ushort S_SELECT = 0;
        public const ushort S_PRINT = 0;
        public const ushort S_EXECUTE = 0;
        public const ushort S_SNAPSHOT = 84;
        public const ushort S_INSERT = 82;
        public const ushort S_DELETE = 83;
        public const ushort S_HELP = 99;
        public const ushort S_APOSTROPHE = 41;
        public const ushort S_0 = 11;
        public const ushort S_1 = 2;
        public const ushort S_2 = 3;
        public const ushort S_3 = 4;
        public const ushort S_4 = 5;
        public const ushort S_5 = 6;
        public const ushort S_6 = 7;
        public const ushort S_7 = 8;
        public const ushort S_8 = 9;
        public const ushort S_9 = 10;
        public const ushort S_A = 16;
        public const ushort S_B = 48;
        public const ushort S_C = 46;
        public const ushort S_D = 32;
        public const ushort S_E = 18;
        public const ushort S_F = 33;
        public const ushort S_G = 34;
        public const ushort S_H = 35;
        public const ushort S_I = 23;
        public const ushort S_J = 36;
        public const ushort S_K = 37;
        public const ushort S_L = 38;
        public const ushort S_M = 39;
        public const ushort S_N = 49;
        public const ushort S_O = 24;
        public const ushort S_P = 25;
        public const ushort S_Q = 30;
        public const ushort S_R = 19;
        public const ushort S_S = 31;
        public const ushort S_T = 20;
        public const ushort S_U = 22;
        public const ushort S_V = 47;
        public const ushort S_W = 44;
        public const ushort S_X = 45;
        public const ushort S_Y = 21;
        public const ushort S_Z = 17;
        public const ushort S_LWIN = 91;
        public const ushort S_RWIN = 92;
        public const ushort S_APPS = 93;
        public const ushort S_SLEEP = 95;
        public const ushort S_NUMPAD0 = 82;
        public const ushort S_NUMPAD1 = 79;
        public const ushort S_NUMPAD2 = 80;
        public const ushort S_NUMPAD3 = 81;
        public const ushort S_NUMPAD4 = 75;
        public const ushort S_NUMPAD5 = 76;
        public const ushort S_NUMPAD6 = 77;
        public const ushort S_NUMPAD7 = 71;
        public const ushort S_NUMPAD8 = 72;
        public const ushort S_NUMPAD9 = 73;
        public const ushort S_MULTIPLY = 55;
        public const ushort S_ADD = 78;
        public const ushort S_SEPARATOR = 0;
        public const ushort S_SUBTRACT = 74;
        public const ushort S_DECIMAL = 83;
        public const ushort S_DIVIDE = 53;
        public const ushort S_F1 = 59;
        public const ushort S_F2 = 60;
        public const ushort S_F3 = 61;
        public const ushort S_F4 = 62;
        public const ushort S_F5 = 63;
        public const ushort S_F6 = 64;
        public const ushort S_F7 = 65;
        public const ushort S_F8 = 66;
        public const ushort S_F9 = 67;
        public const ushort S_F10 = 68;
        public const ushort S_F11 = 87;
        public const ushort S_F12 = 88;
        public const ushort S_F13 = 100;
        public const ushort S_F14 = 101;
        public const ushort S_F15 = 102;
        public const ushort S_F16 = 103;
        public const ushort S_F17 = 104;
        public const ushort S_F18 = 105;
        public const ushort S_F19 = 106;
        public const ushort S_F20 = 107;
        public const ushort S_F21 = 108;
        public const ushort S_F22 = 109;
        public const ushort S_F23 = 110;
        public const ushort S_F24 = 118;
        public const ushort S_NUMLOCK = 69;
        public const ushort S_SCROLL = 70;
        public const ushort S_LeftShift = 42;
        public const ushort S_RightShift = 54;
        public const ushort S_LeftControl = 29;
        public const ushort S_RightControl = 29;
        public const ushort S_LMENU = 56;
        public const ushort S_RMENU = 56;
        public const ushort S_BROWSER_BACK = 106;
        public const ushort S_BROWSER_FORWARD = 105;
        public const ushort S_BROWSER_REFRESH = 103;
        public const ushort S_BROWSER_STOP = 104;
        public const ushort S_BROWSER_SEARCH = 101;
        public const ushort S_BROWSER_FAVORITES = 102;
        public const ushort S_BROWSER_HOME = 50;
        public const ushort S_VOLUME_MUTE = 32;
        public const ushort S_VOLUME_DOWN = 46;
        public const ushort S_VOLUME_UP = 48;
        public const ushort S_MEDIA_NEXT_TRACK = 25;
        public const ushort S_MEDIA_PREV_TRACK = 16;
        public const ushort S_MEDIA_STOP = 36;
        public const ushort S_MEDIA_PLAY_PAUSE = 34;
        public const ushort S_LAUNCH_MAIL = 108;
        public const ushort S_LAUNCH_MEDIA_SELECT = 109;
        public const ushort S_LAUNCH_APP1 = 107;
        public const ushort S_LAUNCH_APP2 = 33;
        public const ushort S_OEM_1 = 27;
        public const ushort S_OEM_PLUS = 13;
        public const ushort S_OEM_COMMA = 50;
        public const ushort S_OEM_MINUS = 0;
        public const ushort S_OEM_PERIOD = 51;
        public const ushort S_OEM_2 = 52;
        public const ushort S_OEM_3 = 40;
        public const ushort S_OEM_4 = 12;
        public const ushort S_OEM_5 = 43;
        public const ushort S_OEM_6 = 26;
        public const ushort S_OEM_7 = 41;
        public const ushort S_OEM_8 = 53;
        public const ushort S_OEM_102 = 86;
        public const ushort S_PROCESSKEY = 0;
        public const ushort S_PACKET = 0;
        public const ushort S_ATTN = 0;
        public const ushort S_CRSEL = 0;
        public const ushort S_EXSEL = 0;
        public const ushort S_EREOF = 93;
        public const ushort S_PLAY = 0;
        public const ushort S_ZOOM = 98;
        public const ushort S_NONAME = 0;
        public const ushort S_PA1 = 0;
        public const ushort S_OEM_CLEAR = 0;
        public static string drivertype;
        public static int[] wd = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static int[] wu = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
        public static void valchanged(int n, bool val)
        {
            if (val)
            {
                if (wd[n] <= 1)
                {
                    wd[n] = wd[n] + 1;
                }
                wu[n] = 0;
            }
            else
            {
                if (wu[n] <= 1)
                {
                    wu[n] = wu[n] + 1;
                }
                wd[n] = 0;
            }
        }
        public static void UnLoadKM()
        {
            SetKM("kmevent", 0, 0, 0, 0, 0, 0, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
            SetKM("sendinput", 0, 0, 0, 0, 0, 0, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false);
        }
        public static void SetKM(string KeyboardMouseDriverType, double MouseMoveX, double MouseMoveY, double MouseAbsX, double MouseAbsY, double MouseDesktopX, double MouseDesktopY, bool SendLeftClick, bool SendRightClick, bool SendMiddleClick, bool SendWheelUp, bool SendWheelDown, bool SendLeft, bool SendRight, bool SendUp, bool SendDown, bool SendLButton, bool SendRButton, bool SendCancel, bool SendMBUTTON, bool SendXBUTTON1, bool SendXBUTTON2, bool SendBack, bool SendTab, bool SendClear, bool SendReturn, bool SendSHIFT, bool SendCONTROL, bool SendMENU, bool SendPAUSE, bool SendCAPITAL, bool SendKANA, bool SendHANGEUL, bool SendHANGUL, bool SendJUNJA, bool SendFINAL, bool SendHANJA, bool SendKANJI, bool SendEscape, bool SendCONVERT, bool SendNONCONVERT, bool SendACCEPT, bool SendMODECHANGE, bool SendSpace, bool SendPRIOR, bool SendNEXT, bool SendEND, bool SendHOME, bool SendLEFT, bool SendUP, bool SendRIGHT, bool SendDOWN, bool SendSELECT, bool SendPRINT, bool SendEXECUTE, bool SendSNAPSHOT, bool SendINSERT, bool SendDELETE, bool SendHELP, bool SendAPOSTROPHE, bool Send0, bool Send1, bool Send2, bool Send3, bool Send4, bool Send5, bool Send6, bool Send7, bool Send8, bool Send9, bool SendA, bool SendB, bool SendC, bool SendD, bool SendE, bool SendF, bool SendG, bool SendH, bool SendI, bool SendJ, bool SendK, bool SendL, bool SendM, bool SendN, bool SendO, bool SendP, bool SendQ, bool SendR, bool SendS, bool SendT, bool SendU, bool SendV, bool SendW, bool SendX, bool SendY, bool SendZ, bool SendLWIN, bool SendRWIN, bool SendAPPS, bool SendSLEEP, bool SendNUMPAD0, bool SendNUMPAD1, bool SendNUMPAD2, bool SendNUMPAD3, bool SendNUMPAD4, bool SendNUMPAD5, bool SendNUMPAD6, bool SendNUMPAD7, bool SendNUMPAD8, bool SendNUMPAD9, bool SendMULTIPLY, bool SendADD, bool SendSEPARATOR, bool SendSUBTRACT, bool SendDECIMAL, bool SendDIVIDE, bool SendF1, bool SendF2, bool SendF3, bool SendF4, bool SendF5, bool SendF6, bool SendF7, bool SendF8, bool SendF9, bool SendF10, bool SendF11, bool SendF12, bool SendF13, bool SendF14, bool SendF15, bool SendF16, bool SendF17, bool SendF18, bool SendF19, bool SendF20, bool SendF21, bool SendF22, bool SendF23, bool SendF24, bool SendNUMLOCK, bool SendSCROLL, bool SendLeftShift, bool SendRightShift, bool SendLeftControl, bool SendRightControl, bool SendLMENU, bool SendRMENU)
        {
            drivertype = KeyboardMouseDriverType;
            if (MouseMoveX != 0f | MouseMoveY != 0f)
                mousebrink((int)(MouseMoveX), (int)(MouseMoveY));
            if (MouseAbsX != 0f | MouseAbsY != 0f)
                mousemw3((int)(MouseAbsX), (int)(MouseAbsY));
            if (MouseDesktopX != 0f | MouseDesktopY != 0f)
            {
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)(MouseDesktopX), (int)(MouseDesktopY));
                SetPhysicalCursorPos((int)(MouseDesktopX), (int)(MouseDesktopY));
                SetCaretPos((int)(MouseDesktopX), (int)(MouseDesktopY));
                SetCursorPos((int)(MouseDesktopX), (int)(MouseDesktopY));
            }
            valchanged(1, SendLeftClick);
            if (wd[1] == 1)
                mouseclickleft();
            if (wu[1] == 1)
                mouseclickleftF();
            valchanged(2, SendRightClick);
            if (wd[2] == 1)
                mouseclickright();
            if (wu[2] == 1)
                mouseclickrightF();
            valchanged(3, SendMiddleClick);
            if (wd[3] == 1)
                mouseclickmiddle();
            if (wu[3] == 1)
                mouseclickmiddleF();
            valchanged(4, SendWheelUp);
            if (wd[4] == 1)
                mousewheelup();
            valchanged(5, SendWheelDown);
            if (wd[5] == 1)
                mousewheeldown();
            valchanged(6, SendLeft);
            if (wd[6] == 1)
                keyboardArrows(VK_LEFT, S_LEFT);
            if (wu[6] == 1)
                keyboardArrowsF(VK_LEFT, S_LEFT);
            valchanged(7, SendRight);
            if (wd[7] == 1)
                keyboardArrows(VK_RIGHT, S_RIGHT);
            if (wu[7] == 1)
                keyboardArrowsF(VK_RIGHT, S_RIGHT);
            valchanged(8, SendUp);
            if (wd[8] == 1)
                keyboardArrows(VK_UP, S_UP);
            if (wu[8] == 1)
                keyboardArrowsF(VK_UP, S_UP);
            valchanged(9, SendDown);
            if (wd[9] == 1)
                keyboardArrows(VK_DOWN, S_DOWN);
            if (wu[9] == 1)
                keyboardArrowsF(VK_DOWN, S_DOWN);
            valchanged(10, SendLButton);
            if (wd[10] == 1)
                keyboard(VK_LBUTTON, S_LBUTTON);
            if (wu[10] == 1)
                keyboardF(VK_LBUTTON, S_LBUTTON);
            valchanged(11, SendRButton);
            if (wd[11] == 1)
                keyboard(VK_RBUTTON, S_RBUTTON);
            if (wu[11] == 1)
                keyboardF(VK_RBUTTON, S_RBUTTON);
            valchanged(12, SendCancel);
            if (wd[12] == 1)
                keyboard(VK_CANCEL, S_CANCEL);
            if (wu[12] == 1)
                keyboardF(VK_CANCEL, S_CANCEL);
            valchanged(13, SendMBUTTON);
            if (wd[13] == 1)
                keyboard(VK_MBUTTON, S_MBUTTON);
            if (wu[13] == 1)
                keyboardF(VK_MBUTTON, S_MBUTTON);
            valchanged(14, SendXBUTTON1);
            if (wd[14] == 1)
                keyboard(VK_XBUTTON1, S_XBUTTON1);
            if (wu[14] == 1)
                keyboardF(VK_XBUTTON1, S_XBUTTON1);
            valchanged(15, SendXBUTTON2);
            if (wd[15] == 1)
                keyboard(VK_XBUTTON2, S_XBUTTON2);
            if (wu[15] == 1)
                keyboardF(VK_XBUTTON2, S_XBUTTON2);
            valchanged(16, SendBack);
            if (wd[16] == 1)
                keyboard(VK_BACK, S_BACK);
            if (wu[16] == 1)
                keyboardF(VK_BACK, S_BACK);
            valchanged(17, SendTab);
            if (wd[17] == 1)
                keyboard(VK_Tab, S_Tab);
            if (wu[17] == 1)
                keyboardF(VK_Tab, S_Tab);
            valchanged(18, SendClear);
            if (wd[18] == 1)
                keyboard(VK_CLEAR, S_CLEAR);
            if (wu[18] == 1)
                keyboardF(VK_CLEAR, S_CLEAR);
            valchanged(19, SendReturn);
            if (wd[19] == 1)
                keyboard(VK_Return, S_Return);
            if (wu[19] == 1)
                keyboardF(VK_Return, S_Return);
            valchanged(20, SendSHIFT);
            if (wd[20] == 1)
                keyboard(VK_SHIFT, S_SHIFT);
            if (wu[20] == 1)
                keyboardF(VK_SHIFT, S_SHIFT);
            valchanged(21, SendCONTROL);
            if (wd[21] == 1)
                keyboard(VK_CONTROL, S_CONTROL);
            if (wu[21] == 1)
                keyboardF(VK_CONTROL, S_CONTROL);
            valchanged(22, SendMENU);
            if (wd[22] == 1)
                keyboard(VK_MENU, S_MENU);
            if (wu[22] == 1)
                keyboardF(VK_MENU, S_MENU);
            valchanged(23, SendPAUSE);
            if (wd[23] == 1)
                keyboard(VK_PAUSE, S_PAUSE);
            if (wu[23] == 1)
                keyboardF(VK_PAUSE, S_PAUSE);
            valchanged(24, SendCAPITAL);
            if (wd[24] == 1)
                keyboard(VK_CAPITAL, S_CAPITAL);
            if (wu[24] == 1)
                keyboardF(VK_CAPITAL, S_CAPITAL);
            valchanged(25, SendKANA);
            if (wd[25] == 1)
                keyboard(VK_KANA, S_KANA);
            if (wu[25] == 1)
                keyboardF(VK_KANA, S_KANA);
            valchanged(26, SendHANGEUL);
            if (wd[26] == 1)
                keyboard(VK_HANGEUL, S_HANGEUL);
            if (wu[26] == 1)
                keyboardF(VK_HANGEUL, S_HANGEUL);
            valchanged(27, SendHANGUL);
            if (wd[27] == 1)
                keyboard(VK_HANGUL, S_HANGUL);
            if (wu[27] == 1)
                keyboardF(VK_HANGUL, S_HANGUL);
            valchanged(28, SendJUNJA);
            if (wd[28] == 1)
                keyboard(VK_JUNJA, S_JUNJA);
            if (wu[28] == 1)
                keyboardF(VK_JUNJA, S_JUNJA);
            valchanged(29, SendFINAL);
            if (wd[29] == 1)
                keyboard(VK_FINAL, S_FINAL);
            if (wu[29] == 1)
                keyboardF(VK_FINAL, S_FINAL);
            valchanged(30, SendHANJA);
            if (wd[30] == 1)
                keyboard(VK_HANJA, S_HANJA);
            if (wu[30] == 1)
                keyboardF(VK_HANJA, S_HANJA);
            valchanged(31, SendKANJI);
            if (wd[31] == 1)
                keyboard(VK_KANJI, S_KANJI);
            if (wu[31] == 1)
                keyboardF(VK_KANJI, S_KANJI);
            valchanged(32, SendEscape);
            if (wd[32] == 1)
                keyboard(VK_Escape, S_Escape);
            if (wu[32] == 1)
                keyboardF(VK_Escape, S_Escape);
            valchanged(33, SendCONVERT);
            if (wd[33] == 1)
                keyboard(VK_CONVERT, S_CONVERT);
            if (wu[33] == 1)
                keyboardF(VK_CONVERT, S_CONVERT);
            valchanged(34, SendNONCONVERT);
            if (wd[34] == 1)
                keyboard(VK_NONCONVERT, S_NONCONVERT);
            if (wu[34] == 1)
                keyboardF(VK_NONCONVERT, S_NONCONVERT);
            valchanged(35, SendACCEPT);
            if (wd[35] == 1)
                keyboard(VK_ACCEPT, S_ACCEPT);
            if (wu[35] == 1)
                keyboardF(VK_ACCEPT, S_ACCEPT);
            valchanged(36, SendMODECHANGE);
            if (wd[36] == 1)
                keyboard(VK_MODECHANGE, S_MODECHANGE);
            if (wu[36] == 1)
                keyboardF(VK_MODECHANGE, S_MODECHANGE);
            valchanged(37, SendSpace);
            if (wd[37] == 1)
                keyboard(VK_Space, S_Space);
            if (wu[37] == 1)
                keyboardF(VK_Space, S_Space);
            valchanged(38, SendPRIOR);
            if (wd[38] == 1)
                keyboard(VK_PRIOR, S_PRIOR);
            if (wu[38] == 1)
                keyboardF(VK_PRIOR, S_PRIOR);
            valchanged(39, SendNEXT);
            if (wd[39] == 1)
                keyboard(VK_NEXT, S_NEXT);
            if (wu[39] == 1)
                keyboardF(VK_NEXT, S_NEXT);
            valchanged(40, SendEND);
            if (wd[40] == 1)
                keyboard(VK_END, S_END);
            if (wu[40] == 1)
                keyboardF(VK_END, S_END);
            valchanged(41, SendHOME);
            if (wd[41] == 1)
                keyboard(VK_HOME, S_HOME);
            if (wu[41] == 1)
                keyboardF(VK_HOME, S_HOME);
            valchanged(42, SendLEFT);
            if (wd[42] == 1)
                keyboard(VK_LEFT, S_LEFT);
            if (wu[42] == 1)
                keyboardF(VK_LEFT, S_LEFT);
            valchanged(43, SendUP);
            if (wd[43] == 1)
                keyboard(VK_UP, S_UP);
            if (wu[43] == 1)
                keyboardF(VK_UP, S_UP);
            valchanged(44, SendRIGHT);
            if (wd[44] == 1)
                keyboard(VK_RIGHT, S_RIGHT);
            if (wu[44] == 1)
                keyboardF(VK_RIGHT, S_RIGHT);
            valchanged(45, SendDOWN);
            if (wd[45] == 1)
                keyboard(VK_DOWN, S_DOWN);
            if (wu[45] == 1)
                keyboardF(VK_DOWN, S_DOWN);
            valchanged(46, SendSELECT);
            if (wd[46] == 1)
                keyboard(VK_SELECT, S_SELECT);
            if (wu[46] == 1)
                keyboardF(VK_SELECT, S_SELECT);
            valchanged(47, SendPRINT);
            if (wd[47] == 1)
                keyboard(VK_PRINT, S_PRINT);
            if (wu[47] == 1)
                keyboardF(VK_PRINT, S_PRINT);
            valchanged(48, SendEXECUTE);
            if (wd[48] == 1)
                keyboard(VK_EXECUTE, S_EXECUTE);
            if (wu[48] == 1)
                keyboardF(VK_EXECUTE, S_EXECUTE);
            valchanged(49, SendSNAPSHOT);
            if (wd[49] == 1)
                keyboard(VK_SNAPSHOT, S_SNAPSHOT);
            if (wu[49] == 1)
                keyboardF(VK_SNAPSHOT, S_SNAPSHOT);
            valchanged(50, SendINSERT);
            if (wd[50] == 1)
                keyboard(VK_INSERT, S_INSERT);
            if (wu[50] == 1)
                keyboardF(VK_INSERT, S_INSERT);
            valchanged(51, SendDELETE);
            if (wd[51] == 1)
                keyboard(VK_DELETE, S_DELETE);
            if (wu[51] == 1)
                keyboardF(VK_DELETE, S_DELETE);
            valchanged(52, SendHELP);
            if (wd[52] == 1)
                keyboard(VK_HELP, S_HELP);
            if (wu[52] == 1)
                keyboardF(VK_HELP, S_HELP);
            valchanged(53, SendAPOSTROPHE);
            if (wd[53] == 1)
                keyboard(VK_APOSTROPHE, S_APOSTROPHE);
            if (wu[53] == 1)
                keyboardF(VK_APOSTROPHE, S_APOSTROPHE);
            valchanged(54, Send0);
            if (wd[54] == 1)
                keyboard(VK_0, S_0);
            if (wu[54] == 1)
                keyboardF(VK_0, S_0);
            valchanged(55, Send1);
            if (wd[55] == 1)
                keyboard(VK_1, S_1);
            if (wu[55] == 1)
                keyboardF(VK_1, S_1);
            valchanged(56, Send2);
            if (wd[56] == 1)
                keyboard(VK_2, S_2);
            if (wu[56] == 1)
                keyboardF(VK_2, S_2);
            valchanged(57, Send3);
            if (wd[57] == 1)
                keyboard(VK_3, S_3);
            if (wu[57] == 1)
                keyboardF(VK_3, S_3);
            valchanged(58, Send4);
            if (wd[58] == 1)
                keyboard(VK_4, S_4);
            if (wu[58] == 1)
                keyboardF(VK_4, S_4);
            valchanged(59, Send5);
            if (wd[59] == 1)
                keyboard(VK_5, S_5);
            if (wu[59] == 1)
                keyboardF(VK_5, S_5);
            valchanged(60, Send6);
            if (wd[60] == 1)
                keyboard(VK_6, S_6);
            if (wu[60] == 1)
                keyboardF(VK_6, S_6);
            valchanged(61, Send7);
            if (wd[61] == 1)
                keyboard(VK_7, S_7);
            if (wu[61] == 1)
                keyboardF(VK_7, S_7);
            valchanged(62, Send8);
            if (wd[62] == 1)
                keyboard(VK_8, S_8);
            if (wu[62] == 1)
                keyboardF(VK_8, S_8);
            valchanged(63, Send9);
            if (wd[63] == 1)
                keyboard(VK_9, S_9);
            if (wu[63] == 1)
                keyboardF(VK_9, S_9);
            valchanged(64, SendA);
            if (wd[64] == 1)
                keyboard(VK_A, S_A);
            if (wu[64] == 1)
                keyboardF(VK_A, S_A);
            valchanged(65, SendB);
            if (wd[65] == 1)
                keyboard(VK_B, S_B);
            if (wu[65] == 1)
                keyboardF(VK_B, S_B);
            valchanged(66, SendC);
            if (wd[66] == 1)
                keyboard(VK_C, S_C);
            if (wu[66] == 1)
                keyboardF(VK_C, S_C);
            valchanged(67, SendD);
            if (wd[67] == 1)
                keyboard(VK_D, S_D);
            if (wu[67] == 1)
                keyboardF(VK_D, S_D);
            valchanged(68, SendE);
            if (wd[68] == 1)
                keyboard(VK_E, S_E);
            if (wu[68] == 1)
                keyboardF(VK_E, S_E);
            valchanged(69, SendF);
            if (wd[69] == 1)
                keyboard(VK_F, S_F);
            if (wu[69] == 1)
                keyboardF(VK_F, S_F);
            valchanged(70, SendG);
            if (wd[70] == 1)
                keyboard(VK_G, S_G);
            if (wu[70] == 1)
                keyboardF(VK_G, S_G);
            valchanged(71, SendH);
            if (wd[71] == 1)
                keyboard(VK_H, S_H);
            if (wu[71] == 1)
                keyboardF(VK_H, S_H);
            valchanged(72, SendI);
            if (wd[72] == 1)
                keyboard(VK_I, S_I);
            if (wu[72] == 1)
                keyboardF(VK_I, S_I);
            valchanged(73, SendJ);
            if (wd[73] == 1)
                keyboard(VK_J, S_J);
            if (wu[73] == 1)
                keyboardF(VK_J, S_J);
            valchanged(74, SendK);
            if (wd[74] == 1)
                keyboard(VK_K, S_K);
            if (wu[74] == 1)
                keyboardF(VK_K, S_K);
            valchanged(75, SendL);
            if (wd[75] == 1)
                keyboard(VK_L, S_L);
            if (wu[75] == 1)
                keyboardF(VK_L, S_L);
            valchanged(76, SendM);
            if (wd[76] == 1)
                keyboard(VK_M, S_M);
            if (wu[76] == 1)
                keyboardF(VK_M, S_M);
            valchanged(77, SendN);
            if (wd[77] == 1)
                keyboard(VK_N, S_N);
            if (wu[77] == 1)
                keyboardF(VK_N, S_N);
            valchanged(78, SendO);
            if (wd[78] == 1)
                keyboard(VK_O, S_O);
            if (wu[78] == 1)
                keyboardF(VK_O, S_O);
            valchanged(79, SendP);
            if (wd[79] == 1)
                keyboard(VK_P, S_P);
            if (wu[79] == 1)
                keyboardF(VK_P, S_P);
            valchanged(80, SendQ);
            if (wd[80] == 1)
                keyboard(VK_Q, S_Q);
            if (wu[80] == 1)
                keyboardF(VK_Q, S_Q);
            valchanged(81, SendR);
            if (wd[81] == 1)
                keyboard(VK_R, S_R);
            if (wu[81] == 1)
                keyboardF(VK_R, S_R);
            valchanged(82, SendS);
            if (wd[82] == 1)
                keyboard(VK_S, S_S);
            if (wu[82] == 1)
                keyboardF(VK_S, S_S);
            valchanged(83, SendT);
            if (wd[83] == 1)
                keyboard(VK_T, S_T);
            if (wu[83] == 1)
                keyboardF(VK_T, S_T);
            valchanged(84, SendU);
            if (wd[84] == 1)
                keyboard(VK_U, S_U);
            if (wu[84] == 1)
                keyboardF(VK_U, S_U);
            valchanged(85, SendV);
            if (wd[85] == 1)
                keyboard(VK_V, S_V);
            if (wu[85] == 1)
                keyboardF(VK_V, S_V);
            valchanged(86, SendW);
            if (wd[86] == 1)
                keyboard(VK_W, S_W);
            if (wu[86] == 1)
                keyboardF(VK_W, S_W);
            valchanged(87, SendX);
            if (wd[87] == 1)
                keyboard(VK_X, S_X);
            if (wu[87] == 1)
                keyboardF(VK_X, S_X);
            valchanged(88, SendY);
            if (wd[88] == 1)
                keyboard(VK_Y, S_Y);
            if (wu[88] == 1)
                keyboardF(VK_Y, S_Y);
            valchanged(89, SendZ);
            if (wd[89] == 1)
                keyboard(VK_Z, S_Z);
            if (wu[89] == 1)
                keyboardF(VK_Z, S_Z);
            valchanged(90, SendLWIN);
            if (wd[90] == 1)
                keyboard(VK_LWIN, S_LWIN);
            if (wu[90] == 1)
                keyboardF(VK_LWIN, S_LWIN);
            valchanged(91, SendRWIN);
            if (wd[91] == 1)
                keyboard(VK_RWIN, S_RWIN);
            if (wu[91] == 1)
                keyboardF(VK_RWIN, S_RWIN);
            valchanged(92, SendAPPS);
            if (wd[92] == 1)
                keyboard(VK_APPS, S_APPS);
            if (wu[92] == 1)
                keyboardF(VK_APPS, S_APPS);
            valchanged(93, SendSLEEP);
            if (wd[93] == 1)
                keyboard(VK_SLEEP, S_SLEEP);
            if (wu[93] == 1)
                keyboardF(VK_SLEEP, S_SLEEP);
            valchanged(94, SendNUMPAD0);
            if (wd[94] == 1)
                keyboard(VK_NUMPAD0, S_NUMPAD0);
            if (wu[94] == 1)
                keyboardF(VK_NUMPAD0, S_NUMPAD0);
            valchanged(95, SendNUMPAD1);
            if (wd[95] == 1)
                keyboard(VK_NUMPAD1, S_NUMPAD1);
            if (wu[95] == 1)
                keyboardF(VK_NUMPAD1, S_NUMPAD1);
            valchanged(96, SendNUMPAD2);
            if (wd[96] == 1)
                keyboard(VK_NUMPAD2, S_NUMPAD2);
            if (wu[96] == 1)
                keyboardF(VK_NUMPAD2, S_NUMPAD2);
            valchanged(97, SendNUMPAD3);
            if (wd[97] == 1)
                keyboard(VK_NUMPAD3, S_NUMPAD3);
            if (wu[97] == 1)
                keyboardF(VK_NUMPAD3, S_NUMPAD3);
            valchanged(98, SendNUMPAD4);
            if (wd[98] == 1)
                keyboard(VK_NUMPAD4, S_NUMPAD4);
            if (wu[98] == 1)
                keyboardF(VK_NUMPAD4, S_NUMPAD4);
            valchanged(99, SendNUMPAD5);
            if (wd[99] == 1)
                keyboard(VK_NUMPAD5, S_NUMPAD5);
            if (wu[99] == 1)
                keyboardF(VK_NUMPAD5, S_NUMPAD5);
            valchanged(100, SendNUMPAD6);
            if (wd[100] == 1)
                keyboard(VK_NUMPAD6, S_NUMPAD6);
            if (wu[100] == 1)
                keyboardF(VK_NUMPAD6, S_NUMPAD6);
            valchanged(101, SendNUMPAD7);
            if (wd[101] == 1)
                keyboard(VK_NUMPAD7, S_NUMPAD7);
            if (wu[101] == 1)
                keyboardF(VK_NUMPAD7, S_NUMPAD7);
            valchanged(102, SendNUMPAD8);
            if (wd[102] == 1)
                keyboard(VK_NUMPAD8, S_NUMPAD8);
            if (wu[102] == 1)
                keyboardF(VK_NUMPAD8, S_NUMPAD8);
            valchanged(103, SendNUMPAD9);
            if (wd[103] == 1)
                keyboard(VK_NUMPAD9, S_NUMPAD9);
            if (wu[103] == 1)
                keyboardF(VK_NUMPAD9, S_NUMPAD9);
            valchanged(104, SendMULTIPLY);
            if (wd[104] == 1)
                keyboard(VK_MULTIPLY, S_MULTIPLY);
            if (wu[104] == 1)
                keyboardF(VK_MULTIPLY, S_MULTIPLY);
            valchanged(105, SendADD);
            if (wd[105] == 1)
                keyboard(VK_ADD, S_ADD);
            if (wu[105] == 1)
                keyboardF(VK_ADD, S_ADD);
            valchanged(106, SendSEPARATOR);
            if (wd[106] == 1)
                keyboard(VK_SEPARATOR, S_SEPARATOR);
            if (wu[106] == 1)
                keyboardF(VK_SEPARATOR, S_SEPARATOR);
            valchanged(107, SendSUBTRACT);
            if (wd[107] == 1)
                keyboard(VK_SUBTRACT, S_SUBTRACT);
            if (wu[107] == 1)
                keyboardF(VK_SUBTRACT, S_SUBTRACT);
            valchanged(108, SendDECIMAL);
            if (wd[108] == 1)
                keyboard(VK_DECIMAL, S_DECIMAL);
            if (wu[108] == 1)
                keyboardF(VK_DECIMAL, S_DECIMAL);
            valchanged(109, SendDIVIDE);
            if (wd[109] == 1)
                keyboard(VK_DIVIDE, S_DIVIDE);
            if (wu[109] == 1)
                keyboardF(VK_DIVIDE, S_DIVIDE);
            valchanged(110, SendF1);
            if (wd[110] == 1)
                keyboard(VK_F1, S_F1);
            if (wu[110] == 1)
                keyboardF(VK_F1, S_F1);
            valchanged(111, SendF2);
            if (wd[111] == 1)
                keyboard(VK_F2, S_F2);
            if (wu[111] == 1)
                keyboardF(VK_F2, S_F2);
            valchanged(112, SendF3);
            if (wd[112] == 1)
                keyboard(VK_F3, S_F3);
            if (wu[112] == 1)
                keyboardF(VK_F3, S_F3);
            valchanged(113, SendF4);
            if (wd[113] == 1)
                keyboard(VK_F4, S_F4);
            if (wu[113] == 1)
                keyboardF(VK_F4, S_F4);
            valchanged(114, SendF5);
            if (wd[114] == 1)
                keyboard(VK_F5, S_F5);
            if (wu[114] == 1)
                keyboardF(VK_F5, S_F5);
            valchanged(115, SendF6);
            if (wd[115] == 1)
                keyboard(VK_F6, S_F6);
            if (wu[115] == 1)
                keyboardF(VK_F6, S_F6);
            valchanged(116, SendF7);
            if (wd[116] == 1)
                keyboard(VK_F7, S_F7);
            if (wu[116] == 1)
                keyboardF(VK_F7, S_F7);
            valchanged(117, SendF8);
            if (wd[117] == 1)
                keyboard(VK_F8, S_F8);
            if (wu[117] == 1)
                keyboardF(VK_F8, S_F8);
            valchanged(118, SendF9);
            if (wd[118] == 1)
                keyboard(VK_F9, S_F9);
            if (wu[118] == 1)
                keyboardF(VK_F9, S_F9);
            valchanged(119, SendF10);
            if (wd[119] == 1)
                keyboard(VK_F10, S_F10);
            if (wu[119] == 1)
                keyboardF(VK_F10, S_F10);
            valchanged(120, SendF11);
            if (wd[120] == 1)
                keyboard(VK_F11, S_F11);
            if (wu[120] == 1)
                keyboardF(VK_F11, S_F11);
            valchanged(121, SendF12);
            if (wd[121] == 1)
                keyboard(VK_F12, S_F12);
            if (wu[121] == 1)
                keyboardF(VK_F12, S_F12);
            valchanged(122, SendF13);
            if (wd[122] == 1)
                keyboard(VK_F13, S_F13);
            if (wu[122] == 1)
                keyboardF(VK_F13, S_F13);
            valchanged(123, SendF14);
            if (wd[123] == 1)
                keyboard(VK_F14, S_F14);
            if (wu[123] == 1)
                keyboardF(VK_F14, S_F14);
            valchanged(124, SendF15);
            if (wd[124] == 1)
                keyboard(VK_F15, S_F15);
            if (wu[124] == 1)
                keyboardF(VK_F15, S_F15);
            valchanged(125, SendF16);
            if (wd[125] == 1)
                keyboard(VK_F16, S_F16);
            if (wu[125] == 1)
                keyboardF(VK_F16, S_F16);
            valchanged(126, SendF17);
            if (wd[126] == 1)
                keyboard(VK_F17, S_F17);
            if (wu[126] == 1)
                keyboardF(VK_F17, S_F17);
            valchanged(127, SendF18);
            if (wd[127] == 1)
                keyboard(VK_F18, S_F18);
            if (wu[127] == 1)
                keyboardF(VK_F18, S_F18);
            valchanged(128, SendF19);
            if (wd[128] == 1)
                keyboard(VK_F19, S_F19);
            if (wu[128] == 1)
                keyboardF(VK_F19, S_F19);
            valchanged(129, SendF20);
            if (wd[129] == 1)
                keyboard(VK_F20, S_F20);
            if (wu[129] == 1)
                keyboardF(VK_F20, S_F20);
            valchanged(130, SendF21);
            if (wd[130] == 1)
                keyboard(VK_F21, S_F21);
            if (wu[130] == 1)
                keyboardF(VK_F21, S_F21);
            valchanged(131, SendF22);
            if (wd[131] == 1)
                keyboard(VK_F22, S_F22);
            if (wu[131] == 1)
                keyboardF(VK_F22, S_F22);
            valchanged(132, SendF23);
            if (wd[132] == 1)
                keyboard(VK_F23, S_F23);
            if (wu[132] == 1)
                keyboardF(VK_F23, S_F23);
            valchanged(133, SendF24);
            if (wd[133] == 1)
                keyboard(VK_F24, S_F24);
            if (wu[133] == 1)
                keyboardF(VK_F24, S_F24);
            valchanged(134, SendNUMLOCK);
            if (wd[134] == 1)
                keyboard(VK_NUMLOCK, S_NUMLOCK);
            if (wu[134] == 1)
                keyboardF(VK_NUMLOCK, S_NUMLOCK);
            valchanged(135, SendSCROLL);
            if (wd[135] == 1)
                keyboard(VK_SCROLL, S_SCROLL);
            if (wu[135] == 1)
                keyboardF(VK_SCROLL, S_SCROLL);
            valchanged(136, SendLeftShift);
            if (wd[136] == 1)
                keyboard(VK_LeftShift, S_LeftShift);
            if (wu[136] == 1)
                keyboardF(VK_LeftShift, S_LeftShift);
            valchanged(137, SendRightShift);
            if (wd[137] == 1)
                keyboard(VK_RightShift, S_RightShift);
            if (wu[137] == 1)
                keyboardF(VK_RightShift, S_RightShift);
            valchanged(138, SendLeftControl);
            if (wd[138] == 1)
                keyboard(VK_LeftControl, S_LeftControl);
            if (wu[138] == 1)
                keyboardF(VK_LeftControl, S_LeftControl);
            valchanged(139, SendRightControl);
            if (wd[139] == 1)
                keyboard(VK_RightControl, S_RightControl);
            if (wu[139] == 1)
                keyboardF(VK_RightControl, S_RightControl);
            valchanged(140, SendLMENU);
            if (wd[140] == 1)
                keyboard(VK_LMENU, S_LMENU);
            if (wu[140] == 1)
                keyboardF(VK_LMENU, S_LMENU);
            valchanged(141, SendRMENU);
            if (wd[141] == 1)
                keyboard(VK_RMENU, S_RMENU);
            if (wu[141] == 1)
                keyboardF(VK_RMENU, S_RMENU);
        }
        public static void mousebrink(int x, int y)
        {
            if (drivertype == "sendinput")
                MoveMouseBy(x, y);
            else
                MouseBrink(x, y);
        }
        public static void mousemw3(int x, int y)
        {
            if (drivertype == "sendinput")
                MoveMouseTo(x, y);
            else
                MouseMW3(x, y);
        }
        public static void mouseclickleft()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonLeft();
            else
                LeftClick();
        }
        public static void mouseclickleftF()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonLeftF();
            else
                LeftClickF();
        }
        public static void mouseclickright()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonRight();
            else
                RightClick();
        }
        public static void mouseclickrightF()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonRightF();
            else
                RightClickF();
        }
        public static void mouseclickmiddle()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonMiddle();
            else
                MiddleClick();
        }
        public static void mouseclickmiddleF()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonMiddleF();
            else
                MiddleClickF();
        }
        public static void mousewheelup()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonWheelUp();
            else
                WheelUpF();
        }
        public static void mousewheeldown()
        {
            if (drivertype == "sendinput")
                SendMouseEventButtonWheelDown();
            else
                WheelDownF();
        }
        public static void keyboard(UInt16 bVk, UInt16 bScan)
        {
            if (drivertype == "sendinput")
                SendKey(bVk, bScan);
            else
                SimulateKeyDown(bVk, bScan);
        }
        public static void keyboardF(UInt16 bVk, UInt16 bScan)
        {
            if (drivertype == "sendinput")
                SendKeyF(bVk, bScan);
            else
                SimulateKeyUp(bVk, bScan);
        }
        public static void keyboardArrows(UInt16 bVk, UInt16 bScan)
        {
            if (drivertype == "sendinput")
                SendKeyArrows(bVk, bScan);
            else
                SimulateKeyDownArrows(bVk, bScan);
        }
        public static void keyboardArrowsF(UInt16 bVk, UInt16 bScan)
        {
            if (drivertype == "sendinput")
                SendKeyArrowsF(bVk, bScan);
            else
                SimulateKeyUpArrows(bVk, bScan);
        }
    }
}