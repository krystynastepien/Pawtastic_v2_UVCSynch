using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private GameObject _livesCounter;
    [SerializeField] private GameObject _firstBarImg;
    [SerializeField] private GameObject _lastBarImg;
    [SerializeField] private GameObject _middlebarImg;
    #endregion

    public void OnAwake()
    {
        InitUIComponents();
        ConfigureUI();
    }

    /// <summary>
    /// Initialize all player components
    /// </summary>
    private void InitUIComponents()
    {
        
    }     

        /// <summary>
    /// Initialize all player components
    /// </summary>
    private void ConfigureUI()
    {
        
    }     

}
