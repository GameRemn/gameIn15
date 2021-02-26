using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class CellGrid : MonoBehaviour
{
    public Cell cellPrefab;
    public Vector3Int numberOfColumnsRowsLayers = new Vector3Int(2, 2, 2);
    public Vector3 lengthWidthHeight = new Vector3(100, 100, 100);
    public Vector3Int axisDirection = new Vector3Int(1, 1, 1);
    public List<Cell> cells;

    private Vector3 cellsScale;

    public void CreateCellGrid()
    {
        if (cells.Count > 0)
        {
            DestroyCells();
        }

        cellsScale = new Vector3(lengthWidthHeight.x / numberOfColumnsRowsLayers.x,
            lengthWidthHeight.y / numberOfColumnsRowsLayers.y, lengthWidthHeight.z / numberOfColumnsRowsLayers.z);
        for (var z = 0; z < numberOfColumnsRowsLayers.z; z++)
        {
            for (var y = 0; y < numberOfColumnsRowsLayers.y; y++)
            {
                for (var x = 0; x < numberOfColumnsRowsLayers.x; x++)
                {

                    Cell newCell;
                    if (cellPrefab)
                    {
                        newCell = Instantiate(cellPrefab, transform, false);
                    }
                    else
                    {
                        newCell = CreateCell();
                    }
                    newCell.transform.localPosition = OddrCoordinatesInLocalPosition(new Vector3Int(x, y, z));
                    newCell.positionInOddrCoordinates = new Vector3Int(x, y, z);
                    newCell.centerPosition = CenterOddrCoordinates(newCell.positionInOddrCoordinates);
                    cells.Add(newCell);
                }
            }
        }
    }

    private Cell CreateCell()
    {
        var newCellGameObject = new GameObject("Cell");
        var newCell = newCellGameObject.AddComponent<Cell>();
        newCellGameObject.transform.SetParent(transform, false);
        return newCell;
    }

    public void DestroyCells()
    {
        foreach (var cell in cells)
        {
            Destroy(cell.gameObject);
        }

        cells.Clear();
    }

    public Vector3 IndexInLocalPosition(int number) //Перевод порядкового номера ячейки в локальные координаты
    {
        return OddrCoordinatesInLocalPosition(IndexInOddrCoordinates(number));
    }

    public Vector3Int IndexInOddrCoordinates(int number) //Перевод порядкового номера ячейки в квадратные координаты
    {
        return new Vector3Int(number % numberOfColumnsRowsLayers.x,
            number / numberOfColumnsRowsLayers.x % numberOfColumnsRowsLayers.y,
            number / (numberOfColumnsRowsLayers.x * numberOfColumnsRowsLayers.y) % numberOfColumnsRowsLayers.z);
    }

    public int OddrCoordinatesInIndex(Vector3Int coordinates) //Перевод квадратных координат ячейки в порядковый номер
    {
        if (coordinates.x >= numberOfColumnsRowsLayers.x || coordinates.y >= numberOfColumnsRowsLayers.y ||
            coordinates.z >= numberOfColumnsRowsLayers.z || coordinates.x < 0 || coordinates.y < 0 ||
            coordinates.z < 0)
            return -1;
        return (coordinates.z * numberOfColumnsRowsLayers.y * numberOfColumnsRowsLayers.x) +
                   (coordinates.y * numberOfColumnsRowsLayers.x) + coordinates.x;
    }

    public Vector3 OddrCoordinatesInLocalPosition(Vector3Int coordinates) //Перевод квадратных координат ячейки в локальные координаты
    {
        return new Vector3(CenterOddrCoordinates(coordinates).x * cellsScale.x * axisDirection.x,
            CenterOddrCoordinates(coordinates).y * cellsScale.y * axisDirection.y,
            CenterOddrCoordinates(coordinates).z * cellsScale.z * axisDirection.z);
    }

    public Vector3 CenterOddrCoordinates(Vector3Int coordinates) //Квадратые координаты относительно центра системы координат
    {
        return new Vector3(coordinates.x - ((float)numberOfColumnsRowsLayers.x - 1) / 2,
            coordinates.y - ((float)numberOfColumnsRowsLayers.y - 1) / 2,
            coordinates.z - ((float)numberOfColumnsRowsLayers.z - 1) / 2);
    }
    
}