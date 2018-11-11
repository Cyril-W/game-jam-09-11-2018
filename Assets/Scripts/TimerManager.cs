using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {

    public float time = 0f;
    public float timeBeforeNextRecipe = 5f;

    [SerializeField] CauldronManager cauldronManager;
    [SerializeField] Transform hourglass;
    [SerializeField] Image hourglassUp;
    [SerializeField] Image hourglassDown;
    [SerializeField] float timeToFlipHourglass = 0.1f;

    private void OnEnable()
    {
        time = 0f;
        UpdateFillAmounts();
    }
   
    void Update () {
        time += Time.deltaTime;
        if (time > timeBeforeNextRecipe)
        {
            //StartCoroutine(FlipHourGlass());
            time = 0f;
            cauldronManager.CauldronFailed();
        }
        UpdateFillAmounts();
    }

    void UpdateFillAmounts()
    {
        hourglassUp.fillAmount = (timeBeforeNextRecipe - time) / timeBeforeNextRecipe;
        hourglassDown.fillAmount = time / timeBeforeNextRecipe;
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
