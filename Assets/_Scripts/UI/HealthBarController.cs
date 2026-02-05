using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private TextMeshPro _livesCounter;
    [SerializeField] private Image _firstBarImg;
    [SerializeField] private Image _lastBarImg;


    private int _maxHealth = PlayerController.Instance.PlayerStats.MaxHealth; 
    private int _currentHealth = PlayerController.Instance.PlayerStats.CurrentHealth; 
    private int _maxLives = PlayerController.Instance.PlayerStats.MaxLives; 
    private int _currentLives = PlayerController.Instance.PlayerStats.CurrentLives; 



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
