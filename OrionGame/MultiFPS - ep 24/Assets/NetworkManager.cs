using UnityEngine;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {

	// TEMPORARY TESTING STUFF
	public string botResourceName;
	public Waypoint botSpawnWaypoint;

	public AbstractGameType _gameType = new AbstractGameType ();

	// END OF TESTING


	public GameObject standbyCamera;
	SpawnSpot[] spawnSpots;

	bool connecting = false;


	// Use this for initialization
	void Start () {
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot>();
		PhotonNetwork.player.name = PlayerPrefs.GetString("Username", "Awesome Dude");
	}

	void OnDestroy() {
		PlayerPrefs.SetString("Username", PhotonNetwork.player.name);
	}

	public void AddChatMessage(string m) {
		GetComponent<PhotonView>().RPC ("AddChatMessage_RPC", PhotonTargets.AllBuffered, m);
	}

	[RPC]
	void AddPlayerToWorld(PlayerController pc)
	{
		if (PhotonNetwork.isMasterClient) 
		{

		}
	}

	void Connect() {
		PhotonNetwork.ConnectUsingSettings( "MultiFPS v004" );
	}
	 
	void OnGUI() {
		GUILayout.Label( PhotonNetwork.connectionStateDetailed.ToString() );

		if(PhotonNetwork.connected == false && connecting == false ) {
			// We have not yet connected, so ask the player for online vs offline mode.
			GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height) );
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Username: ");
			PhotonNetwork.player.name = GUILayout.TextField(PhotonNetwork.player.name);
			GUILayout.EndHorizontal();

			connecting = true;
			Connect ();

			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		if(PhotonNetwork.connected == true && connecting == false) {

			GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height) );
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			
			if( GUILayout.Button("Red Team") ) {
				SpawnMyPlayer(1);
			}
			
			if( GUILayout.Button("Green Team") ) {
				SpawnMyPlayer(2);
			}

			if( GUILayout.Button("Random") ) {
				SpawnMyPlayer(Random.Range(1,3));	// 1 or 2
			}
			
			if( GUILayout.Button("Renegade!") ) {
				SpawnMyPlayer(0);
			}
			
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

	}

	void OnJoinedLobby() {
		Debug.Log ("OnJoinedLobby");
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed() {
		Debug.Log ("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom( null );
	}

	void OnJoinedRoom() {
		Debug.Log ("OnJoinedRoom");

		connecting = false;
		//SpawnMyPlayer();
	}

	void SpawnMyPlayer(int teamID) {
		this.teamID = teamID;
		hasPickedTeam = true;
		AddChatMessage("Spawning player: " + PhotonNetwork.player.name);

		if(spawnSpots == null) {
			Debug.LogError ("WTF?!?!?");
			return;
		}

		SpawnSpot mySpawnSpot = spawnSpots[ Random.Range (0, spawnSpots.Length) ];
		GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate("PlayerController", mySpawnSpot.transform.position, mySpawnSpot.transform.rotation, 0);
		standbyCamera.SetActive(false);

		//((MonoBehaviour)myPlayerGO.GetComponent("FPSInputController")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("MouseLook")).enabled = true;
		((MonoBehaviour)myPlayerGO.GetComponent("PlayerController")).enabled = true;

		//myPlayerGO.GetComponent<PhotonView>().RPC ("SetTeamID", PhotonTargets.AllBuffered, teamID);

		myPlayerGO.transform.FindChild("Main Camera").gameObject.SetActive(true);

		GetComponent<PhotonView>().RPC ("AddPlayerToWorld", PhotonTargets.AllBuffered, myPlayerGO);


		// BOT TESTING
		//GameObject botGO = (GameObject)PhotonNetwork.Instantiate(botResourceName, botSpawnWaypoint.transform.position, botSpawnWaypoint.transform.rotation, 0);
		//((MonoBehaviour)botGO.GetComponent("BotController")).enabled = true;
		// END OF BOT TESTING
	}

	void Update() {
		if(respawnTimer > 0) {
			respawnTimer -= Time.deltaTime;

			if(respawnTimer <= 0) {
				// Time to respawn the player!
				SpawnMyPlayer(teamID);
			}
		}
	}
}
