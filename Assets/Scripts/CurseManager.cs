using System;
using System.Collections.Generic;
using UnityEngine;

public class CurseManager : MonoBehaviour
{
    public enum CurseType { Slippery, InvertMovementX, InvertMovementY, InvertMovements, MoveSpeedIncrease, MoveSpeedDecrease }

    [SerializeField] GameObject curseSlippery;
    [SerializeField] GameObject curseInvertMovementX;
    [SerializeField] GameObject curseInvertMovementY;
    [SerializeField] GameObject curseInvertMovements;
    [SerializeField] GameObject curseMoveSpeedIncrease;
    [SerializeField] GameObject curseMoveSpeedDecrease;

    public void CursePlayer (GameObject player, CurseType curse)
	{
        switch (curse)
        {
            case CurseType.Slippery:
                Instantiate(curseSlippery, player.transform);
                break;
            case CurseType.InvertMovementX:
                Instantiate(curseInvertMovementX, player.transform);
                break;
            case CurseType.InvertMovementY:
                Instantiate(curseInvertMovementY, player.transform);
                break;
            case CurseType.InvertMovements:
                Instantiate(curseInvertMovements, player.transform);
                break;
            case CurseType.MoveSpeedIncrease:
                Instantiate(curseMoveSpeedIncrease, player.transform);
                break;
            case CurseType.MoveSpeedDecrease:
                Instantiate(curseMoveSpeedDecrease, player.transform);
                break;
            default:
                break;
    }
}

	public void UnCursePlayer (GameObject player)
	{
        var curses = GameObject.FindGameObjectsWithTag("Curse");
        foreach (GameObject curse in curses)
        {
            Destroy(curse);
        }
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
