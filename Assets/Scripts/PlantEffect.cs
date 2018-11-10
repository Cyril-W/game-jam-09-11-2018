using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEffect : MonoBehaviour, IPlantCollect
{
	[System.Serializable]
	public class Ingredient
	{
		public enum IngredientType { Rat, Frog, Arm, Doll };
		public IngredientType ingredientType;
		[Range(1,4)]
		public int ingredientColor;
		public CurseManager.CurseType curseType;
	}

	public Ingredient ingredient;

	public void OnPlantCollect()
	{

	}

	public void OnPlantDropInCauldron ()
	{

	}
}
