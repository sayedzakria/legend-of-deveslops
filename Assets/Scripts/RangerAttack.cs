using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAttack : MonoBehaviour
{
    [SerializeField] private float range = 3f;
    [SerializeField] private float timeBetweenAttcks = 1f;
    [SerializeField] Transform fireLocation;
    private Animator anim;
    private GameObject player;
    private GameObject arrow;
    private bool playerInRange;
    
    private EnemyHealth enemyHealth;
    // Start is called before the first frame update
    void Start()
    {
        arrow = GameManager.instance.Arrow;
        player = GameManager.instance.Player;
        anim = GetComponent<Animator>();
        StartCoroutine(attack());
        enemyHealth = GetComponent<EnemyHealth>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < range && enemyHealth.IsAlive)
        {
            playerInRange = true;
            anim.SetBool("PlayerInRange", true);
            RotatTowards(player.transform);
        }
        else
        {
            playerInRange = false;
            anim.SetBool("PlayerInRange", false);
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
     private void RotatTowards(Transform player)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }
    public void FireArrow()
    {
        GameObject newArrow = Instantiate(arrow) as GameObject;
        newArrow.transform.position = fireLocation.position;
        newArrow.transform.rotation = transform.rotation;
        newArrow.GetComponent<Rigidbody>().velocity = transform.forward * 25f;
    }
}
