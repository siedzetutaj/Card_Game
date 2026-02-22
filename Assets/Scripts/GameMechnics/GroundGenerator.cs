using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int _groundAmount;
    [SerializeField] private int _offeset;
    

    [SerializeField] private GameObject _groundPrefab;

    private void Start()
    {
        ClearGrid();
        GenerateGrid();
    }
    public void GenerateGrid()
    {
        for(int i = _groundAmount.x; i > 0; i--)
        {
            for (int j = _groundAmount.y; j > 0; j--)
            {
                GameObject newTile = Instantiate(_groundPrefab, transform);
                
                RectTransform rectTransform = newTile.GetComponent<RectTransform>();
                
                rectTransform.anchoredPosition = new Vector2(i * _offeset, j * _offeset);

            }
        }
    }
    public void ClearGrid()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
