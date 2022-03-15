using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GsmsRazor.Server
{
    public class SocketListener
    {
        private SignalRHub _hub;
        public SocketListener(SignalRHub hub)
        {
            _hub = hub;
        }
        public void RedirectOnScanned()
        {
            _hub.ReloadPage();
        }

        public void ProcessMessage(object parm)
        {
            string data;
            int count;
            try
            {
                TcpClient tcpClient = parm as TcpClient;
                Byte[] buffer = new Byte[1024];
                NetworkStream stream = tcpClient.GetStream();
                while ((count = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    data = System.Text.Encoding.UTF8.GetString(buffer, 0, count);
                    Debug.WriteLine(data);
                    if (data.Contains("OK"))
                    {
                        this.RedirectOnScanned();
                    }
                }
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public string StartServer()
        {
            Debug.WriteLine(GetLocalIPAddress());
            TcpListener listener = null;
            try
            {
                Debug.WriteLine("listening");
                listener = new TcpListener(GetLocalIPAddress(), 11000);
                listener.Start();
                while (true)
                {
                    TcpClient tcpClient = listener.AcceptTcpClient();
                    Thread thread = new Thread(new ParameterizedThreadStart(ProcessMessage));
                    thread.Start(tcpClient);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                listener.Stop();
            }
            return GetLocalIPAddress().ToString();
        }
    }
}
