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

    private void OnEnable()
    {
        gameEvents.onChangedAbility += OnChangedAbility;
        image = GetComponent<Image>();
    }
    private void OnDisable()
    {
        gameEvents.onChangedAbility -= OnChangedAbility;

    }
    private void OnChangedAbility(Collectible ability)
    {
        switch (ability)
        {
            case Collectible.none:
                image.color = inactiveColor;
                break;
            case Collectible.oneShot:
                image.color = oneShotColor;
                break;
            case Collectible.sideBoost:
                image.color = sideBoostColor;
                break;
        }
    }
}
