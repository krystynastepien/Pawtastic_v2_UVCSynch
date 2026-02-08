using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Pawtastic.Utils;

public class HealthBarController : MonoBehaviour
{
    #region Consts

    #endregion
    
    #region Serialized Fields
    [SerializeField] private GameObject _heartObj;
    [SerializeField] private GameObject _heartsPanel;
    #endregion

    #region Fields
    //List<GameObject> _heartObjList = new List<GameObject>();
    #endregion

    public void OnStart()
    {
        ConfigureUI(PlayerStats.INIT_MAX_LIVES);
    }  

    /// <summary>
    /// Initialize heart objects in panel at the start (full)
    /// </summary>
    private void ConfigureUI(int livesCount)
    {
        Sprite fullHeartSprite = SpriteGrabber.LoadHeartSprite(SpriteGrabber.HUD_HeartState.Full);
        if (fullHeartSprite == null)
        {
            GameLogger.LogError("[HealthBarController][ConfigureUI] Failed to load heart sprite!", LoggerConfig.LogCategoryFlags.UI);
            return;
        }
       
        for (int i = 0; i < livesCount; i++)
        {         
            CreateFullHeartObj(fullHeartSprite);
        } 
    }     

    /// <summary>
    /// Create copy of object with given sprite
    /// </summary>
    /// <param name="fullHeartSprite"></param>
    /// <returns></returns>
    private GameObject CreateFullHeartObj(Sprite fullHeartSprite)
    {        
        // create prefab object
        GameObject newHeart = Instantiate(_heartObj, _heartsPanel.transform);
        if(newHeart == null){ 
            GameLogger.LogError("[HealthBarController][CreateFullHeartObj()] Cannot create new heart object for HUD lifebar.", LoggerConfig.LogCategoryFlags.UI, newHeart);
            return null;
        }
        
        // Get existing Image component
        Image heartImage = newHeart.GetComponent<Image>();
        heartImage.sprite = fullHeartSprite;

        return newHeart;
    }     

}
