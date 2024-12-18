using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider playerHealthBar;
    public Image PlayerImage;

    public GameObject enemyUI;
    public Slider enemyHealthBar;
    public Image enemyImage;

    private PlayerController player;

    private float enemyUITime = 4f;
    private float enemyTimer;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();

        playerHealthBar.maxValue = player.maxHealth;

        playerHealthBar.value = playerHealthBar.maxValue;

        PlayerImage.sprite = player.playerImage;
    }

    void Update()
    {
        enemyTimer += Time.deltaTime;

        if (enemyTimer >= enemyUITime)
        {
            enemyUI.SetActive(false);
            enemyTimer = 0;
        }
    }

    public void UpdatePlayerHealth(int amount )
    {
        playerHealthBar.value = amount;
    }

    public void UpdateEnemyUI(int maxHealth, int currentHealth, Sprite Image)
    {
        enemyHealthBar.maxValue = maxHealth; 
        enemyHealthBar.value = currentHealth;
        enemyImage.sprite = Image;

        enemyTimer = 0; 
        
        enemyUI.SetActive(true);
    }
}
