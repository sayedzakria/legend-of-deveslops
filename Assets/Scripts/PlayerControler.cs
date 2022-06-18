using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private LayerMask layerMask;
    private CharacterController characterController;
    private Vector3 currentLookTarget = Vector3.zero;

    private Animator animator;
    private BoxCollider[] swardColliders;
    private GameObject fireTrailer;
    private ParticleSystem fireTrailerparticles;
    // Start is called before the first frame update
    void Start()
    {
        fireTrailer = GameObject.FindWithTag("Fire") as GameObject;
        fireTrailer.SetActive(false);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        swardColliders = GetComponentsInChildren<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.GameOver)
        {
            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            characterController.SimpleMove(moveDirection * moveSpeed);
            if (moveDirection == Vector3.zero)
            {
                animator.SetBool("IsWailking", false);
            }
            else
            {
                animator.SetBool("IsWailking", true);
            }
            if (Input.GetMouseButtonDown(0))
            {
                animator.Play("DoubleChop");
            }
            else if (Input.GetMouseButtonDown(1))
            {
                animator.Play("SpinAttack");
            }
        }
    }
    private void FixedUpdate()
    {
        if (!GameManager.instance.GameOver)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //draw line towards the mouse point
            Debug.DrawRay(ray.origin, ray.direction * 500, Color.blue);
            if (Physics.Raycast(ray, out hit, 500, layerMask, QueryTriggerInteraction.Ignore))
            {
                //if not looking to mouse turn player towards the mouse point
                if (hit.point != currentLookTarget)
                {
                    currentLookTarget = hit.point;
                }
                Vector3 targetPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                //rotate the player
                Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10f);
            }
        }
    }
    //BeginAttack
    public void BeginAttack()
    {
        foreach (var weapon in swardColliders)
        {
            weapon.enabled = true;
        }
    }
    public void EndAttack()
    {
        foreach (var weapon in swardColliders)
        {
            weapon.enabled = false;
        }
    }
   public void SpeedPowerUP()
    {
        StartCoroutine(fireTrailRoutine());
    }
    IEnumerator fireTrailRoutine()
    {
        fireTrailer.SetActive(true);
        moveSpeed = 10f;
        yield return new WaitForSeconds(10f);
        moveSpeed = 6f;
        fireTrailerparticles = fireTrailer.GetComponent<ParticleSystem>();
        var em = fireTrailerparticles.emission;
        em.enabled = false;
        yield return new WaitForSeconds(3f);
        em.enabled = true;
        fireTrailer.SetActive(false);
    }
}
