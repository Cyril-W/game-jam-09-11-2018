using UnityEngine;

public class CurseMoveSpeed : CursePlayer {

    [SerializeField] bool increase;
    [SerializeField] float speedMultiplier = 2f;

    protected PlayerMovement playerMovement;

    protected override void OnCurseBegin()
    {
        playerMovement = gameObject.GetComponentInParent<PlayerMovement>();
        if (increase)
        {
            playerMovement.speedMult = speedMultiplier;
        }
        else
        {
            playerMovement.speedMult = 1/speedMultiplier;
        }
    }

    protected override void OnCurseEnd()
    {
        playerMovement.speedMult = 1f;
    }

    protected override void OnCurseUpdate()
    {
        
    }
}
