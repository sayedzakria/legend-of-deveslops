using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int startingHealth = 100;
    [SerializeField] float timSinceLastHit = 2f;
    [SerializeField] Slider healthSlider; 
    private float timer = 0f;
    private CharacterController characterController;
    private Animator anim;
    private int currentHealth;
    public int CurrentHealth
    {
        get { return currentHealth; }
        set { if (value < 0)
                currentHealth = 0;
            else
                currentHealth = value;


                    }
    }
    private AudioSource audio;
    private ParticleSystem blood;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        currentHealth = startingHealth;
        audio = GetComponent<AudioSource>();
        blood = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (timer >= timSinceLastHit && !GameManager.instance.GameOver)
        {
            if (other.tag == "Weapon")
            {
                takeHit();
                timer = 0;
                blood.Play();
            }
        }
    }
    void takeHit()
    {
        if (currentHealth > 0)
        {
            GameManager.instance.PlayerHit(currentHealth);
            anim.Play("Hurt");
            currentHealth -= 10;
            healthSlider.value = currentHealth;
            audio.PlayOneShot(audio.clip);
            
        }
        if (currentHealth <= 0)
        {
            killPlayer();
        }
    }
    void killPlayer()
    {
        GameManager.instance.PlayerHit(currentHealth);
        anim.SetTrigger("PlayerDie");
        characterController.enabled = false;
        audio.PlayOneShot(audio.clip);
       
    }
    public void PowerUPHealth()
    {
        if (currentHealth <= 70)
        {
            CurrentHealth += 30;
        }else if (currentHealth < startingHealth)
        {
            CurrentHealth = startingHealth;
        }
        healthSlider.value = currentHealth;
    }
}
