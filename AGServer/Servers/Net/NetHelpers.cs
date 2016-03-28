using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace AGServer.Servers.Net
{
    class NetHelpers
    {
        public static bool CheckPortIsFree(IPAddress ipAddress, int port)
        {
            bool portAvailable = true;
            try
            {
                TcpListener tcpListener = new TcpListener(ipAddress, port);
                tcpListener.Start();
                tcpListener.Stop();
                tcpListener = null;
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.ToString());
                portAvailable = false;
            }

            return portAvailable;
        }

        public static IPAddress GetLocalIpAddress()
        {
            IPHostEntry host;
            IPAddress localIP = null;

            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString() != "192.168.56.1")
                {
                    localIP = ip;
                    break;
                }
            }
            return localIP;
        }

        public static Port FindProcessUsingPort(int portToCheck)
        {
            List<Port> ports = GetNetStatPorts();
            foreach(Port process in ports) {
                if (process.port_number == portToCheck)
                {
                    return process;
                }
            }
            return null;
        }

        public static List<Port> GetNetStatPorts()
        {
            var Ports = new List<Port>();

            try
            {
                using (Process p = new Process())
                {
                    ProcessStartInfo ps = new ProcessStartInfo();
                    ps.Arguments = "-a -n -o";
                    ps.FileName = "netstat.exe";
                    ps.UseShellExecute = false;
                    ps.WindowStyle = ProcessWindowStyle.Hidden;
                    ps.RedirectStandardInput = true;
                    ps.RedirectStandardOutput = true;
                    ps.RedirectStandardError = true;

                    p.StartInfo = ps;
                    p.Start();

                    StreamReader stdOutput = p.StandardOutput;
                    StreamReader stdError = p.StandardError;

                    string content = stdOutput.ReadToEnd() + stdError.ReadToEnd();
                    string exitStatus = p.ExitCode.ToString();

                    if (exitStatus != "0")
                    {
                        // Command Errored. Handle Here If Need Be
                    }

                    //Get The Rows
                    string[] rows = Regex.Split(content, "\r\n");
                    foreach (string row in rows)
                    {
                        //Split it baby
                        string[] tokens = Regex.Split(row, "\\s+");
                        if (tokens.Length > 4 && (tokens[1].Equals("UDP") || tokens[1].Equals("TCP")))
                        {
                            string localAddress = Regex.Replace(tokens[2], @"\[(.*?)\]", "1.1.1.1");
                            Ports.Add(new Port
                            {
                                protocol = localAddress.Contains("1.1.1.1") ? String.Format("{0}v6", tokens[1]) : String.Format("{0}v4", tokens[1]),
                                port_number = Int32.Parse(localAddress.Split(':')[1]),
                                process_name = tokens[1] == "UDP" ? LookupProcess(Convert.ToInt16(tokens[4])) : LookupProcess(Convert.ToInt16(tokens[5]))
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ports;
        }

        public static string LookupProcess(int pid)
        {
            string procName;
            try
            {
                procName = Process.GetProcessById(pid).ProcessName;
            }
            catch (Exception)
            {
                procName = "-";
            }
            return procName;
        }

    }
}
