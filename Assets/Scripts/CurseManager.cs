using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseManager : MonoBehaviour
{
	public enum CurseType { InvertMovement, MoveSpeedIncrease, MoveSpeedDecrease, Slippery, CameraEffect, None}
	public void CursePlayer (GameObject player, CurseType curse)
	{

	}

	#region Singleton
	public static CurseManager instance;
	private void Awake ()
	{
		instance = this;
	}
	#endregion

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
