public class CurseSlippery : CursePlayer {

    protected PlayerMovement playerMovement;

    protected override void OnCurseBegin()
    {
        playerMovement = gameObject.GetComponentInParent<PlayerMovement>();
        playerMovement.useInertia = true;
    }

    protected override void OnCurseEnd()
    {
        playerMovement.useInertia = false;
    }

    protected override void OnCurseUpdate()
    {
        
    }
}
