using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace Huffy.Utilities
{
    [XmlRoot("Root")]
    public class Container
    {
        [XmlArray("Container"), XmlArrayItem("Entry")]
        public List<ContainerData> data = new List<ContainerData>();

        public static string ClientAssetPath(string name)
        {
#if UNITY_EDITOR
            return Application.dataPath + "/../Config/" + name;
#else
			return Application.dataPath + "/../Config/" + name;
#endif
        }

        public static Container Load(string name)
        {
            var serializer = new XmlSerializer(typeof(Container));
            using (var stream = new StreamReader(ClientAssetPath(name + ".xml")))
            {
                return serializer.Deserialize(stream) as Container;
            }
        }

        public string DebugString()
        {
            return DebugString(-1);
        }

        public string DebugString(int _entry)
        {
            string s = "";
            for (int i = 0; i < this.data.Count; i++)
            {
                if (_entry < 0 || i == _entry)
                {
                    s += "\n\nEntry: " + i;
                    s += "\ntestEnum: " + (int)this.data[i].testEnum;
                    s += "\nfloatValue: " + this.data[i].floatValue;
                    s += "\nstringValue: " + this.data[i].stringValue;

                    foreach (string t in this.data[i].Text)
                        s += "\nText: " + t;
                }
            }
            return s;
        }

        public void PrintString()
        {
            string s = "";
            s += "////////////////////////////////////////////////////////////////////////////////\n";
            s += "// XML: " + this.data.Count + " //\n";
            s += "////////////////////////////////////////////////////////////////////////////////";

            s += DebugString();

            s += "\n////////////////////////////////////////////////////////////////////////////////\n";
            UIDebug.Log(s);
        }
    }
}