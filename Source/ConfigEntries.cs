using BepInEx;
using BepInEx.Configuration;
using System;

namespace BedMobile
{
	partial class BedMobile : BaseUnityPlugin
	{
		static ConfigEntry<bool> ConfigEntry_ShowMarker_A;
		static ConfigEntry<bool> ConfigEntry_ShowMarker_B;
		
		static ConfigEntry<bool> ConfigEntry_CheckExposure;
		static ConfigEntry<bool> ConfigEntry_CheckFire;
		static ConfigEntry<bool> ConfigEntry_CheckWet;
		static ConfigEntry<bool> ConfigEntry_CheckEnemies;
		
		void Add_ConfigEntries()
		{
			ConfigurationManagerAttributes Config_AdminOnly = new ConfigurationManagerAttributes{ IsAdminOnly = true };
			
			ConfigDescription ConfigDescription_ShowMarker_A = new ConfigDescription("Show Marker of Self.");
			ConfigDescription ConfigDescription_ShowMarker_B = new ConfigDescription("Show Markers of Others.");
			
			ConfigDescription ConfigDescription_CheckExposure = new ConfigDescription(("Check for Exposure."), null, Config_AdminOnly);
			ConfigDescription ConfigDescription_CheckFire = new ConfigDescription(("Check for Fire."), null, Config_AdminOnly);
			ConfigDescription ConfigDescription_CheckWet = new ConfigDescription(("Check for Wet."), null, Config_AdminOnly);
			ConfigDescription ConfigDescription_CheckEnemies = new ConfigDescription(("Check for Enemies."), null, Config_AdminOnly);
			
			ConfigEntry_ShowMarker_A = Config.Bind("C00 Client", "ShowMarkerSelf", true, ConfigDescription_ShowMarker_A);
			ConfigEntry_ShowMarker_B = Config.Bind("C00 Client", "ShowMarkerOthers", true, ConfigDescription_ShowMarker_B);

			ConfigEntry_CheckExposure = Config.Bind("S00 Server", "CheckExposure", true, ConfigDescription_ShowMarker_A);
			ConfigEntry_CheckFire = Config.Bind("S00 Server", "CheckFire", true, ConfigDescription_ShowMarker_B);
			ConfigEntry_CheckWet = Config.Bind("S00 Server", "CheckWet", true, ConfigDescription_ShowMarker_A);
			ConfigEntry_CheckEnemies = Config.Bind("S00 Server", "CheckEnemies", true, ConfigDescription_ShowMarker_B);
			
			Config.SettingChanged += Config_SettingChanged;
			Config.SaveOnConfigSet = true;
			Config.Save();
		}
		
		static void Config_SettingChanged(object New_Object, EventArgs New_EventArgs)
		{
			foreach(Bed New_Bed in FindObjectsOfType<Bed>())
			{
				if(New_Bed.GetComponent<Bed_BM_Marker>())
				{
					New_Bed.GetComponent<Bed_BM_Marker>().Marker_Stop();
					New_Bed.GetComponent<Bed_BM_Marker>().Marker_Start(New_Bed);
				}
			}
		}
	}
}