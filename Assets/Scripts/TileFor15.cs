using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StepByStepMovement))]
public class TileFor15 : Tile
{
    public Cell trueCell;
    private StepByStepMovement stepByStepMovement;

    private void Start()
    {
        stepByStepMovement = GetComponent<StepByStepMovement>();
    }

    public void Move(Cell newCell)
    {
        Cell = newCell;
        stepByStepMovement.StartMoving(newCell.transform.position);
    }

    public void Click()
    {
        GameManager.Instance.NewTarget(this);
    }
}
