using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Precipice : MonoBehaviour, IEffect
{
    private PlayerBehaviour playerBehaviour;

    private void Awake()
    {
        playerBehaviour = GameObject.Find("Hero").GetComponent<PlayerBehaviour>();
    }

    public void ApplyEffect()
    {
        playerBehaviour.Die();
    }
}
