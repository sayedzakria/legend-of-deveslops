using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float range = 3f;
    [SerializeField] private float timeBetweenAttcks = 1f;

    private Animator anim;
    private GameObject player;
    private bool playerInRange;
    private BoxCollider[] weaponColliders;
    private EnemyHealth enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        weaponColliders = GetComponentsInChildren<BoxCollider>();
        player = GameManager.instance.Player;
        anim = GetComponent<Animator>();
        StartCoroutine(attack());
        enemyHealth = GetComponent<EnemyHealth>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position)<range&&enemyHealth.IsAlive)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }
    IEnumerator attack()
    {
        if (playerInRange && !GameManager.instance.GameOver)
        {
            anim.Play("Attack");
            yield return new WaitForSeconds(timeBetweenAttcks);
        }
        yield return null;
        StartCoroutine(attack());
    }
    public void EnemyBeginAttack()
    {
        foreach (var weapon in weaponColliders)
        {
            weapon.enabled = true;
        }
    }
    public void EnemyEndAttack()
    {
        foreach (var weapon in weaponColliders)
        {
            weapon.enabled = false;
        }
    }
}
