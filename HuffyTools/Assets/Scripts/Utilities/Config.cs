using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace Huffy.Utilities
{
    public enum CONFIG_KEYS
    {
        vsync,
        pixellightcount,
        aa,
        af,
        blendweights,
        screenwidth,
        screenheight,
        fullscreen,
        refreshrate,
        debug,
    }

    public class Config : SingletonBehaviour<Config>
    {
        private Dictionary<string, string> configData = new Dictionary<string, string>();
        
        public static string Read(CONFIG_KEYS _key)
        {
            return Instance.configData[_key.ToString()];
        }

        public static bool HasKey(CONFIG_KEYS _key)
        {
            if (Instance.configData.ContainsKey(_key.ToString()))
                return true;
            else
                return false;
        }

        public static void Initialize()
        {
            Instance.DontDestroy();
        }

        void Awake()
        {
            DontDestroy();
        }

        IEnumerator Start()
        {
            configData = GetConfigData();
            yield return null;
            //DebugSystemInformation();
            ApplyQualitySettings();
        }

        private Dictionary<string, string> GetConfigData()
        {
            Dictionary<string, string> configData = new Dictionary<string, string>();

            string line;
            StreamReader inStream;
            
            if (!File.Exists(Application.dataPath + "/../Config/config.txt"))
                File.Create(Application.dataPath + "/../Config/config.txt");

            inStream = new StreamReader(Application.dataPath + "/../Config/config.txt");

            while ((line = inStream.ReadLine()) != null)
            {
                line = line.Replace(" ", "");

                if (line.Length > 0 && string.Compare(line[0].ToString(), "/") != 0)
                {
                    string[] words = line.Split('=', '/');
                    if (words.Length > 1)
                    {
                        string key = words[0];
                        string value = words[1];
                        configData.Add(key.ToLower(), value);
                    }
                }
            }
            inStream.Close();

            return configData;
        }

        private void DebugSystemInformation()
        {
            string s = "";
            s += "////////////////////////////////////////////////////////////////////////////////\n";
            s += "// System Info: //\n";
            s += "////////////////////////////////////////////////////////////////////////////////\n";

            s += "\nTime: " + DateTime.Now.ToString("MM-dd-yy_HHmm");
            s += "\nMachineName: " + System.Environment.MachineName;
            s += "\nUserName: " + System.Environment.UserName;
            s += "\nUserDomainName: " + System.Environment.UserDomainName;
            s += "\nOSVersion: " + System.Environment.OSVersion;

            s += "\n////////////////////////////////////////////////////////////////////////////////\n";
            Debug.Log(s);
        }

        private void DebugQulitySettings()
        {
            string s = "";
            s += "////////////////////////////////////////////////////////////////////////////////\n";
            s += "// QualitySettings: //\n";
            s += "////////////////////////////////////////////////////////////////////////////////\n";
            
            s += "\nvSyncCount: " + QualitySettings.vSyncCount;
            s += "\npixelLightCount: " + QualitySettings.pixelLightCount;
            s += "\nantiAliasing: " + QualitySettings.antiAliasing;
            s += "\nanisotropicFiltering: " + QualitySettings.anisotropicFiltering;
            s += "\nblendWeights: " + QualitySettings.blendWeights;
            s += "\nresolution : " + Screen.width + "x" + Screen.height;
            s += "\nfullScreen: " + Screen.fullScreen;
            s += "\nrefreshRate: " + Screen.currentResolution.refreshRate;

            s += "\n////////////////////////////////////////////////////////////////////////////////\n";
            Debug.Log(s);
        }

        private void ApplyQualitySettings()
        {
            // vsync
            if (HasKey(CONFIG_KEYS.vsync))
                QualitySettings.vSyncCount = int.Parse(Read(CONFIG_KEYS.vsync));// Config.configData[CONFIG_KEYS.vsync.ToString()]);

            // pixel light count
            if (HasKey(CONFIG_KEYS.pixellightcount))
                QualitySettings.pixelLightCount = int.Parse(Read(CONFIG_KEYS.pixellightcount));

            // aa
            if (HasKey(CONFIG_KEYS.aa))
                QualitySettings.antiAliasing = int.Parse(Read(CONFIG_KEYS.aa));

            // af
            if (HasKey(CONFIG_KEYS.af))
            {
                int tmp = int.Parse(Read(CONFIG_KEYS.af));
                if (Enum.IsDefined(typeof(AnisotropicFiltering), tmp))
                {
                    QualitySettings.anisotropicFiltering = (AnisotropicFiltering)tmp;
                }
            }

            // blend Weights
            if (HasKey(CONFIG_KEYS.blendweights))
            {
                int tmp = int.Parse(Read(CONFIG_KEYS.blendweights));
                if (Enum.IsDefined(typeof(BlendWeights), tmp))
                {
                    QualitySettings.blendWeights = (BlendWeights)tmp;
                }
            }

            // set resolution
            int width = Screen.width;
            int height = Screen.height;
            if (HasKey(CONFIG_KEYS.screenwidth) && HasKey(CONFIG_KEYS.screenheight))
            {
                width = int.Parse(Read(CONFIG_KEYS.screenwidth));
                height = int.Parse(Read(CONFIG_KEYS.screenheight));
            }

            bool fullScreen = true;
            if (HasKey(CONFIG_KEYS.fullscreen))
                fullScreen = bool.Parse(Read(CONFIG_KEYS.fullscreen));

            int refreshRate = Screen.currentResolution.refreshRate;
            if (HasKey(CONFIG_KEYS.refreshrate))
                refreshRate = int.Parse(Read(CONFIG_KEYS.refreshrate));

            Screen.SetResolution(width, height, fullScreen, refreshRate);
            
            // debug settings
            //DebugQulitySettings();
        }
    }
}