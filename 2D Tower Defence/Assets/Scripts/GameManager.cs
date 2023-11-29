using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// Delegate for the currency change event
public delegate void ChangeOfCurrency();

public class GameManager : Singleton<GameManager>
{
    // Event is triggered when the currency changes
    public event ChangeOfCurrency ChangeC;

    private Tower selectedTower;

    // Keep the count of active enemies
    private List<Enemy> activeEnemies = new List<Enemy>();

    // Private
    private int _currency;
    private int _health = 100;
    private int _wave = 0;
    private int _lives;
    private bool _gameOver = false;

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

    // Properties
    public TowerButton ClickedBtn { get; set; }
    public ObjectPool Pool { get; set; }
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
            return _currency; 
        }
        set
        {
            this._currency = value;
            this.currencyTxt.text = value.ToString();

            OnCurrencyChange();
        }
    }
    public int Lives
    {
        get
        {
            return _lives;
        }
        set
        {
            this._lives = value;

            if (_lives <= 0)
            {
                this._lives = 0;
                GameOver();
            }

            livesTxt.text = _lives.ToString();
        }
    }

    private void Awake()
    {
       Pool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        Currency = 100;   
        Lives = 20;
    }

    private void Update()
    {
        HandleEscapeButton();
    }

    // Pick tower from the shop
    public void PickTower(TowerButton twrButton)
    {
        // If player has enough money and the wave is not active
        if (Currency >= twrButton.Price && !ActiveWave)
        {
            // Store clicked button
            this.ClickedBtn = twrButton;
            // Activate hover icon
            Hover.Instance.Activate(twrButton.Sprite);
        }
    }

    // Buy tower, change currency accordingly
    public void BuyTower()
    {
        // If player has enough money
        if(Currency >= ClickedBtn.Price)
        {
            // Subtract
            Currency -= ClickedBtn.Price;
            // Deactivate hover
            Hover.Instance.Deactivate();
        }
    }

    // If currency changed, Debug.Log
    public void OnCurrencyChange()
    {
        if (ChangeC != null)
        {
            ChangeC();
            //Debug.Log("Currency changed");
        }
    }

    // Select tower and set text, enable upgrade panel
    public void SelectTower(Tower tower)
    {
        // Select tower
        if(selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = tower;
        selectedTower.Select();

        // Set the text for selling tower, half price
        sellTxt.text = "+ " + (selectedTower.Price / 2).ToString();
        
        // Enable upgrade panel
        upgradePanel.SetActive(true);
    }

    // Deselect tower if holding
    public void DeselectTower()
    {
        // If the tower is selected
        if(selectedTower != null)
        {
            selectedTower.Select();
        }

        // Disable upgrade panel
        upgradePanel.SetActive(false);

        // Remove reference to the tower
        selectedTower = null;
    }

    // Pause the game on ESC press
    private void HandleEscapeButton()
    {
        // If ESC is pressed
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if player is not holding a tower in hand
            if(selectedTower == null && !Hover.Instance.isVisible)
            {
                // Pause the game, open menu
                PauseMenu();
            }
            else if(Hover.Instance.isVisible)
            {
                // If a player is holding the tower, drop it
                DropTower();
            }
            else if(selectedTower != null)
            {
                // Deselect the tower if a player has selected it
                DeselectTower();
            }
        }
    }

    // Increase wave count and start Coroutine
    public void WaveStart()
    {
        // Increase wave count
        _wave++;
        // Update wave text
        waveTxt.text = string.Format("Wave: {0}", _wave);
        // Start spawning wave
        StartCoroutine(WaveSpawn());
        // Disable wave button
        waveBtn.SetActive(false);
    }

    // Spawn random enemy waves
    private IEnumerator WaveSpawn()
    {
        // A* finds path from A to B
        LevelManager.Instance.GeneratePath();

        for(int i = 0; i < _wave; i++)
        {
            LevelManager.Instance.GeneratePath();

            // Pick random enemy
            int enemyIndex = Random.Range(0, 4);
            string type = string.Empty;

            switch (enemyIndex)
            {
                case 0:
                    type = "SnowPulse";
                    break;
                case 1:
                    type = "ShieldCarrier";
                    break;
                case 2:
                    type = "SnowCorpse";
                    break;
                case 3:
                    type = "BoxHead";
                    break;
                default:
                    break;
            }

            // Get the enemy from the object pool and spawn
            Enemy enemy = Pool.getObject(type).GetComponent<Enemy>();
            enemy.Spawn(_health);

            // Increase health every 3 waves
            if(_wave % 3 == 0)
            {
                _health += 5;
            }

            // Add a spawned enemy to list to keep count for current wave
            activeEnemies.Add(enemy);

            yield return new WaitForSeconds(2.5f);
        }
    }

    // Remove enemy and activate "Wave" button
    public void EnemyRemove(Enemy enemy)
    {
        // Remove enemies from active list
        activeEnemies.Remove(enemy);

        // If wave is finished and its not a game over, enable wave button
        if(!ActiveWave && !_gameOver)
        {
            waveBtn.SetActive(true);
        }
    }

    // Game over after lives = 0
    public void GameOver()
    {
        if(!_gameOver)
        {
            _gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    // Selling selected tower
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

    // Restart the level
    public void Restart()
    {
        // Unfreeze the scene
        Time.timeScale = 1;

        // Reload the same level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Exit application
    public void Quit()
    {
        Application.Quit();
    }

    // Enable/Disable tooltip
    public void ShowTooltip()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    // Enable/Disable upgrade stats panel
    public void ShowUpgradeStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        UpdateUpgradeTooltip();
    }

    // Tooltip text
    public void SetTooltipTxt(string text)
    {
        statsTxt.text = text;
    }

    // Update upgrade text
    public void UpdateUpgradeTooltip()
    {
        if(selectedTower != null)
        {
            // Sell a tower half price text
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

    // Upgrading tower
    public void UpgradeTower()
    {
        if(selectedTower != null)
        {
            // Check if selected tower level is less than the upgrade level & that we have enough money
            if(selectedTower.Level <= selectedTower.Upgrades.Length && Currency >= selectedTower.NextUpgrade.Price)
            {
                // Can upgrade
                selectedTower.Upgrade();
            }
        }
    }

    // Show pause menu
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
            // Unfreeze
            else
            {
                Time.timeScale = 0;
            }
        }
    }

    // Deactivate hover when tower is placed
    private void DropTower()
    {
        ClickedBtn = null;
        Hover.Instance.Deactivate();
    }

    // SetActive - Options menu
    public void Options()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    // SetActive - Menu
    public void ShowMainMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}