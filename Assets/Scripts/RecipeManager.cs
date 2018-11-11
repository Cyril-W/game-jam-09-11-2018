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
    [SerializeField] float minTimeForRecipe = 20f;
    [SerializeField] float timeAddedPerIngredient = 5f;

    CauldronManager cauldron;
	IngredientSpawner ingredientSpawner;

	// Use this for initialization
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

        // set time
        timerManager.timeBeforeNextRecipe = minTimeForRecipe + timeAddedPerIngredient * recipe.Length; // divide by number of player

        return recipe;
	}
}
