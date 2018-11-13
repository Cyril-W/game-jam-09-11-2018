using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHolding : MonoBehaviour
{
	public string ingredientTag = "Ingredient";
	public string cauldronTag = "Cauldron";
	public float pickupCooldown = 1f;
	public PlantEffect currentlyHeldIngredient;

    [SerializeField] Animator[] charactersAnimator;
    [SerializeField] AudioSource audioSourceThrow;

    public void UpdatePlayerModel(int modelIndex, bool isActive)
    {
        charactersAnimator[modelIndex].gameObject.SetActive(isActive);
    }

    void SetTrigger(string trigger)
    {
        foreach (var animator in charactersAnimator)
        {
            if (animator.gameObject.activeSelf)
            {
				animator.ResetTrigger("Throw");
				animator.ResetTrigger("Take");
				animator.SetTrigger(trigger);
            }
        }
    }

	public void ResetAnimation ()
	{
		foreach (var animator in charactersAnimator)
		{
			if (animator.gameObject.activeSelf)
			{
				animator.ResetTrigger("Throw");
				animator.ResetTrigger("Take");
				animator.Play("IdleHands");
			}
		}
	}

	public void SetIsWalking(bool newIsWalking)
    {
        foreach (var animator in charactersAnimator)
        {
            if (animator.gameObject.activeSelf)
            {
                animator.SetBool("IsWalking", newIsWalking);
            }
        }
    }

    void OnTriggerEnter (Collider other)
	{
		if (other.tag == ingredientTag)
		{
			other.GetComponent<SphereCollider>().enabled = false;
			if (currentlyHeldIngredient != null)
			{
				CurseManager.instance.UnCursePlayer(gameObject);
				currentlyHeldIngredient.transform.localPosition = Vector3.zero;
				currentlyHeldIngredient.transform.parent = null;
                currentlyHeldIngredient.animator.enabled = true;
                StartCoroutine(ActivateColliderAfter(currentlyHeldIngredient.gameObject));
			} else
            {
                SetTrigger("Take");
            }
			currentlyHeldIngredient = other.GetComponent<PlantEffect>();
			currentlyHeldIngredient.OnPlantCollect(gameObject);
            audioSourceThrow.PlayOneShot(audioSourceThrow.clip);
            currentlyHeldIngredient.animator.enabled = false;
        }
        else if (other.tag == cauldronTag && currentlyHeldIngredient != null)
		{
			currentlyHeldIngredient.OnPlantDropInCauldron(gameObject);
			currentlyHeldIngredient = null;
            audioSourceThrow.PlayOneShot(audioSourceThrow.clip);        
			SetTrigger("Throw");
		}
	}

	IEnumerator ActivateColliderAfter(GameObject pickup)
	{
		yield return new WaitForSeconds(pickupCooldown);
		if(pickup != null)
		{
		    pickup.GetComponent<SphereCollider>().enabled = true;
		}
	}
}
