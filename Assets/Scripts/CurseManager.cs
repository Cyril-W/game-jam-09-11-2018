using System;
using System.Collections.Generic;
using UnityEngine;

public class CurseManager : MonoBehaviour
{
    public enum CurseType { InvertMovement } //, MoveSpeedIncrease, MoveSpeedDecrease, Slippery, CameraEffect, None }

    [SerializeField] InvertMovement invertMovement;

	public void CursePlayer (GameObject player, CurseType curse)
	{
		Debug.Log("Player " + player.name + " cursed with " + curse.ToString());

        switch (curse)
        {
            case CurseType.InvertMovement:
                player.AddComponent<InvertMovement>();
                break;
            default:
                break;
        }
    }

	public void UnCursePlayer (GameObject player, CurseType curse)
	{
		Debug.Log("Player " + player.name + " cured from " + curse.ToString());
	}

	public void CurseAllPlayers (CurseType curse)
	{
		Debug.Log("All Players cursed with" + curse.ToString());
	}

	#region Singleton
	public static CurseManager instance;
	private void Awake ()
	{
		instance = this;
	}
	#endregion
}
