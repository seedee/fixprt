/*
 * fixprt
 * Copyright 06/4/2020
 * by seedee
 * 
 * fixprt is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * fixprt is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with fixprt. If not, see <https://www.gnu.org/licenses/>.
 */
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace fixprt
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static string[] args; // Command line arguments
        static string filenametrimsafe = String.Empty; // Current file name without path
        static string filenametrim = String.Empty; // Current file name with trimmed extension
        static string prt = String.Empty; // Imported portal file contents
        static string prtfix = String.Empty; // Fixed portal file contents
        static Regex open = new Regex(@"[^\.\-\(\)\s\d]");
        static Regex fix = new Regex(@"(?<=^(?:.*[\n]+){2})(?:(?!.*[()]).*[\r\n]+)+");

        internal static class NativeMethods
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern int AllocConsole();
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern int FreeConsole();
        }

        [STAThread]
        static void Main()
        {
            args = Environment.GetCommandLineArgs();
            if (args.ElementAtOrDefault(1) != null)
            {
                Cmdline();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main());
            }
        }

        static void Cmdline()
        {
            NativeMethods.AllocConsole();
            filenametrim = args[1].Substring(0, args[1].LastIndexOf(".") + 1);
            if (filenametrim == "")
            {
                filenametrim = args[1] + ".";
            }
            filenametrimsafe = Path.GetFileNameWithoutExtension(filenametrim);
            StreamWriter sw = File.AppendText(filenametrim + "log");
            Console.WriteLine("fixprt v1.0.0 (25/3/2020)");
            sw.WriteLine("fixprt v1.0.0 (25/3/2020)");
            Console.WriteLine("by seedee (cdaniel9000@gmail.com)");
            sw.WriteLine("by seedee (cdaniel9000@gmail.com)");
            Console.WriteLine("-----  BEGIN  fixprt -----");
            sw.WriteLine("-----  BEGIN  fixprt -----");
            Console.WriteLine("Command line: " + String.Join(" ", args));
            sw.WriteLine("Command line: " + String.Join(" ", args));
            Console.WriteLine();
            sw.WriteLine();
            if (File.Exists(filenametrim + "prt"))
            {
                using (StreamReader sr = new StreamReader(filenametrim + "prt"))
                {
                    var sb = new StringBuilder();
                    while (!sr.EndOfStream)
                    {
                        sb.AppendLine(sr.ReadLine());
                    }
                    prt = sb.ToString();

                    if (String.IsNullOrWhiteSpace(prt))
                    {
                        Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenametrimsafe + ".prt is not a valid portal file (null or whitespace)");
                        sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenametrimsafe + ".prt is not a valid portal file (null or whitespace)");
                        Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Closed " + filenametrimsafe + ".prt");
                        sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Closed " + filenametrimsafe + ".prt");
                        Console.WriteLine();
                        sw.WriteLine();
                        Console.WriteLine("-----   END   fixprt -----");
                        sw.WriteLine("-----   END   fixprt -----");
                        Console.WriteLine();
                        sw.WriteLine();
                        Console.WriteLine();
                        sw.WriteLine();
                        Console.WriteLine();
                        sw.WriteLine();
                        sw.Dispose();
                        NativeMethods.FreeConsole();
                        Application.Exit();
                    }
                    else if (prt.Contains("PRT1")) // Source engine portal files contain the PRT1 signature
                    {
                        Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenametrimsafe + ".prt is not a valid portal file (PRT signature detected)");
                        sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenametrimsafe + ".prt is not a valid portal file (PRT signature detected)");
                        Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Closed " + filenametrimsafe + ".prt");
                        sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Closed " + filenametrimsafe + ".prt");
                        Console.WriteLine();
                        sw.WriteLine();
                        Console.WriteLine("-----   END   fixprt -----");
                        sw.WriteLine("-----   END   fixprt -----");
                        Console.WriteLine();
                        sw.WriteLine();
                        Console.WriteLine();
                        sw.WriteLine();
                        Console.WriteLine();
                        sw.WriteLine();
                        sw.Dispose();
                        NativeMethods.FreeConsole();
                        Application.Exit();
                    }
                    else if (open.IsMatch(prt)) // Only digits, whitespace, dashes, parentheses and full stops are allowed

                    {
                        Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenametrimsafe + ".prt is not a valid portal file (invalid characters)");
                        sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenametrimsafe + ".prt is not a valid portal file (invalid characters)");
                        Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Closed " + filenametrimsafe + ".prt");
                        sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Closed " + filenametrimsafe + ".prt");
                        Console.WriteLine();
                        sw.WriteLine();
                        Console.WriteLine("-----   END   fixprt -----");
                        sw.WriteLine("-----   END   fixprt -----");
                        Console.WriteLine();
                        sw.WriteLine();
                        Console.WriteLine();
                        sw.WriteLine();
                        Console.WriteLine();
                        sw.WriteLine();
                        sw.Dispose();
                        NativeMethods.FreeConsole();
                        Application.Exit();
                    }
                    else if (!(open.IsMatch(prt)))
                    {
                        Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Opened " + filenametrimsafe + ".prt");
                        sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Opened " + filenametrimsafe + ".prt");
                    }
                }
                if (fix.IsMatch(prt))
                {
                    prtfix = fix.Replace(prt, "");
                    int prtcount = 0;
                    int prtfixcount = 0;
                    int prtposition = -1;
                    int prtfixposition = -1;
                    while ((prtposition = prt.IndexOf(Environment.NewLine, prtposition + 1)) != -1)
                    {
                        prtcount++;
                    }
                    while ((prtfixposition = prtfix.IndexOf(Environment.NewLine, prtfixposition + 1)) != -1)
                    {
                        prtfixcount++;
                    }
                    Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Fixed " + filenametrimsafe + ".prt (reduced " + prtcount + " lines to " + prtfixcount + " lines)");
                    sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Fixed " + filenametrimsafe + ".prt (reduced " + prtcount + " lines to " + prtfixcount + " lines)");
                }
                else
                {
                    Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenametrimsafe + ".prt is already fixed" + Environment.NewLine);
                    sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenametrimsafe + ".prt is already fixed" + Environment.NewLine);
                    Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Closed " + filenametrimsafe + ".prt");
                    sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Closed " + filenametrimsafe + ".prt");
                }
                if (!(String.IsNullOrWhiteSpace(prtfix)))
                {
                    File.WriteAllText(filenametrim + "prt", prtfix);
                }
                else if (!(String.IsNullOrWhiteSpace(prt)))
                {
                    File.WriteAllText(filenametrim + "prt", prt);
                }
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Saved " + filenametrimsafe + ".prt");
                sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Saved " + filenametrimsafe + ".prt");
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Closed " + filenametrimsafe + ".prt");
                sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Closed " + filenametrimsafe + ".prt");
            }
            else if (!(File.Exists(filenametrim + ".prt")))
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: Could not find the " + filenametrimsafe + ".prt file");
                sw.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: Could not find the " + filenametrimsafe + ".prt file");
            }
            Console.WriteLine();
            sw.WriteLine();
            Console.WriteLine("-----   END   fixprt -----");
            sw.WriteLine("-----   END   fixprt -----");
            Console.WriteLine();
            sw.WriteLine();
            Console.WriteLine();
            sw.WriteLine();
            Console.WriteLine();
            sw.WriteLine();
            sw.Dispose();
            NativeMethods.FreeConsole();
            Application.Exit();
        }
    }
}
