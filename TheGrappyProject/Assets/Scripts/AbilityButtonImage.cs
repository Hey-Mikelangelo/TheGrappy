using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class AbilityButtonImage : MonoBehaviour
{
    public Color inactiveColor;
    public Color oneShotColor;
    public Color sideBoostColor;
    public GameEventsSO gameEvents;
    private Image image;

    public void OnEnable()
    {
        gameEvents.onChangedAbility += OnCollectedAbility;
        image = GetComponent<Image>();
    }
    public void OnDisable()
    {
        gameEvents.onChangedAbility -= OnCollectedAbility;
    }
    void OnCollectedAbility(Ability ability)
    {
        switch (ability)
        {
            case Ability.none:
                image.color = inactiveColor;
                break;
            case Ability.oneShot:
                image.color = oneShotColor;
                break;
            case Ability.sideBoost:
                image.color = sideBoostColor;
                break;
        }
    }
}
