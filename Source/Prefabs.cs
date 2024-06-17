using BepInEx;
using Jotunn.Managers;
using UnityEngine;

namespace BedMobile
{
	partial class BedMobile : BaseUnityPlugin
	{
		static void Add_Prefabs()
		{
			GameObject New_GameObject = PrefabManager.Instance.CreateClonedPrefab("GO_BM_Effect_Marker", "PlaceMarker");
			foreach(Transform New_Transform in New_GameObject.transform)
			{
				if(New_Transform.name == "Square")
				{ New_Transform.gameObject.SetActive(false); }
			}
			PrefabManager.Instance.AddPrefab(New_GameObject);
			
			PrefabManager.OnVanillaPrefabsAvailable -= Add_Prefabs;
		}
	}
}