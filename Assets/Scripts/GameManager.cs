using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager> //Да, я знаю, что так называть классы нельзя, по хорошему нужно разделять ответственность
{
    public RectTransform gamePlaceRectTransform;
    public CellGrid cellGrid;
    public TileGrid tileGrid;
    public Cell missingCell;

    private void Start()
    {
        BuildLvl();
    }

    public void BuildLvl()
    {
        
        if (!cellGrid)
        {
            cellGrid = CreateCellGridObject();
        }

        if (!tileGrid)
        {
            tileGrid = CreateTileGridObject();
        }

        if (cellGrid.cells.Count > 0 || tileGrid.tiles.Count > 0)
        {
            DestroyLvl();
        }
        SetupSettings(LvlManager.Instance.lvl);
        cellGrid.CreateCellGrid();
        Debug.Log("Сетка ячеек создана");
        missingCell = cellGrid.cells[cellGrid.cells.Count - 1];
        tileGrid.CreateTileGrid(cellGrid.cells.GetRange(0, cellGrid.cells.Count - 1));
        Debug.Log("Сетка тайлов создана");
        ChangeTilesSize(tileGrid.tiles);
        RandCellForTile(tileGrid.tiles, cellGrid.cells.GetRange(0, cellGrid.cells.Count - 1));
        ChangeTiles(tileGrid.tiles, LvlManager.Instance.lvl);
    }

    public void DestroyLvl()
    {
        tileGrid.DestroyTiles();
        cellGrid.DestroyCells();
        Debug.Log("Уровень отчищен");
    }

    public CellGrid CreateCellGridObject()
    {
        var newCellGridGameObject = new GameObject("CellGrid");
        CellGrid newCellGrid = newCellGridGameObject.AddComponent<CellGrid>();
        newCellGridGameObject.transform.SetParent(transform, false);
        return newCellGrid;
    }

    public TileGrid CreateTileGridObject()
    {
        var newTileGridGameObject = new GameObject("TilesGenerator");
        TileGrid newTileGrid = newTileGridGameObject.AddComponent<TileGrid>();
        newTileGridGameObject.transform.SetParent(transform, false);
        return newTileGrid;
    }

    private void RandCellForTile(List<Tile> tiles, List<Cell> cells) //раздаём каждой плитке случайное место
    {
        foreach(var tile in tiles)
        {
            var r = UnityEngine.Random.Range(0, cells.Count);
            ((TileFor15)tile).trueCell = cells[r]; //TODO: Может выдать ошибку из-за привязки к конкретному классу, нужно что-то придумать с этим
            cells.RemoveAt(r);
        }

        /*if (!CanWin())
        {
            Debug.Log("Собираемость изменена");
            //TODO: Тут явно что-то не так, нужно переписать
            var trueCell = ((TileFor15)tiles[0]).trueCell;
            ((TileFor15)tiles[0]).trueCell = ((TileFor15)tiles[1]).trueCell;
            ((TileFor15)tiles[1]).trueCell = trueCell;
        }
        if (Win())
        {
            RandCellForTile(tiles, cells);
        }*/
    }

    public bool CanWin() //проверка на собираемость
    {
        var rez = false;
        var missingCount = cellGrid.numberOfColumnsRowsLayers.x;
        for(var i = 0; i < tileGrid.tiles.Count; i++)
        {
            for(var j = i; j < tileGrid.tiles.Count; j++)
            {
                if (cellGrid.OddrCoordinatesInIndex(tileGrid.tiles[i].Cell.positionInOddrCoordinates) > cellGrid.OddrCoordinatesInIndex(tileGrid.tiles[j].Cell.positionInOddrCoordinates))
                    missingCount++;
            }
        }
        if (missingCount % 2 == cellGrid.numberOfColumnsRowsLayers.x % 2) //если собрать возможно
        {
            rez = true;
        }
        return rez;
    }
    
    private bool Win()
    {
        for(var i = 0; i < tileGrid.tiles.Count; i++)
        {
            if (tileGrid.tiles[i].Cell != ((TileFor15)tileGrid.tiles[i]).trueCell)
                return false;
        }
        return true;
    }
    
    public void NewTarget(Tile tile) //перемещение плитки
    {
        var naprav = (missingCell.positionInOddrCoordinates - tile.Cell.positionInOddrCoordinates);
        if (naprav.x != 0 && naprav.y != 0) return;
        naprav /= Mathf.Abs(naprav.x + naprav.y);
        var newCell = cellGrid.cells[cellGrid.OddrCoordinatesInIndex(new Vector3Int(tile.Cell.positionInOddrCoordinates.x + naprav.x, tile.Cell.positionInOddrCoordinates.y + naprav.y, tile.Cell.positionInOddrCoordinates.z + naprav.z))];
        if (newCell.tile != null)
        {
            NewTarget(newCell.tile);
        }
        missingCell = tile.Cell;
        ((TileFor15)tile).Move(newCell);
        if (Win()) 
            Debug.Log("WIN!!!");
    }

    public void ChangeTilesSize(List<Tile> tiles) //Меняем размеры тайлов //TODO: Не красиво, можно лучше
    {
        foreach (var tile in tiles)
        {
            var tileMaxLocalSize = tile.GetComponent<MaxLocalSize>();
            if (!tileMaxLocalSize)
            {
                tileMaxLocalSize = tile.gameObject.AddComponent<MaxLocalSize>();
            }

            tileMaxLocalSize.AsSquare = false;
            tileMaxLocalSize.ParentRectTransform = gamePlaceRectTransform;
            if (cellGrid)
            {
                tileMaxLocalSize.Scale = new Vector2(1f / cellGrid.numberOfColumnsRowsLayers.x,
                    1f / cellGrid.numberOfColumnsRowsLayers.y);
            }
        }
    }

    public void ChangeTiles(List<Tile> tiles, Lvl lvl)
    {
        if (lvl.gameMode == Lvl.GameMode.TextAndImage || lvl.gameMode == Lvl.GameMode.Text)
            ChangeTilesText(tiles);
        if (lvl.gameMode == Lvl.GameMode.TextAndImage || lvl.gameMode == Lvl.GameMode.Image)
            ChangeTilesImage(tiles, LvlManager.Instance.lvl);
    }

    public void ChangeTilesText(List<Tile> tiles)
    {
        foreach (var tile in tiles)
        {
            tile.GetComponentInChildren<TextMeshProUGUI>().text =
                (cellGrid.OddrCoordinatesInIndex(((TileFor15) tile).trueCell.positionInOddrCoordinates) + 1).ToString();
        }
    }
    
    public void ChangeTilesImage(List<Tile> tiles, Lvl lvl)
    {
        var newSprites = SpriteEditor.CutTheSprite(lvl.texture2D, lvl.numberOfColumnsAndRows);
        foreach (var tile in tiles)
        {
            var position = ((TileFor15) tile).trueCell.positionInOddrCoordinates;
            var number = cellGrid.OddrCoordinatesInIndex(new Vector3Int(position.x, position.y, position.z));
            tile.GetComponent<Image>().sprite = newSprites[number];
        }
    }

    public void SetupSettings(Lvl lvl)
    {
        if (!gamePlaceRectTransform)
        {
            gamePlaceRectTransform = GetComponent<RectTransform>();
        }
        Rect rect = gamePlaceRectTransform.rect;
        cellGrid.lengthWidthHeight = new Vector2(rect.width, rect.height);
        cellGrid.numberOfColumnsRowsLayers = new Vector3Int(lvl.numberOfColumnsAndRows.x, lvl.numberOfColumnsAndRows.y, 1);
    }
    /*private Vector3 NumberInLocalPosition(int n) //Перевод порядкового номера ячейки в локальные координаты
    {
        --n;
        return new Vector3((_numbersScale - _maxDistance) * 0.5f + _numbersScale * (n % cellsInLine), ((_numbersScale - _maxDistance) * 0.5f + _numbersScale * (n / cellsInLine)) * -1, 0);
    }
    private Vector2Int NumberInOddrCoordinates(int n)
    {
        --n;
        return new Vector2Int(n % cellsInLine, n / cellsInLine);
    }
    private int OddrCoordinatesInNumber(Vector2Int coordinates)
    {
        return (coordinates.y * cellsInLine) + coordinates.x + 1;
    }*/
}
