using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using MSL.model.repository;

namespace MSL.server
{
    public class EmbeddedServer
    {
        private HttpListener _listener;
        private Thread _serverThread;
        private readonly string _serverIP;
        private const int ServerPort = 5000;
        private readonly Dictionary<RouteKey, Action<HttpListenerRequest, HttpListenerResponse>> _routes = 
            new Dictionary<RouteKey, Action<HttpListenerRequest, HttpListenerResponse>>();
        
        public static readonly CityDataRepository CityDataRepository = new CityDataRepository(null);

        public EmbeddedServer(string ip)
        {
            _serverIP = ip;
        }

        public void Start()
        {
            try
            {
                var url = $"http://{_serverIP}:{ServerPort}/";
                RegisterRoutes();
                _listener = new HttpListener();
                _listener.Prefixes.Add(url);
                _serverThread = new Thread(Run);
                _serverThread.Start();
                MslLogger.LogServer($"Server start on {url}");
            }
            catch (Exception ex)
            {
                MslLogger.LogError($"Unable to launch de server : {ex.Message}");
            }
        }

        private void RegisterRoutes()
        {
            var routingTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(Routing).IsAssignableFrom(t));

            foreach (var type in routingTypes)
            {
                if (!(Activator.CreateInstance(type) is Routing instance)) continue;
                foreach (var route in instance.Route())
                {
                    _routes[route.Key] = route.Value;
                    MslLogger.LogServer($"Registered route: {route.Key.HttpMethod} {route.Key.Path}");
                }
            }
        }
        
        private void Run()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://127.0.0.1:5000/");
            _listener.Prefixes.Add("http://localhost:5000/");
            _listener.Prefixes.Add("http://+:5000/");
            _listener.Start();
            MslLogger.LogServer("Server is running and listening for requests...");

            while (true)
            {
                var context = _listener.GetContext();
                var request = context.Request;
                var response = context.Response;
                response.ContentType = "application/json";
                var clientIP = request.RemoteEndPoint?.Address.ToString() ?? "Unknown";
                var routeKey = new RouteKey(request.HttpMethod, request.Url.AbsolutePath);

                if (_routes.TryGetValue(routeKey, out var action))
                {
                    action(request, response);
                    MslLogger.LogServer($"Handled request ({clientIP}): {routeKey.HttpMethod} {routeKey.Path}");
                }
                else
                {
                    MslLogger.LogServer($"Unhandled request ({clientIP}): {routeKey.HttpMethod} {routeKey.Path}");
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                response.Close();
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _serverThread.Abort();
            MslLogger.LogServer("Server stopped");
        }
    }
}