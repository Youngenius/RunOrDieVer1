using System;
using System.Collections;
using UnityEngine;

public class Pond : MonoBehaviour, IEffect
{
    private PlayerBehaviour playerBehaviour;

    public EventHandler OnDrawningEvent;

    private void Awake()
    {
        playerBehaviour = GameObject.Find("Hero").GetComponent<PlayerBehaviour>();
    }

    private IEnumerator DrowningCoroutine()
    {
        //implementing gradual slowing down in the pond and turning on the animation

        int affectLength = 4;
        playerBehaviour._animator.SetBool("isDrowning", true);
        GameObject.Find("Hero").GetComponent<InputManager>().enabled = false;

        if (!playerBehaviour.OnPond())
        {
            playerBehaviour.Speed = 3;

            for (int i = 0; i < affectLength; i++)
            {
                yield return new WaitForSeconds(0.3f);
                playerBehaviour.Speed -= 0.4f;
                Debug.Log("On the ground!");
            }
        }
        else
        {
            playerBehaviour.Speed = 1;
            yield return null;
            Debug.Log("From the air!");
        }
    }

    public void ApplyEffect()
    {
        StartCoroutine(DrowningCoroutine());
        StopCoroutine(DrowningCoroutine());

        OnDrawningEvent?.Invoke(this, EventArgs.Empty);
    }
}
