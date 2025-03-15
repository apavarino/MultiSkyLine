using System;
using UnityEngine;

namespace MSL.model
{
    [Serializable]
    public class MSLConfig
    {
        private string _serverURL = "127.0.0.1";
        
        public string ServerURL {
            get
            {
                MslLogger.LogWarn("Getting ServerURL");
                return _serverURL;
            }
            set
            {
                MslLogger.LogWarn("Setting ServerUrl to " + value);
                _serverURL = value;
            }
        }
        
        private bool _isServerEnabled = true;

        public bool IsServerEnabled
        {
            get
            {
                MslLogger.LogWarn("Getting IsServerEnabled");
                return _isServerEnabled;
            }
            set
            {
                MslLogger.LogWarn("Setting IsServerEnabled to " + value);
                _isServerEnabled = value;
            }
        }
    }
}