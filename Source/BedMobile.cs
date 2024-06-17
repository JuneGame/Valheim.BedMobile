using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;
using Jotunn.Managers;
using Jotunn.Utils;
using System.Text.RegularExpressions;
using UnityEngine;

namespace BedMobile
{
	[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	[BepInDependency(Jotunn.Main.ModGuid)]
	[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Patch)]
	
	partial class BedMobile : BaseUnityPlugin
	{
		const string PluginGUID = "JuneGame.Valheim.BedMobile";
		const string PluginName = "Bed Mobile";
		const string PluginVersion = "1.0.1";
		
		static bool Mod_ValheimRAFT;
		
		void Awake()
		{
			Add_ConfigEntries();
			
			Harmony New_Harmony = new Harmony(PluginGUID);
			New_Harmony.PatchAll();
			
			PrefabManager.OnVanillaPrefabsAvailable += Add_Prefabs;
		}
		
		void Start()
		{
			Mod_ValheimRAFT = Chainloader.PluginInfos.ContainsKey("BepIn.Sarcen.ValheimRAFT");
		}
		
		void Update()
		{
			if(Player.m_localPlayer != null)
			{
				foreach(string GlobalKey in ZoneSystem.m_instance.GetGlobalKeys())
				{
					bool New_Bool = false;
					
					if(GlobalKey.StartsWith(("GK_BM_" + Player.m_localPlayer.GetPlayerName()).ToLower()))
					{
						if(Game.instance.GetPlayerProfile().GetCustomSpawnPoint() != GetVector3(GlobalKey))
						{ Game.instance.GetPlayerProfile().SetCustomSpawnPoint(GetVector3(GlobalKey)); }
						
						New_Bool = true;
					}
					
					if(New_Bool)
					{ break; }
				}
			}
		}
		
		static void Remove_Marker(Bed __instance)
		{
			foreach(Bed New_Bed in FindObjectsOfType<Bed>())
			{
				if(New_Bed.GetOwnerName() == __instance.GetOwnerName())
				{
					if((New_Bed != __instance) && New_Bed.GetComponent<Bed_BM_Marker>())
					{
						New_Bed.GetComponent<Bed_BM_Marker>().Marker_Stop();
						Destroy(New_Bed.GetComponent<Bed_BM_Marker>());
					}
				}
			}
		}
		
		static Vector3 GetVector3(string New_String)
		{
			var New_Regex = new Regex(@"\(([-\d.]+)\, ([-\d.]+), ([-\d.]+)\)$").Match(New_String);
			
			if(New_Regex.Groups.Count == 4)
			{ return new Vector3(float.Parse(New_Regex.Groups[1].Value), float.Parse(New_Regex.Groups[2].Value), float.Parse(New_Regex.Groups[3].Value)); }

			return Vector3.zero;
		}
	}
}