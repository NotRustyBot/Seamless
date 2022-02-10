using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SFDGameScriptInterface;


namespace SFDScript
{

    public partial class GameScript : GameScriptInterface
    {

        /* CLASS STARTS HERE - COPY BELOW INTO THE SCRIPT WINDOW */

        static class Change
        {
            public static void Name(IObject item, string name)
            {
                int end = item.CustomID.Length;
                if (item.CustomID.IndexOf('(') != -1) end = item.CustomID.IndexOf('(');
                if (item.CustomID.IndexOf('#') != -1) end = item.CustomID.IndexOf('#');
                item.CustomID = name + item.CustomID.Substring(end);
            }

            public static void AddGroup(IObject item, string group)
            {
                List<string> groups = new List<string>(Fetch.Groups(item));
                groups.Add(group);
                Object(item, Fetch.Name(item), groups.ToArray(), Fetch.Parameters(item));
            }

            public static void RemoveGroup(IObject item, string group)
            {
                List<string> groups = new List<string>(Fetch.Groups(item));
                Object(item, Fetch.Name(item), groups.Where(s => !s.StartsWith(group)).ToArray(), Fetch.Parameters(item));
            }

            public static void Parameter(IObject item, string parameter, object value)
            {
                Dictionary<string, object> parameters = Fetch.Parameters(item);
                parameters[parameter] = value;
                Object(item, Fetch.Name(item), Fetch.Groups(item).ToArray(), parameters);
            }

            public static void RemoveParameter(IObject item, string parameter)
            {
                Dictionary<string, object> parameters = Fetch.Parameters(item);
                parameters.Remove(parameter);
                Object(item, Fetch.Name(item), Fetch.Groups(item).ToArray(), parameters);
            }

            public static string Object(IObject item, string name, string[] groups, Dictionary<string, object> parameters)
            {
                item.CustomID = Object(name, groups, parameters);
                return item.CustomID;
            }
            public static string Object(string name, string[] groups, Dictionary<string, object> parameters)
            {
                string cid = name;
                foreach (string group in groups)
                {
                    cid += "#" + group;
                }
                if (parameters.Count > 0)
                {
                    cid += "(";
                    foreach (KeyValuePair<string, object> pair in parameters)
                    {
                        cid += pair.Key + ":";
                        if (pair.Value is string)
                        {
                            cid += "\"" + pair.Value + "\"";
                        }
                        else
                        {
                            cid += pair.Value.ToString();
                        }
                        cid += ";";

                    }
                    cid += ")";

                }

                return cid;
            }
        }
        static class Fetch
        {
            public static string Name(IObject item)
            {
                int end = item.CustomID.Length;
                if (item.CustomID.IndexOf('(') != -1) end = item.CustomID.IndexOf('(');
                if (item.CustomID.IndexOf('#') != -1) end = item.CustomID.IndexOf('#');
                return item.CustomID.Substring(0, end);
            }
            public static string[] Groups(IObject item)
            {
                int end = item.CustomID.Length;
                if (item.CustomID.IndexOf('(') != -1) end = item.CustomID.IndexOf('(');
                string selector = item.CustomID.Substring(0, end);
                List<string> groups = new List<string>(selector.Split('#'));
                groups.RemoveAt(0);
                return groups.ToArray();
            }
            public static List<IObject> ByGroup(string name)
            {
                return ByGroup(name, Game.GetObjects<IObject>());
            }
            public static List<IObject> ByGroup(string name, IObject[] input)
            {
                List<IObject> output = new List<IObject>();
                foreach (IObject item in input)
                {
                    if (Groups(item).Contains(name))
                    {
                        output.Add(item);
                    }
                }
                return output;
            }
            public static List<IObject> ByGroup(string name, List<IObject> input)
            {
                return ByGroup(name, input.ToArray());
            }

            public static IObject ByName(string name)
            {
                return ByName(name, Game.GetObjects<IObject>());
            }

            public static IObject ByName(string name, List<IObject> input)
            {
                return ByName(name, input.ToArray());
            }
            public static IObject ByName(string name, IObject[] input)
            {
                foreach (IObject item in input)
                {
                    if (Name(item) == name)
                    {
                        return item;
                    }
                }
                return null;
            }

            public static Dictionary<string, object> Parameters(IObject item)
            {
                return Parameters(item.CustomID);
            }

            public static Dictionary<string, object> Parameters(string parameters)
            {
                Dictionary<string, object> pairs = new Dictionary<string, object>();
                if (parameters.IndexOf('(') == -1) return pairs;
                int start = parameters.IndexOf('(') + 1;
                int end = parameters.IndexOf(')');
                parameters = parameters.Substring(start, end - start);
                string[] keyvals = parameters.Split(';');
                foreach (string keyval in keyvals)
                {
                    if (keyval.Length == 0) continue;
                    string key = keyval.Split(':')[0];
                    string val = keyval.Split(':')[1];

                    if (val.StartsWith("\"") && val.EndsWith("\""))
                    {
                        pairs.Add(key, val.Substring(1, val.Length - 2));
                    }
                    else if (val.ToLower() == "true")
                    {
                        pairs.Add(key, true);
                    }
                    else if (val.ToLower() == "false")
                    {
                        pairs.Add(key, false);
                    }
                    else
                    {
                        float output;
                        if (float.TryParse(val, out output))
                        {
                            pairs.Add(key, output);
                        }
                        else
                        {
                            pairs.Add(key, val);
                        }
                    }
                }

                return pairs;
            }
        }


        /* CLASS ENDS HERE - COPY ABOVE INTO THE SCRIPT WINDOW */

    }
}
