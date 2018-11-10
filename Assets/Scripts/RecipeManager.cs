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
	public int currentRecipeSize;

	public int colorAmount;

	CauldronManager cauldron;

	// Use this for initialization
	void Start ()
	{
		currentRecipeSize = startRecipeSize;
		cauldron = FindObjectOfType<CauldronManager>();
		cauldron.recipe = GenerateRandomRecipe();
	}

	public void IncreaseRecipeSize (bool loadNewRecipe)
	{
		currentRecipeSize += recipeSizeIncByLevel;
		if(loadNewRecipe)
		{
			cauldron.recipe = GenerateRandomRecipe();
		}
	}

	public void AddRecipeToDropList(PlantEffect.Ingredient recipe)
	{
		//Adds current recipe to the drop table so the right ingredient spawns soon-ish
	}

	public PlantEffect.Ingredient GenerateRandomIngredient()
	{
		PlantEffect.Ingredient recipe = new PlantEffect.Ingredient();

		int randomColor = Random.Range(1, colorAmount+1);
		recipe.ingredientColor = randomColor;

		Debug.Log("Enum Length " + System.Enum.GetNames(typeof(PlantEffect.Ingredient.IngredientType)).Length);

		int randomType = Random.Range(1, System.Enum.GetNames(typeof(PlantEffect.Ingredient.IngredientType)).Length);

		recipe.ingredientType = (PlantEffect.Ingredient.IngredientType)randomType;

		return recipe;
	}

	public PlantEffect.Ingredient[] GenerateRandomRecipe ()
	{
		PlantEffect.Ingredient[] recipe = new PlantEffect.Ingredient[currentRecipeSize];
		for(int i = 0; i<currentRecipeSize; i++)
		{
			recipe[i] = GenerateRandomIngredient();
		}

		return recipe;
	}


		// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			Debug.Log("BOOYA");
			IncreaseRecipeSize(true);
		}
	}
}
