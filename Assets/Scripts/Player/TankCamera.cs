using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace TankFighters.Player
{
	public class TankCamera : MonoBehaviour
	{
		public Vector3 offsetVector;
		public float smoothFactor = 0.8f;
		/*public GameObject tankGameObject;
		// Use this for initialization
		void Start ()
		{
			if(!tankGameObject.GetComponent<NetworkIdentity>().hasAuthority)
				this.gameObject.SetActive(false);
		}
		*/
		void Update()
		{
			foreach (GameObject tank in GameObject.FindGameObjectsWithTag("Player"))
			{
				if(tank.GetComponent<NetworkIdentity>().hasAuthority)
					this.transform.position = Vector3.Lerp(this.transform.position, tank.transform.position + offsetVector, smoothFactor);
			}
		}
	}
}