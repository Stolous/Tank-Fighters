using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
		public Image joystickImage;

		private CharacterController controller;
		private Ray ray = new Ray();

		public float reloadTime = 0.3f;
		private bool canFire = true;

		private Tank tank;

		void Start()
		{
			tank = GetComponent<Tank>();
			controller = GetComponent<CharacterController>();
			controller.enabled = true;
			joystickImage = GameObject.Find("MobileJoystick").GetComponent<Image>();
			InvokeRepeating("Reload", reloadTime, reloadTime);
		}


		void Update()
		{
			if(!GetComponent<NetworkIdentity>().isLocalPlayer)
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

#if !UNITY_EDITOR
			for (var i = 0; i < Input.touchCount; ++i)
			{
				if (Input.GetTouch(i).phase == TouchPhase.Began)
				{
					ray = Camera.main.ScreenPointToRay((Vector3)Input.GetTouch(i).position);
					RaycastHit hit;
					if(canFire && tank.health > 0 && Physics.Raycast(ray, out hit, LayerMask.NameToLayer("Ground")) && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
					{
						headTransform.LookAt(new Vector3(hit.point.x, headTransform.position.y, hit.point.z));
						CmdSpawnMissile(missileSpawn.position);
						canFire = false;
					}
				}
			}
#else
			if(Input.GetMouseButtonDown(0))
			{
				ray = Camera.main.ScreenPointToRay(Input.mousePosition + new Vector3(0f, 0f, 10f));
				RaycastHit hit;
				if(canFire && tank.health > 0 && Physics.Raycast(ray, out hit, LayerMask.NameToLayer("Ground")))
				{
					headTransform.LookAt(new Vector3(hit.point.x, headTransform.position.y, hit.point.z));
					CmdSpawnMissile(missileSpawn.position);
					canFire = false;
				}
			}
#endif
		}

		[Command]
		void CmdSpawnMissile(Vector3 spawnPosition)
		{
			GameObject missile = (GameObject)Instantiate(missilePrefab, spawnPosition, Quaternion.LookRotation(spawnPosition - headTransform.position));
			NetworkServer.Spawn(missile);
		}

		private void Reload()
		{
			this.canFire = true;
		}

		void OnDrawGizmos()
		{
			Gizmos.DrawRay(ray);
		}
	}
}
