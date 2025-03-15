using System;

namespace MSL.model
{
    [Serializable]
    public class MslConfig
    {
        public string ServerURL { get; set; } = "127.0.0.1";
        public bool IsServerEnabled { get; set; } = true;
    }
}