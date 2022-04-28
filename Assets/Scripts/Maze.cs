using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    UP, DOWN, LEFT, RIGHT, NO_DIRECTION
}

public class MazeCell
{
    public bool blockUp;
    public bool blockDown;
    public bool blockLeft;
    public bool blockRight;
    public bool steppedIn;
    public MazeCell()
    {
        blockUp = false;
        blockDown = false;
        blockLeft = false;
        blockRight = false;
        steppedIn = false;
    }
}

public class Array2D
{
    public int rowPosition;
    public int columnPosition;
    public Array2D(int rowPosition, int columnPosition)
    {
        this.rowPosition = rowPosition;
        this.columnPosition = columnPosition;
    }
}

public class Maze
{
    private int rows;
    private int columns;
    public MazeCell[,] matrix;
    private int rowStart;
    private int columnStart;
    private int counter;
    public Maze(int rows, int columns, int rowStart, int columnStart)
    {
        this.rows = rows;
        this.columns = columns;
        matrix = new MazeCell[this.rows, this.columns];
        this.rowStart = rowStart;
        this.columnStart = columnStart;
        counter = 0;
        FillArray();
    }

    public void BuildMazeData()
    {
        BuildRecursive(rowStart, columnStart);
    }

    private void FillArray()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                matrix[i,j] = new MazeCell();
            }
        }
    }
    private void BuildRecursive(int currentRow, int currentColumn, Direction direction = Direction.NO_DIRECTION)
    {
        counter++;
        matrix[currentRow, currentColumn].steppedIn = true;
        if(direction != Direction.NO_DIRECTION)
        {
            PlaceBlockers(currentRow, currentColumn, direction);
        }
        while (true)
        {
            // look for possible steps
            List<Array2D> posibleSteps = new List<Array2D>();
            List<Direction> posibleDirections = new List<Direction>();
            // check up
            if (CheckDirection(currentRow, currentColumn, Direction.UP))
            {
                posibleSteps.Add(new Array2D(currentRow - 1, currentColumn));
                posibleDirections.Add(Direction.UP);
            }
            // check down
            if (CheckDirection(currentRow, currentColumn, Direction.DOWN))
            {
                posibleSteps.Add(new Array2D(currentRow + 1, currentColumn));
                posibleDirections.Add(Direction.DOWN);
            }
            // check left
            if (CheckDirection(currentRow, currentColumn, Direction.LEFT))
            {
                posibleSteps.Add(new Array2D(currentRow, currentColumn - 1));
                posibleDirections.Add(Direction.LEFT);
            }
            // check right
            if (CheckDirection(currentRow, currentColumn, Direction.RIGHT))
            {
                posibleSteps.Add(new Array2D(currentRow, currentColumn + 1));
                posibleDirections.Add(Direction.RIGHT);
            }
            int optionCount = posibleSteps.Count;
            // if option count are present
            if(optionCount > 0)
            {
                int randomDirection = UnityEngine.Random.Range(0, optionCount);
                int newRowPosition = posibleSteps.ElementAt(randomDirection).rowPosition;
                int newColumnPosition = posibleSteps.ElementAt(randomDirection).columnPosition;
                Direction chosenDirection = posibleDirections.ElementAt(randomDirection);
                // search for new direction
                BuildRecursive(newRowPosition, newColumnPosition, chosenDirection);
            }
            // if options are not present then go back
            else
            {
                break;
            }
        }
    }

    private void PlaceBlockers(int rowPosition, int columnPosition, Direction direction)
    {
        // upper
        if (rowPosition > 0 && direction != Direction.DOWN)
        {
            if(matrix[rowPosition - 1, columnPosition].steppedIn)
            {
                matrix[rowPosition - 1, columnPosition].blockDown = true;
                matrix[rowPosition, columnPosition].blockUp = true;
            }
        }
        // lower
        if (rowPosition < this.rows - 1 && direction != Direction.UP)
        {
            if(matrix[rowPosition + 1, columnPosition].steppedIn)
            {
                matrix[rowPosition + 1, columnPosition].blockUp = true;
                matrix[rowPosition, columnPosition].blockDown = true;
            }
        }
        // left
        if (columnPosition > 0 && direction != Direction.RIGHT)
        {
            if(matrix[rowPosition, columnPosition - 1].steppedIn)
            {
                matrix[rowPosition, columnPosition - 1].blockRight = true;
                matrix[rowPosition, columnPosition].blockLeft = true;
            }
        }
        // right
        if (columnPosition < this.columns - 1 && direction != Direction.LEFT)
        {
            if(matrix[rowPosition, columnPosition + 1].steppedIn)
            {
                matrix[rowPosition, columnPosition + 1].blockLeft = true;
                matrix[rowPosition, columnPosition].blockRight = true;
            }
        }
    }

    private bool CheckDirection(int currentRow, int currentColumn, Direction direction)
    {
        bool ans = false;
        switch (direction)
        {
            case Direction.UP:
                if (currentRow > 0)
                {
                    if( (!matrix[currentRow - 1, currentColumn].blockDown) && (!matrix[currentRow - 1, currentColumn].steppedIn) )
                    {
                        ans = true;
                    }
                }
            break;
            case Direction.DOWN:
                if (currentRow < this.rows - 1)
                {
                    if( (!matrix[currentRow + 1, currentColumn].blockUp) && (!matrix[currentRow + 1, currentColumn].steppedIn) )
                    {
                        ans = true;
                    }
                }
            break;
            case Direction.LEFT:
                if (currentColumn > 0)
                {
                    if( (!matrix[currentRow, currentColumn - 1].blockRight) && (!matrix[currentRow, currentColumn - 1].steppedIn) )
                    {
                        ans = true;
                    }
                }
            break;
            case Direction.RIGHT:
                if (currentColumn < this.columns - 1)
                {
                    if( (!matrix[currentRow, currentColumn + 1].blockLeft) && (!matrix[currentRow, currentColumn + 1].steppedIn) )
                    {
                        ans = true;
                    }
                }
            break;
            default: return ans;
        }
        return ans;
    }

}
