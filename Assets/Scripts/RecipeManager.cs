using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
	#region Singleton
	public static RecipeManager instance;
	private void Awake ()
	{
		instance = this;
	}
	#endregion

	public int startRecipeSize = 3;
	public int recipeSizeIncByLevel = 1;
	public int currentRecipeSize = 3;

	public int colorAmount;

    [Space]

    [SerializeField] TimerManager timerManager;
    [SerializeField] string recipeTime = "recipeTime";
    [SerializeField] string ingredientTime = "ingredientTime";
    [SerializeField] float defaultRecipeTime = 20f;
    [SerializeField] float defaultIngredientTime = 5f;

    CauldronManager cauldron;
	IngredientSpawner ingredientSpawner;

	void Start ()
	{
		currentRecipeSize = startRecipeSize;
		cauldron = FindObjectOfType<CauldronManager>();
		cauldron.SetCauldronRecipe(GenerateRandomRecipe());
		cauldron.StartCoroutine(cauldron.NewBatch());
	}

	public void IncreaseRecipeSize (bool loadNewRecipe)
	{
		currentRecipeSize += recipeSizeIncByLevel;
		if(loadNewRecipe)
		{    
			cauldron.SetCauldronRecipe(GenerateRandomRecipe());
		}
	}

	public void AddRecipeToDropList(PlantEffect.Ingredient recipe, int index)
	{
		ingredientSpawner.ingredientsLists[ingredientSpawner.currentBatch, index] = recipe;
	}

	public PlantEffect.Ingredient GenerateRandomIngredient()
	{
		PlantEffect.Ingredient ingredient = new PlantEffect.Ingredient();

		int randomColor = Random.Range(0, colorAmount);
		ingredient.ingredientColor = randomColor;

        var ingredientTypes = System.Enum.GetNames(typeof(PlantEffect.Ingredient.IngredientType)).Length;
        int randomType = Random.Range(0, ingredientTypes);
        ingredient.ingredientType = (PlantEffect.Ingredient.IngredientType)randomType;

        var curseTypes = System.Enum.GetNames(typeof(CurseManager.CurseType)).Length;
        CurseManager.CurseType randomCurse = (CurseManager.CurseType)Random.Range(0, curseTypes);
        ingredient.curseType = randomCurse;

		return ingredient;
	}

	public PlantEffect.Ingredient[] GenerateRandomRecipe ()
	{
		ingredientSpawner = FindObjectOfType<IngredientSpawner>();
		ingredientSpawner.InitIngredientLists(currentRecipeSize);
		PlantEffect.Ingredient[] recipe = new PlantEffect.Ingredient[currentRecipeSize];
		for(int i = 0; i<currentRecipeSize; i++)
		{
			ingredientSpawner.currentBatch = i;
			recipe[i] = GenerateRandomIngredient();
			AddRecipeToDropList(recipe[i], 0);
			for(int j = 1; j < ingredientSpawner.ingredientsByBatch; j++)
			{
				AddRecipeToDropList(GenerateRandomIngredient(), j);
			}
		}
		ingredientSpawner.currentBatch = 0;

        var timePerRecipe = PlayerPrefs.HasKey(recipeTime) ? PlayerPrefs.GetFloat(recipeTime) : defaultRecipeTime;
        var timePerIngredient = PlayerPrefs.HasKey(ingredientTime) ? PlayerPrefs.GetFloat(ingredientTime) : defaultIngredientTime;
        var newTime = timePerRecipe + timePerIngredient * recipe.Length; // divide by number of player
        timerManager.SetNewTime(newTime); 

        return recipe;
	}
}
