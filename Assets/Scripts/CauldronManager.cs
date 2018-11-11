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

	[SerializeField] float explosionRange = 5f;
	[SerializeField] LayerMask explosionMask;
	[SerializeField] float explosionForce = 500f;
	[SerializeField] float upwardsModifier = 3f;



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
		StartCoroutine(NewBatch());
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

	public IEnumerator DisablePlayerMovement(PlayerMovement player)
	{
		player.enabled = false;
		yield return new WaitForSeconds(1f);
		player.enabled = true;
	}
	
    public void CauldronFailed()
    {
		Collider[] players = Physics.OverlapSphere(transform.position, explosionRange, explosionMask);
		foreach(Collider col in players)
		{
			Rigidbody rigid = col.GetComponent<Rigidbody>();
			PlayerMovement playerMvt = col.GetComponent<PlayerMovement>();
			if(playerMvt != null && rigid != null)
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

	void DestroyAllItems()
	{
		PlantEffect[] plants = FindObjectsOfType<PlantEffect>();
	}

	public IEnumerator NewBatch()
	{
		IngredientSpawner spawner = FindObjectOfType<IngredientSpawner>();
		spawner.SpawnIngredients();
		for(int i = 0; i< spawner.ingredientsByBatch; i++)
		{
			spawner.ThrowPickup();
			yield return new WaitForSeconds(0.1f);

		}
	}
}
