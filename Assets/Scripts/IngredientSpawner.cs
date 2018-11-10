using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
	public int ingredientsByBatch;
	public int currentBatch = 0;
	public List<PlantEffect.Ingredient>[] ingredientsLists;

	// Use this for initialization
	void Start ()
	{
		
	}

	public void InitIngredientLists()
	{
		ingredientsLists = new List<PlantEffect.Ingredient>[RecipeManager.instance.currentRecipeSize];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
