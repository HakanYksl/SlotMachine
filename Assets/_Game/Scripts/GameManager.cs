using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Column> columns;
    private bool isRolling;
    private void OnEnable()
    {
        Column.OnLastColumnStopped += HandleRollingFinished;
    }
    private void OnDisable()
    {
        Column.OnLastColumnStopped -= HandleRollingFinished;
    }
    private void HandleRollingFinished()
    {
        isRolling = false;
    }
    public void RollTheColumns()
    {
        if(isRolling)
        {
            return;
        }

        isRolling = true;
        foreach(Column column in columns)
        {
            column.Roll();
        }
    }
}
