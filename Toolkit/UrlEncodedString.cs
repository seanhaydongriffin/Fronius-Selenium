using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Toolkit
{
    public static class UrlEncodedString
    {

        public static JObject ToJson(String queryString)
        {
            var parsed = HttpUtility.ParseQueryString(queryString);
            var ww = parsed.AllKeys.ToDictionary(k => k, k => parsed[k]);

            JObject json = new JObject();

            foreach (var setting in ww)
            {
                var setting_value = setting.Value;

                // check for merged values for duplicate keys, and if so only take the first value

                if (setting.Value.Contains(','))
                {
                    if (Regex.Matches("&" + queryString, "&" + setting.Key).Count > 1)
                    {
                        setting_value = setting.Value.Split(',')[0];
                    }
                }


                // if an array

                if (Regex.IsMatch(setting.Key, "\\[\\d+\\]"))
                {
                    var array_index = Regex.Matches(setting.Key, "\\[(\\d+)\\]")[0].Groups[1].Value.ToInt();
                    var key_part = Regex.Split(setting.Key, "\\[\\d+\\].");

                    if (json[key_part[0]] == null)

                        json.Add(new JProperty(key_part[0], new JArray()));

                    if (json[key_part[0]] != null)
                    {
                        if (((JArray)json[key_part[0]]).Count <= array_index)
                        {
                            var json_arr_obj = new JObject(new JProperty(key_part[1], setting_value));
                            ((JArray)json[key_part[0]]).Add(json_arr_obj);
                        }
                        else

                            ((JObject)json[key_part[0]][array_index]).Add(new JProperty(key_part[1], setting_value));
                    }
                }
                else

                    // not an array

                    json.Add(setting.Key, setting_value);
            }

            return json;
        }


        public static JObject ToJson2(String queryString)
        {
            var parsed = HttpUtility.ParseQueryString(queryString);
            var ww = parsed.AllKeys.ToDictionary(k => k, k => parsed[k]);

            JObject json = new JObject();

            foreach (var setting in ww)
            {
                var setting_value = setting.Value;

                // check for merged values for duplicate keys, and if so only take the first value

                if (setting.Value.Contains(','))
                {
                    if (Regex.Matches("&" + queryString, "&" + setting.Key).Count > 1)
                    {
                        setting_value = setting.Value.Split(',')[0];
                    }
                }

                JContainer current_container = json;
                int array_index = 0;

                if (setting.Key.Contains('.'))
                {
                    var setting_keys = setting.Key.Split('.');

                    foreach (var setting_key in setting_keys)
                    {

                        // if an array

                        if (Regex.IsMatch(setting_key, "\\[\\d+\\]"))
                        {
                            var parent_array_index = array_index;
                            array_index = Regex.Matches(setting_key, "\\[(\\d+)\\]")[0].Groups[1].Value.ToInt();
                            var setting_key2 = setting_key.Substring(0, setting_key.IndexOf('['));

                            //if (!((JObject)current_container).ContainsKey(setting_key2))
                            if ((current_container.GetType() == typeof(JObject) && !((JObject)current_container).ContainsKey(setting_key2)) ||
                                (current_container.GetType() == typeof(JArray) && (current_container.Count <= parent_array_index || current_container[parent_array_index][setting_key2] == null)))

                            {

                                // Add the array

                                var new_container = new JArray();

                                if (current_container.GetType() == typeof(JArray))
                                {
                                    //current_container.Add(new JObject(new JProperty(setting_key2, new_container)));


                                    if (current_container.Count <= array_index)

                                        current_container.Add(new JObject(new JProperty(setting_key2, new_container)));
                                    else

                                        ((JObject)current_container[parent_array_index]).Add(new JProperty(setting_key2, new_container));






                                }
                                else
                                    current_container.Add(new JProperty(setting_key2, new_container));

                                current_container = new_container;
                            }
                            else

                                if (current_container.GetType() == typeof(JArray))

                                    current_container = (JContainer)current_container[parent_array_index][setting_key2];
                                else

                                    current_container = (JContainer)current_container[setting_key2];


                        }
                        else

                            //if (current_container.Where(v => v[setting_key] != null) != null)

                            //    if (current_container[setting_key] == null)
                            //if ((current_container.GetType() == typeof(JObject) && !((JObject)current_container).ContainsKey(setting_key)) ||
                            //    (current_container.GetType() == typeof(JArray) && current_container.Where(v => v[setting_key] != null) != null))
                            if ((current_container.GetType() == typeof(JObject) && !((JObject)current_container).ContainsKey(setting_key)) ||
                                (current_container.GetType() == typeof(JArray) && (current_container.Count <= array_index || current_container[array_index][setting_key] == null)))

                            {
                                // Add the non-array

                                if (setting_key.Equals(setting_keys.Last()))
                                {
                                    if (current_container.GetType() == typeof(JArray))
                                    {
                                        if (current_container.Count <= array_index)

                                            current_container.Add(new JObject(new JProperty(setting_key, setting_value)));
                                        else

                                            ((JObject)current_container[array_index]).Add(new JProperty(setting_key, setting_value));
                                    } else

                                        ((JObject)current_container).Add(setting_key, setting_value);


                                }
                                else
                                {
                                    var new_container = new JObject();

                                    if (current_container.GetType() == typeof(JArray))
                                    {
                                        if (current_container.Count <= array_index)

                                            current_container.Add(new JObject(new JProperty(setting_key, new_container)));
                                        else

                                            ((JObject)current_container[array_index]).Add(new JProperty(setting_key, new_container));
                                    }
                                    else

                                        current_container.Add(new JProperty(setting_key, new_container));


                                    current_container = new_container;
                                }
                            }
                            else

                                if (current_container.GetType() == typeof(JArray))

                                    current_container = (JContainer)current_container[array_index][setting_key];
                                else

                                    current_container = (JContainer)current_container[setting_key];


                    }
                } else

                    // not an array

                    json.Add(setting.Key, setting_value);
/*
                // if an object

                if (Regex.IsMatch(setting.Key, "\\."))
                {
                    var setting_key = setting.Key.Split('.');

                    if (current_container[setting_key[0]] == null)
                    {
                        var new_container = new JProperty(setting_key[0], new JObject());
                        current_container.Add(new_container);
                        current_container = new_container;
                    }
                }

                // if an array

                if (Regex.IsMatch(setting.Key, "\\[\\d+\\]"))
                {
                    var first_array_index = Regex.Matches(setting.Key, "\\[(\\d+)\\]")[0].Groups[1].Value.ToInt();
                    int second_array_index = 0;
                    var key_part = Regex.Split(setting.Key, "\\[\\d+\\].");

                    // second array

                    if (key_part.Length > 2 && ((JArray)json[key_part[0]]).Where(v => v[key_part[1]] != null) != null)
                    {
                        second_array_index = Regex.Matches(setting.Key, "\\[(\\d+)\\]")[0].Groups[2].Value.ToInt();

                        if (((JArray)json[key_part[0]]).Count <= first_array_index)
                        {
                            var json_arr_obj = new JObject(new JProperty(key_part[1], new JArray()));
                            ((JArray)json[key_part[0]]).Add(json_arr_obj);
                        }
                        else

                            ((JObject)json[key_part[0]][first_array_index]).Add(new JProperty(key_part[1], new JArray()));
                    }

                    if (key_part.Length > 2)
                    {

                        if (((JArray)json[key_part[0]][key_part[1]]).Count <= second_array_index)
                        {
                            var json_arr_obj = new JObject(new JProperty(key_part[2], setting_value));
                            ((JArray)json[key_part[0]][key_part[1]]).Add(json_arr_obj);
                        }
                        else

                            ((JObject)json[key_part[0]][first_array_index]).Add(new JProperty(key_part[1], setting_value));

                    }
                    else
                    {
                        if (json[key_part[0]] == null)

                            json.Add(new JProperty(key_part[0], new JArray()));

                        if (json[key_part[0]] != null)
                        {
                            if (((JArray)json[key_part[0]]).Count <= first_array_index)
                            {
                                var json_arr_obj = new JObject(new JProperty(key_part[1], setting_value));
                                ((JArray)json[key_part[0]]).Add(json_arr_obj);
                            }
                            else

                                ((JObject)json[key_part[0]][first_array_index]).Add(new JProperty(key_part[1], setting_value));
                        }
                    }
                }
                else
                {


                    // not an array

                    json.Add(setting.Key, setting_value);
                }
*/
            }

            return json;
        }


        // the following works for two types (standards) of arrays in URL Encoded Strings.
        //  first array type is key[0]=value notation.
        //  second array type is key=value1&key=value2&key=valuen notation.
        public static JObject ToJson3(String queryString)
        {

            // query string to dictionary

            var settings = new Dictionary<string, List<string>>();
            string[] queryStringSections = queryString.Split('&');
            foreach (var section in queryStringSections)
            {
                var sides = section.Split('=');
                var key = WebUtility.UrlDecode(sides[0]);
                var value = sides[1];
                var values2 = value.Split(',');
                var decodedValues = values2.Select(x => WebUtility.UrlDecode(x));

                if (!settings.ContainsKey(key))
                {
                    settings.Add(key, new List<string>());
                }

                settings[key].AddRange(decodedValues);
            }




            var parsed = HttpUtility.ParseQueryString(queryString);
            var ww = parsed.AllKeys.ToDictionary(k => k, k => parsed[k]);

            JObject json = new JObject();

            foreach (var setting in settings)
            {
                //var setting_value = setting.Value.FirstOrDefault();

                JToken setting_value = null;

                if (setting.Value.Count <= 1)

                    // not an array
                    setting_value = setting.Value.FirstOrDefault();
                else

                    // an array
                    setting_value = JArray.FromObject(setting.Value);



                // check for merged values for duplicate keys, and if so only take the first value

                //if (setting.Value.Contains(','))
                //{
                //    if (Regex.Matches("&" + queryString, "&" + setting.Key).Count > 1)
                //    {
                //        setting_value = setting.Value.Split(',')[0];
                //    }
                //}

                JContainer current_container = json;
                int array_index = 0;

                if (setting.Key.Contains('.'))
                {
                    var setting_keys = setting.Key.Split('.');

                    foreach (var setting_key in setting_keys)
                    {

                        // if an array

                        if (Regex.IsMatch(setting_key, "\\[\\d+\\]"))
                        {
                            var parent_array_index = array_index;
                            array_index = Regex.Matches(setting_key, "\\[(\\d+)\\]")[0].Groups[1].Value.ToInt();
                            var setting_key2 = setting_key.Substring(0, setting_key.IndexOf('['));

                            if ((current_container.GetType() == typeof(JObject) && !((JObject)current_container).ContainsKey(setting_key2)) ||
                                (current_container.GetType() == typeof(JArray) && (current_container.Count <= parent_array_index || current_container[parent_array_index][setting_key2] == null)))

                            {
                                // Add the array

                                var new_container = new JArray();

                                if (current_container.GetType() == typeof(JArray))
                                {
                                    //current_container.Add(new JObject(new JProperty(setting_key2, new_container)));


                                    if (current_container.Count <= array_index)

                                        current_container.Add(new JObject(new JProperty(setting_key2, new_container)));
                                    else

                                        ((JObject)current_container[parent_array_index]).Add(new JProperty(setting_key2, new_container));
                                }
                                else
                                    current_container.Add(new JProperty(setting_key2, new_container));

                                current_container = new_container;
                            }
                            else

                                if (current_container.GetType() == typeof(JArray))

                                current_container = (JContainer)current_container[parent_array_index][setting_key2];
                            else

                                current_container = (JContainer)current_container[setting_key2];
                        }
                        else

                            if ((current_container.GetType() == typeof(JObject) && !((JObject)current_container).ContainsKey(setting_key)) ||
                                (current_container.GetType() == typeof(JArray) && (current_container.Count <= array_index || current_container[array_index][setting_key] == null)))
                            {
                                // Add the non-array

                                if (setting_key.Equals(setting_keys.Last()))
                                {
                                    if (current_container.GetType() == typeof(JArray))
                                    {
                                        if (current_container.Count <= array_index)

                                            current_container.Add(new JObject(new JProperty(setting_key, setting_value)));
                                        else

                                            ((JObject)current_container[array_index]).Add(new JProperty(setting_key, setting_value));
                                    }
                                    else

                                        ((JObject)current_container).Add(setting_key, setting_value);
                                }
                                else
                                {
                                    var new_container = new JObject();

                                    if (current_container.GetType() == typeof(JArray))
                                    {
                                        if (current_container.Count <= array_index)

                                            current_container.Add(new JObject(new JProperty(setting_key, new_container)));
                                        else

                                            ((JObject)current_container[array_index]).Add(new JProperty(setting_key, new_container));
                                    }
                                    else

                                        current_container.Add(new JProperty(setting_key, new_container));

                                    current_container = new_container;
                                }
                            }
                            else

                                if (current_container.GetType() == typeof(JArray))

                                    current_container = (JContainer)current_container[array_index][setting_key];
                                else

                                    current_container = (JContainer)current_container[setting_key];

                    }
                }
                else
                {
                    // if not an array

                    json.Add(setting.Key, setting_value);
                }
            }

            return json;
        }







    }
}
