using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
public class Column : MonoBehaviour
{
    [SerializeField] private List<SlotNumber> columnNumbers;
    [SerializeField] private Column nextColumn;
    [SerializeField] private bool isFirst;
    [SerializeField] private float elementHeight;
    [SerializeField] private float turnDuration;
    [SerializeField] private float delayForNextColumStop;
    [SerializeField] private float speed;
    [SerializeField] private float speedWhenStopping;
    [SerializeField] private List<Column> speedUpColumns;
    private int randomNumber;
    private bool isMoving;
    private bool isStoppingStarted;
    private float currentSpeed;

    #region Events
    public static event LastColumnStoppedEvent OnLastColumnStopped = delegate { };
    public delegate void LastColumnStoppedEvent();
    public void RaiseLastColumnStopped() { OnLastColumnStopped.Invoke(); }
    #endregion
    private void Start()
    {
        int index=0;
        foreach(SlotNumber t in columnNumbers)
        {
            t.transform.Translate(0, (elementHeight * index) -(columnNumbers.Count/2*elementHeight), 0);
            if(index>0)
            {
                t.SetNext(columnNumbers[index-1]);
            }
            else
            {
                columnNumbers[0].SetNext(columnNumbers[columnNumbers.Count-1]);
            }
            index++;
        }
    }
    private void FinalizeMovement()
    {
        transform.DOPunchPosition(Vector3.down, 0.25f, 1).OnComplete(() => 
        { 
            if(nextColumn==null)
            {
                RaiseLastColumnStopped();
            }
        });
    }
    private void Update()
    {
        if(isStoppingStarted)
        {
            if (columnNumbers[randomNumber].transform.position.y<0.25f && columnNumbers[randomNumber].transform.position.y > -0.25f)
            {
                if(nextColumn)
                {
                    DOVirtual.DelayedCall(delayForNextColumStop, () => 
                    {
                        nextColumn.StartStopping();
                    });
                }
                float delta = -columnNumbers[randomNumber].transform.position.y;
                foreach (SlotNumber slotNumber in columnNumbers)
                {
                    slotNumber.Move(delta);
                    slotNumber.StopLooping();
                }

                if (speedUpColumns != null)
                {
                    foreach (Column column in speedUpColumns)
                    {
                        column.SpeedUp();
                    }
                }

                isMoving = false;
                isStoppingStarted = false;
                FinalizeMovement();
            }
        }
        if (!isMoving)
        {
            return;
        }
        float movement = currentSpeed * Time.deltaTime;
        foreach (SlotNumber t in columnNumbers)
        {
            t.Move(movement);
        }
    }
    public void SpeedUp()
    {
        currentSpeed = speedWhenStopping;
    }
    public void StartStopping()
    {
        isStoppingStarted = true;
    }
    public void Roll()
    {
        currentSpeed = speed;
        randomNumber=Random.Range(0, columnNumbers.Count);
        isStoppingStarted = false;
        isMoving=true;
        foreach (SlotNumber t in columnNumbers)
        {
            t.StartLooping();
        }
        DOVirtual.DelayedCall(turnDuration-2, () =>
        {
            if (isFirst)
            {
                isStoppingStarted = true;
            }
        });
    }
}
