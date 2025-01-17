﻿using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AbilityDisplay : MonoBehaviour
{
    public Collectible ability;
    public Image fillImage;
    public Image highlightImage;

    [Inject] private GameEventsSO gameEvents;

    private void OnEnable()
    {
        gameEvents.onCollected += OnCollected;
        gameEvents.onUseAbility += OnUsed;
        gameEvents.onPlayerDeath += ResetStats;
        if(fillImage == null)
        {
            fillImage = GetComponent<Image>();
        }
        if (highlightImage == null){
            Debug.Log("no highlightImage on " + gameObject);
        }
        ResetStats();
    }
    private void OnDisable()
    {
        gameEvents.onCollected -= OnCollected;
        gameEvents.onUseAbility -= OnUsed;
        gameEvents.onPlayerDeath -= ResetStats;
    }
    private void ResetStats()
    {
        fillImage.fillAmount = 0;
        highlightImage.enabled = false;
    }
    private void OnUsed(Collectible col)
    {
        /*if (col == Collectible.coin)
        {
            return;
        }
        AbilityAccess abilityAccess = linker.playerVars.GetAbilityAcccess(ability);
        int count = abilityAccess.GetCount();
        fillImage.fillAmount = (float)count / 6;
        if (count == 0 || linker.playerVars.currentSelectedAbility != ability)
        {
            highlightImage.enabled = false;
        }
        else
        {
            highlightImage.enabled = true;
        }*/
    }
   
    void OnCollected(Collectible col)
    {
       /* if(col == Collectible.coin)
        {
            return;
        }
        if (col == ability)
        {
            highlightImage.enabled = true;
            AbilityAccess abilityAccess = linker.playerVars.GetAbilityAcccess(col);
            int count = abilityAccess.GetCount();
            fillImage.fillAmount = (float)count / 6;
        }
        else if (col != Collectible.none)
        {
            highlightImage.enabled = false;

        }*/
    }

}
