using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUP : MonoBehaviour
{
    private GameObject player;
    private PlayerControler playerControler;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player;
        playerControler = player.GetComponent<PlayerControler>();
        GameManager.instance.RegisterPowerUP();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            playerControler.SpeedPowerUP();
            Destroy(gameObject);
        }
    }
}
