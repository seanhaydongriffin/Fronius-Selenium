using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Toolkit
{
    public static class JSON
    {

        public static dynamic Deserialize<T>(String json)
        {
            dynamic result = null;

            try
            {
                result = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
            }

            return result;
        }

        public static dynamic Deserialize(String json)
        {
            dynamic result = null;

            try
            {
                result = JsonConvert.DeserializeObject(json);
            }
            catch (Exception e)
            {
            }

            return result;
        }

        public static dynamic DeserializeLineDelimited(String json)
        {
            var json_list = new List<JToken>();

            var jsonReader = new JsonTextReader(new StringReader(json))
            {
                SupportMultipleContent = true // This is important!
            };

            var jsonSerializer = new JsonSerializer();
            while (jsonReader.Read())
            {
                json_list.Add((JToken)jsonSerializer.Deserialize(jsonReader));
            }

            return json_list;
        }

        public static dynamic FromFile<T>(string path)
        {
            var JsonStr = File.read(path);
            var JsonObj = Deserialize<T>(JsonStr);
            return JsonObj;
        }

        public static void ToFile(string path, dynamic JsonObj)
        {
            var JsonStr = ToJSONString(JsonObj);
            Toolkit.File.overwrite(path, JsonStr);
        }

        public static string ToJSONString<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string ToJSONString(dynamic obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static JObject Parse(String json)
        {
            JObject obj = null;

            try
            {
                JObject.Parse(json);
            }
            catch (Exception e)
            {
                int i = 0;
            }

            return obj;
        }

        public static int ArrayCount(dynamic json_array)
        {
            JArray json_array_items = (JArray)json_array;
            int num_items = json_array_items.Count;
            return num_items;
        }

    }
}
