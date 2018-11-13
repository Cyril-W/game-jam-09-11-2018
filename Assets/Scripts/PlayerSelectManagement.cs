using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectManagement : MonoBehaviour
{
	public Image blackImage;
	public Animator fadeAnim;
	public AudioClip playerSelect;
	public AudioClip validation;
	public int sceneIndexToLoad = 1;
	public string[] hInputs = new string[4];
	public string[] vInputs = new string[4];

	public Vector3 rotateSpeed;
	public GameObject[] characterModels = new GameObject[4];
	GameObject[] spawnedCharacters = new GameObject[4];
	public Transform[] slots = new Transform[4];

	public Button startButton;

    [SerializeField] Text instructions;
    [SerializeField] string startInstructions = "Press any key";
    [SerializeField] string playInstructions = "Press enter";

    bool[] selectedPlayers = {false, false, false, false};
	int[] modelIndexes = { 0, 1, 2, 3 };
	int playerAmount;
	int finalPlayerAmount = 0;
    bool settingsOpen = false;

	PlayerManagement playerMgr;

    public void ToggleSettingsOpen()
    {
        settingsOpen = !settingsOpen;
    }

	void DisplayCharacter(int index)
	{
		if(selectedPlayers[index] == false)
		{
			AudioSource.PlayClipAtPoint(playerSelect, transform.position);
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
			AudioSource.PlayClipAtPoint(playerSelect, transform.position);
		}
	}

	void StartGameButton(bool available)
	{
		if(startButton.interactable == !available)
		{
			startButton.interactable = available;
            instructions.text = available ? playInstructions : startInstructions;
        }
	}

	public void StartGame()
	{
		AffectPlayers();
		AudioSource.PlayClipAtPoint(validation, transform.position);
		StartCoroutine(Fading());
	}

	public IEnumerator Fading()
	{
		fadeAnim.SetBool("Fade", true);
		yield return new WaitUntil(() => blackImage.color.a == 1);
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
        StartGameButton(false);
    }
	
	void Update ()
	{
        if (settingsOpen)
        {
            return;
        }

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
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartGame();
            }
            StartGameButton(true);
        }
		else
		{
			StartGameButton(false);
		}
	}
}
