using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace TankFighters.Player {
	
	public class TankCamera : MonoBehaviour {

		public GameObject tankGameObject;
		// Use this for initialization
		void Start () {
			if(!tankGameObject.GetComponent<NetworkIdentity>().hasAuthority)
				this.gameObject.SetActive(false);
		}
	}
}