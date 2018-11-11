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

    [Space]

    [SerializeField] GameObject particleCurseSlippery;
    [SerializeField] GameObject particleCurseInvertMovementX;
    [SerializeField] GameObject particleCurseInvertMovementY;
    [SerializeField] GameObject particleCurseInvertMovements;
    [SerializeField] GameObject particleCurseMoveSpeedIncrease;
    [SerializeField] GameObject particleCurseMoveSpeedDecrease;

    //int currentPlayerModel = 0;

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
				StartCoroutine(ActivateColliderAfter(currentlyHeldIngredient.gameObject));
			} else
            {
                SetTrigger("Take");
            }
			currentlyHeldIngredient = other.GetComponent<PlantEffect>();
			currentlyHeldIngredient.OnPlantCollect(gameObject);
            UpdateParticleCurse(currentlyHeldIngredient.GetIngredient());
            audioSourceThrow.PlayOneShot(audioSourceThrow.clip);            
        }
        else if (other.tag == cauldronTag && currentlyHeldIngredient != null)
		{
			currentlyHeldIngredient.OnPlantDropInCauldron(gameObject);
			currentlyHeldIngredient = null;
            DisableAllCurses();
            audioSourceThrow.PlayOneShot(audioSourceThrow.clip);
			SetTrigger("Throw");
		}
	}

    void UpdateParticleCurse(PlantEffect.Ingredient currentlyHeldIngredient)
    {
        DisableAllCurses();
        switch (currentlyHeldIngredient.curseType)
        {
            case CurseManager.CurseType.Slippery:
                particleCurseSlippery.SetActive(true);
                break;
            case CurseManager.CurseType.InvertMovementX:
                particleCurseInvertMovementX.SetActive(true);
                break;
            case CurseManager.CurseType.InvertMovementY:
                particleCurseInvertMovementY.SetActive(true);
                break;
            case CurseManager.CurseType.InvertMovements:
                particleCurseInvertMovements.SetActive(true);
                break;
            case CurseManager.CurseType.MoveSpeedIncrease:
                particleCurseMoveSpeedIncrease.SetActive(true);
                break;
            case CurseManager.CurseType.MoveSpeedDecrease:
                particleCurseMoveSpeedDecrease.SetActive(true);
                break;
            default:
                break;
        }
    }

    void DisableAllCurses()
    {
        particleCurseSlippery.SetActive(false);
        particleCurseInvertMovementX.SetActive(false);
        particleCurseInvertMovementY.SetActive(false);
        particleCurseInvertMovements.SetActive(false);
        particleCurseMoveSpeedIncrease.SetActive(false);
        particleCurseMoveSpeedDecrease.SetActive(false);
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
