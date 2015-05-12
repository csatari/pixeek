using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
#if WINDOWS
using System.Net.Http;
using System.Net.Http.Headers;
#endif
#if ANDROID
using Java.Net;
using Java.IO;
using Java.Lang;
#endif
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Pixeek.ServerCommunicator
{
    public abstract class ServerCommunicator
    {
        private const string URL = "http://nipglab09.inf.elte.hu:8000/pixeek";
        private const string URL_MULTI = "nipglab09.inf.elte.hu";
        private const int PORT_MULTI = 8001;
        private bool connected = false;
        SocketPermission permission;
        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint ipEndPoint;
        Socket socket;

        public delegate void CommandResult(string result);

        /// <summary>
        /// Command is sent (parameters are in path), and the result will be in commandResult
        /// </summary>
        /// <param name="path"></param>
        /// <param name="commandResult"></param>
        protected void SendGetCommand(string path, CommandResult commandResult)
        {
#if WINDOWS
            new Thread(() =>
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(URL + path).Result;

                if (response.IsSuccessStatusCode)
                {
                    commandResult(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    //HIBA
                    //commandResult(null);
                }  
            }).Start();
            
#endif
#if ANDROID
            new System.Threading.Thread(() =>
            {
                Java.Net.URL obj = new Java.Net.URL(URL + path);
                HttpURLConnection con = (HttpURLConnection)obj.OpenConnection();
                con.RequestMethod = "GET";

                HttpStatus responseCode = con.ResponseCode;

                BufferedReader br = new BufferedReader(new InputStreamReader(con.InputStream));
                System.String inputLine;
		        System.String response = "";
 
                inputLine = br.ReadLine();
		        while (inputLine != null) {
                    response += inputLine;
                    inputLine = br.ReadLine();
		        }
                br.Close();

                commandResult(response);
            }).Start();
            
#endif
        }

        /// <summary>
        /// Command is sent (parameters are in path), the body is the parameter Object, what will be serialized via json
        /// Result will be in commandResult.
        /// PUT method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parameter"></param>
        /// <param name="commandResult"></param>
        protected void SendPutCommand(string path, System.Object parameter, CommandResult commandResult)
        {
#if WINDOWS
            new Thread(() =>
            {
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(fastJSON.JSON.ToJSON(parameter), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PutAsync(URL + path,content).Result;

                if (response.IsSuccessStatusCode)
                {
                    commandResult(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    //HIBA
                    //commandResult(null);
                }
            }).Start();
#endif
#if ANDROID
            new System.Threading.Thread(() =>
            {
                Java.Net.URL obj = new Java.Net.URL(URL + path);
                HttpURLConnection con = (HttpURLConnection)obj.OpenConnection();
                con.RequestMethod = "PUT";

                OutputStreamWriter outstream = new OutputStreamWriter(con.OutputStream);

                outstream.Write(fastJSON.JSON.ToJSON(parameter));
                outstream.Close();

                HttpStatus responseCode = con.ResponseCode;

                BufferedReader br = new BufferedReader(new InputStreamReader(con.InputStream));
                /*System.String inputLine;
		        System.String response = "";
 
                inputLine = br.ReadLine();
		        while (inputLine != null) {
                    response += inputLine;
                    inputLine = br.ReadLine();
		        }*/
                br.Close();

                commandResult(null);
                

                /*HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(URL + path).Result;

                if (response.IsSuccessStatusCode)
                {
                    commandResult(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    //HIBA
                    //commandResult(null);
                }*/
            }).Start();
            
#endif

        }

        /// <summary>
        /// Command is sent (parameters are in path), the body is the parameter Object, what will be serialized via json
        /// Result will be in commandResult
        /// POST method
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parameter"></param>
        /// <param name="commandResult"></param>
        protected void SendPostCommand(string path, System.Object parameter, CommandResult commandResult)
        {
#if WINDOWS
            new Thread(() =>
            {
                HttpClient client = new HttpClient();
                HttpContent content = new StringContent(fastJSON.JSON.ToJSON(parameter), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(URL + path, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    commandResult(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    //HIBA
                    //commandResult(null);
                }
            }).Start();
#endif
#if ANDROID
            new System.Threading.Thread(() =>
            {
                Java.Net.URL obj = new Java.Net.URL(URL + path);
                HttpURLConnection con = (HttpURLConnection)obj.OpenConnection();
                con.RequestMethod = "POST";

                HttpStatus responseCode = con.ResponseCode;

                OutputStreamWriter outstream = new OutputStreamWriter(con.OutputStream);

                outstream.Write(fastJSON.JSON.ToJSON(parameter));
                outstream.Close();

                BufferedReader br = new BufferedReader(new InputStreamReader(con.InputStream));
                System.String inputLine;
		        System.String response = "";
 
                inputLine = br.ReadLine();
		        while (inputLine != null) {
                    response += inputLine;
                    inputLine = br.ReadLine();
		        }
                br.Close();

                commandResult(response);
                

                /*HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync(URL + path).Result;

                if (response.IsSuccessStatusCode)
                {
                    commandResult(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    //HIBA
                    //commandResult(null);
                }*/
            }).Start();
            
#endif
        }

        private void Connect()
        {
            if (!connected)
            {
                permission = new SocketPermission(NetworkAccess.Connect,
                           TransportType.Tcp, URL_MULTI, PORT_MULTI);
                ipHost = Dns.GetHostEntry(URL_MULTI);
                ipAddr = ipHost.AddressList[0];
                ipEndPoint = new IPEndPoint(ipAddr, PORT_MULTI);
                socket = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                socket.Connect(ipEndPoint);

                connected = true;
            }
        }
        protected void SendSocket(string text)
        {
            new Thread(() =>
            {
                Connect();

                byte[] msg = Encoding.UTF8.GetBytes(text);
                try
                {
                    socket.Send(msg);
                }
                catch (SocketException)
                {
                    connected = false;
                    Connect();
                    SendSocket(text);
                }
            }).Start();
        }

        protected void SocketListener(CommandResult commandResult)
        {
            new Thread(() =>
            {
                Connect();

                String theMessageToReceive = "";
                bool read = false;
                while (true)
                {
                    while (socket.Available > 0)
                    {
                        byte[] bytes = new byte[1024];
                        int bytesRec = socket.Receive(bytes);

                        theMessageToReceive += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                        read = true;
                    }
                    if (commandResult != null && read)
                    {
                        commandResult(theMessageToReceive);
                        theMessageToReceive = "";
                        read = false;
                    }
                }
            }).Start();
        }

        public void StopListening()
        {
            socket.Close();
        }

    }
    public class InvalidParameterException : System.Exception
    {
        public InvalidParameterException() {}
        public InvalidParameterException(string message) {}
        public InvalidParameterException(string message, System.Exception inner) {}

        /*protected InvalidParameterException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) {}*/
    }
}