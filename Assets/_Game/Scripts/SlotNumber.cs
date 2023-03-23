using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SlotNumber : MonoBehaviour
{
    [SerializeField] private float _size;
    [SerializeField] private float _moveDuration;
    [SerializeField] private Transform _bottomLimit;
    private Tween _tween;
    private bool _isMoving;
    private SlotNumber _nextNumber;

    public void Move()
    {
        if(!_isMoving)
        {
            return;
        }
        _tween=transform.DOMoveY(transform.position.y - _size, _moveDuration).OnComplete(() =>
        {
            Move();
        }).SetEase(Ease.Linear);
    }
    public void SetNext(SlotNumber next)
    {
        _nextNumber=next;
    }
    public void StopMoving()
    {
        _isMoving = false;
        _tween.Kill();
    }
    public void StartMoving()
    {
        _isMoving = true;
        Move();
    }
    public void Move(float verticalMovement)
    {
        transform.Translate(0, verticalMovement, 0);
    }
    private void LateUpdate()
    {
        if(transform.position.y<_bottomLimit.position.y)
        {
            _tween.Kill();
            transform.position = _nextNumber.transform.position;
            transform.Translate(0, _size, 0);
            Move();
        }
    }
}
