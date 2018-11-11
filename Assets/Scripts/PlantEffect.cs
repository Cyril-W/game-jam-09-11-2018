using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEffect : MonoBehaviour
{
	[System.Serializable]
	public class Ingredient
	{
		public enum IngredientType { Rat, Frog, Leg, Doll };
		public IngredientType ingredientType;
		[Range(0,3)]
		public int ingredientColor = 0;
		public CurseManager.CurseType curseType;
	}
    	
	public Vector3 offsetFromPlayer = new Vector3(0f, 3f, 0f);

    [Space]

    [SerializeField] GameObject[] rats;
    [SerializeField] GameObject[] frogs;
    [SerializeField] GameObject[] legs;
    [SerializeField] GameObject[] dolls;

    Ingredient ingredient;
    CauldronManager cauldron;

	private void Awake ()
	{
		cauldron = FindObjectOfType<CauldronManager>();
    }

    public Ingredient GetIngredient()
    {
        return ingredient;
    }

    public void SetIngredient(Ingredient newIngredient)
    {
        if (newIngredient == null)
        {
            Debug.LogError("new ingredient is null");
            return;
        }

        ingredient = newIngredient;

        switch (ingredient.ingredientType)
        {
            case Ingredient.IngredientType.Rat:
                rats[ingredient.ingredientColor].SetActive(true);
                break;
            case Ingredient.IngredientType.Frog:
                frogs[ingredient.ingredientColor].SetActive(true);
                break;
            case Ingredient.IngredientType.Leg:
                legs[ingredient.ingredientColor].SetActive(true);
                break;
            case Ingredient.IngredientType.Doll:
                dolls[ingredient.ingredientColor].SetActive(true);
                break;
            default:
                break;
        }
    }

	public void OnPlantCollect(GameObject player)
	{
		CurseManager.instance.CursePlayer(player, ingredient.curseType);
		transform.parent = player.transform;
		transform.localPosition = offsetFromPlayer;
	}

	public void OnPlantDropInCauldron (GameObject player)
	{
        CurseManager.instance.UnCursePlayer(player);
		cauldron.CheckPlant(ingredient);
        // play anim then destroy
		Destroy(gameObject);
	}
}
