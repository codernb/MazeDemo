using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectController : MonoBehaviour
{
    public MazeGenerator mazeGenerator;
    public GameObject spawnPrefab;
    public int prefabCount;
    public bool rearrangeObjects;
    public float rearrangeDelay = 1;

    private GameObject[,] maze;
    private List<ObjectContainer> objects = new List<ObjectContainer>();
    private List<Position> emptyTiles;
    private System.Random random = new System.Random();

    void Start()
    {
        maze = mazeGenerator.MakeBlocks();
        emptyTiles = currentEmptyTiles;
        var maxObjects = System.Math.Min(emptyTiles.Count, prefabCount);
        for (int i = 0; i < maxObjects; i++)
            spawnRandom(spawnPrefab);
        if (rearrangeObjects)
            StartCoroutine(rearrange());
    }

    private List<Position> currentEmptyTiles {
        get {
            var emptyTiles = new List<Position>();
            for (int i = 1; i < maze.GetLength(0) - 1; i++)
                for (int j = 1; j < maze.GetLength(1) - 1; j++)
                    if (maze[i, j] == null)
                        emptyTiles.Add(new Position(i, j));
            return emptyTiles;
        }
    }

    public ObjectContainer spawnRandom(GameObject prefab)
    {
        return spawnObj(prefab, randomFreePosition);
    }
    
    public ObjectContainer spawnObject(GameObject prefab, Position position)
    {
        if (position.x < 1
            || position.x >= maze.GetLength(0) - 1
            || position.y < 1
            || position.y >= maze.GetLength(1) - 1
            || maze[position.x, position.y] != null)
            return null;
        return spawnObj(prefab, position);
    }

    private ObjectContainer spawnObj(GameObject prefab, Position position)
    {
        var obj = (GameObject) Instantiate(prefab, new Vector3(position.x, 0, position.y), Quaternion.identity);
        maze[position.x, position.y] = obj;
        emptyTiles.Remove(position);
        var objectContainer = new ObjectContainer(obj, position);
        objects.Add(objectContainer);
        return objectContainer;
    }

    private IEnumerator rearrange()
    {
        while (rearrangeObjects) {
            yield return new WaitForSeconds(rearrangeDelay);
            var objectContainer = randomObjectContainer;
            var newPosition = randomFreePosition;
            moveObjectContainer(objectContainer, newPosition);
        }
    }

    public void moveObjectContainer(ObjectContainer objectContainer, Position newPosition)
    {
        moveObject(objectContainer.obj.transform, newPosition);
        updateMaze(objectContainer.tile, newPosition, objectContainer.obj);
        swapPositions(objectContainer.tile, newPosition);
    }
    
    private ObjectContainer randomObjectContainer {
        get {
            return objects[random.Next(0, objects.Count)];
        }
    }

    private Position randomFreePosition {
        get {
            return emptyTiles[random.Next(0, emptyTiles.Count)];
        }
    }

    private void moveObject(Transform transform, Position newPosition)
    {
        var position = transform.position;
        position.x = newPosition.x;
        position.z = newPosition.y;
        transform.position = position;
    }

    private void updateMaze(Position oldPosition, Position newPosition, GameObject obj)
    {
        maze[oldPosition.x, oldPosition.y] = null;
        maze[newPosition.x, newPosition.y] = obj;
    }

    private void swapPositions(Position oldPosition, Position newPosition)
    {
        var x = newPosition.x;
        var y = newPosition.y;
        newPosition.x = oldPosition.x;
        newPosition.y = oldPosition.y;
        oldPosition.x = x;
        oldPosition.y = y;
    }

    public class ObjectContainer
    {
        public GameObject obj;
        public Position tile;

        public ObjectContainer(GameObject obj, Position tile)
        {
            this.obj = obj;
            this.tile = tile;
        }
    }

    public class Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
	
}
