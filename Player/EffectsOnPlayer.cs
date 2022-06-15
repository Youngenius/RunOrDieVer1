using UnityEngine;

public class EffectsOnPlayer : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;

    private void Awake()
    {
        playerBehaviour = GetComponent<PlayerBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEffect effect = collision.GetComponentInParent<IEffect>();
        effect.ApplyEffect();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Bush":
                playerBehaviour._animator.SetBool("isInBushZone", false);
                playerBehaviour.Speed = 10;
                break;
        }
    }
}