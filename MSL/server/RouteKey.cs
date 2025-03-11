using System;

namespace MSL.server
{
    public class RouteKey : IEquatable<RouteKey>
    {
        public string HttpMethod { get; }
        public string Path { get; }

        public RouteKey(string httpMethod, string path)
        {
            HttpMethod = httpMethod;
            Path = path.ToLower();
        }
        
        public bool Equals(RouteKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return HttpMethod == other.HttpMethod && Path == other.Path;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RouteKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((HttpMethod != null ? HttpMethod.GetHashCode() : 0) * 397) ^ (Path != null ? Path.GetHashCode() : 0);
            }
        }
    }
}