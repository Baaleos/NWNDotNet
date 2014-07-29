using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Web;
using NetMessage;
using System.Reflection;
using System.IO;
namespace AsmodeiServices
{
    public class NWNListener
    {
        public NWNListener()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);

            DoStartUp();
         }


        static byte[] StreamToBytes(Stream input)
        {

            int capacity = input.CanSeek ? (int)input.Length : 0;
            using (MemoryStream output = new MemoryStream(capacity))
            {
                int readLength;
                byte[] buffer = new byte[4096];
                do
                {
                    readLength = input.Read(buffer, 0, buffer.Length);
                    output.Write(buffer, 0, readLength);
                }
                while (readLength != 0);
                return output.ToArray();
            }
        }

        private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            // string resName = args.Name + ".dll";
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            string[] toSplitWith = { "," };
            string sFileName = args.Name.Split(toSplitWith, StringSplitOptions.RemoveEmptyEntries)[0] + ".dll";


            using (Stream input = thisAssembly.GetManifestResourceStream("AsmodeiServices.Resources." + sFileName))
                {
                    return input != null ? Assembly.Load(StreamToBytes(input)) : null;
                }
            
        }



        public TcpListener TcpListener = null;
        public IPAddress LOCALBIND = IPAddress.Parse("10.147.143.148");
        public int PORT = 52122;
        public void DoStartUp()
        {
            try
            {

                
                IPAddress TheIP = LOCALBIND;
               
                TcpListener = new TcpListener(TheIP, PORT);//Binds the listener object to the designated network adapter, and port.
                TcpListener.Start(); //Start the Listener
                Thread T = new Thread(ServerMainLoop); //Make a new thread, for the function ServerMainLoop.
                T.IsBackground = true;  //Background thread means that the thread will close when the application closes, otherwise we get a disjointed thread that remains open while the application is closed.
                T.Start(); //Start
                
            }
            catch (Exception ee)
            {
                
            }
        }
        public void ServerMainLoop()
        {

            try
            {
                while (true)
                {
                    try
                    {
                        if (TcpListener.Pending())
                        {
                            TcpClient newClient = TcpListener.AcceptTcpClient();
                            ProcessRequest(newClient);
                        }
                    }
                    catch (Exception ee)
                    {
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception eInWhileLoop)
            {
                
            }
        }

        public void DisconnectClient(TcpClient newClient)
        {
            try
            {
                newClient.Client.Disconnect(false);
            }
            catch (Exception ee)
            {

            }
            try
            {
                newClient.GetStream().Close();
            }
            catch (Exception ee)
            {

            }
        }

        

        public void ProcessRequest(TcpClient newClient)
        {
            try
            {
                NetworkStream mainStream = newClient.GetStream();
                NetMessage.NetMessage message = FunctionLibrary.ReceivedNetMessage(mainStream);

                
                if (message == null)
                {
                    DisconnectClient(newClient);
                    return;
                }
                NWNDotNet.Hooks.MainLoop.AddToQueue(message);
                DisconnectClient(newClient);
                return;

                string ARG = "";
                string ARG2 = "";
                switch (message.GetCommandType())
                {
                    case "SHOUT":
                        ARG = message.GetCommandArgumentList()[0].ToString();
                        NwnxAssembly.CBinding.RunAScript("nwnx_shout", ARG);
                        break;

                    default:
                        try
                        {
                            ARG = message.GetCommandArgumentList()[0].ToString();
                            ARG2 = message.GetCommandArgumentList()[1].ToString();
                            NwnxAssembly.CBinding.RunAScript(ARG, ARG2);
                        }
                        catch (Exception ee) { }
                        break;
                }
                
            }
            catch (Exception ee)
            {

            }



            
        }



        // Convert an object to a byte array
        public static byte[] ObjectToByteArray(object _Object)
        {
            try
            {
                // create new memory stream
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream();

                // create new BinaryFormatter
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter
                            = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                // Serializes an object, or graph of connected objects, to the given stream.
                _BinaryFormatter.Serialize(_MemoryStream, _Object);

                // convert stream to byte array and return
                return _MemoryStream.ToArray();
            }
            catch (Exception _Exception)
            {
                // Error
                //System.Windows.Forms.MessageBox.Show("Exception caught in process: {0}", _Exception.ToString());
            }

            // Error occured, return null
            return null;
        }
        // Convert a byte array to an Object
        public static object ByteArrayToObject(byte[] _ByteArray)
        {
            try
            {
                // convert byte array to memory stream
                System.IO.MemoryStream _MemoryStream = new System.IO.MemoryStream(_ByteArray);

                // create new BinaryFormatter
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter _BinaryFormatter
                            = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                _MemoryStream.Flush();

                // set memory stream position to starting point
                //_MemoryStream.Position = 0;
                _MemoryStream.Seek(0, SeekOrigin.Begin);

                // Deserializes a stream into an object graph and return as a object.
                return _BinaryFormatter.Deserialize(_MemoryStream);
            }
            catch (Exception _Exception)
            {
                // Error
                //System.Windows.Forms.MessageBox.Show(_Exception.ToString());
            }

            // Error occured, return null
            return null;
        }

    }


    public class FunctionLibrary
    {
        public static bool FetchWebsite(string sURL)
        {
            try
            {
                // used to build entire input
                StringBuilder sb = new StringBuilder();

                // used on each read operation
                byte[] buf = new byte[8192];

                // prepare the web page we will be asking for
                HttpWebRequest request = (HttpWebRequest)
                    WebRequest.Create(sURL);

                // execute the request
                HttpWebResponse response = (HttpWebResponse)
                    request.GetResponse();

                // we will read data via the response stream
                Stream resStream = response.GetResponseStream();

                string tempString = null;
                int count = 0;

                do
                {
                    // fill the buffer with data
                    count = resStream.Read(buf, 0, buf.Length);

                    // make sure we read some data
                    if (count != 0)
                    {
                        // translate from bytes to ASCII text
                        tempString = Encoding.ASCII.GetString(buf, 0, count);

                        // continue building the string
                        sb.Append(tempString);
                    }
                }
                while (count > 0); // any more data to read?
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {

            }
            // print out page source
        }





        public static byte[] ReadImageURL(string sURL)
        {
            try
            {
                WebClient w = new WebClient();
                return w.DownloadData(sURL);
            }
            catch (Exception e)
            {
                return null;
            }

        }


        //public const string UPDATE_SERVER = "54.225.78.12";



        


        public static void SendBytesToStream(NetworkStream TheStream, byte[] TheMessage)
        {

            //IAsyncResult r = TheStream.BeginWrite(TheMessage, 0, TheMessage.Length, null, null);
            // r.AsyncWaitHandle.WaitOne(10000);
            //TheStream.EndWrite(r); 
            try
            {
                long len = TheMessage.Length;

                byte[] Bytelen = BitConverter.GetBytes(len);

                TheStream.Write(Bytelen, 0, Bytelen.Length);
                TheStream.Write(TheMessage, 0, TheMessage.Length);
                TheStream.Flush();
            }
            catch (Exception e)
            {
                //System.Windows.Forms.MessageBox.Show(e.ToString());
            }
        }

        public static NetMessage.NetMessage ReceivedNetMessage(NetworkStream MainStream)
        {
            byte[] b = ReceivedBytes(MainStream);
            object o = NWNListener.ByteArrayToObject(b);
            NetMessage.NetMessage n = (NetMessage.NetMessage)o;
            b = null;
            o = null;
            return n;
        }

        private static byte[] ReceivedBytes(NetworkStream MainStream)
        {
            try
            {
                //byte[] myReadBuffer = new byte[1024];
                int receivedDataLength = 0;
                byte[] data = { };
                long len = 0;
                int i = 0;
                MainStream.ReadTimeout = 60000;
                
                if (MainStream.CanRead)
                {
                    
                    byte[] byteLen = new byte[8];
                    MainStream.Read(byteLen, 0, 8);
                    len = BitConverter.ToInt64(byteLen, 0);
                    data = new byte[len];

                    
                    Thread.Sleep(100);
                    while (receivedDataLength < data.Length)
                    {
                        receivedDataLength += MainStream.Read(data, receivedDataLength, 1);
                    }
                   
                    return data;
                }



            }
            catch (Exception E)
            {

                //System.Windows.Forms.MessageBox.Show("Exception:" + E.ToString());
            }
            return null;
        }

        public static bool SendNetMessageToStream(NetworkStream TheStream, ref NetMessage.NetMessage NetMessage)
        {
            try
            {
                byte[] b = NWNListener.ObjectToByteArray(NetMessage);
                SendBytesToStream(TheStream, b);
                NetMessage = null;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
