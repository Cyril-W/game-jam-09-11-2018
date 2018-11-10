public class InvertMovement : CursePlayer {
    protected override void OnCurseBegin()
    {
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
