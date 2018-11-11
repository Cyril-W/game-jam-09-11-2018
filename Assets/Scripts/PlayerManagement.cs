using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagement : MonoBehaviour
{
	public class Player
	{
		public string hInput = "Horizontal1";
		public string vInput = "Vertical1";
		public int playerSkin = 0;
	}

	public List<Player> playerList;
	public Vector3[] spawnLocations = new Vector3[4];
	public GameObject playerPrefab;
	public int playerAmount = 0;
	// Use this for initialization
	void Awake ()
	{
		DontDestroyOnLoad(gameObject);
	}

	public void InitPlayerList ()
	{
		playerAmount = 0;
		playerList = new List<Player>();
	}

	public void SpawnPlayers()
	{
		for (int i = 0; i < playerAmount; i++)
		{
			GameObject player = Instantiate(playerPrefab, spawnLocations[i], playerPrefab.transform.rotation);
			PlantHolding skinMgr = player.GetComponent<PlantHolding>();
			if(skinMgr != null)
			{
				skinMgr.UpdatePlayerModel(playerList[i].playerSkin, true);
			}

			PlayerMovement playerMvt = player.GetComponent<PlayerMovement>();
			if(playerMvt != null)
			{
				playerMvt.hInput = playerList[i].hInput;
				playerMvt.vInput = playerList[i].vInput;
				playerMvt.spawnPosition = spawnLocations[i];
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
