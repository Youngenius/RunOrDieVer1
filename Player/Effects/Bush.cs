using System;
using UnityEngine;

public class Bush : MonoBehaviour, IEffect
{
    private PlayerBehaviour playerBehaviour;

    public event EventHandler OnSpiderDetainedEvent;

    private void Awake()
    {
        playerBehaviour = GameObject.Find("Hero").GetComponent<PlayerBehaviour>();
    }
    public void ApplyEffect()
    {
        playerBehaviour._animator.SetBool("isInBushZone", true);
        playerBehaviour.Speed = 6;

        OnSpiderDetainedEvent?.Invoke(this, EventArgs.Empty);
    }
}
