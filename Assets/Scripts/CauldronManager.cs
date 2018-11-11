using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronManager : MonoBehaviour
{
    [SerializeField] Animator cauldronAnimator;
    [SerializeField] ParticleSystem cauldronParticlesSmoke;
    [SerializeField] AudioSource cauldronAudioSource;
    [SerializeField] AudioClip cauldronGood;
    [SerializeField] AudioClip cauldronBad;
    [SerializeField] AudioClip cauldronApplause;

    [Space]

    [SerializeField] Animator recipeAnimator;
    [SerializeField] Transform recipeUI;
    [SerializeField] Transform ingredientUIPrefab;

    int index = 0;
	PlantEffect.Ingredient[] recipe;
    List<IngredientUI> ingredientsUI = new List<IngredientUI>();

    public void SetCauldronRecipe(PlantEffect.Ingredient[] newRecipe)
    {
        if (newRecipe.Length > 0)
        {
            recipe = newRecipe;

            for (var i = 0; i < recipe.Length; i++)
            {
                var ingredient = recipe[i];
                var newIngredient = Instantiate(ingredientUIPrefab, recipeUI);
                var newIngredientUI = newIngredient.GetComponent<IngredientUI>();
                ingredientsUI.Add(newIngredientUI);
                newIngredientUI.DisplayIngredient(ingredient);
                if (i == 0)
                {
                    newIngredientUI.ToggleSelected();
                }
            }

            recipeAnimator.SetTrigger("Open");
        }
    }

	//Check if plant is similar to the one we put inside the cauldron, and cast a curse otherwise
	public void CheckPlant(PlantEffect.Ingredient ingredient)
	{
		if(recipe[index].ingredientType == ingredient.ingredientType && recipe[index].ingredientColor == ingredient.ingredientColor)
		{
	        cauldronAudioSource.PlayOneShot(cauldronGood);
            cauldronAnimator.Play("Happy");
            cauldronParticlesSmoke.Stop();

            if (index < RecipeManager.instance.currentRecipeSize)
			{
                ingredientsUI[index].ToggleSelected();
                index++;
                ingredientsUI[index].ToggleSelected();
            }
			else
			{
				index = 0;

				foreach (var item in ingredientsUI)
                {
                    Destroy(item.gameObject);
                }
                ingredientsUI.Clear();
                recipeAnimator.SetTrigger("Close");
                cauldronAudioSource.PlayOneShot(cauldronApplause);
                RecipeManager.instance.IncreaseRecipeSize(true);
			}
		}
		else
		{
            CurseManager.instance.DoomPlayers();
            cauldronAudioSource.PlayOneShot(cauldronBad);
            cauldronAnimator.Play("Sad");
            cauldronParticlesSmoke.Play();

            CauldronFailed();
        }
	}
	
    public void CauldronFailed()
    {
        foreach (var item in ingredientsUI)
        {
            Destroy(item.gameObject);
        }
        ingredientsUI.Clear();
        recipeAnimator.SetTrigger("Close");      
        SetCauldronRecipe(RecipeManager.instance.GenerateRandomRecipe());
        index = 0;
    }
}
