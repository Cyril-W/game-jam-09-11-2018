using UnityEngine;

public abstract class CursePlayer : MonoBehaviour
{
    protected PlayerMovement playerMovement;

    private void OnEnable()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        OnCurseBegin();
    }

    private void OnDisable()
    {
        OnCurseEnd();
    }

    private void Update()
    {
        OnCurseUpdate();
    }

    protected abstract void OnCurseBegin();
    protected abstract void OnCurseUpdate();
    protected abstract void OnCurseEnd();
}
