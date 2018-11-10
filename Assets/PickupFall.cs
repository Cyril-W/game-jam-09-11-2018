using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFall : MonoBehaviour {

    [SerializeField] float timeToFall = 1f;
    [SerializeField] Transform pickUp;

    [Space]

    [SerializeField] Vector3 targetScale = 0.25f * Vector3.one;
    [SerializeField] Transform target;

    void Start () {
        target.localScale = Vector3.zero;
        target.gameObject.SetActive(true);
        StartCoroutine(Fall());
	}

    IEnumerator Fall()
    {
        // paramters for pickUp position
        var initialPos = pickUp.localPosition;
        var finalPos = initialPos;
        finalPos.y = 0;

        // parameters for target scale
        var initialScale = target.localScale;
        var finalScale = targetScale;

        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToFall;
            pickUp.localPosition = Vector3.Slerp(initialPos, finalPos, t);
            target.localScale = Vector3.Slerp(initialScale, finalScale, t);
            yield return new WaitForEndOfFrame();
        }

        target.gameObject.SetActive(false);
    }
}
