using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Pawtastic.Utils;
using System.Linq;
using System;

public class HealthBarController : MonoBehaviour
{
    #region Consts

    #endregion
    
    #region Serialized Fields
    [SerializeField] private GameObject _heartObj;
    [SerializeField] private GameObject _heartsPanel;
    #endregion Serialized Fields

    #region Fields
    private List<GameObject> _existingHeartsList = new List<GameObject>();
    private PlayerHealthController _playerCtx;
    private Sprite _halfHeartSprite;
    private Sprite _fulHeartSprite;
    #endregion Fields

    #region Lifecycle Methods (GameFlow)
    public void OnStart()
    {
        AssignSprites();
        ConfigureUI(PlayerStats.INIT_MAX_HEART_COUNT);
        GetPlayerCtx();
    }  
    #endregion Lifecycle Methods (GameFlow)

    #region public methods
    public void UpdateHeartPanel()
    {
        if (_playerCtx == null)
        {
            GameLogger.LogError("_playerCtx is null", LoggerConfig.LogCategoryFlags.Player | LoggerConfig.LogCategoryFlags.UI, _playerCtx);
            return;
        }

        int currentHealth = _playerCtx.CurrentHealth;
        if (currentHealth > 0)
        {
            // count new amount of hearts
            float dirtyHeartCount = (float)currentHealth / PlayerStats.BASE_HEART_LIFE;
            //GameLogger.Log($"currentHealth: {currentHealth} / PlayerStats.BASE_HEART_LIFE: {PlayerStats.BASE_HEART_LIFE}", LoggerConfig.LogCategoryFlags.Player | LoggerConfig.LogCategoryFlags.UI, this);
            //GameLogger.Log($"dirtyHeartCount: {dirtyHeartCount}", LoggerConfig.LogCategoryFlags.Player | LoggerConfig.LogCategoryFlags.UI, this);

            int newHeartCount = (int)Math.Ceiling(dirtyHeartCount);
            //GameLogger.Log($"newHeartCount: {newHeartCount}", LoggerConfig.LogCategoryFlags.Player | LoggerConfig.LogCategoryFlags.UI, this);


            foreach (Transform child in _heartsPanel.transform) {
                Destroy(child.gameObject);
            }

            //update heart ui panel
            _existingHeartsList.Clear();
            ConfigureUI(newHeartCount);

        }
        else
        {
            foreach (Transform child in _heartsPanel.transform) {
                Destroy(child.gameObject);
            }
        }

        //update Last heart object to correct sprite
        GameObject lastHeart = _existingHeartsList.LastOrDefault();
        if(lastHeart != null){
            Image lastHeartImg = lastHeart.GetComponent<Image>();
            int healthRest = _playerCtx.CurrentHealth % PlayerStats.BASE_HEART_LIFE; //modulo   7 mod 2 = 1 (the rest is 1)
            //0 -> full heart
            //1 -> half heart
            switch(healthRest)
            {
                case 0:
                    //lastHeartImg.sprite = _fulHeartSprite; //- technically it's not necessary anymore
                    break;
                case 1:
                    lastHeartImg.sprite = _halfHeartSprite; 
                    break;
            }
        }

    }
    #endregion public methods


    #region private methods
    private void GetPlayerCtx()
    {
        _playerCtx = PlayerManager.Instance._playerHealth;
        if (_playerCtx == null)
        {
            GameLogger.LogError("Failed to get player context", LoggerConfig.LogCategoryFlags.Player | LoggerConfig.LogCategoryFlags.UI, _playerCtx);
        }
    }

    /// <summary>
    ///  Initialize heart objects in panel with set amount of full hearts
    /// </summary>
    /// <param name="heartCount">amount of hearts to create</param>
    private void ConfigureUI(int heartCount)
    {
        //GameLogger.Log($"Configuring UI, hearts to render: {heartCount} ", LoggerConfig.LogCategoryFlags.UI);
        for (int i = 0; i < heartCount; i++)
        {         
            CreateFullHeartObj(_fulHeartSprite);
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
            GameLogger.LogError("Cannot create new heart object for HUD lifebar.", LoggerConfig.LogCategoryFlags.UI, newHeart);
            return null;
        }
        
        // Get existing Image component
        Image heartImage = newHeart.GetComponent<Image>();
        heartImage.sprite = fullHeartSprite;

        _existingHeartsList.Add(newHeart);
        return newHeart;
    }
    #endregion private methods


    #region helpers?

    /// <summary>
    /// Assign all health Bar sprites
    /// </summary>
    private void AssignSprites()
    {
        _fulHeartSprite = SpriteGrabber.LoadHeartSprite(SpriteGrabber.HUD_HeartState.Full);
        _halfHeartSprite = SpriteGrabber.LoadHeartSprite(SpriteGrabber.HUD_HeartState.Broken);
    }
    #endregion helpers?


}
