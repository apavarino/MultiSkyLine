using System;
using System.Collections.Generic;
using System.Net;

namespace MSL.server
{
    public abstract class Routing
    {
        private readonly Dictionary<RouteKey, Action<HttpListenerRequest, HttpListenerResponse>> _routes;
        
        protected Routing(Dictionary<RouteKey, Action<HttpListenerRequest, HttpListenerResponse>> route)
        {
            _routes = route;
        }
        
        public Dictionary<RouteKey, Action<HttpListenerRequest, HttpListenerResponse>> Route()
        {
            return _routes;
        }
    }
}