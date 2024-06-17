using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;
using UnityEngine;
using ValheimRAFT;

namespace BedMobile
{
	partial class BedMobile : BaseUnityPlugin
	{
		[HarmonyPatch(typeof(Bed), nameof(Bed.Awake))]
		class Patch_Bed_Awake
		{
			static void Postfix(ref Bed __instance)
			{
				if(__instance != null)
				{
					if(__instance.m_nview.GetZDO() != null)
					{
						if(!__instance.GetComponent<Bed_BM>())
						{ __instance.gameObject.AddComponent<Bed_BM>(); }
						
						__instance.GetComponent<Bed_BM>().Invoke("Update_Start", 0.50F);
					}
				}
			}
		}
		
		[HarmonyPatch(typeof(Bed), nameof(Bed.Interact))]
		class Patch_Bed_Interact
		{
			static void Postfix(ref Bed __instance, ref Humanoid human)
			{
				if( __instance != null)
				{
					if(__instance.IsMine() && __instance.IsCurrent())
					{
						if(!__instance.GetComponent<Bed_BM>())
						{ __instance.gameObject.AddComponent<Bed_BM>(); }
						
						Remove_Marker(__instance);
						
						if(!__instance.GetComponent<Bed_BM_Marker>())
						{ __instance.gameObject.AddComponent<Bed_BM_Marker>(); }
						
						__instance.GetComponent<Bed_BM_Marker>().Marker_Awake();
						
						foreach(string GlobalKey in ZoneSystem.m_instance.GetGlobalKeys())
						{
							if(GlobalKey.StartsWith(("GK_BM_" + __instance.GetOwnerName()).ToLower()))
							{ ZoneSystem.m_instance.RemoveGlobalKey(GlobalKey); }
						}
						
						ZoneSystem.m_instance.SetGlobalKey("GK_BM_" + __instance.GetOwnerName() + "_" + __instance.GetSpawnPoint());
						
						foreach(Bed New_Bed in FindObjectsOfType<Bed>())
						{
							if(New_Bed.GetOwnerName() == __instance.GetOwnerName())
							{
								if((New_Bed != __instance) && New_Bed.GetComponent<Bed_BM_Marker>())
								{ New_Bed.GetComponent<Bed_BM>().CancelInvoke("Update_SpawnPoint"); }
							}
						}
						
						__instance.GetComponent<Bed_BM>().Invoke("Update_Start", 0.00F);
					}
				}
			}
		}
		
		[HarmonyPatch(typeof(Bed), nameof(Bed.IsCurrent))]
		class Patch_Bed_IsCurrent
		{
			static void Postfix(ref Bed __instance, ref bool __result)
			{
				if(!__instance.IsMine())
				{ return; }
				
				__result = (Vector3.Distance(__instance.GetSpawnPoint(), Game.instance.GetPlayerProfile().GetCustomSpawnPoint()) <= 5.00F);
			}
		}
		
		[HarmonyPatch(typeof(Bed), nameof(Bed.CheckExposure))]
		class Patch_Bed_CheckExposure
		{
			static bool Prefix(ref Bed __instance, ref bool __result)
			{
				if(__instance != null)
				{
					if(!ConfigEntry_CheckExposure.Value)
					{
						__result = true;
						return false;
					}
				}
				
				return true;
			}
		}
		
		[HarmonyPatch(typeof(Bed), nameof(Bed.CheckFire))]
		class Patch_Bed_CheckFire
		{
			static bool Prefix(ref Bed __instance, ref bool __result)
			{
				if(__instance != null)
				{
					if(!ConfigEntry_CheckFire.Value)
					{
						__result = true;
						return false;
					}
				}
				
				return true;
			}
		}

		[HarmonyPatch(typeof(Bed), nameof(Bed.CheckWet))]
		class Patch_Bed_CheckWet
		{
			static bool Prefix(ref Bed __instance, ref bool __result)
			{
				if(__instance != null)
				{
					if(!ConfigEntry_CheckWet.Value)
					{
						__result = true;
						return false;
					}
				}
				
				return true;
			}
		}
		
		[HarmonyPatch(typeof(Bed), nameof(Bed.CheckEnemies))]
		class Patch_Bed_CheckEnemies
		{
			static bool Prefix(ref Bed __instance, ref bool __result)
			{
				if(__instance != null)
				{
					if(!ConfigEntry_CheckEnemies.Value)
					{
						__result = true;
						return false;
					}
				}
				
				return true;
			}
		}

		[HarmonyPatch(typeof(Ship), nameof(Ship.CustomFixedUpdate))]
		class Patch_Ship_CustomFixedUpdate
		{
			static void Postfix(ref Ship __instance)
			{
				if(__instance != null)
				{
					if(Mod_ValheimRAFT)
					{
						if(__instance.m_players.Count > 0)
						{
							bool New_Bool_Dead = false;
							bool New_Bool_Sleeping = false;
						
							foreach(Player New_Player in __instance.m_players)
							{
								if(New_Player.IsDead())
								{ New_Bool_Dead = true; }
							
								if(New_Player.IsSleeping())
								{ New_Bool_Sleeping = true; }
							}
						
							if(New_Bool_Dead || New_Bool_Sleeping)
							{
								if(__instance.GetComponent<MoveableBaseShipComponent>())
								{ __instance.m_nview.InvokeRPC("SetAnchor", true); }
							}
						}
					}
				}
			}
		}
	}
}