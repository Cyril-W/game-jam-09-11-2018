using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelectManagement : MonoBehaviour
{
	public int sceneIndexToLoad = 1;
	public string[] hInputs = new string[4];
	public string[] vInputs = new string[4];

	public Vector3 rotateSpeed;
	public GameObject[] characterModels = new GameObject[4];
	GameObject[] spawnedCharacters = new GameObject[4];
	public Transform[] slots = new Transform[4];

	public GameObject startButton;
	bool[] selectedPlayers = {false, false, false, false};
	int[] modelIndexes = { 0, 1, 2, 3 };
	int playerAmount;
	int finalPlayerAmount = 0;

	PlayerManagement playerMgr;

	void DisplayCharacter(int index)
	{
		if(selectedPlayers[index] == false)
		{
			GameObject characterModel = Instantiate(characterModels[modelIndexes[index]], slots[index].position + Vector3.up, characterModels[modelIndexes[index]].transform.rotation);
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

	public void StartGame()
	{
		AffectPlayers();
		SceneManager.LoadScene(sceneIndexToLoad);
	}

	void AffectPlayers()
	{
		finalPlayerAmount = 0;
		playerMgr.InitPlayerList();
		for(int i = 0; i<spawnedCharacters.Length; i++)
		{
			if(spawnedCharacters[i] != null)
			{
				PlayerManagement.Player newPlayer = new PlayerManagement.Player(); ;
				newPlayer.hInput = hInputs[i];
				newPlayer.vInput = vInputs[i];
				newPlayer.playerSkin = modelIndexes[i];
				playerMgr.playerList.Add(newPlayer);
				finalPlayerAmount++;
			}
		}
		playerMgr.playerAmount = finalPlayerAmount;
	}
	void Start ()
	{
		playerMgr = FindObjectOfType<PlayerManagement>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		for(int i = 0; i<hInputs.Length; i++)
		{
			if(Input.GetButtonDown(vInputs[i]))
			{
				DisplayCharacter(i);
				SwitchCharacterModel(i, Input.GetAxis(vInputs[i]));
			}
			if(Input.GetButton(hInputs[i]))
			{
				DisplayCharacter(i);
				spawnedCharacters[i].transform.Rotate(rotateSpeed * Time.deltaTime * Input.GetAxis(hInputs[i]));
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
