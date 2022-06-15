using System.Collections;
using UnityEngine;

public class SpiderAttack : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Vector2 _attackSize;
    [SerializeField] private LayerMask _objectToAttack;
    private bool _isAttacking = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.localPosition, PlayerBehaviour.PlayerInstance.localPosition) < 6.5f && !_isAttacking)
        {
            _animator.SetBool("InAngerZone", true);
            StartCoroutine(AttackCoroutine());
            _isAttacking = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(_attackPoint.position, _attackSize);
    }

    public IEnumerator AttackCoroutine()
    {
        Collider2D hitEnemy;
        float timeBetweenAttacks = 3;

        float affectLength = _animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(affectLength);

        _animator.SetBool("InAngerZone", false);
        _animator.SetTrigger("Attack");

        affectLength = _animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(affectLength / 2);

        hitEnemy = Physics2D.OverlapBox(_attackPoint.position, _attackSize, _objectToAttack);

        if (hitEnemy != null)
        {
            Destroy(hitEnemy.gameObject);
        }

        yield return new WaitForSeconds(timeBetweenAttacks);

        _isAttacking = false;
    }
}
