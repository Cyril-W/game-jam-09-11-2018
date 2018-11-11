using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
	PlayerManagement playerMgr;
	// Use this for initialization
	void Start ()
	{
		playerMgr = FindObjectOfType<PlayerManagement>();
		if(playerMgr != null)
		{
			playerMgr.SpawnPlayers();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
