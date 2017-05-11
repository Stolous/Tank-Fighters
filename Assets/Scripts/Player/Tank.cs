using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace TankFighters.Player
{
	public class Tank : NetworkBehaviour
	{
		[SyncVar]
		private int health = 100;

		//public Slider healthSlider;*

		void Update()
		{
			if(this.health <= 0)
			{
				Renderer[] subRenderers = this.GetComponentsInChildren<Renderer>();
				foreach(Renderer subRenderer in subRenderers)
				{
					subRenderer.enabled = false;
				}
				// TODO spawn particles
			}
		}

		public void Damage(int damages)
		{
			this.health -= damages;
			Debug.Log("Took damages");
			if(this.health <= 0)
				Invoke("Respawn", 3f);
		}

		// Not to be called on client !
		void Respawn()
		{
			Transform spawn = NetworkManager.singleton.GetStartPosition();
			GameObject newPlayer = (GameObject)Instantiate(NetworkManager.singleton.playerPrefab, spawn.position, spawn.rotation);
			NetworkServer.Destroy(this.gameObject);
			NetworkServer.ReplacePlayerForConnection(this.connectionToClient, newPlayer, this.playerControllerId);
		}
	}
}
