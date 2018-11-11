using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectManagement : MonoBehaviour
{
	public string[] hInputs = new string[4];
	public string[] vInputs = new string[4];

	public GameObject[] characterModels = new GameObject[4];
	GameObject[] spawnedCharacters = new GameObject[4];
	public Transform[] slots = new Transform[4];

	public GameObject startButton;
	bool[] selectedPlayers = {false, false, false, false};
	int[] modelIndexes = { 0, 1, 2, 3 };
	int playerAmount;
	// Use this for initialization

	void DisplayCharacter(int index)
	{
		if(selectedPlayers[index] == false)
		{
			GameObject characterModel = Instantiate(characterModels[modelIndexes[index]], slots[index].position + Vector3.up, Quaternion.identity);
			spawnedCharacters[index] = characterModel;
			playerAmount++;
			selectedPlayers[index] = true;
		}
	}

	public void DeleteCharacterModel (int index)
	{
		if(selectedPlayers[index]==true)
		{
			selectedPlayers[index] = false;
			Destroy(spawnedCharacters[index]);
			playerAmount--;
		}
	}
	void SwitchCharacterModel(int index, float dir)
	{
		if (selectedPlayers[index] == true)
		{
			if(dir > 0f)
			{
				if (modelIndexes[index] < 3)
				{
					modelIndexes[index]++;
				}
				else
				{
					modelIndexes[index] = 0;
				}
			}
			else
			{
				if (modelIndexes[index] > 0)
				{
					modelIndexes[index]--;
				}
				else
				{
					modelIndexes[index] = 3;
				}
			}
			Destroy(spawnedCharacters[index]);
			GameObject characterModel = Instantiate(characterModels[modelIndexes[index]], slots[index].position + Vector3.up, characterModels[modelIndexes[index]].transform.rotation);
			spawnedCharacters[index] = characterModel;
		}
	}

	void StartGameButton(bool available)
	{
		if(startButton.activeSelf == !available)
		{
			startButton.SetActive(available);
		}
	}
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		for(int i = 0; i<hInputs.Length; i++)
		{
			if(Input.GetButtonDown(vInputs[i]))
			{
				DisplayCharacter(i);
			}
			if(Input.GetButtonDown(hInputs[i]))
			{
				DisplayCharacter(i);
				SwitchCharacterModel(i, Input.GetAxis(hInputs[i]));
			}
		}

		if(playerAmount > 0)
		{
			StartGameButton(true);
		}
		else
		{
			StartGameButton(false);
		}
	}
}
