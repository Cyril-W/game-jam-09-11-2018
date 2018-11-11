using System.Collections;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour {

    [SerializeField] float secondsToRespawn = 2f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float secondsToDestroyLight = 3f;
    [SerializeField] GameObject resurrectionLight;

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
        var newPosition = playerMovement.ResetPosition();
        var newLight = Instantiate(resurrectionLight, newPosition, Quaternion.identity);
        Destroy(newLight, secondsToDestroyLight);
        playerMovement.enabled = true;
    }
}
