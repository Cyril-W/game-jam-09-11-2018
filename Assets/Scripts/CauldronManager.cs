using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronManager : MonoBehaviour
{
	public PlantEffect.Ingredient[] recipe;

	int index = 0;

	//Check if plant is similar to the one we put inside the cauldron, and cast a curse otherwise
	public void CheckPlant(PlantEffect.Ingredient ingredient)
	{
		if(recipe[index].ingredientType == ingredient.ingredientType && recipe[index].ingredientColor == ingredient.ingredientColor)
		{
			//ADD FEEDBACK HERE
			index++;
		}
		else
		{
			CurseManager.instance.CurseAllPlayers(ingredient.curseType);
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
