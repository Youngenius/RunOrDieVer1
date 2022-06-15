using UnityEngine;

public class DrowningAnimation : MonoBehaviour
{
    public Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}
