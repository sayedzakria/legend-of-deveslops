using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
     
    private Transform player;
    private NavMeshAgent nav;
    private Animator anim;
    private EnemyHealth enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.Player.transform;
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        enemyHealth= GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        //goto player
        if (!GameManager.instance.GameOver&&enemyHealth.IsAlive)
        {
            nav.SetDestination(player.position);
        }
        else if((!GameManager.instance.GameOver|| GameManager.instance.GameOver)&&!enemyHealth.IsAlive)
        {
            nav.enabled = false;
            
        }
        else
        {
            nav.enabled = false;
            anim.Play("Idle");
        }

    }
}
