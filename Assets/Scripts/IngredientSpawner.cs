using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
	#region Singleton
	public static IngredientSpawner instance;
	private void Awake ()
	{
		instance = this;
	}
	#endregion
	public GameObject pickup;
	public int ingredientsByBatch = 5;
	public int currentBatch = 0;
	public PlantEffect.Ingredient[,] ingredientsLists;

    [Header("Spawning Options")]
    public Vector2 posXPickup;
    public float posZPickup;
    public List<float> posYPickup;

    public float minDistanceToCauldron = 4f;
	public float minDistanceToPickups = 1f;

    public Collider targetsSpawningZone;
    public Transform cauldron;

	List<PickupFall> pickupFalls = new List<PickupFall>();
	int batchAmount = 0;

	// Use this for initialization
	void Start ()
	{
		if(cauldron == null)
		{
			cauldron = GameObject.FindGameObjectWithTag("Cauldron").transform;
		}
	}

	/*Vector3 GetSpawningPosition(int index)
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
	}*/

	public Vector3 GetSpawnPosFarFromCauldron()
	{
		int debug = 0;
		Vector3 testPos = GetRandomPointInBounds(targetsSpawningZone.bounds);
		while(Vector3.Distance(testPos, cauldron.position) < minDistanceToCauldron || debug < 50)
		{
			testPos = GetRandomPointInBounds(targetsSpawningZone.bounds);
			debug++;
		}
		return testPos;
	}

    public Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public void InitIngredientLists (int recipeSize)
	{
		ingredientsLists = new PlantEffect.Ingredient[recipeSize, ingredientsByBatch];
		batchAmount = recipeSize;
	}

	public void SpawnIngredients()
	{
		for (int j = 0; j < ingredientsByBatch; j++)
		{
            // set spawn position x, along the shelf
            float xPos = Random.Range(posXPickup.x, posXPickup.y);

            // set spawn position z, which shelf it is
            // CAREFULL : the position is applied to the pickup renderer, not the whole prefab
            float yPos = posYPickup[Random.Range(0, posYPickup.Count)];

            // set pickup position, always the same z
            Vector3 initialPosPickup = new Vector3(xPos, 0, posZPickup);

			// set target position
			Vector3 targetPos = GetSpawnPosFarFromCauldron(); // GetSpawningPosition(j);

			// spawn pick up
			GameObject spawnedPickup = Instantiate(pickup, initialPosPickup, Quaternion.identity);
            var pickupFall = spawnedPickup.GetComponent<PickupFall>();
            pickupFall.SetPickupHeight(yPos);
            pickupFall.SetTargetPos(targetPos);
            pickupFalls.Add(pickupFall);

            // attribute the ingredient
            spawnedPickup.GetComponentInChildren<PlantEffect>().SetIngredient(ingredientsLists[currentBatch, j]);
		}
		currentBatch++;
	}

    public void ThrowPickup()
    {
        if (pickupFalls.Count > 0)
        {
			int random = Random.Range(0, pickupFalls.Count);
            pickupFalls[random].Throw();
            pickupFalls.RemoveAt(random);
        }
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
		} else if (Input.GetKeyDown(KeyCode.Z))
        {
            ThrowPickup();
        }
	}
}
