using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
	public GameObject pickup;
	public int ingredientsByBatch = 5;
	public int currentBatch = 0;
	public PlantEffect.Ingredient[,] ingredientsLists;
	int batchAmount = 0;
	// Use this for initialization
	void Start ()
	{
	}

	public void InitIngredientLists (int recipeSize)
	{
		Debug.Log("Initialized lists");
		Debug.Log("CurrentRecipeSize : " + recipeSize);
		ingredientsLists = new PlantEffect.Ingredient[recipeSize, ingredientsByBatch];
		batchAmount = recipeSize;
	}


	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			if (currentBatch < batchAmount)
			{
				for (int j = 0; j < ingredientsByBatch; j++)
				{
					GameObject spawnedPickup = Instantiate(pickup, transform.position, transform.rotation);
					spawnedPickup.GetComponentInChildren<PlantEffect>().ingredient = ingredientsLists[currentBatch, j];
				}
				currentBatch++;
			}
		}
	}
}
