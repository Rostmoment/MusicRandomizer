using HarmonyLib;
using MTM101BaldAPI.AssetTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine.Profiling.Memory.Experimental;

namespace RandomMusic
{
    public class CustomSongData
    {
        public string OriginalName { get; set; }
        public string[] NewDatas { get; set; }
        public static bool Exists(string name)
        {
            return BasePlugin.midis.Exists(element => element.OriginalName == name);
        }
        public static bool CanBeUsed(string name)
        {
            if (!Exists(name)) return false;
            if (GetByName(name).NewDatas.Length == 0) return false;
            return true;
        }
        public static CustomSongData GetByName(string name)
        {
            return BasePlugin.midis.Find(element => element.OriginalName == name);
        }
        public static void CreateSongData(string songName)
        {
            string[] names = Directory.GetFiles(BasePlugin.path + songName);
            if (names.Length == 0)
            {
                UnityEngine.Debug.Log("Folder " + songName + " is empty!");
                return;
            }
            string[] data = new string[] { };
            foreach (string name in names)
            {
                if (name.EndsWith(".mid"))
                {
                    UnityEngine.Debug.Log(name);
                    data = data.AddToArray(AssetLoader.MidiFromFile(name, Path.GetFileNameWithoutExtension(name)));
                }
            }
            BasePlugin.midis.Add(new CustomSongData
            {
                OriginalName = songName,
                NewDatas = data
            });
        }
    }
}
