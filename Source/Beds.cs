using BepInEx;
using Jotunn.Managers;
using UnityEngine;

namespace BedMobile
{
	partial class BedMobile : BaseUnityPlugin
	{
		class Bed_BM : MonoBehaviour
		{
			public Bed New_Bed;
			public string Key = "";
			
			public void Update_Start()
			{
				New_Bed = gameObject.GetComponent<Bed>();
				
				if(New_Bed.GetOwner() != 0L)
				{
					foreach(string GlobalKey in ZoneSystem.m_instance.GetGlobalKeys())
					{
						bool New_Bool = false;
						
						if(GlobalKey.StartsWith(("GK_BM_" + New_Bed.GetOwnerName()).ToLower()))
						{
							Key = GlobalKey;
							New_Bool = true;
						}
						
						if(New_Bool)
						{ break; }
					}
					
					Remove_Marker(New_Bed);
					
					if(Vector3.Distance(New_Bed.GetSpawnPoint(), GetVector3(Key)) < 5.00F)
					{ New_Bed.gameObject.AddComponent<Bed_BM_Marker>(); }
					
					if(GetComponent<Bed_BM_Marker>())
					{ New_Bed.GetComponent<Bed_BM_Marker>().Marker_Awake(); }
					
					New_Bed.GetComponent<Bed_BM>().InvokeRepeating("Update_SpawnPoint", 0.00F, 0.01F);
				}
			}
			
			public void Update_SpawnPoint()
			{
				if(Key != "")
				{
					if((Vector3.Distance(New_Bed.GetSpawnPoint(), GetVector3(Key)) > 0.90F) && New_Bed.GetComponent<Bed_BM_Marker>())
					{
						Minimap.MapMode New_MapMode = Minimap.m_instance.m_mode;
						
						ZoneSystem.m_instance.RemoveGlobalKey(Key);
						Key = "GK_BM_" + New_Bed.GetOwnerName() + "_" + New_Bed.GetSpawnPoint();
						ZoneSystem.m_instance.SetGlobalKey(Key);
						
						Minimap.m_instance.SetMapMode(New_MapMode);
					}
				}
			}
		}
		
		class Bed_BM_Marker : MonoBehaviour
		{
			EffectList New_EffectList = new EffectList{ m_effectPrefabs = new EffectList.EffectData[1] };
			Quaternion New_Quaternion;
			
			public void Marker_Awake()
			{
				New_EffectList.m_effectPrefabs[0] = new EffectList.EffectData{ m_prefab = PrefabManager.Instance.GetPrefab("GO_BM_Effect_Marker"), m_attach = true};
				New_Quaternion = transform.rotation * Quaternion.Euler(270.00F, 0.00F, 0.00F);
				
				Marker_Stop();
				Marker_Start(GetComponent<Bed>());
			}
			
			public void Marker_Start(Bed New_Bed)
			{
				if(New_Bed.IsMine())
				{
					if(ConfigEntry_ShowMarker_A.Value)
					{ New_EffectList.Create(GetComponent<Bed>().GetSpawnPoint(), New_Quaternion, transform); }
				}
				else
				{
					if(ConfigEntry_ShowMarker_B.Value)
					{ New_EffectList.Create(GetComponent<Bed>().GetSpawnPoint(), New_Quaternion, transform); }
				}
			}
			
			public void Marker_Stop()
			{
				foreach(Transform New_Transform in transform)
				{
					if(New_Transform.gameObject.name.StartsWith("GO_BM_Effect_Marker"))
					{ Destroy(New_Transform.gameObject); }
				}
			}
		}
	}
}