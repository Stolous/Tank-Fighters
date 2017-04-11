using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankFighters.Projectile
{
	public class Missile : MonoBehaviour {

		public int damages = 50;
		public float speed = 1f;

		void FixedUpdate()
		{
			transform.Translate(Vector3.up * speed);
		}

		void OnTriggerEnter(Collider other)
		{
			Debug.Log("collided with " + other);
			Destroy(this.gameObject);
		}
	}
}
