using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // Properties
    public TowerButton ClickedBtn { get; set; }
    public ObjectPool Pool { get; set; }

    private int currency;
    private int wave = 0;

    [SerializeField] private GameObject waveBtn;
    [SerializeField] private TextMeshProUGUI waveTxt;
    [SerializeField] private TextMeshProUGUI currencyTxt;

    // To keep the count of active enemies
    private List<Enemy> activeEnemies = new List<Enemy>();

    // Return true if count is > 0, meaning that current wave is still going
    public bool ActiveWave
    {
        get
        {
            return activeEnemies.Count > 0;
        }
    }

    public int Currency 
    {
        get 
        {
            return currency; 
        }
        set
        {
            this.currency = value;
            this.currencyTxt.text = value.ToString();
        }
    }

    private void Awake()
    {
       Pool = GetComponent<ObjectPool>();
    }
    private void Start()
    {
        Currency = 500;   
    }
    private void Update()
    {
        HandleEscapeButton();
    }
    public void PickTower(TowerButton twrButton)
    {
        if (Currency >= twrButton.Price && !ActiveWave)
        {
            // Store clicked button
            this.ClickedBtn = twrButton;
            // Activate hover icon
            Hover.Instance.Activate(twrButton.Sprite);
        }
    }
    public void BuyTower()
    {
        if(Currency >= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;
            Hover.Instance.Deactivate();
        }
    }

    // What happens when ESC is pressed
    private void HandleEscapeButton()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }

    public void WaveStart()
    {
        wave++;

        waveTxt.text = string.Format("Wave: {0}", wave);

        StartCoroutine(WaveSpawn());

        waveBtn.SetActive(false);
    }

    private IEnumerator WaveSpawn()
    {
        LevelManager.Instance.GeneratePath();

        for(int i = 0; i < wave; i++)
        {
            LevelManager.Instance.GeneratePath();

            int enemyIndex = Random.Range(0, 2);
            string type = string.Empty;

            switch (enemyIndex)
            {
                case 0:
                    type = "forest_soul";
                    break;
                case 1:
                    type = "fog_carrier";
                    break;
                //case 2:
                //    type = "N/A";
                //    break;
                //case 3:
                //    type = "N/A";
                //    break;
                default:
                    break;
            }

            // Get the enemy from the object pool and spawn
            Enemy enemy = Pool.getObject(type).GetComponent<Enemy>();
            enemy.Spawn();

            // Add a spawned enemy to list to keep count for current wave
            activeEnemies.Add(enemy);

            yield return new WaitForSeconds(2.5f);
        }
    }

    public void EnemyRemove(Enemy enemy)
    {
        activeEnemies.Remove(enemy);

        // If wave is finished, enable wave button
        if(!ActiveWave)
        {
            waveBtn.SetActive(true);
        }
    }
}
