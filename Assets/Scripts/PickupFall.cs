using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFall : MonoBehaviour {

    [SerializeField] float timeToFall = 1f;
    [SerializeField] Transform pickUp;

    [Space]

    [SerializeField] Vector3 targetScale = 0.25f * Vector3.one;
    [SerializeField] Transform target;

    [Space]

    [SerializeField] AnimationCurve curveX;
    [SerializeField] AnimationCurve curveY;
    [SerializeField] AnimationCurve curveZ;

    [Space]

    [SerializeField] GameObject glassJar;
    [SerializeField] AudioSource breakGlassAudioSource;
    [SerializeField] int numbersToEmit = 30;
    [SerializeField] ParticleSystem particlesGlass;

    public void SetPickupHeight(float zPos)
    {
        pickUp.localPosition += Vector3.up * zPos;
    }

    public void SetTargetPos(Vector3 targetPos)
    {
        target.position = targetPos;
        target.localScale = Vector3.zero;
        target.gameObject.SetActive(true);
    }

    public void Throw()
    {
        StartCoroutine(Fall());
    }

    IEnumerator Fall()
    {
        // paramters for pickUp position
        var initialPos = pickUp.localPosition;
        var finalPos = target.localPosition;
        finalPos.y = 0;

        // parameters for target scale
        var initialScale = target.localScale;
        var finalScale = targetScale;

        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToFall;

            Vector3 newLocalPosition;
            newLocalPosition.x = Mathf.Lerp(initialPos.x, finalPos.x, curveX.Evaluate(t));
            newLocalPosition.y = Mathf.Lerp(initialPos.y, finalPos.y, curveY.Evaluate(t));
            newLocalPosition.z = Mathf.Lerp(initialPos.z, finalPos.z, curveZ.Evaluate(t));
            pickUp.localPosition = newLocalPosition;

            target.localScale = Vector3.Slerp(initialScale, finalScale, t);
            yield return new WaitForEndOfFrame();
        }

        target.gameObject.SetActive(false);
        particlesGlass.Emit(numbersToEmit);
        glassJar.SetActive(false);
        breakGlassAudioSource.PlayOneShot(breakGlassAudioSource.clip);
    }
}
