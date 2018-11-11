using System.Collections;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour {

    [SerializeField] float secondsToRespawn = 2f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] float secondsToDestroyLight = 3f;
    [SerializeField] GameObject resurrectionLight;
    [SerializeField] AudioClip falling;
    [SerializeField] AudioClip resurecting;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.enabled = false;
            audioSource.PlayOneShot(falling);
            StartCoroutine(RespawnPlayerAbove(playerMovement));
        }
    }

    IEnumerator RespawnPlayerAbove(PlayerMovement playerMovement)
    {
        yield return new WaitForSeconds(secondsToRespawn);
        var newPosition = playerMovement.ResetPosition();
        audioSource.PlayOneShot(resurecting);
        var newLight = Instantiate(resurrectionLight, newPosition, Quaternion.identity);
        Destroy(newLight, secondsToDestroyLight);
        playerMovement.enabled = true;
    }
}
