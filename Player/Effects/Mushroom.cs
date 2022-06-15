using System;
using System.Collections;
using UnityEngine;

public class Mushroom : MonoBehaviour, IEffect
{
    private PlayerBehaviour playerBehaviour;

    public EventHandler OnMushroomPickedEvent;
    public EventHandler OnMushroomEatenEvent;

    private void Awake()
    {
        playerBehaviour = GameObject.Find("Hero").GetComponent<PlayerBehaviour>();
    }

    private IEnumerator MushroomEffectCoroutine()
    {
        SpriteRenderer[] sprites = this.gameObject.GetComponentsInChildren<SpriteRenderer>();

        float speedWithMushroom = 14;
        float speedWithoutMushroom = playerBehaviour.Speed;
        float affectLength = playerBehaviour._animator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log("Speed: " + speedWithoutMushroom);

        playerBehaviour._animator.SetBool("isInMushroomZone", true);
        playerBehaviour.Speed = speedWithMushroom;

        Turn_OFF_ON_Sprites(sprites, false);      
        yield return new WaitForSeconds(affectLength);

        playerBehaviour.Speed = speedWithoutMushroom;
        OnMushroomEatenEvent?.Invoke(this, EventArgs.Empty);

        if (!playerBehaviour.OnPond())
        {
            playerBehaviour.Speed = speedWithoutMushroom;
        }

        Turn_OFF_ON_Sprites(sprites, true);
        playerBehaviour._animator.SetBool("isInMushroomZone", false);
    }

    public void ApplyEffect()
    {
        OnMushroomPickedEvent?.Invoke(this, EventArgs.Empty);

        StartCoroutine(MushroomEffectCoroutine());
        StopCoroutine(MushroomEffectCoroutine());
    }

    private void Turn_OFF_ON_Sprites(SpriteRenderer[] sprites, bool onOrOff)
    {
        foreach (var sprite in sprites)
        {
            sprite.enabled = onOrOff;
        }
    }
}
