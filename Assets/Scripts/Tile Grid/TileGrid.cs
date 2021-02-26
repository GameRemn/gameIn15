using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public Tile tilePrefab;
    public List<Tile> tiles;
    public virtual void CreateTileGrid(List<Cell> cells)
    {
        for (var i = 0; i < cells.Count; i++)
        {
            var tilesCell = cells[i];
            if (tilesCell.tile)
            {
                Debug.LogError("Cell hewe Tile");
                continue;
            }
            var position = tilesCell.transform.localPosition;
            Tile newTile;
            if (tilePrefab)
            {
                newTile = Instantiate(tilePrefab, transform, false);
                newTile.transform.localPosition = position;
            }
            else
            {
                newTile = CreateTile(position);
            }
            newTile.Cell = tilesCell;
            tiles.Add(newTile);
        }
    }

    public Tile CreateTile(Vector3 position)
    {
        var newTileGameObject = new GameObject("Tile");
        var newTile = newTileGameObject.AddComponent<Tile>();
        newTileGameObject.transform.SetParent(transform, false);
        newTileGameObject.transform.localPosition = position;
        return newTile;
    }

    public bool DestroyTiles(List<Tile> tileList)
    {
        if (!tiles.Any(tileList.Contains)) return false;
        tiles.RemoveAll(tileList.Contains);
        foreach (var tile in tileList)
        {
            Destroy(tile.gameObject);
        }
        return true;
    }

    public void DestroyTiles()
    {
        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }
        tiles.Clear();
    }

    public void DestroyTile(Tile tile)
    {
        if (!tiles.Contains(tile))
            tiles.Remove(tile);
        Destroy(tile.gameObject);
    }
}