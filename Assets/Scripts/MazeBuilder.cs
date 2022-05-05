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
    private GameObject viewCenter = null;
    private GameObject endBox = null;
    private List<CombineInstance> wallCI;
    private List<CombineInstance> outerWallsCI;
    private List<CombineInstance> floorCI;
    private List<MeshFilter> subMeshes;
    void Start()
    {
        viewCenter = transform.GetChild(0).GetComponent<Transform>().gameObject;
        endBox = transform.GetChild(1).GetComponent<Transform>().gameObject;
        wallCI = new List<CombineInstance>();
        outerWallsCI = new List<CombineInstance>();
        floorCI = new List<CombineInstance>();
        subMeshes = new List<MeshFilter>();
        maze = new Maze(mazeRows, mazeColumns, 0, 0);
        maze.BuildMazeData();
        BuildMaze();
        CombineMeshes();
    }

    private void BuildMaze()
    {
        float initX = this.transform.position.x;
        float initY = this.transform.position.y;
        float initZ = this.transform.position.z;
        // place viewcenter to center
        viewCenter.transform.position = new Vector3(
                                                transform.position.x + (mazeColumns - 1) * spaceOffest / 2f,
                                                transform.position.y,
                                                transform.position.z + (mazeRows - 1) * spaceOffest / 2f
                                                );
        // place endbox to end
        endBox.transform.position = new Vector3(
                                                transform.position.x + (mazeColumns - 1)* spaceOffest,
                                                transform.position.y + spaceOffestHight,
                                                transform.position.z + (mazeRows - 1)* spaceOffest
                                                );
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
                        new Vector3(initX + spaceOffest * j + spaceOffest / 2f,
                                    initY + spaceOffestHight,
                                    initZ + spaceOffest * i),
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
                        new Vector3(initX + spaceOffest * j,
                                    initY + spaceOffestHight,
                                    initZ + spaceOffest * i + spaceOffest / 2f),
                        Quaternion.identity,
                        this.transform);
                    prefabInstance.transform.localScale = new Vector3(spaceOffest * 1.2f, spaceOffest, spaceOffest * 0.2f);
                    wallCI.Add(GetCombineInstanceInfo(prefabInstance));
                }
                // place floor
                prefabInstance = Instantiate(
                    floorPrefab,
                    new Vector3(initX + spaceOffest * j,
                                initY,
                                initZ + spaceOffest * i),
                    Quaternion.identity,
                    this.transform
                );
                floorCI.Add(GetCombineInstanceInfo(prefabInstance));

                // place outer walls, just run once
                if (i == 0 && j == 0)
                {
                    float xPos = 0f;
                    float zPos = 0f;
                    float yPos = initY + spaceOffest / 2f;
                    float xSize = spaceOffest;
                    float zSize = spaceOffest;
                    float ySize = 2 * spaceOffest;
                    // bottom
                    xPos = initX + spaceOffest * ((mazeColumns - 1) / 2f);
                    zPos = initZ - spaceOffest;
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
                    xPos = initX + spaceOffest * ((mazeColumns - 1) / 2f);
                    zPos = initZ + spaceOffest * mazeRows;
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
                    xPos = initX - spaceOffest;
                    zPos = initZ + spaceOffest * ((mazeRows - 1) / 2f);
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
                    xPos = initX + spaceOffest * (mazeColumns);
                    zPos = initZ + spaceOffest * ((mazeRows - 1) / 2f);
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
            if(GameObject.ReferenceEquals(transform.GetChild(i).gameObject, viewCenter)
                ||
               GameObject.ReferenceEquals(transform.GetChild(i).gameObject, endBox))
            {
                continue;
            }
            Destroy(transform.GetChild(i).gameObject);
        }
        // combine walls
        prefabInstance = Instantiate(wallPrefab, this.transform.position, Quaternion.identity, this.transform);
        prefabInstance.GetComponent<MeshFilter>().mesh = new Mesh();
        prefabInstance.GetComponent<MeshFilter>().mesh.CombineMeshes(wallCI.ToArray(), true);
        prefabInstance.GetComponent<MeshCollider>().sharedMesh = null;
        prefabInstance.GetComponent<MeshCollider>().sharedMesh = prefabInstance.GetComponent<MeshFilter>().mesh;
        prefabInstance.SetActive(true);
        subMeshes.Add(prefabInstance.GetComponent<MeshFilter>());
        // combine floor
        prefabInstance = Instantiate(floorPrefab, this.transform.position, Quaternion.identity, this.transform);
        prefabInstance.GetComponent<MeshFilter>().mesh = new Mesh();
        prefabInstance.GetComponent<MeshFilter>().mesh.CombineMeshes(floorCI.ToArray(), true);
        prefabInstance.GetComponent<MeshCollider>().sharedMesh = null;
        prefabInstance.GetComponent<MeshCollider>().sharedMesh = prefabInstance.GetComponent<MeshFilter>().mesh;
        prefabInstance.SetActive(true);
        subMeshes.Add(prefabInstance.GetComponent<MeshFilter>());
        // combine combine outerwalls
        prefabInstance = Instantiate(outerWallPrefab, this.transform.position, Quaternion.identity, this.transform);
        prefabInstance.GetComponent<MeshFilter>().mesh = new Mesh();
        prefabInstance.GetComponent<MeshFilter>().mesh.CombineMeshes(outerWallsCI.ToArray(), true);
        prefabInstance.GetComponent<MeshCollider>().sharedMesh = null;
        prefabInstance.GetComponent<MeshCollider>().sharedMesh = prefabInstance.GetComponent<MeshFilter>().mesh;
        prefabInstance.SetActive(true);
        subMeshes.Add(prefabInstance.GetComponent<MeshFilter>());
        transform.gameObject.SetActive(true);
    }

    private void UniteSubmeshes()
    {
        int count = subMeshes.Count;
        MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
        meshFilter.mesh.subMeshCount = count + 1;
        for (int i = 0; i < count + 1; i++)
        {
            Mesh mesh = subMeshes[i].mesh;
            meshFilter.mesh.SetSubMesh(
                i,
                mesh.GetSubMesh(0)
                );
        }
    }

    private void CombineMeshByUnity()
    {
        int count = subMeshes.Count;
        CombineInstance[] combine = new CombineInstance[count];
        Debug.Log(count);

        int i = 0;
        while (i < count)
        {
            combine[i].mesh = subMeshes[i].sharedMesh;
            combine[i].transform = subMeshes[i].transform.localToWorldMatrix;
            subMeshes[i].gameObject.SetActive(false);
            i++;
        }
        transform.GetComponent<MeshFilter>().mesh.subMeshCount = count + 1;
        Debug.Log(transform.GetComponent<MeshFilter>().mesh.subMeshCount);
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, false);
        transform.gameObject.SetActive(true);
    }

    void Update()
    {

    }
}
