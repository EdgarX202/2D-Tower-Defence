using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;

// Delegate for the currency change event
public delegate void ChangeOfCurrency();

public class GameManager : Singleton<GameManager>
{
    // Event is triggered when the currency changes
    public event ChangeOfCurrency ChangeC;

    // Properties
    public TowerButton ClickedBtn { get; set; }
    public ObjectPool Pool { get; set; }

    private int currency;
    private int health = 15;
    private int wave = 0;
    private int lives;
    private bool gameOver = false;

    // Serialized fields
    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI livesTxt;
    [SerializeField] private TextMeshProUGUI waveTxt;
    [SerializeField] private TextMeshProUGUI currencyTxt;
    [SerializeField] private TextMeshProUGUI sellTxt;
    [SerializeField] private TextMeshProUGUI statsTxt;
    [SerializeField] private TextMeshProUGUI upgradeTxt;
    [Header("GameObject Fields")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject waveBtn;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;

    // Currently selected tower
    private Tower selectedTower;

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

            OnCurrencyChange();
        }
    }

    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            this.lives = value;

            if (lives <= 0)
            {
                this.lives = 0;
                GameOver();
            }

            livesTxt.text = lives.ToString();
        }
    }

    private void Awake()
    {
       Pool = GetComponent<ObjectPool>();
    }
    private void Start()
    {
        Currency = 500;   
        Lives = 5;
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

    public void OnCurrencyChange()
    {
        if (ChangeC != null)
        {
            ChangeC();
            Debug.Log("Currency changed");
        }
    }

    public void SelectTower(Tower tower)
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = tower;
        selectedTower.Select();

        // Set the text for selling tower, half price -- change?
        sellTxt.text = "+ " + (selectedTower.Price / 2).ToString();

        upgradePanel.SetActive(true);
    }
    public void DeselectTower()
    {
        // If the tower is selected
        if(selectedTower != null)
        {
            selectedTower.Select();
        }

        upgradePanel.SetActive(false);

        // Remove reference to the tower
        selectedTower = null;
    }

    /// <summary>
    /// Handle ESC press
    /// </summary>
    private void HandleEscapeButton()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if player is not holding a tower in hand
            if(selectedTower == null && !Hover.Instance.isVisible)
            {
                PauseMenu();
            }
            else if(Hover.Instance.isVisible)
            {
                DropTower();
            }
            else if(selectedTower != null)
            {
                DeselectTower();
            }
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
                    type = "SnowPulse";
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
            enemy.Spawn(health);

            // Increase health every 3 waves
            if(wave % 3 == 0)
            {
                health += 5;
            }

            // Add a spawned enemy to list to keep count for current wave
            activeEnemies.Add(enemy);

            yield return new WaitForSeconds(2.5f);
        }
    }

    public void EnemyRemove(Enemy enemy)
    {
        // Remove enemies from active list
        activeEnemies.Remove(enemy);

        // If wave is finished and its not a game over, enable wave button
        if(!ActiveWave && !gameOver)
        {
            waveBtn.SetActive(true); // **** FIX THIS - BUTTON REAPPEARS AFTER FIRST ENEMY IS GONE *********
        }
    }

    // When lives are at 0, call game over
    public void GameOver()
    {
        if(!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    public void TowerSell()
    {
        if (selectedTower != null)
        {
            // Sell the tower for half price
            Currency += selectedTower.Price / 2;

            // Tower becomes a child of a tile once it is placed 
            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;

            // Destroy the selected tower
            Destroy(selectedTower.transform.parent.gameObject);

            // Deselect so there was no null reference
            DeselectTower();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;

        // Reload the same level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShowTooltip()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    public void ShowUpgradeStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        UpdateUpgradeTooltip();
    }

    public void SetTooltipTxt(string text)
    {
        statsTxt.text = text;
    }

    public void UpdateUpgradeTooltip()
    {
        if(selectedTower != null)
        {
            sellTxt.text = "+ " + (selectedTower.Price / 2).ToString();
            SetTooltipTxt(selectedTower.GetStats());

            if(selectedTower.NextUpgrade != null)
            {
                upgradeTxt.text = selectedTower.NextUpgrade.Price.ToString();
            }
            else
            {
                upgradeTxt.text = string.Empty;
            }
        }
    }

    public void UpgradeTower()
    {
        if(selectedTower != null)
        {
            // Check if selected tower level is less than the upgrade level && that we have enough money
            if(selectedTower.Level <= selectedTower.Upgrades.Length && Currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
            }
        }
    }

    public void PauseMenu()
    {
        if (optionsMenu.activeSelf)
        {
            ShowMainMenu();
        }
        else
        {

            pauseMenu.SetActive(!pauseMenu.activeSelf);

            // Freeze the game when the pause menu is active
            if (!pauseMenu.activeSelf)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }

    private void DropTower()
    {
        ClickedBtn = null;
        Hover.Instance.Deactivate();
    }

    public void Options()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void ShowMainMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
