using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
	public GameObject pickup;
	public int ingredientsByBatch = 5;
	public int currentBatch = 0;
	public PlantEffect.Ingredient[,] ingredientsLists;

	[Header("Spawning Options")]
	public Vector2 initialSpawnPositions = new Vector2(5f, 5f);
	public float minDistanceToCauldron = 2f;
	public float minDistanceToPickups = 1f;
	public Vector2 minPos;
	public Vector2 maxPos;
	public Transform cauldron;

	Vector3[] itemPositions;
	int batchAmount = 0;

	// Use this for initialization
	void Start ()
	{
		if(cauldron == null)
		{
			cauldron = GameObject.FindGameObjectWithTag("Cauldron").transform;
		}
	}

	Vector3 GetSpawningPosition(int index)
	{
		bool isSomethingNearby = false;
		Vector3 targetSpawnPos;
		do
		{
			isSomethingNearby = false;
			float xPos = Random.Range(minPos.x, maxPos.x);
			float zPos = Random.Range(minPos.y, maxPos.y);
			targetSpawnPos = new Vector3(xPos, 0f, zPos);

			if(index > 0)
			{
				for(int i = 0; i<index; i++)
				{
					if (Vector3.Distance(targetSpawnPos, itemPositions[i]) < minDistanceToPickups)
					{
						isSomethingNearby = true;
					}
				}
			}
			if(Vector3.Distance(targetSpawnPos, cauldron.position) < minDistanceToCauldron)
			{
				isSomethingNearby = true;
			}
		}
		while (isSomethingNearby == true);
		itemPositions[index] = targetSpawnPos;
		return targetSpawnPos;
	}

	public void InitIngredientLists (int recipeSize)
	{
		ingredientsLists = new PlantEffect.Ingredient[recipeSize, ingredientsByBatch];
		batchAmount = recipeSize;
	}

	public void SpawnIngredients()
	{
		itemPositions = new Vector3[ingredientsByBatch];
		for (int j = 0; j < ingredientsByBatch; j++)
		{
			Vector3 targetSpawnPos = GetSpawningPosition(j);
			Vector3 initialSpawnPos = new Vector3(targetSpawnPos.x, initialSpawnPositions.x, initialSpawnPositions.y);
			GameObject spawnedPickup = Instantiate(pickup, targetSpawnPos, Quaternion.identity);
			spawnedPickup.GetComponentInChildren<PlantEffect>().ingredient = ingredientsLists[currentBatch, j];
		}
		currentBatch++;
	}


	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			if (currentBatch < batchAmount)
			{
				SpawnIngredients();
			}
		}
	}
}
