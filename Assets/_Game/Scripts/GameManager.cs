using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Column> _columns;

    public void RollTheColumns()
    {
        foreach(Column column in _columns)
        {
            column.Roll();
        }
    }
}
