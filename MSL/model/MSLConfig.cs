using System;
using UnityEngine;

namespace MSL.model
{
    [Serializable]
    public class MSLConfig
    {
        public string ServerURL { get; set; } = "127.0.0.1";
        public bool IsServerEnabled { get; set; } = true;
    }
}