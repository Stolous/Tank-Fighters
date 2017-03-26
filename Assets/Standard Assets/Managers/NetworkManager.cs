using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

namespace TankFighters.Networking {

	[RequireComponent(typeof(NetworkMatch))]
	public class NetworkManager : MonoBehaviour {

		private NetworkMatch networkMatch;
		public List<MatchInfoSnapshot> matchList = new List<MatchInfoSnapshot>();

		public Text gameNameField; 

		void Awake() {
			networkMatch = gameObject.AddComponent<NetworkMatch>();
		}

		public void CreateRoom() {
			networkMatch.CreateMatch(gameNameField.text, 2, true, "", "", "", 0, 0, OnMatchCreate);
		}

		public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
			if(!success) {
				Debug.LogError("Create match failed: " + extendedInfo);
				return;
			}
			Debug.Log("Create match succeeded");
			NetworkServer.Listen(matchInfo, 9000);
			Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);
		}

		public void JoinRoom() {
			ListRooms();
			foreach (var match in matchList)
			{
				if (match.name == gameNameField.text)
				{
					networkMatch.JoinMatch(match.networkId, "", "", "", 0, 0, OnMatchJoined);
				}
			}
		}

		public void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo) {
			if(!success) {
				Debug.LogError("Join match failed: " + extendedInfo);
				return;
			}
			Debug.Log("Match joined successfuly");
			Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);
			NetworkClient client = new NetworkClient();
			client.RegisterHandler(MsgType.Connect, OnConnected);
			client.Connect(matchInfo);
		}

		public void OnConnected(NetworkMessage msg)
		{
			Debug.Log("Connected!");
		}

		public void ListRooms() {
			networkMatch.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
		}

		public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {
			if(!success) {
				Debug.LogError("Failed to retreive server list" + extendedInfo);
				return;
			}
			this.matchList = matches;
		}
	}
}
