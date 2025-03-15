using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using fastJSON;
using MSL.model;

namespace MSL.server.rest
{
    public class CityDataRoute : Routing
    {
        public CityDataRoute() : base(new Dictionary<RouteKey, Action<HttpListenerRequest, HttpListenerResponse>> {
            { new RouteKey("POST", "/api/cityData/update"), UpdateCityDataRoute },
            { new RouteKey("GET", "/api/cityData/all"), FetchAllCityDataRoute },
            { new RouteKey("GET", "/api/cityData/contracts/open"), FetchOpenContractRoute },
        }){}

        private static void UpdateCityDataRoute(HttpListenerRequest request, HttpListenerResponse response)
        {
            string json;
            using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
            {
                json = reader.ReadToEnd();
            }
            var data = JSON.ToObject<CityData>(json);
            EmbeddedServer.CityDataRepository.UpdateOneByCityName(data.CityName,data);
            response.StatusCode = (int)HttpStatusCode.OK;
        }
        
        private static void FetchAllCityDataRoute(HttpListenerRequest request, HttpListenerResponse response)
        {
            var jsonResponse = JSON.ToJSON(EmbeddedServer.CityDataRepository.FindAll());
            var buffer = Encoding.UTF8.GetBytes(jsonResponse);
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
        
        
        private static void FetchOpenContractRoute(HttpListenerRequest request, HttpListenerResponse response)
        {
            var jsonResponse = JSON.ToJSON(EmbeddedServer.CityDataRepository.FindAllOpenContracts());
            var buffer = Encoding.UTF8.GetBytes(jsonResponse);
            response.OutputStream.Write(buffer, 0, buffer.Length);
        }
    }
}