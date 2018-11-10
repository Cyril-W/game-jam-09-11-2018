﻿using System.Collections;
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
			Debug.Log("Good Ingredient put inside !");
			//ADD FEEDBACK HERE
			if(index < RecipeManager.instance.currentRecipeSize)
			{
				index++;
			}
			else
			{
				Debug.Log("Recipe complete !");
				RecipeManager.instance.IncreaseRecipeSize(true);
			}
		}
		else
		{
			Debug.Log("Bad Ingredient put inside !");
			CurseManager.instance.CurseAllPlayers(ingredient.curseType);
			RecipeManager.instance.GenerateRandomRecipe();
			index = 0;
		}
	}
	

}
