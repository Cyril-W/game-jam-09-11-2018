using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEffect : MonoBehaviour
{
	[System.Serializable]
	public class Ingredient
	{
		public enum IngredientType { Rat, Frog, Arm, Doll };
		public IngredientType ingredientType;
		[Range(1,4)]
		public int ingredientColor = 1;
		public CurseManager.CurseType curseType;
	}

	public Ingredient ingredient;
	public Vector3 offsetFromPlayer = new Vector3(0f, 3f, 0f);
	CauldronManager cauldron;

	private void Awake ()
	{
		cauldron = FindObjectOfType<CauldronManager>();
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
		CurseManager.instance.UnCursePlayer(player, ingredient.curseType);
		cauldron.CheckPlant(ingredient);
		Destroy(gameObject);
	}
}
