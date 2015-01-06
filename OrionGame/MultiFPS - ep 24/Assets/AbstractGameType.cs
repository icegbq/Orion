using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class AbstractGameType : MonoBehaviour {

	protected bool _allowRespawn = false;
	protected float _respawnTimer = 0;
	List<PlayerController> _clients = new List<PlayerController>();
	public enum eTeam {RED, BLUE};

	Dictionary<eTeam, List<PlayerController>> _teams;

	// Use this for initialization
	void Start () 
	{
		_teams [eTeam.RED] = new List<PlayerController> ();
		_teams [eTeam.BLUE] = new List<PlayerController> ();
	}

	void OnConnected(PlayerController controller)
	{

	}

	public void SpawnMyPlayer(PlayerController pc) 
	{
		if (PhotonNetwork.isMasterClient) 
		{

		}
	}

	// Update is called once per frame
	void Update () 
	{
		if(_respawnTimer > 0) {
			_respawnTimer -= Time.deltaTime;
			
			if(_respawnTimer <= 0) {
				// Time to respawn the player!
				SpawnMyPlayer(teamID);
			}
		}
	}

	protected virtual eTeam getTeam()
	{
		if (_teams [eTeam.RED].Count < _teams [eTeam.BLUE].Count) 
		{
			return eTeam.RED;
		}
		return eTeam.BLUE;
	}

	protected virtual void addToTeam(PlayerController pc, eTeam team)
	{

	}
}
