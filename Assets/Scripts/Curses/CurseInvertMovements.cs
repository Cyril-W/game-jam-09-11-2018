using UnityEngine;

public class CurseInvertMovements : CursePlayer {

    [SerializeField] bool affectX;
    [SerializeField] bool affectY;

    protected PlayerMovement playerMovement;

    protected override void OnCurseBegin()
    {
        playerMovement = gameObject.GetComponentInParent<PlayerMovement>();
        if (affectX)
        {
            playerMovement.hDirMult = -1;
        }
        if (affectY)
        {
            playerMovement.vDirMult = -1;
        }
    }

    protected override void OnCurseEnd()
    {
        playerMovement.hDirMult = playerMovement.vDirMult = 1;
    }

    protected override void OnCurseUpdate()
    {
        
    }
}
