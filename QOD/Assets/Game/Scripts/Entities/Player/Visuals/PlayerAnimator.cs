using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _playerAnimator;

    private int _isMovingHash;
    private int _moveXHash;
    private int _moveYHash;
    private int _isInactiveHash;

    private void Awake()
    {
        _isMovingHash = Animator.StringToHash("IsMoving");
        _moveXHash = Animator.StringToHash("MoveX");
        _moveYHash = Animator.StringToHash("MoveY");
        _isInactiveHash = Animator.StringToHash("IsInactive");
    }

    public void SetIsMoving(bool isMoving)
    {
        _playerAnimator.SetBool(_isMovingHash, isMoving);
    }

    public void SetMove(Vector2 direction)
    {
        _playerAnimator.SetFloat(_moveXHash, direction.x);
        _playerAnimator.SetFloat(_moveYHash, direction.y);
    }

    public void SetInactive(bool isInactive)
    {
        _playerAnimator.SetBool(_isInactiveHash, isInactive);
    }
}
