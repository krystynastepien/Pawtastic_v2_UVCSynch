using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBaController : MonoBehaviour
{
    [SerializeField] private TextMeshPro _livesCounter;
    [SerializeField] private Image _firstBarImg;
    [SerializeField] private Image _lastBarImg;


    private int _maxHealth = PlayerManager.Instance._playerHealth.MaxHealth; 
    private int _currentHealth = PlayerManager.Instance._playerHealth.CurrentHealth; 
    private int _maxLives = PlayerManager.Instance._playerHealth.MaxLives; 
    private int _currentLives = PlayerManager.Instance._playerHealth.CurrentLives; 



    /// <summary>
    /// Initialize all player components
    /// </summary>
    private void InitUIComponents()
    {
        

    }     

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
