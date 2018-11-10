using UnityEngine;

public abstract class CursePlayer : MonoBehaviour
{
    private void OnEnable()
    {
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
