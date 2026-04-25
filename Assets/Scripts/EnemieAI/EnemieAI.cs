using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemieAI : MonoBehaviour
{
    public EnemieSO EnemieSO;
    public GameObject SpawnBuildingPrefab;

    [SerializeField] private GroundGenerator _groundGenerator;
    private List<EnemieGroundTileHandler> _tiles = new List<EnemieGroundTileHandler>();
    private TurnManager _turnManager => TurnManager.Instance;
    
    private int _power;
    private float _powerScale;
    private int _maxHealth;
    private int _currentHealth;

    private void OnEnable()
    {
        _turnManager.OnRoundEnd += TakeAction;
    }
    private void OnDisable()
    {
        _turnManager.OnRoundEnd -= TakeAction;
    }
    private void Start()
    {
        _power = EnemieSO.StartingPower;
        _powerScale = EnemieSO.AdditionalPower;
        _maxHealth = EnemieSO.Health;
        _currentHealth = _maxHealth;
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
        float currentPower = _power + (_powerScale * (1 - (_currentHealth/_maxHealth)));

        while (currentPower > 0)
        {
            var randomTile = GetRandomTile();
            if (randomTile == null)
                return;
            var buildingToSpawn = GetBuildingToSpawn(currentPower);
            if (buildingToSpawn == null)
                return;
            currentPower -= buildingToSpawn.BuildCost;
            randomTile.IsOccupied = true;
            SpawnBuildingHandler(randomTile, buildingToSpawn);
        }
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
    private EnemieBuildingSO GetBuildingToSpawn(float power)
    {
        List<EnemieBuildingSO> shuffledBuildingList = new List<EnemieBuildingSO>(EnemieSO.BuildingsToSpawn);

        for (int i = shuffledBuildingList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            EnemieBuildingSO temp = shuffledBuildingList[i];
            shuffledBuildingList[i] = shuffledBuildingList[randomIndex];
            shuffledBuildingList[randomIndex] = temp;
        }

        foreach (EnemieBuildingSO item in shuffledBuildingList)
        {
            if(item.BuildCost <= power)
            {
                return item;
            }
        }
        return null;
    }
    private void SpawnBuildingHandler(EnemieGroundTileHandler tile, BuildingSO buildingSO)
    {
        GameObject buildingGO = Instantiate(SpawnBuildingPrefab, tile.transform.position, Quaternion.identity, tile.transform);
        buildingGO.transform.localPosition = new Vector3(55, 30, 20);
        BuildingHandler buildingHandler = buildingGO.GetComponent<BuildingHandler>();
        buildingHandler.IsPlayerBuilding = false;
        buildingHandler.Initialize(buildingSO);
    }
}
