using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientUI : MonoBehaviour {

    [SerializeField] GameObject selectedCircle;

    [SerializeField] GameObject[] rats;
    [SerializeField] GameObject[] frogs;
    [SerializeField] GameObject[] legs;
    [SerializeField] GameObject[] dolls;

    bool isSelected = false;

    public void DisplayIngredient(PlantEffect.Ingredient newIngredient)
    {
        if (newIngredient == null)
        {
            Debug.LogError("new ingredient is null");
            return;
        }

        switch (newIngredient.ingredientType)
        {
            case PlantEffect.Ingredient.IngredientType.Rat:
                rats[newIngredient.ingredientColor].SetActive(true);
                break;
            case PlantEffect.Ingredient.IngredientType.Frog:
                frogs[newIngredient.ingredientColor].SetActive(true);
                break;
            case PlantEffect.Ingredient.IngredientType.Leg:
                legs[newIngredient.ingredientColor].SetActive(true);
                break;
            case PlantEffect.Ingredient.IngredientType.Doll:
                dolls[newIngredient.ingredientColor].SetActive(true);
                break;
            default:
                break;
        }
    }

    public void ToggleSelected ()
    {
        isSelected = !isSelected;
        selectedCircle.SetActive(isSelected);
    }
}
