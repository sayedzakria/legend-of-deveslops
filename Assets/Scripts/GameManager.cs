using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField]
    private GameObject player;
    private bool gamover = false;
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] GameObject[] powerUPPoints;
    [SerializeField] GameObject tanker;
    [SerializeField] GameObject ranger;
    [SerializeField] GameObject soldier;
    [SerializeField] GameObject arrow;
    [SerializeField] GameObject healthpowerUP;
    [SerializeField] GameObject speedPowerUP;
    [SerializeField] int maxPowerUPs;
    [SerializeField] int finalLevel = 20;
    private GameObject newPowerUP;
    public GameObject Arrow
    {
        get { return arrow; }
    }
    [SerializeField] Text levelText;
    [SerializeField] Text endGameText;

    private int currentLevel;
    private float genearatedSpawnTime = 1;
    private float currentSpawnTime=0;
    private float powerUpSpawnTime = 60;
    private float currentpowerUpSpawnTime = 0;
    private int powerups = 0;
    private GameObject newEnemy;
    private List<EnemyHealth> enemies = new List<EnemyHealth>();
    private List<EnemyHealth> killedenemies = new List<EnemyHealth>();
    public void RegisterEnemy(EnemyHealth enemy)
    {
        enemies.Add(enemy);
    }
    public void RegisterPowerUP( )
    {
        powerups++;
    }
    public void killedEnemy(EnemyHealth enemy)
    {
        killedenemies.Add(enemy);
    }
    public bool GameOver
    {
        get { return gamover; }
    }
    public GameObject Player
    {
        get { return player; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(gameObject);
        }
       //DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        endGameText.GetComponent<Text>().enabled = false;
        StartCoroutine(spawn());
        StartCoroutine(powerUPSpawn());
        currentLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnTime += Time.deltaTime;
        currentpowerUpSpawnTime += Time.deltaTime;
    }
    public void PlayerHit(int currentHp)
    {
        if (currentHp > 0)
        {
            gamover = false;
        }
        else
        {
            gamover = true;
            StartCoroutine(endGame("Defeat"));
        }
    }
    IEnumerator spawn()
    {
        if (currentSpawnTime > genearatedSpawnTime)
        {
            currentSpawnTime = 0;
            if (enemies.Count < currentLevel)
            {
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];
                int randomEnemy = Random.Range(0, 3);
                if (randomEnemy == 0)
                {
                    newEnemy = Instantiate(soldier) as GameObject;
                }else if (randomEnemy == 1)
                {
                    newEnemy = Instantiate(ranger) as GameObject;
                }
                else if (randomEnemy == 2)
                {
                    newEnemy = Instantiate(tanker) as GameObject;
                }
                newEnemy.transform.position = spawnLocation.transform.position;
            }
            if (killedenemies.Count == currentLevel&&currentLevel!=finalLevel)
            {
                enemies.Clear();
                killedenemies.Clear();
                yield return new WaitForSeconds(3f);
                currentLevel++;
                levelText.text = "Level " + currentLevel;
            }
            if (killedenemies.Count == finalLevel)
            {
                StartCoroutine(endGame("Victory!"));
            }
        }
        yield return null;
        StartCoroutine(spawn());
    }
    IEnumerator powerUPSpawn()
    {
        if (currentpowerUpSpawnTime > powerUpSpawnTime)
        {
            currentpowerUpSpawnTime = 0;
            if (powerups < maxPowerUPs)
            {
                int randomNumber = Random.Range(0, powerUPPoints.Length - 1);
                GameObject spawnLocation = powerUPPoints[randomNumber];
                int randomPowerUP = Random.Range(0, 2);
                if (randomPowerUP == 0)
                {
                    newPowerUP = Instantiate(healthpowerUP) as GameObject;
                }else if (randomPowerUP == 1)
                {
                    newPowerUP = Instantiate(speedPowerUP) as GameObject;
                }
                newPowerUP.transform.position = spawnLocation.transform.position;
            }
        }
        yield return null;
        StartCoroutine(powerUPSpawn());
    }
    IEnumerator endGame(string outstring)
    {
        endGameText.text = outstring;
        endGameText.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameMenu");
    }
}
