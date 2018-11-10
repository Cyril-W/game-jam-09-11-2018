using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHolding : MonoBehaviour
{
	public string ingredientTag = "Ingredient";
	public string cauldronTag = "Cauldron";
	PlantEffect currentlyHeldIngredient;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter (Collider other)
	{
		if(other.tag == ingredientTag)
		{
			currentlyHeldIngredient = other.GetComponent<PlantEffect>();
			currentlyHeldIngredient.OnPlantCollect(gameObject);
		}
		if (other.tag == cauldronTag)
		{
			if(currentlyHeldIngredient != null)
			{
				currentlyHeldIngredient.OnPlantDropInCauldron(gameObject);
				currentlyHeldIngredient = null;
			}
		}
	}
}
