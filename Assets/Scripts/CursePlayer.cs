using UnityEngine;

public abstract class CursePlayer : MonoBehaviour
{
    [SerializeField] protected float duration;

    private void OnEnable()
    {
        OnCurseBegin();
        if (duration > 0f)
        {
            Destroy(gameObject, duration);
        }
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
