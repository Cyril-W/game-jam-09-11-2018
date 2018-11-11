﻿using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CurseManager : MonoBehaviour
{
    public enum CurseType { Slippery, InvertMovementX, InvertMovementY, InvertMovements, MoveSpeedIncrease, MoveSpeedDecrease }
    public enum DoomType { Greyscale, Red }

    [SerializeField] GameObject curseSlippery;
    [SerializeField] GameObject curseInvertMovementX;
    [SerializeField] GameObject curseInvertMovementY;
    [SerializeField] GameObject curseInvertMovements;
    [SerializeField] GameObject curseMoveSpeedIncrease;
    [SerializeField] GameObject curseMoveSpeedDecrease;

    [Space]

    [SerializeField] float secondsToUndoom = 2f;
    [SerializeField] PostProcessingBehaviour cameraProfile;
    [SerializeField] PostProcessingProfile normalProfile;
    [SerializeField] PostProcessingProfile greyscaleProfile;
    [SerializeField] PostProcessingProfile redProfile;

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

	public void DoomPlayers ()
	{
        var dooms = System.Enum.GetNames(typeof(DoomType));
        var doom = (DoomType)Random.Range(0, dooms.Length);
        switch (doom)
        {
            case DoomType.Greyscale:
                cameraProfile.profile = greyscaleProfile;
                break;
            case DoomType.Red:
                cameraProfile.profile = redProfile;
                break;
            default:
                break;
        }
        StartCoroutine(UndoomPlayers());
    }

    IEnumerator UndoomPlayers()
    {
        yield return new WaitForSeconds(secondsToUndoom);
        cameraProfile.profile = normalProfile;
    }

	#region Singleton
	public static CurseManager instance;
	private void Awake ()
	{
		instance = this;
	}
	#endregion
}
