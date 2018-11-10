using System;
using System.Collections.Generic;
using UnityEngine;

public class CurseManager : MonoBehaviour
{
    public enum CurseType { Slippery } //InvertMovement, MoveSpeedIncrease, MoveSpeedDecrease, CameraEffect, None }

    [SerializeField] GameObject curseSlippery;

	public void CursePlayer (GameObject player, CurseType curse)
	{
		Debug.Log("Player " + player.name + " cursed with " + curse.ToString());
        switch (curse)
        {
              case CurseType.Slippery:
                var newCurse = Instantiate(curseSlippery, player.transform);
                newCurse.tag = "Curse";
                break;
                default:
                break;
    }
}

	public void UnCursePlayer (GameObject player, CurseType curseType)
	{
		Debug.Log("Player " + player.name + " cured from " + curseType.ToString());

        var curses = GameObject.FindGameObjectsWithTag("Curse");
        foreach (GameObject curse in curses)
        {
            switch (curseType)
            {
                case CurseType.Slippery:
                    if (curse.GetComponent<CurseSlippery>())
                    {
                        Destroy(curse.gameObject);
                    }
                    break;
                default:
                    break;
            }
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
