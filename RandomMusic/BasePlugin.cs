using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.AssetTools;
using UnityEngine;

namespace RandomMusic
{
	[HarmonyPatch(typeof(MusicManager))]
	internal class MidiPlayerPatch
	{
		[HarmonyPatch("PlayMidi")]
		[HarmonyPrefix]
		private static void PlayMidiPatch(ref string song)
        {
			if (Directory.Exists(BasePlugin.path + song))
			{
				if (!CustomSongData.Exists(song))
				{
					CustomSongData.CreateSongData(song);
				}
				CustomSongData songData = CustomSongData.GetByName(song);
				try
				{
					if (songData.NewDatas.Length > 0)
					{
						int id = 0;
						if (songData.NewDatas.Length > 1)
						{
							id = UnityEngine.Random.Range(0, songData.NewDatas.Length - 1);
						}
						song = songData.NewDatas[id];
					}
				}
				catch (NullReferenceException) { }
			}
			else
			{
				Directory.CreateDirectory(BasePlugin.path + song);
			}
		}
    }

	[BepInPlugin("rost.moment.baldiplus.randommusic", "Random Music","1.0")]
	public class BasePlugin : BaseUnityPlugin
	{
		void Awake()
		{
			Harmony harmony = new Harmony("rost.moment.baldiplus.randommusic");
			harmony.PatchAll();
			Instance = this;
			path = AssetLoader.GetModPath(this);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			path += "\\";
        }
		public static string path;
		public static BasePlugin Instance;
        public static List<CustomSongData> midis = new List<CustomSongData>() { };
    }
}
