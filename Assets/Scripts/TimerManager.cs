using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour {
    #region Singleton
    public static TimerManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] CauldronManager cauldronManager;
    [SerializeField] Transform hourglass;
    [SerializeField] Image hourglassUp;
    [SerializeField] Image hourglassDown;
    [SerializeField] float timeToFlipHourglass = 0.1f;
    [SerializeField] AudioSource audioSource;

    float time = 0f;
    float timeBeforeNextRecipe = 10000f;

    void OnEnable()
    {
        time = 0f;
        UpdateFillAmounts();
    }
   
    void Update () {
        time += Time.deltaTime;
        if (time > timeBeforeNextRecipe)
        {
            time = 0f;
            SceneManager.LoadScene("LostScene");
        }
        else if (time >= timeBeforeNextRecipe - audioSource.clip.length && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        UpdateFillAmounts();
    }

    void UpdateFillAmounts()
    {
        hourglassUp.fillAmount = (timeBeforeNextRecipe - time) / timeBeforeNextRecipe;
        hourglassDown.fillAmount = time / timeBeforeNextRecipe;
    }

    public void SetNewTime(float newTime)
    {
        timeBeforeNextRecipe = newTime;
        time = 0f;
        StartCoroutine(CoroutineFlipHourGlass());
    }

    public float GetTime()
    {
        return timeBeforeNextRecipe;
    }

    IEnumerator CoroutineFlipHourGlass()
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
