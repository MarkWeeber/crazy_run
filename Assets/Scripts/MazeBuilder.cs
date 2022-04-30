using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeBuilder : MonoBehaviour
{
    [SerializeField]
    private int mazeRows = 4;
    [SerializeField]
    private int mazeColumns = 4;
    [SerializeField]
    private GameObject wallPrefab = null;
    [SerializeField]
    private GameObject outerWallPrefab = null;
    [SerializeField]
    private GameObject floorPrefab = null;

    [SerializeField]
    private float spaceOffest = 1f;
    private float spaceOffestHight = 1f;
    private Maze maze;
    private GameObject prefabInstance = null;
    private List<CombineInstance> wallCI;
    private List<CombineInstance> outerWallsCI;
    private List<CombineInstance> floorCI;
    void Start()
    {
        wallCI = new List<CombineInstance>();
        outerWallsCI = new List<CombineInstance>();
        floorCI = new List<CombineInstance>();
        maze = new Maze(mazeRows, mazeColumns, 0, 0);
        maze.BuildMazeData();
        BuildMaze();
        CombineMeshes();
    }

    private void BuildMaze()
    {
        for (int i = 0; i < mazeRows; i++)
        {
            for (int j = 0; j < mazeColumns; j++)
            {
                // check for sideways
                if ((j < mazeColumns - 1) && (maze.matrix[i, j].blockRight && maze.matrix[i, j + 1].blockLeft))
                {
                    Vector3 rotation = this.transform.rotation.eulerAngles;
                    rotation = new Vector3(rotation.x, rotation.y + 90, rotation.z);
                    prefabInstance = Instantiate(
                        wallPrefab,
                        new Vector3(this.transform.position.x + spaceOffest * j + spaceOffest / 2f,
                                    this.transform.position.y + spaceOffestHight,
                                    this.transform.position.z + spaceOffest * i),
                        Quaternion.Euler(rotation),
                        this.transform);
                    prefabInstance.transform.localScale = new Vector3(spaceOffest * 1.2f, spaceOffest, spaceOffest * 0.2f);
                    wallCI.Add(GetCombineInstanceInfo(prefabInstance));
                }
                // check for normal
                if ((i < mazeRows - 1) && (maze.matrix[i, j].blockDown && maze.matrix[i + 1, j].blockUp))
                {
                    prefabInstance = Instantiate(
                        wallPrefab,
                        new Vector3(this.transform.position.x + spaceOffest * j,
                                    this.transform.position.y + spaceOffestHight,
                                    this.transform.position.z + spaceOffest * i + spaceOffest / 2f),
                        Quaternion.identity,
                        this.transform);
                    prefabInstance.transform.localScale = new Vector3(spaceOffest * 1.2f, spaceOffest, spaceOffest * 0.2f);
                    wallCI.Add(GetCombineInstanceInfo(prefabInstance));
                }
                // place floor
                prefabInstance = Instantiate(
                    floorPrefab,
                    new Vector3(this.transform.position.x + spaceOffest * j,
                                this.transform.position.y,
                                this.transform.position.z + spaceOffest * i),
                    Quaternion.identity,
                    this.transform
                );
                floorCI.Add(GetCombineInstanceInfo(prefabInstance));

                // place outer walls, just run once
                if (i == 0 && j == 0)
                {
                    float xPos = 0f;
                    float zPos = 0f;
                    float yPos = this.transform.position.z + spaceOffest / 2f;
                    float xSize = spaceOffest;
                    float zSize = spaceOffest;
                    float ySize = 2 * spaceOffest;
                    // bottom
                    xPos = this.transform.position.x + spaceOffest * ((mazeColumns - 1) / 2f);
                    zPos = this.transform.position.z - spaceOffest;
                    xSize = spaceOffest * (mazeColumns + 1);
                    zSize = spaceOffest;
                    prefabInstance = Instantiate(
                        outerWallPrefab,
                        new Vector3(xPos,
                                    yPos,
                                    zPos),
                        Quaternion.identity,
                        this.transform
                    );
                    prefabInstance.transform.localScale = new Vector3(xSize, ySize, zSize);
                    prefabInstance.transform.name = "bottomWall";
                    outerWallsCI.Add(GetCombineInstanceInfo(prefabInstance));
                    // top
                    xPos = this.transform.position.x + spaceOffest * ((mazeColumns - 1) / 2f);
                    zPos = this.transform.position.z + spaceOffest * mazeRows;
                    xSize = spaceOffest * (mazeColumns + 1);
                    zSize = spaceOffest;
                    prefabInstance = Instantiate(
                        outerWallPrefab,
                        new Vector3(xPos,
                                    yPos,
                                    zPos),
                        Quaternion.identity,
                        this.transform
                    );
                    prefabInstance.transform.localScale = new Vector3(xSize, ySize, zSize);
                    prefabInstance.transform.name = "topWall";
                    outerWallsCI.Add(GetCombineInstanceInfo(prefabInstance));
                    // left
                    xPos = this.transform.position.x - spaceOffest;
                    zPos = this.transform.position.z + spaceOffest * ((mazeRows - 1) / 2f);
                    xSize = spaceOffest;
                    zSize = spaceOffest * (mazeRows + 2);
                    prefabInstance = Instantiate(
                        outerWallPrefab,
                        new Vector3(xPos,
                                    yPos,
                                    zPos),
                        Quaternion.identity,
                        this.transform
                    );
                    prefabInstance.transform.localScale = new Vector3(xSize, ySize, zSize);
                    prefabInstance.transform.name = "leftWall";
                    outerWallsCI.Add(GetCombineInstanceInfo(prefabInstance));
                    // right
                    xPos = this.transform.position.x + spaceOffest * (mazeColumns);
                    zPos = this.transform.position.z + spaceOffest * ((mazeRows - 1) / 2f);
                    xSize = spaceOffest;
                    zSize = spaceOffest * (mazeRows + 2);
                    prefabInstance = Instantiate(
                        outerWallPrefab,
                        new Vector3(xPos,
                                    yPos,
                                    zPos),
                        Quaternion.identity,
                        this.transform
                    );
                    prefabInstance.transform.localScale = new Vector3(xSize, ySize, zSize);
                    prefabInstance.transform.name = "rightWall";
                    outerWallsCI.Add(GetCombineInstanceInfo(prefabInstance));
                }
            }
        }
    }

    private CombineInstance GetCombineInstanceInfo(GameObject obj)
    {
        CombineInstance combineInstance = new CombineInstance();
        combineInstance.mesh = obj.GetComponent<MeshFilter>().sharedMesh;
        combineInstance.transform = obj.transform.localToWorldMatrix;
        return combineInstance;
    }

    private void CombineMeshes()
    {
        // deactivate all chidren
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        // combine walls
        prefabInstance = Instantiate(wallPrefab, this.transform.position, Quaternion.identity, this.transform);
        prefabInstance.GetComponent<MeshFilter>().mesh = new Mesh();
        prefabInstance.GetComponent<MeshFilter>().mesh.CombineMeshes(wallCI.ToArray(), true);
        prefabInstance.GetComponent<MeshCollider>().sharedMesh = null;
        prefabInstance.GetComponent<MeshCollider>().sharedMesh = prefabInstance.GetComponent<MeshFilter>().mesh;
        prefabInstance.SetActive(true);
        // combine floor
        prefabInstance = Instantiate(floorPrefab, this.transform.position, Quaternion.identity, this.transform);
        prefabInstance.GetComponent<MeshFilter>().mesh = new Mesh();
        prefabInstance.GetComponent<MeshFilter>().mesh.CombineMeshes(floorCI.ToArray(), true);
        prefabInstance.GetComponent<MeshCollider>().sharedMesh = null;
        prefabInstance.GetComponent<MeshCollider>().sharedMesh = prefabInstance.GetComponent<MeshFilter>().mesh;
        prefabInstance.SetActive(true);
        // combine combine outerwalls
        prefabInstance = Instantiate(outerWallPrefab, this.transform.position, Quaternion.identity, this.transform);
        prefabInstance.GetComponent<MeshFilter>().mesh = new Mesh();
        prefabInstance.GetComponent<MeshFilter>().mesh.CombineMeshes(outerWallsCI.ToArray(), true);
        prefabInstance.GetComponent<MeshCollider>().sharedMesh = null;
        prefabInstance.GetComponent<MeshCollider>().sharedMesh = prefabInstance.GetComponent<MeshFilter>().mesh;
        prefabInstance.SetActive(true);

        transform.gameObject.SetActive(true);
    }

    private void CombineMeshByUnity()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, false);
        transform.gameObject.SetActive(true);
    }

    void Update()
    {

    }
}
