using System.Collections;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour {

    [SerializeField] float secondsToRespawn = 2f;
    [SerializeField] AudioSource audioSource;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.enabled = false;
            audioSource.PlayOneShot(audioSource.clip);
            StartCoroutine(RespawnPlayerAbove(playerMovement));
        }
    }

    IEnumerator RespawnPlayerAbove(PlayerMovement playerMovement)
    {
        yield return new WaitForSeconds(secondsToRespawn);
        playerMovement.transform.position = new Vector3(-10, 10, 0);
        playerMovement.enabled = true;
    }
}
