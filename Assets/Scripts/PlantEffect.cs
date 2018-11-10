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

	public Ingredient ingredient;
	public Vector3 offsetFromPlayer = new Vector3(0f, 3f, 0f);

    [Space]

    [SerializeField] GameObject[] rats;
    [SerializeField] GameObject[] frogs;
    [SerializeField] GameObject[] legs;
    [SerializeField] GameObject[] dolls;

    [Space]
    [SerializeField] AudioSource throwCauldronAudioSource;

    CauldronManager cauldron;

	private void Awake ()
	{
		cauldron = FindObjectOfType<CauldronManager>();
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
		// Place Item on top of player + play anim
		transform.parent = player.transform;
		transform.localPosition = offsetFromPlayer;
	}

	public void OnPlantDropInCauldron (GameObject player)
	{
        throwCauldronAudioSource.PlayOneShot(throwCauldronAudioSource.clip);
        CurseManager.instance.UnCursePlayer(player, ingredient.curseType);
		cauldron.CheckPlant(ingredient);
		Destroy(gameObject, 0.5f);
	}
}
