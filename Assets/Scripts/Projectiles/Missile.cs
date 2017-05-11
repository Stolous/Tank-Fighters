using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using TankFighters.Player;

namespace TankFighters.Projectile
{
	public class Missile : NetworkBehaviour
	{
		public int damages = 40;
		public float speed = 1f;

		private Rigidbody rigidbody;

		void Start()
		{
			this.rigidbody = GetComponent<Rigidbody>();
			this.rigidbody.velocity = transform.forward * speed;
		}

		void LateUpdate()
		{
			//this.rigidbody.velocity = (speed * this.rigidbody.velocity.normalized);
		}

		void OnTriggerEnter(Collider other)
		{
			if(!GetComponent<NetworkIdentity>().isServer)
				return;
			Debug.Log("collided with " + other);
			Destroy(this.gameObject);
			other.GetComponent<Tank>().Damage(damages);
		}
	}
}
