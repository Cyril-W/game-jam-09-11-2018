using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHolding : MonoBehaviour
{
	public string ingredientTag = "Ingredient";
	public string cauldronTag = "Cauldron";
	public float pickupCooldown = 1f;
	PlantEffect currentlyHeldIngredient;

	private void OnTriggerEnter (Collider other)
	{
		if (other.tag == ingredientTag)
		{
			other.GetComponent<SphereCollider>().enabled = false;
			if (currentlyHeldIngredient != null)
			{
				CurseManager.instance.UnCursePlayer(gameObject, currentlyHeldIngredient.GetIngredient().curseType);
				currentlyHeldIngredient.transform.localPosition = Vector3.zero;
				currentlyHeldIngredient.transform.parent = null;
				StartCoroutine(ActivateColliderAfter(currentlyHeldIngredient.gameObject));
			}
			currentlyHeldIngredient = other.GetComponent<PlantEffect>();
			currentlyHeldIngredient.OnPlantCollect(gameObject);
		}
		if (other.tag == cauldronTag)
		{
			if (currentlyHeldIngredient != null)
			{
				currentlyHeldIngredient.OnPlantDropInCauldron(gameObject);
				currentlyHeldIngredient = null;
			}
		}
	}

	IEnumerator ActivateColliderAfter(GameObject pickup)
	{
		yield return new WaitForSeconds(pickupCooldown);
		pickup.GetComponent<SphereCollider>().enabled = true;
	}
}
