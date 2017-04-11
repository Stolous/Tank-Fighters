using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

namespace TankFighters.Player
{
	[RequireComponent (typeof(CharacterController))]
	public class TankMovements : NetworkBehaviour
	{
		public int speed = 1;
		public Transform headTransform;
		public Transform missileSpawn;

		public GameObject missilePrefab;

		private CharacterController controller;
		private Ray ray = new Ray();


		void Start()
		{
			controller = GetComponent<CharacterController>();
			controller.enabled = true;
		}


		void Update()
		{
			if (!GetComponent<NetworkIdentity>().isLocalPlayer)
				return;
			
			Vector3 movement = Vector3.zero;
			movement.x += CrossPlatformInputManager.GetAxis("Horizontal");
			movement.z += CrossPlatformInputManager.GetAxis("Vertical");

			movement *= speed;

			if(movement.magnitude > 0)
				transform.rotation = Quaternion.LookRotation(movement.normalized);

			movement += Physics.gravity;
			movement *= Time.deltaTime;
			controller.Move(movement);

			if(Input.GetMouseButtonDown(0))
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition + new Vector3(0f, 0f, 10f));
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit, LayerMask.NameToLayer("Ground")))
				{
					headTransform.LookAt(new Vector3(hit.point.x, headTransform.position.y, hit.point.z));
					CmdSpawnMissile(missileSpawn.position);
				}
			}
			/*for (var i = 0; i < Input.touchCount; ++i) {
				if (Input.GetTouch(i).phase == TouchPhase.Began && !moveJoystick.gui.HitTest(Input.GetTouch(i).position)) {
					RaycastHit hit;
					Physics.Raycast(Camera.main.ScreenPointToRay((Vector3)Input.GetTouch(i).position), out hit, 30);
					Vector3 aimPosition = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);

					Debug.Log("fire");
				}
			}*/
		}

		[Command]
		void CmdSpawnMissile(Vector3 spawnPosition)
		{
			GameObject missile = (GameObject)Instantiate(missilePrefab, spawnPosition, Quaternion.LookRotation(spawnPosition - headTransform.position));
			NetworkServer.Spawn(missile);
		}

		void OnDrawGizmos()
		{
			Gizmos.DrawRay(ray);
		}
	}
}
