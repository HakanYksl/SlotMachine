using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Column : MonoBehaviour
{
    [SerializeField] private List<SlotNumber> _columnNumbers;
    [SerializeField] private Column _nextColumn;
    [SerializeField] private bool _IsFirst;
    [SerializeField] private float _elementHeight;
    [SerializeField] private float _turnDuration;
    [SerializeField] private float _DelayForNextColumStop;
    private int m_RandomNumber;
    private bool _isMoving;
    private bool _isStoppingStarted;
    [SerializeField] private float speed;
    private float _currrentSpeed;
    private void Start()
    {
        int index=0;
        foreach(SlotNumber t in _columnNumbers)
        {
            t.transform.Translate(0, (_elementHeight * index) + (_elementHeight/2), 0);
            if(index>0)
            {
                t.SetNext(_columnNumbers[index-1]);
            }
            else
            {
                _columnNumbers[0].SetNext(_columnNumbers[_columnNumbers.Count-1]);
            }
            index++;
        }
    }
    private void Update()
    {
        if(_isStoppingStarted)
        {
            if (_columnNumbers[m_RandomNumber].transform.position.y<0.25f && _columnNumbers[m_RandomNumber].transform.position.y > -0.25f)
            {
                if(_nextColumn)
                {
                    DOVirtual.DelayedCall(_DelayForNextColumStop, () => 
                    {
                        _nextColumn.StartStopping();
                    });
                }
                _isMoving = false;
                _isStoppingStarted = false;
            }
        }

        if (!_isMoving)
        {
            return;
        }
        float movement = _currrentSpeed * Time.deltaTime;
        foreach (SlotNumber t in _columnNumbers)
        {
            t.Move(movement);
        }
    }
    public void StartStopping()
    {
        _isStoppingStarted = true;
    }
    public void Roll()
    {
        m_RandomNumber=Random.Range(0, _columnNumbers.Count);
        _isStoppingStarted = false;
        _isMoving=true;
        _currrentSpeed = speed;

        DOVirtual.DelayedCall(_turnDuration-2, () =>
        {
            if (_IsFirst)
            {
                _isStoppingStarted = true;
            }
        });
    }
}
