using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBuilder : MonoBehaviour
{
    [SerializeField]
    private int mazeRows = 4;
    [SerializeField]
    private int mazeColumns = 4;
    [SerializeField]
    private GameObject wallPrefab = null;
    [SerializeField]
    private GameObject wallPrefabSideWays = null;
    [SerializeField]
    private GameObject outerWallPrefab = null;
    [SerializeField]
    private GameObject floorPrefab = null;


    [SerializeField]
    private float spaceOffest = 1f;
    private float spaceOffestHight = 1f;

    private Maze maze;
    void Start()
    {
        maze = new Maze(mazeRows, mazeColumns, 0, 0);
        maze.BuildMazeData();
        BuildMaze();
        CombineMeshesInChildren();
    }

    private void BuildMaze()
    {
        if (wallPrefab != null && wallPrefabSideWays != null)
        {
            for (int i = 0; i < mazeRows; i++)
            {
                for (int j = 0; j < mazeColumns; j++)
                {
                    // check for sideways
                    if ((j < mazeColumns - 1) && (maze.matrix[i, j].blockRight && maze.matrix[i, j + 1].blockLeft))
                    {
                        GameObject sideWaysWall = new GameObject();
                        Vector3 rotation = this.transform.rotation.eulerAngles;
                        rotation = new Vector3(rotation.x, rotation.y + 90, rotation.z);
                        sideWaysWall = Instantiate(
                            wallPrefabSideWays,
                            new Vector3(this.transform.position.x + spaceOffest * j + spaceOffest / 2,
                                        this.transform.position.y + spaceOffestHight,
                                        this.transform.position.z + spaceOffest * i),
                            Quaternion.Euler(rotation),
                            this.transform);
                    }
                    // check for normal
                    if ((i < mazeRows - 1) && (maze.matrix[i, j].blockDown && maze.matrix[i + 1, j].blockUp))
                    {
                        GameObject normalWall = new GameObject();
                        normalWall = Instantiate(
                            wallPrefab,
                            new Vector3(this.transform.position.x + spaceOffest * j,
                                        this.transform.position.y + spaceOffestHight,
                                        this.transform.position.z + spaceOffest * i + spaceOffest / 2),
                            Quaternion.identity,
                            this.transform);
                    }
                    // place floor
                    if (i > 0 || j > 0)
                    {
                        GameObject floor = new GameObject();
                        floor = Instantiate(
                            floorPrefab,
                            new Vector3(this.transform.position.x + spaceOffest * j,
                                        this.transform.position.y,
                                        this.transform.position.z + spaceOffest * i),
                            Quaternion.identity,
                            this.transform
                        );
                    }
                    // place outer walls, just run once
                    if (i == 0 && j == 0)
                    {
                        GameObject outerWall = new GameObject();
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
                        outerWall = Instantiate(
                            outerWallPrefab,
                            new Vector3(xPos,
                                        yPos,
                                        zPos),
                            Quaternion.identity,
                            this.transform
                        );
                        outerWall.transform.localScale = new Vector3(xSize, ySize, zSize);
                        outerWall.transform.name = "bottomWall";
                        // top
                        xPos = this.transform.position.x + spaceOffest * ((mazeColumns - 1) / 2f);
                        zPos = this.transform.position.z + spaceOffest * mazeRows;
                        xSize = spaceOffest * (mazeColumns + 1);
                        zSize = spaceOffest;
                        outerWall = Instantiate(
                            outerWallPrefab,
                            new Vector3(xPos,
                                        yPos,
                                        zPos),
                            Quaternion.identity,
                            this.transform
                        );
                        outerWall.transform.localScale = new Vector3(xSize, ySize, zSize);
                        outerWall.transform.name = "topWall";
                        // left
                        xPos = this.transform.position.x - spaceOffest;
                        zPos = this.transform.position.z + spaceOffest * ((mazeRows - 1) / 2f);
                        xSize = spaceOffest;
                        zSize = spaceOffest * (mazeRows + 2);
                        outerWall = Instantiate(
                            outerWallPrefab,
                            new Vector3(xPos,
                                        yPos,
                                        zPos),
                            Quaternion.identity,
                            this.transform
                        );
                        outerWall.transform.localScale = new Vector3(xSize, ySize, zSize);
                        outerWall.transform.name = "leftWall";
                        // right
                        xPos = this.transform.position.x + spaceOffest * (mazeColumns);
                        zPos = this.transform.position.z + spaceOffest * ((mazeRows - 1) / 2f);
                        xSize = spaceOffest;
                        zSize = spaceOffest * (mazeRows + 2);
                        outerWall = Instantiate(
                            outerWallPrefab,
                            new Vector3(xPos,
                                        yPos,
                                        zPos),
                            Quaternion.identity,
                            this.transform
                        );
                        outerWall.transform.localScale = new Vector3(xSize, ySize, zSize);
                        outerWall.transform.name = "rightWall";
                    }
                }
            }
        }
    }

    private void CombineMeshesInChildren()
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
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.GetComponent<MeshCollider>().sharedMesh = null;
        transform.GetComponent<MeshCollider>().sharedMesh = transform.GetComponent<MeshFilter>().mesh;
        transform.gameObject.SetActive(true);
    }
    void Update()
    {

    }
}
