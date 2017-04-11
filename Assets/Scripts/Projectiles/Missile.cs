using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankFighters.Projectile
{
	public class Missile : MonoBehaviour {

		public int damages = 50;
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
			Debug.Log("collided with " + other);
			Destroy(this.gameObject);
		}
	}
}
