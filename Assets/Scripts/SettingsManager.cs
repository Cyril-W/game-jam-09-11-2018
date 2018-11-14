using UnityEngine.UI;
using UnityEngine;

public class SettingsManager : MonoBehaviour {

    [SerializeField] Animator settingsPopUp;
    [SerializeField] string triggerName = "PopUp";

    [Space]

    [SerializeField] InputField inputFieldRecipe;
    [SerializeField] InputField inputFieldIngredient;
    [SerializeField] string recipeTime = "recipeTime";
    [SerializeField] string ingredientTime = "ingredientTime";

    [Space]

    [SerializeField] Text warningTextRecipe;
    [SerializeField] Text warningTextIngredient;
    [SerializeField] string warningString = "Not a number!";

    void Start()
    {
        inputFieldRecipe.text = PlayerPrefs.HasKey(recipeTime) ? PlayerPrefs.GetFloat(recipeTime).ToString() : "";
        inputFieldIngredient.text = PlayerPrefs.HasKey(ingredientTime) ? PlayerPrefs.GetFloat(ingredientTime).ToString() : "";
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            QuitApplication();
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void ToggleSettings()
    {
        settingsPopUp.SetTrigger(triggerName);
    }

    public void OnRecipeTimeUpdate(string newTime)
    {
        float newTimeFloat;
        if (float.TryParse(newTime, out newTimeFloat))
        {
            PlayerPrefs.SetFloat(recipeTime, newTimeFloat);
            warningTextRecipe.text = "";
        }
        else
        {
            warningTextRecipe.text = warningString;
        }
    }

    public void OnIngredientTimeUpdate(string newTime)
    {
        float newTimeFloat;
        if (float.TryParse(newTime, out newTimeFloat))
        {
            PlayerPrefs.SetFloat(ingredientTime, newTimeFloat);
            warningTextIngredient.text = "";
        }
        else
        {
            warningTextIngredient.text = warningString;
        }
    }
}
