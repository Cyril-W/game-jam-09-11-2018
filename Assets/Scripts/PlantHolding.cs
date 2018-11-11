using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHolding : MonoBehaviour
{
	public string ingredientTag = "Ingredient";
	public string cauldronTag = "Cauldron";
	public float pickupCooldown = 1f;
	PlantEffect currentlyHeldIngredient;

    [SerializeField] Animator[] charactersAnimator;
    [SerializeField] AudioSource audioSourceThrow;

    int currentPlayerModel = 0;

    void Update()
    {/*
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UpdatePlayerModel(currentPlayerModel, false);
            currentPlayerModel++;
            if (currentPlayerModel >= charactersAnimator.Length)
            {
                currentPlayerModel = 0;
            }
            UpdatePlayerModel(currentPlayerModel, true);
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            UpdatePlayerModel(currentPlayerModel, false);
            currentPlayerModel--;
            if (currentPlayerModel < 0)
            {
                currentPlayerModel = charactersAnimator.Length - 1;
            }
            UpdatePlayerModel(currentPlayerModel, true);
        }*/
    }

    public void UpdatePlayerModel(int modelIndex, bool isActive)
    {
        charactersAnimator[modelIndex].gameObject.SetActive(isActive);
    }

    void SetTrigger()
    {
        foreach (var animator in charactersAnimator)
        {
            if (animator.gameObject.activeSelf)
            {
                animator.SetTrigger("Take");
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
				StartCoroutine(ActivateColliderAfter(currentlyHeldIngredient.gameObject));
			} else
            {
                SetTrigger();
            }
			currentlyHeldIngredient = other.GetComponent<PlantEffect>();
			currentlyHeldIngredient.OnPlantCollect(gameObject);
            audioSourceThrow.PlayOneShot(audioSourceThrow.clip);            
        }
        else if (other.tag == cauldronTag && currentlyHeldIngredient != null)
		{
			currentlyHeldIngredient.OnPlantDropInCauldron(gameObject);
			currentlyHeldIngredient = null;
            audioSourceThrow.PlayOneShot(audioSourceThrow.clip);
            SetTrigger();
        }
	}

	IEnumerator ActivateColliderAfter(GameObject pickup)
	{
		yield return new WaitForSeconds(pickupCooldown);
		pickup.GetComponent<SphereCollider>().enabled = true;
	}
}
