using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [SerializeField] private UnityEvent dustParticles_OnJump;

    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private float _jumpForce;

    private void Awake()
    {
        playerBehaviour = GetComponent<PlayerBehaviour>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerBehaviour.IsOnGround)
        {
            playerBehaviour._rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            playerBehaviour._animator.SetTrigger("Jump");
            playerBehaviour._animator.SetBool("isInMushroomZone", false);

            dustParticles_OnJump.Invoke();
        }
    }
}
