using System.Collections.Generic;

namespace GDLib.State
{
    public class Blackboard
    {
        private readonly Dictionary<string, object> blackboard = new Dictionary<string, object>();

        public object this[string key]
        {
            private get => blackboard[key]; // Risk of null reference
            set => blackboard[key] = value;
        }

        public bool ContainsKey(string key) => blackboard.ContainsKey(key);

        public bool TryGetValue<T>(string key, out T var)
        {
            var = default;

            if (!blackboard.ContainsKey(key))
                return false;

            if (blackboard[key] is T t)
            {
                var = t;
                return true;
            }

            return false;
        }
    }

}