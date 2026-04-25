using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public List<GameObject> GroundTiles = new List<GameObject>();

    [SerializeField] private Vector2Int _groundAmount;
    [SerializeField] private int _offeset;
    

    [SerializeField] private GameObject _groundPrefab;

    private void OnEnable()
    {
        GenerateGrid();
    }
    public void GenerateGrid()
    {
        ClearGrid();
        for (int i = _groundAmount.x; i > 0; i--)
        {
            for (int j = _groundAmount.y; j > 0; j--)
            {
                GameObject newTile = Instantiate(_groundPrefab, transform);
                GroundTiles.Add(newTile);

                RectTransform rectTransform = newTile.GetComponent<RectTransform>();
                
                rectTransform.anchoredPosition = new Vector2(i * _offeset, j * _offeset);

            }
        }
    }
    public void ClearGrid()
    {
        foreach (GameObject tile in GroundTiles)
        {
            if (tile != null)
            {
                DestroyImmediate(tile);
            }
        }
        GroundTiles.Clear();
    }
}
