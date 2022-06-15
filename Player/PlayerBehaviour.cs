using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerBehaviour : MonoBehaviour
{
    public Rigidbody2D _rb { get; set; }
    public Animator _animator;

    public float Speed;
    public bool IsOnGround { get; set; } = true;

    [SerializeField] private ParticleSystem _dustParticles;

    private InputManager inputManager;

    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private LayerMask _pondMask;

    public static Transform PlayerInstance { get; set; }
    

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        _animator = GetComponent<Animator>();
        PlayerInstance = this.gameObject.transform.GetComponent<Transform>();
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        this.gameObject.transform.Translate(this.gameObject.transform.right * Speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IsOnGround = true;
        _dustParticles.Play();
    }

    public bool OnGround()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, 1.3f, _groundMask);

        return raycastHit.collider != null;
    }

    public bool OnPond()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, 2f, _pondMask);

        return raycastHit.collider != null;
    }

    public void Die()
    {
        SceneManager.LoadScene(0);
    }
}