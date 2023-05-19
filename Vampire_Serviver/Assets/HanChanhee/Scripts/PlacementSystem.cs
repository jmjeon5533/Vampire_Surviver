using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField]
    GameObject mouseIndicator, cellindicator;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDataBaseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;



    // Start is called before the first frame update
    void Start()
    {
        StopPlacement();

    }


    public void StartPlacement(int Id)
    {
        StopPlacement();

        selectedObjectIndex = database.objectDatas.FindIndex(data => data.Id == Id);    
        if(selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID Found {Id}");
            return;
        }
        gridVisualization.SetActive(true);
        cellindicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if(inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        GameObject PgameObject = Instantiate(database.objectDatas[selectedObjectIndex].Prefab);
        PgameObject.transform.position = grid.CellToWorld(gridPos);
    }

    void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellindicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) StartPlacement(1);
        if (selectedObjectIndex < 0) return;
        Vector3 mousePos = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        mouseIndicator.transform.position = mousePos;
        cellindicator.transform.position = grid.CellToWorld(gridPos);

    }
}
