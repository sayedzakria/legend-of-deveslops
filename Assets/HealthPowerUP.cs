using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUP : MonoBehaviour
{
    private GameObject player;
    private PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        playerHealth = player.GetComponent<PlayerHealth>();
        GameManager.instance.RegisterPowerUP();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerHealth.PowerUPHealth();
            Destroy(gameObject);
        }
    }
   
}
