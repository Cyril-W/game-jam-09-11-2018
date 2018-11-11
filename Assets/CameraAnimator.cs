using UnityEngine;

public class CameraAnimator : MonoBehaviour {

    [SerializeField] int numberOfShakeAnimations;
    [SerializeField] Animator animator;

    public void Shake()
    {
        var shakeNumber = Random.Range(0, numberOfShakeAnimations).ToString();
        animator.SetTrigger("Shake" + shakeNumber);
    }

    public void Doom(string doomName)
    {
        animator.Play(doomName);        
    }

    public void Undoom()
    {
        animator.SetTrigger("Undoom");
    }
}
