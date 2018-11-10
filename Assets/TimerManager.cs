using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {

    [Range(0, 1)]
    public float time = 0f;

    [Space]

    [SerializeField] Text timeCurrent;
    [SerializeField] Text timeLeft;

    [Space]

    [SerializeField] Transform hourglass;
    [SerializeField] Image hourglassUp;
    [SerializeField] Image hourglassDown;
    [SerializeField] float timeToFlipHourglass = 0.1f;

	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time > 1f)
        {
            StartCoroutine(FlipHourGlass());
            time = 0f;
        }
        UpdateFillAmounts();
        UpdateTexts();
    }

    void UpdateFillAmounts()
    {
        hourglassUp.fillAmount = 1f - time;
        hourglassDown.fillAmount = time;
    }

    void UpdateTexts()
    {
        timeCurrent.text = time.ToString("F2");
        timeLeft.text = "1";
    }

    IEnumerator FlipHourGlass()
    {
        var initialRot = hourglass.localRotation;
        var finalRot = Quaternion.AngleAxis(180f, Vector3.forward) * initialRot;

        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToFlipHourglass;
            hourglass.localRotation = Quaternion.Slerp(initialRot, finalRot, t);
            yield return new WaitForEndOfFrame();
        }

        hourglassUp.transform.localRotation *= Quaternion.AngleAxis(180f, Vector3.forward);
        hourglassDown.transform.localRotation *= Quaternion.AngleAxis(180f, Vector3.forward);
    }
}
