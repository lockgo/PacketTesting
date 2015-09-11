using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace ConsoleLineWriter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UdpClient udpClient = new UdpClient(11000);
            try
            {
                Console.WriteLine("Trying again");

                IPAddress serverAddr = IPAddress.Parse("127.0.0.1");
                udpClient.Connect(serverAddr, 11000);

                Console.WriteLine("Trying again2");
                // Sends a message to the host to which you have connected.
                //Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");
                Byte[] sendBytes = Encoding.ASCII.GetBytes(textBox1.Text);
                Console.WriteLine("Trying again2-1");
                udpClient.Send(sendBytes, sendBytes.Length);
                Console.WriteLine("Trying again3");
                // Sends a message to a different host using optional hostname and port parameters.
                UdpClient udpClientB = new UdpClient();
                udpClientB.Send(sendBytes, sendBytes.Length, "AlternateHostMachineName", 11000);
                Console.WriteLine("Trying again4");
                //IPEndPoint object will allow us to read datagrams sent from any source.

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                Console.WriteLine("Trying again5");
                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);
                Console.WriteLine("Trying again6");
                // Uses the IPEndPoint object to determine which of these two hosts responded.
                Console.WriteLine("This is the message you received " +
                                             returnData.ToString());
                Console.WriteLine("This message was sent from " +
                                            RemoteIpEndPoint.Address.ToString() +
                                            " on their port number " +
                                            RemoteIpEndPoint.Port.ToString());
                //textBox1.Text = returnData.ToString();
                richTextBox1.Text = returnData.ToString();

                udpClient.Close();
                udpClientB.Close();

            }
            catch (Exception f)
            {
                Console.WriteLine(f.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*UdpClient udpClient = new UdpClient(11000);
            IPAddress serverAddr = IPAddress.Parse(textBox2.Text);
            udpClient.Connect(serverAddr, 11000);*/

            //141
            //byte[] ascii = System.Text.Encoding.ASCII.GetBytes(sampleTestString);
            //byte[] utf8 = System.Text.Encoding.UTF8.GetBytes(sampleTestString);

            //byte[] pkt1426 = {
            //     };


            UdpClient udpClient = new UdpClient(61694);
            //textBox2.Text = "Server Connecting too";
            IPAddress serverAddr = IPAddress.Parse(textBox2.Text);
            udpClient.Connect(serverAddr, 5838);

            Console.WriteLine("Trying again2");
            // Sends a message to the host to which you have connected.
            //Byte[] sendBytes = Encoding.ASCII.GetBytes("Is anybody there?");
            Byte[] sendBytes = Encoding.ASCII.GetBytes(textBox1.Text);
            //Byte[] sendBytes = pkt1426;
            Console.WriteLine("Trying again2-1");
            int abcd = 0;
            while(abcd < 100)
            {
                udpClient.Send(sendBytes, sendBytes.Length);
                abcd++;
            }
            richTextBox1.Text = sendBytes.ToString();

            Console.WriteLine("Trying again3");
            // Sends a message to a different host using optional hostname and port parameters.


            udpClient.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            UdpClient udpClient = new UdpClient(11000);
            try
            {
                Console.WriteLine("Trying again");

                IPAddress serverAddr = IPAddress.Parse(textBox2.Text);
                udpClient.Connect(serverAddr, 11000);

                Console.WriteLine("Trying again2");
                // Sends a message to the host to which you have connected.

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Console.WriteLine("Trying again5");
                // Blocks until a message returns on this socket from a remote host.
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);
                Console.WriteLine("Trying again6");
                // Uses the IPEndPoint object to determine which of these two hosts responded.
                Console.WriteLine("This is the message you received " +
                                             returnData.ToString());
                Console.WriteLine("This message was sent from " +
                                            RemoteIpEndPoint.Address.ToString() +
                                            " on their port number " +
                                            RemoteIpEndPoint.Port.ToString());
                //textBox1.Text = returnData.ToString();
                richTextBox1.Text = returnData.ToString();

                udpClient.Close();

            }
            catch (Exception f)
            {
                Console.WriteLine(f.ToString());
            }

        }
        private void listen_Click(object sender, EventArgs e)
        {
            textBox2.Text = ListenForPing();
        }

        private string ListenForPing()
        {
            int noConnections = 0;
            UdpClient udp = new UdpClient(11000);
            string sendersIP;
            while (noConnections == 0)
            {

                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] recData = udp.Receive(ref anyIP);
                string ping = Encoding.ASCII.GetString(recData);
                if (ping == "ping")
                {
                    Console.WriteLine("Ping received.");
                    Console.WriteLine("Ping was sent from " + anyIP.Address.ToString() +
                                    " on their port number " + anyIP.Port.ToString());
                    sendersIP = anyIP.Address.ToString();
                    //InvokePingReceiveEvent();
                    noConnections = 1;
                    udp.Close();
                    return sendersIP;
                }
                else//send an end connection Packet like this?
                {
                    Console.WriteLine("Wasn't a ping");
                    sendersIP = anyIP.Address.ToString();
                    udp.Close();
                    return sendersIP;
                }
            }
            return "0.0.0.0";
        }

        private void Connect(String server, String message)
        {
            try
            {
                // Create a TcpClient. 
                // Note, for this client to work you need to have a TcpServer  
                // connected to the same address as specified by the server, port 
                // combination.
                Int32 port = 13000;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing. 
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response. 

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }

        private void TCPlistening()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse(textBox2.Text);

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop. 
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client. 
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Connect("192.168.2.19", "What the Hell?");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TCPlistening();
            richTextBox1.Text = "done";
        }

        private void getIPAdress_Click(object sender, EventArgs e)
        {
            textBox2.Text = GetLocalIPAddress();
        }


    }
}
