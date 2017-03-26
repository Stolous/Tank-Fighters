using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankFighters.Menu {
	
	public class LobbyUIManager : MonoBehaviour {

		public GameObject mainPanel, playPanel, lobbyPanel;

		public void GoToLobby() {
			this.lobbyPanel.SetActive(true);
			this.playPanel.SetActive(false);
			this.mainPanel.SetActive(false);
		}

		public void GoToServerList() {
			this.playPanel.SetActive(true);
			this.lobbyPanel.SetActive(false);
			this.mainPanel.SetActive(false);
		}
	}
}
