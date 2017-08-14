﻿using System;
using System.Collections.Generic;

namespace Entoarox.Framework.Core
{
    class InterModHelper : IInterModHelper
    {
        private static Dictionary<string, InterModHelper> _Cache = new Dictionary<string, InterModHelper>();
        private static Dictionary<string, List<ReceiveMessage>> _Map = new Dictionary<string, List<ReceiveMessage>>();
        internal static IInterModHelper Get(string modID)
        {
            if (!_Cache.ContainsKey(modID))
                _Cache.Add(modID, new InterModHelper(modID));
            return _Cache[modID];
        }

        private string ModID;
        internal InterModHelper(string modID) => ModID=modID;
        public event EventHandler<EventArgsMessageReceived> MessageReceived;
        
        public void Subscribe(string channel, ReceiveMessage handler)
        {
            if (!_Map.ContainsKey(channel))
                _Map.Add(channel, new List<ReceiveMessage>());
            _Map[channel].Add(handler);
        }
        public void Publish(string channel, string message)
        {
            if (_Map.ContainsKey(channel))
                foreach (ReceiveMessage handler in _Map[channel])
                    handler(ModID, channel, message, false);
        }
    }
}