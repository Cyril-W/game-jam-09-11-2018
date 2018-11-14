using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronManager : MonoBehaviour
{
	[SerializeField] Animator cauldronAnimator;
	[SerializeField] ParticleSystem cauldronParticlesSmoke;
	[SerializeField] AudioSource cauldronAudioSource;
	[SerializeField] AudioClip cauldronGood;
	[SerializeField] AudioClip cauldronBad;
	[SerializeField] AudioClip cauldronApplause;
	[SerializeField] CameraAnimator cameraAnimator;
    [SerializeField] Text textScore;

    [Space]

	[SerializeField] Animator recipeAnimator;
	[SerializeField] Transform recipeUI;
	[SerializeField] Transform ingredientUIPrefab;

	[SerializeField] float explosionRange = 5f;
	[SerializeField] LayerMask explosionMask;
	[SerializeField] float explosionForce = 500f;
	[SerializeField] float upwardsModifier = 3f;

    [SerializeField] float secondsBetweenSpawn = 0.1f;

    int score = 0;
	int index = 0;
	PlantEffect.Ingredient[] recipe;
	List<IngredientUI> ingredientsUI = new List<IngredientUI>();

	public void SetCauldronRecipe (PlantEffect.Ingredient[] newRecipe)
	{
        DestroyAllItems();
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
        StartCoroutine(NewBatchs());
    }

	//Check if plant is similar to the one we put inside the cauldron, and cast a curse otherwise
	public void CheckPlant (PlantEffect.Ingredient ingredient)
	{
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
                score++;
                UpdateScore();
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

    void UpdateScore()
    {
        textScore.text = score.ToString();
        PlayerPrefs.SetInt("score", score);
        if (!PlayerPrefs.HasKey("highestScore") || score > PlayerPrefs.GetInt("highestScore"))
        {
            PlayerPrefs.SetInt("highestScore", score);
        }
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
            cameraAnimator.Shake();
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

	/*public IEnumerator MoveCauldron (Vector3 targetPos)
	{
		do
		{
			Vector3 dir = targetPos - transform.position;
			dir.y = 0f;
			transform.Translate(dir * 500f * Time.deltaTime);
			yield return null;
		}
		while (Vector3.Distance(transform.position, targetPos) > 0.1f);

		StartCoroutine(NewBatchs());
	}*/

	public IEnumerator NewBatchs ()
	{
        Debug.Log("new batch");
        var secondsBetweenBatch = TimerManager.instance.GetTime() / recipe.Length;
        for (int i = 0; i < recipe.Length; i++)
        {
            IngredientSpawner spawner = FindObjectOfType<IngredientSpawner>();
            spawner.SpawnIngredients();
            for (int j = 0; j < spawner.ingredientsByBatch; j++)
            {
                spawner.ThrowPickup();
                yield return new WaitForSeconds(secondsBetweenSpawn);
            }
            yield return new WaitForSeconds(secondsBetweenBatch);
        }
	}
}
