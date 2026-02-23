using System.Collections.Generic;
using UnityEngine;

public class EnemieAI : MonoBehaviour
{
    public EnemieSO EnemieData;
    public GameObject SpawnBuildingPrefab;

    [SerializeField] private GroundGenerator _groundGenerator;
    private List<EnemieGroundTileHandler> _tiles = new List<EnemieGroundTileHandler>();
    private GameLogicManager _gameLogicManager => GameLogicManager.Instance;
    //trzeba przypomnieæ sobie jak dzia³aj¹ tury
    private void OnEnable()
    {
        _gameLogicManager.OnAllUnitsDead += TakeAction;
    }
    private void OnDisable()
    {
        _gameLogicManager.OnAllUnitsDead -= TakeAction;
    }
    private void Start()
    {
        _groundGenerator.GroundTiles.ForEach(tile =>
        {
            EnemieGroundTileHandler handler = tile.GetComponent<EnemieGroundTileHandler>();
            if (handler != null)
                _tiles.Add(handler);
        });
        TakeAction();
    }
    private void TakeAction()
    {
        var randomTile = GetRandomTile();   
        if(randomTile == null)
            return;
        var buildingToSpawn = GetBuildingToSpawn();
        if(buildingToSpawn == null)
            return;
        randomTile.IsOccupied = true;
        SpawnBuildingHandler(randomTile, buildingToSpawn);

    }
    private EnemieGroundTileHandler GetRandomTile()
    {
        var FreeTile = _tiles.FindAll(tile => tile.IsOccupied == false);
        if (FreeTile.Count > 0)
        {
            int randomIndex = Random.Range(0, FreeTile.Count);
            return FreeTile[randomIndex];
        }
        return null;
    }
    private BuildingSO GetBuildingToSpawn()
    {
        int totalWeight = 0;
        foreach (var building in EnemieData.BuildingsToSpawn)
        {
            totalWeight += building.SpawnChance;
        }
        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;
        foreach (var building in EnemieData.BuildingsToSpawn)
        {
            cumulativeWeight += building.SpawnChance;
            if (randomValue < cumulativeWeight)
            {
                return building;
            }
        }
        return null; 
    }
    private void SpawnBuildingHandler(EnemieGroundTileHandler tile, BuildingSO buildingSO)
    {
        GameObject buildingGO = Instantiate(SpawnBuildingPrefab, tile.transform.position, Quaternion.identity, tile.transform);
        BuildingHandler buildingHandler = buildingGO.GetComponent<BuildingHandler>();
        buildingHandler.IsPlayerBuilding = false;
        buildingHandler.Initialize(buildingSO);
    }
}
