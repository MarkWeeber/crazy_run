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
    private float spaceOffest = 1f;

    private Maze maze;
    void Start()
    {
        maze = new Maze(mazeRows, mazeColumns, 0, 0);
        maze.BuildMazeData();
        BuildMazeWalls();
    }

    private void BuildMazeWalls()
    {
        if(wallPrefab != null && wallPrefabSideWays != null)
        {
            for (int i = 0; i < mazeRows; i++)
            {
                for (int j = 0; j < mazeColumns; j++)
                {
                    // check for sideways
                    if ( (j < mazeColumns -1) && (maze.matrix[i, j].blockRight && maze.matrix[i, j + 1].blockLeft) )
                    {
                        GameObject sideWaysWall = new GameObject();
                        Vector3 rotation = this.transform.rotation.eulerAngles;
                        rotation = new Vector3(rotation.x, rotation.y+90, rotation.z);
                        sideWaysWall =  Instantiate(
                            wallPrefabSideWays,
                            new Vector3(this.transform.position.x + spaceOffest * j + spaceOffest / 2,
                                        this.transform.position.y,
                                        this.transform.position.z + spaceOffest * i),
                            Quaternion.Euler(rotation),
                            this.transform);
                    }
                    // check for normal
                    if ( (i < mazeRows -1) && (maze.matrix[i, j].blockDown && maze.matrix[i + 1, j].blockUp) )
                    {
                        GameObject normalWall = new GameObject();
                        normalWall =  Instantiate(
                            wallPrefab,
                            new Vector3(this.transform.position.x + spaceOffest * j,
                                        this.transform.position.y,
                                        this.transform.position.z + spaceOffest * i + spaceOffest / 2),
                            Quaternion.identity,
                            this.transform);
                    }
                }
            }
        }
    }

    void Update()
    {
        
    }
}
