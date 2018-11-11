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
	[SerializeField] Animator cameraAnimator;

	[Space]

	[SerializeField] Animator recipeAnimator;
	[SerializeField] Transform recipeUI;
	[SerializeField] Transform ingredientUIPrefab;

	[SerializeField] float explosionRange = 5f;
	[SerializeField] LayerMask explosionMask;
	[SerializeField] float explosionForce = 500f;
	[SerializeField] float upwardsModifier = 3f;



	int index = 0;
	PlantEffect.Ingredient[] recipe;
	List<IngredientUI> ingredientsUI = new List<IngredientUI>();

	public void SetCauldronRecipe (PlantEffect.Ingredient[] newRecipe)
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
	public void CheckPlant (PlantEffect.Ingredient ingredient)
	{
		//Debug.Log("Index : " + index);
		//Debug.Log("Recipe Size : " + RecipeManager.instance.currentRecipeSize);
		if (recipe[index].ingredientType == ingredient.ingredientType && recipe[index].ingredientColor == ingredient.ingredientColor)
		{
			cauldronAudioSource.PlayOneShot(cauldronGood);
			cauldronAnimator.Play("Happy");
			cauldronParticlesSmoke.Stop();

			if (index < RecipeManager.instance.currentRecipeSize-1)
			{
				ingredientsUI[index].ToggleSelected();
				index++;
				ingredientsUI[index].ToggleSelected();
				//StartCoroutine(NewBatch());
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
				//Debug.Log("Increasing Recipe Size");
				RecipeManager.instance.IncreaseRecipeSize(true);
				//StartCoroutine(NewBatch());
				//StartCoroutine(MoveCauldron(IngredientSpawner.instance.GetRandomPointInBounds(IngredientSpawner.instance.targetsSpawningZone.bounds)));
			}

		}
		else
		{
			CurseManager.instance.DoomPlayers();
			cauldronAudioSource.PlayOneShot(cauldronBad);
			cauldronAnimator.Play("Sad");
			cauldronParticlesSmoke.Play();

			CauldronFailed();
			//StartCoroutine(NewBatch());
		}
		StartCoroutine(NewBatch());
	}

	public IEnumerator DisablePlayerMovement (PlayerMovement player)
	{
		player.enabled = false;
		yield return new WaitForSeconds(1f);
		player.enabled = true;
	}

	public void CauldronFailed ()
	{
		if (cameraAnimator != null)
		{
			cameraAnimator.SetTrigger("Shake1");
		}

		Collider[] players = Physics.OverlapSphere(transform.position, explosionRange, explosionMask);
		foreach (Collider col in players)
		{
			Rigidbody rigid = col.GetComponent<Rigidbody>();
			PlayerMovement playerMvt = col.GetComponent<PlayerMovement>();
			if (playerMvt != null && rigid != null)
			{
				StartCoroutine(DisablePlayerMovement(playerMvt));
				rigid.AddExplosionForce(explosionForce, transform.position, explosionRange, upwardsModifier);
			}
		}

		foreach (var item in ingredientsUI)
		{
			Destroy(item.gameObject);
		}
		ingredientsUI.Clear();
		recipeAnimator.SetTrigger("Close");
		SetCauldronRecipe(RecipeManager.instance.GenerateRandomRecipe());
		index = 0;
	}

	void DestroyAllItems ()
	{
		PlantEffect[] plants = FindObjectsOfType<PlantEffect>();
		foreach (PlantEffect plant in plants)
		{
			Destroy(plant.gameObject);
		}

		PlantHolding[] players = FindObjectsOfType<PlantHolding>();
		foreach (PlantHolding player in players)
		{
			CurseManager.instance.UnCursePlayer(player.gameObject);
			player.ResetAnimation();
			player.currentlyHeldIngredient = null;
		}
	}

	public IEnumerator MoveCauldron (Vector3 targetPos)
	{
		//Debug.Log("Moving Initialized");
		//Debug.Log("Target Pos : " + targetPos);
		//Debug.Log("TransformPos : " + transform.position);
		do
		{
			//Debug.Log("TransformPos : " + transform.position);
			//Debug.Log("Moving Cauldron");
			Vector3 dir = targetPos - transform.position;
			dir.y = 0f;
			transform.Translate(dir * 500f * Time.deltaTime);
			yield return null;
		}
		while (Vector3.Distance(transform.position, targetPos) > 0.1f);
		//Debug.Log("Moving Over");

		StartCoroutine(NewBatch());
	}

	public IEnumerator NewBatch ()
	{
		//Debug.Log("New Batch !");
		DestroyAllItems();
		IngredientSpawner spawner = FindObjectOfType<IngredientSpawner>();
		spawner.SpawnIngredients();
		for (int i = 0; i < spawner.ingredientsByBatch; i++)
		{
			spawner.ThrowPickup();
			yield return new WaitForSeconds(0.1f);

		}
	}
}
