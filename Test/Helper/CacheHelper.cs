using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace Test.Helper
{
    public class CacheHelper
    {
        private readonly Lazy<ConnectionMultiplexer> LazyConnection;
        public ConnectionMultiplexer Connection => LazyConnection.Value;

        public CacheHelper()
        {
            LazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                string cacheConnection = "localhost";
                return ConnectionMultiplexer.Connect(cacheConnection);
            });
        }

        public T GetMemCache<T>(string key)
        {
            var db = LazyConnection.Value.GetDatabase();
            var value = db.StringGet(key);

            if (!value.HasValue)
                return default(T);
            else
            {
                return JsonConvert.DeserializeObject<T>(value
                        , new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects
                        });
            }
        }


        public void UpdateMemCache(string key, object value, DateTimeOffset? expiredDate = null)
        {
            var db = LazyConnection.Value.GetDatabase();
            db.StringSet(key, JsonConvert.SerializeObject(value
                , Formatting.Indented
                        , new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                            PreserveReferencesHandling = PreserveReferencesHandling.Objects
                        }));
        }


        public void RemoveMemchace(string key)
        {
            var db = LazyConnection.Value.GetDatabase();
            db.KeyDelete(key);
        }
        
    }
}
