using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using fastJSON;

namespace MSL
{
    public class EmbeddedServer
    {
        private HttpListener _listener;
        private Thread _serverThread;
        private readonly string _serverIP;
        private const int ServerPort = 5000;

        private readonly Dictionary<string, CityData> _cityData = new Dictionary<string, CityData>();

        public EmbeddedServer(string ip)
        {
            _serverIP = ip;
        }

        public void Start()
        {
            try
            {
                var url = $"http://{_serverIP}:{ServerPort}/";

                _listener = new HttpListener();
                _listener.Prefixes.Add(url);

                _serverThread = new Thread(Run);
                _serverThread.Start();
                MslLogger.Log($"🌍 Server start on {url}");
            }
            catch (Exception ex)
            {
                MslLogger.Log($"❌ Unable to launch de server : {ex.Message}");
            }
        }

        private void Run()
        {
            _listener = new HttpListener();

            _listener.Prefixes.Add("http://127.0.0.1:5000/");
            _listener.Prefixes.Add("http://localhost:5000/");
            _listener.Prefixes.Add("http://+:5000/");

            _listener.Start();

            while (true)
            {
                var context = _listener.GetContext();
                var request = context.Request;
                var response = context.Response;
                response.ContentType = "application/json";
                var clientIP = request.RemoteEndPoint?.Address.ToString() ?? "Unknown";
                switch (request.HttpMethod)
                {
                    case "POST" when request.Url.AbsolutePath == "/api/cityData/update":
                    {
                        string json;
                        using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
                        {
                            json = reader.ReadToEnd();
                        }

                        var data = JSON.ToObject<CityData>(json);
                        _cityData[data.CityName] = data;

                        response.StatusCode = (int)HttpStatusCode.OK;

                        MslLogger.Log($"✅ Received POST request ({clientIP})");
                        break;
                    }
                    case "GET" when request.Url.AbsolutePath == "/api/cityData/all":
                    {
                        var jsonResponse = JSON.ToJSON(_cityData);
                        var buffer = Encoding.UTF8.GetBytes(jsonResponse);
                        response.OutputStream.Write(buffer, 0, buffer.Length);
                        MslLogger.Log($"✅ Received GET request ({clientIP})");
                        break;
                    }
                    default:
                        MslLogger.Log($"⚠️ Received unhandled request ({clientIP})");
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                }

                response.Close();
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _serverThread.Abort();
            MslLogger.Log("⛔ Server stopped");
        }
    }
}