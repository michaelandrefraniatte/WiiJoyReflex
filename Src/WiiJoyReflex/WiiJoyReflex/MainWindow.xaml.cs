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
using System.Security.Principal;
using System.Management;
using System.Security.Cryptography;
using wjr;
using System.Windows.Threading;
using System.Reflection;
using Microsoft.Win32;
using System.Windows.Input;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace WiiJoyReflex
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    [System.Security.SuppressUnmanagedCodeSecurity]
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            if (!hasAdminRights())
            {
                RunElevated();
                this.Close();
            }
            InitializeComponent();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            ExtractResourceToFile("MotionInputPairing");
            ExtractResourceToFile("keyboard");
            ExtractResourceToFile("mouse");
            ExtractResourceToFile("lhidread");
            System.Windows.Application.Current.MainWindow.Loaded += MainWindow_Loaded;
        }
        public Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(",")) : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources"))
                return null;
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllName);
            return Assembly.Load(bytes);
        }
        public void ExtractResourceToFile(string dllName)
        {
            dllName = dllName.Replace(".", "_");
            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllName);
            dllName = dllName.Replace("_", ".");
            using (FileStream fs = new FileStream(@"C:\Windows\System32\" + dllName + ".dll", FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
            using (FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + dllName + ".dll", FileMode.Create))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }
        public static bool hasAdminRights()
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        public static void RunElevated()
        {
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.Verb = "runas";
                processInfo.FileName = Application.ExecutablePath;
                Process.Start(processInfo);
            }
            catch { }
        }
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint ms);
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint ms);
        [DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        private static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);
        private static uint CurrentResolution = 0;
        public static int irmode = 1;
        private static bool running;
        public static string filename = "";
        private void bopen_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "All Files(*.*)|*.*";
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = op.FileName;
                OpenConfig(filename);
            }
        }
        private void bsave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveConfig(filename);
        }
        private void bsaveas_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "All Files(*.*)|*.*";
            if (filename != "")
                sf.FileName = System.IO.Path.GetFileName(filename);
            if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = sf.FileName;
                SaveConfig(filename);
            }
        }
        private void SaveConfig(string path)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(path))
                {
                    file.WriteLine(cbescape1.SelectionBoxItem.ToString());
                    file.WriteLine(cbescape2.SelectionBoxItem.ToString());
                    file.WriteLine(cbtab1.SelectionBoxItem.ToString());
                    file.WriteLine(cbtab2.SelectionBoxItem.ToString());
                    file.WriteLine(cbshift1.SelectionBoxItem.ToString());
                    file.WriteLine(cbshift2.SelectionBoxItem.ToString());
                    file.WriteLine(cbcontrol1.SelectionBoxItem.ToString());
                    file.WriteLine(cbcontrol2.SelectionBoxItem.ToString());
                    file.WriteLine(cba1.SelectionBoxItem.ToString());
                    file.WriteLine(cba2.SelectionBoxItem.ToString());
                    file.WriteLine(cbe1.SelectionBoxItem.ToString());
                    file.WriteLine(cbe2.SelectionBoxItem.ToString());
                    file.WriteLine(cbx1.SelectionBoxItem.ToString());
                    file.WriteLine(cbx2.SelectionBoxItem.ToString());
                    file.WriteLine(cbc1.SelectionBoxItem.ToString());
                    file.WriteLine(cbc2.SelectionBoxItem.ToString());
                    file.WriteLine(cbv1.SelectionBoxItem.ToString());
                    file.WriteLine(cbv2.SelectionBoxItem.ToString());
                    file.WriteLine(cbspace1.SelectionBoxItem.ToString());
                    file.WriteLine(cbspace2.SelectionBoxItem.ToString());
                    file.WriteLine(cbf1.SelectionBoxItem.ToString());
                    file.WriteLine(cbf2.SelectionBoxItem.ToString());
                    file.WriteLine(cbr1.SelectionBoxItem.ToString());
                    file.WriteLine(cbr2.SelectionBoxItem.ToString());
                    file.WriteLine(cbuproll1.SelectionBoxItem.ToString());
                    file.WriteLine(cbuproll2.SelectionBoxItem.ToString());
                    file.WriteLine(cbdownroll1.SelectionBoxItem.ToString());
                    file.WriteLine(cbdownroll2.SelectionBoxItem.ToString());
                    file.WriteLine(cbleftclick1.SelectionBoxItem.ToString());
                    file.WriteLine(cbleftclick2.SelectionBoxItem.ToString());
                    file.WriteLine(cbrightclick1.SelectionBoxItem.ToString());
                    file.WriteLine(cbrightclick2.SelectionBoxItem.ToString());
                    file.WriteLine(cbads.IsChecked);
                    file.WriteLine(tbstickxsens.Value);
                    file.WriteLine(tbstickysens.Value);
                    file.WriteLine(cbenglish.IsChecked);
                    file.WriteLine(tbirxsens.Value);
                    file.WriteLine(tbirysens.Value);
                    file.WriteLine(cbcursor.IsChecked);
                    file.WriteLine(cbbrink.IsChecked);
                    file.WriteLine(cbmw3.IsChecked);
                    file.WriteLine(cbtitanfall.IsChecked);
                    file.WriteLine(cbmetro.IsChecked);
                    file.WriteLine(cbbo3.IsChecked);
                    file.WriteLine(cbfortnite.IsChecked);
                    file.WriteLine(cbfake.IsChecked);
                    file.WriteLine(cbmc.IsChecked);
                    file.WriteLine(tbviewpower1x.Value);
                    file.WriteLine(tbviewpower2x.Value);
                    file.WriteLine(tbviewpower3x.Value);
                    file.WriteLine(tbviewpower1y.Value);
                    file.WriteLine(tbviewpower2y.Value);
                    file.WriteLine(tbviewpower3y.Value);
                    file.WriteLine(tbcentery.Value);
                    file.WriteLine(cbirmode.IsChecked);
                    file.WriteLine(cbsendinput.IsChecked);
                    file.WriteLine(cbsendescape.SelectionBoxItem.ToString());
                    file.WriteLine(cbsendtab.SelectionBoxItem.ToString());
                    file.WriteLine(cbsendshift.SelectionBoxItem.ToString());
                    file.WriteLine(cbsendcontrol.SelectionBoxItem.ToString());
                    file.WriteLine(cbsenda.SelectionBoxItem.ToString());
                    file.WriteLine(cbsende.SelectionBoxItem.ToString());
                    file.WriteLine(cbsendx.SelectionBoxItem.ToString());
                    file.WriteLine(cbsendc.SelectionBoxItem.ToString());
                    file.WriteLine(cbsendv.SelectionBoxItem.ToString());
                    file.WriteLine(cbsendspace.SelectionBoxItem.ToString());
                    file.WriteLine(cbsendf.SelectionBoxItem.ToString());
                    file.WriteLine(cbsendr.SelectionBoxItem.ToString());
                    file.WriteLine(cbsenduproll.SelectionBoxItem.ToString());
                    file.WriteLine(cbsenddownroll.SelectionBoxItem.ToString());
                    file.WriteLine(cbsendleftclick.SelectionBoxItem.ToString());
                    file.WriteLine(cbsendrightclick.SelectionBoxItem.ToString());
                    file.WriteLine(tbdzx.Value);
                    file.WriteLine(tbdzy.Value);
                    file.WriteLine(cbarrows1.IsChecked);
                    file.WriteLine(cbarrows2.IsChecked);
                    file.WriteLine(cbnumpad.IsChecked);
                }
                this.Title = "WiiJoyReflex: " + System.IO.Path.GetFileName(path);
            }
            catch { }
            Apply();
        }
        private void OpenConfig(string path)
        {
            try
            {
                using (StreamReader file = new StreamReader(path))
                {
                    cbescape1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbescape2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbtab1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbtab2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbshift1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbshift2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbcontrol1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbcontrol2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cba1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cba2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbe1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbe2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbx1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbx2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbc1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbc2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbv1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbv2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbspace1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbspace2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbf1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbf2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbr1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbr2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbuproll1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbuproll2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbdownroll1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbdownroll2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbleftclick1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbleftclick2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbrightclick1.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbrightclick2.SelectedIndex = SelectIndexInput(file.ReadLine());
                    cbads.IsChecked = bool.Parse(file.ReadLine());
                    tbstickxsens.Value = Convert.ToInt32(file.ReadLine());
                    tbstickysens.Value = Convert.ToInt32(file.ReadLine());
                    cbenglish.IsChecked = bool.Parse(file.ReadLine());
                    tbirxsens.Value = Convert.ToInt32(file.ReadLine());
                    tbirysens.Value = Convert.ToInt32(file.ReadLine());
                    cbcursor.IsChecked = bool.Parse(file.ReadLine());
                    cbbrink.IsChecked = bool.Parse(file.ReadLine());
                    cbmw3.IsChecked = bool.Parse(file.ReadLine());
                    cbtitanfall.IsChecked = bool.Parse(file.ReadLine());
                    cbmetro.IsChecked = bool.Parse(file.ReadLine());
                    cbbo3.IsChecked = bool.Parse(file.ReadLine());
                    cbfortnite.IsChecked = bool.Parse(file.ReadLine());
                    cbfake.IsChecked = bool.Parse(file.ReadLine());
                    cbmc.IsChecked = bool.Parse(file.ReadLine());
                    tbviewpower1x.Value = Convert.ToInt32(file.ReadLine());
                    tbviewpower2x.Value = Convert.ToInt32(file.ReadLine());
                    tbviewpower3x.Value = Convert.ToInt32(file.ReadLine());
                    tbviewpower1y.Value = Convert.ToInt32(file.ReadLine());
                    tbviewpower2y.Value = Convert.ToInt32(file.ReadLine());
                    tbviewpower3y.Value = Convert.ToInt32(file.ReadLine());
                    tbcentery.Value = Convert.ToInt32(file.ReadLine());
                    cbirmode.IsChecked = bool.Parse(file.ReadLine());
                    cbsendinput.IsChecked = bool.Parse(file.ReadLine());
                    cbsendescape.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsendtab.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsendshift.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsendcontrol.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsenda.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsende.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsendx.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsendc.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsendv.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsendspace.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsendf.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsendr.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsenduproll.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsenddownroll.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsendleftclick.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    cbsendrightclick.SelectedIndex = SelectIndexOutput(file.ReadLine());
                    tbdzx.Value = Convert.ToInt32(file.ReadLine());
                    tbdzy.Value = Convert.ToInt32(file.ReadLine());
                    cbarrows1.IsChecked = bool.Parse(file.ReadLine());
                    cbarrows2.IsChecked = bool.Parse(file.ReadLine());
                    cbnumpad.IsChecked = bool.Parse(file.ReadLine());
                }
                this.Title = "WiiJoyReflex: " + System.IO.Path.GetFileName(path);
            }
            catch { }
            Apply();
        }
        private int SelectIndexInput(string item)
        {
            int index = 0;
            if (item == "")
                index = 0;
            if (item == "JoyconLeftButtonDPAD_DOWN")
                index = 1;
            if (item == "JoyconLeftButtonDPAD_LEFT")
                index = 2;
            if (item == "JoyconLeftButtonDPAD_RIGHT")
                index = 3;
            if (item == "JoyconLeftButtonDPAD_UP")
                index = 4;
            if (item == "JoyconLeftButtonMINUS")
                index = 5;
            if (item == "JoyconLeftButtonACC")
                index = 6;
            if (item == "JoyconLeftButtonSHOULDER_1")
                index = 7;
            if (item == "JoyconLeftButtonSHOULDER_2")
                index = 8;
            if (item == "JoyconLeftButtonCAPTURE")
                index = 9;
            if (item == "WiimoteButtonStateOne")
                index = 10;
            if (item == "WiimoteButtonStateTwo")
                index = 11;
            if (item == "WiimoteButtonStateDown")
                index = 12;
            if (item == "WiimoteButtonStateLeft")
                index = 13;
            if (item == "WiimoteButtonStateRight")
                index = 14;
            if (item == "WiimoteButtonStateUp")
                index = 15;
            if (item == "WiimoteButtonStateHome")
                index = 16;
            if (item == "WiimoteButtonACC")
                index = 17;
            if (item == "WiimoteButtonStateA")
                index = 18;
            if (item == "WiimoteButtonStateB")
                index = 19;
            if (item == "WiimoteButtonStatePlus")
                index = 20;
            if (item == "WiimoteButtonStateMinus")
                index = 21;
            return index;
        }
        private int SelectIndexOutput(string item)
        {
            int index = 0;
            if (item == "SendLeftClick")
                index = 1;
            if (item == "SendRightClick")
                index = 2;
            if (item == "SendMiddleClick")
                index = 3;
            if (item == "SendWheelUp")
                index = 4;
            if (item == "SendWheelDown")
                index = 5;
            if (item == "SendLeft")
                index = 6;
            if (item == "SendRight")
                index = 7;
            if (item == "SendUp")
                index = 8;
            if (item == "SendDown")
                index = 9;
            if (item == "SendLButton")
                index = 10;
            if (item == "SendRButton")
                index = 11;
            if (item == "SendCancel")
                index = 12;
            if (item == "SendMBUTTON")
                index = 13;
            if (item == "SendXBUTTON1")
                index = 14;
            if (item == "SendXBUTTON2")
                index = 15;
            if (item == "SendBack")
                index = 16;
            if (item == "SendTab")
                index = 17;
            if (item == "SendClear")
                index = 18;
            if (item == "SendReturn")
                index = 19;
            if (item == "SendSHIFT")
                index = 20;
            if (item == "SendCONTROL")
                index = 21;
            if (item == "SendMENU")
                index = 22;
            if (item == "SendPAUSE")
                index = 23;
            if (item == "SendCAPITAL")
                index = 24;
            if (item == "SendKANA")
                index = 25;
            if (item == "SendHANGEUL")
                index = 26;
            if (item == "SendHANGUL")
                index = 27;
            if (item == "SendJUNJA")
                index = 28;
            if (item == "SendFINAL")
                index = 29;
            if (item == "SendHANJA")
                index = 30;
            if (item == "SendKANJI")
                index = 31;
            if (item == "SendEscape")
                index = 32;
            if (item == "SendCONVERT")
                index = 33;
            if (item == "SendNONCONVERT")
                index = 34;
            if (item == "SendACCEPT")
                index = 35;
            if (item == "SendMODECHANGE")
                index = 36;
            if (item == "SendSpace")
                index = 37;
            if (item == "SendPRIOR")
                index = 38;
            if (item == "SendNEXT")
                index = 39;
            if (item == "SendEND")
                index = 40;
            if (item == "SendHOME")
                index = 41;
            if (item == "SendLEFT")
                index = 42;
            if (item == "SendUP")
                index = 43;
            if (item == "SendRIGHT")
                index = 44;
            if (item == "SendDOWN")
                index = 45;
            if (item == "SendSELECT")
                index = 46;
            if (item == "SendPRINT")
                index = 47;
            if (item == "SendEXECUTE")
                index = 48;
            if (item == "SendSNAPSHOT")
                index = 49;
            if (item == "SendINSERT")
                index = 50;
            if (item == "SendDELETE")
                index = 51;
            if (item == "SendHELP")
                index = 52;
            if (item == "SendAPOSTROPHE")
                index = 53;
            if (item == "Send0")
                index = 54;
            if (item == "Send1")
                index = 55;
            if (item == "Send2")
                index = 56;
            if (item == "Send3")
                index = 57;
            if (item == "Send4")
                index = 58;
            if (item == "Send5")
                index = 59;
            if (item == "Send6")
                index = 60;
            if (item == "Send7")
                index = 61;
            if (item == "Send8")
                index = 62;
            if (item == "Send9")
                index = 63;
            if (item == "SendA")
                index = 64;
            if (item == "SendB")
                index = 65;
            if (item == "SendC")
                index = 66;
            if (item == "SendD")
                index = 67;
            if (item == "SendE")
                index = 68;
            if (item == "SendF")
                index = 69;
            if (item == "SendG")
                index = 70;
            if (item == "SendH")
                index = 71;
            if (item == "SendI")
                index = 72;
            if (item == "SendJ")
                index = 73;
            if (item == "SendK")
                index = 74;
            if (item == "SendL")
                index = 75;
            if (item == "SendM")
                index = 76;
            if (item == "SendN")
                index = 77;
            if (item == "SendO")
                index = 78;
            if (item == "SendP")
                index = 79;
            if (item == "SendQ")
                index = 80;
            if (item == "SendR")
                index = 81;
            if (item == "SendS")
                index = 82;
            if (item == "SendT")
                index = 83;
            if (item == "SendU")
                index = 84;
            if (item == "SendV")
                index = 85;
            if (item == "SendW")
                index = 86;
            if (item == "SendX")
                index = 87;
            if (item == "SendY")
                index = 88;
            if (item == "SendZ")
                index = 89;
            if (item == "SendLWIN")
                index = 90;
            if (item == "SendRWIN")
                index = 91;
            if (item == "SendAPPS")
                index = 92;
            if (item == "SendSLEEP")
                index = 93;
            if (item == "SendNUMPAD0")
                index = 94;
            if (item == "SendNUMPAD1")
                index = 95;
            if (item == "SendNUMPAD2")
                index = 96;
            if (item == "SendNUMPAD3")
                index = 97;
            if (item == "SendNUMPAD4")
                index = 98;
            if (item == "SendNUMPAD5")
                index = 99;
            if (item == "SendNUMPAD6")
                index = 100;
            if (item == "SendNUMPAD7")
                index = 101;
            if (item == "SendNUMPAD8")
                index = 102;
            if (item == "SendNUMPAD9")
                index = 103;
            if (item == "SendMULTIPLY")
                index = 104;
            if (item == "SendADD")
                index = 105;
            if (item == "SendSEPARATOR")
                index = 106;
            if (item == "SendSUBTRACT")
                index = 107;
            if (item == "SendDECIMAL")
                index = 108;
            if (item == "SendDIVIDE")
                index = 109;
            if (item == "SendF1")
                index = 110;
            if (item == "SendF2")
                index = 111;
            if (item == "SendF3")
                index = 112;
            if (item == "SendF4")
                index = 113;
            if (item == "SendF5")
                index = 114;
            if (item == "SendF6")
                index = 115;
            if (item == "SendF7")
                index = 116;
            if (item == "SendF8")
                index = 117;
            if (item == "SendF9")
                index = 118;
            if (item == "SendF10")
                index = 119;
            if (item == "SendF11")
                index = 120;
            if (item == "SendF12")
                index = 121;
            if (item == "SendF13")
                index = 122;
            if (item == "SendF14")
                index = 123;
            if (item == "SendF15")
                index = 124;
            if (item == "SendF16")
                index = 125;
            if (item == "SendF17")
                index = 126;
            if (item == "SendF18")
                index = 127;
            if (item == "SendF19")
                index = 128;
            if (item == "SendF20")
                index = 129;
            if (item == "SendF21")
                index = 130;
            if (item == "SendF22")
                index = 131;
            if (item == "SendF23")
                index = 132;
            if (item == "SendF24")
                index = 133;
            if (item == "SendNUMLOCK")
                index = 134;
            if (item == "SendSCROLL")
                index = 135;
            if (item == "SendLeftShift")
                index = 136;
            if (item == "SendRightShift")
                index = 137;
            if (item == "SendLeftControl")
                index = 138;
            if (item == "SendRightControl")
                index = 139;
            if (item == "SendLMENU")
                index = 140;
            if (item == "SendRMENU")
                index = 141;
            return index;
        }
        private void bapply_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Apply();
        }
        private void Apply()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                wjr.Class1.Apply(cbescape1.SelectionBoxItem.ToString(), cbescape2.SelectionBoxItem.ToString(), cbtab1.SelectionBoxItem.ToString(), cbtab2.SelectionBoxItem.ToString(), cbshift1.SelectionBoxItem.ToString(), cbshift2.SelectionBoxItem.ToString(), cbcontrol1.SelectionBoxItem.ToString(), cbcontrol2.SelectionBoxItem.ToString(), cba1.SelectionBoxItem.ToString(), cba2.SelectionBoxItem.ToString(), cbe1.SelectionBoxItem.ToString(), cbe2.SelectionBoxItem.ToString(), cbx1.SelectionBoxItem.ToString(), cbx2.SelectionBoxItem.ToString(), cbc1.SelectionBoxItem.ToString(), cbc2.SelectionBoxItem.ToString(), cbv1.SelectionBoxItem.ToString(), cbv2.SelectionBoxItem.ToString(), cbspace1.SelectionBoxItem.ToString(), cbspace2.SelectionBoxItem.ToString(), cbf1.SelectionBoxItem.ToString(), cbf2.SelectionBoxItem.ToString(), cbr1.SelectionBoxItem.ToString(), cbr2.SelectionBoxItem.ToString(), cbuproll1.SelectionBoxItem.ToString(), cbuproll2.SelectionBoxItem.ToString(), cbdownroll1.SelectionBoxItem.ToString(), cbdownroll2.SelectionBoxItem.ToString(), cbleftclick1.SelectionBoxItem.ToString(), cbleftclick2.SelectionBoxItem.ToString(), cbrightclick1.SelectionBoxItem.ToString(), cbrightclick2.SelectionBoxItem.ToString(), bool.Parse(cbirmode.IsChecked.ToString()) ? 2 : 1, (double)tbcentery.Value, (bool)cbsendinput.IsChecked, (bool)cbads.IsChecked, (bool)cbenglish.IsChecked, (double)tbirxsens.Value / 10f, (double)tbirysens.Value / 10f, (double)tbstickxsens.Value / 10f, (double)tbstickysens.Value / 10f, (double)tbviewpower1x.Value, (double)tbviewpower2x.Value, (double)tbviewpower3x.Value, (double)tbviewpower1y.Value, (double)tbviewpower2y.Value, (double)tbviewpower3y.Value, (bool)cbcursor.IsChecked, (bool)cbbrink.IsChecked, (bool)cbmw3.IsChecked, (bool)cbtitanfall.IsChecked, (bool)cbmetro.IsChecked, (bool)cbbo3.IsChecked, (bool)cbfortnite.IsChecked, (bool)cbfake.IsChecked, (bool)cbmc.IsChecked, cbsendescape.SelectionBoxItem.ToString(), cbsendtab.SelectionBoxItem.ToString(), cbsendshift.SelectionBoxItem.ToString(), cbsendcontrol.SelectionBoxItem.ToString(), cbsenda.SelectionBoxItem.ToString(), cbsende.SelectionBoxItem.ToString(), cbsendx.SelectionBoxItem.ToString(), cbsendc.SelectionBoxItem.ToString(), cbsendv.SelectionBoxItem.ToString(), cbsendspace.SelectionBoxItem.ToString(), cbsendf.SelectionBoxItem.ToString(), cbsendr.SelectionBoxItem.ToString(), cbsenduproll.SelectionBoxItem.ToString(), cbsenddownroll.SelectionBoxItem.ToString(), cbsendleftclick.SelectionBoxItem.ToString(), cbsendrightclick.SelectionBoxItem.ToString(), (double)tbdzx.Value, (double)tbdzy.Value, (bool)cbarrows1.IsChecked, (bool)cbarrows2.IsChecked, (bool)cbnumpad.IsChecked);
            }));
        }
        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            TimeBeginPeriod(1);
            NtSetTimerResolution(1, true, ref CurrentResolution);
            if (System.IO.File.Exists("tempsave"))
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("tempsave"))
                {
                    filename = file.ReadLine();
                }
                if (filename != "" & System.IO.File.Exists(filename))
                {
                    OpenConfig(filename);
                }
                else
                    Apply();
            }
            else
                Apply();
            System.Windows.Application.Current.MainWindow.Closed += MainWindow_Closed;
            running = true;
            Dispatcher.Invoke(new Action(() =>
            {
                Task.Run(() => wjr.Class1.Start());
                Task.Run(() => taskX());
            }));
        }
        private void taskX()
        {
            while (running)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        wjr.Class1.taskB();
                        wjr.Class1.taskX();
                    }
                    catch { }
                }));
                Thread.Sleep(1);
            }
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                if (filename != "")
                {
                    using (StreamWriter createdfile = new StreamWriter("tempsave"))
                    {
                        createdfile.WriteLine(filename);
                    }
                }
                running = false;
                wjr.Class1.Close();
            }
            catch { }
        }
    }
}