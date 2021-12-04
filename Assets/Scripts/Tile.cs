using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{ 
    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    public List<Tile> adjacencyList = new List<Tile>();

    //Needed BFS (breadth first search)
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    public Material[] materials;
    Renderer rend;

    //A* VARIABLES PARA ALGORITMO IA

    public float f = 0;
    public float g = 0;
    public float h = 0;


    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (current)
        {
            rend.sharedMaterial = materials[1];
        }
        else if (target)
        {
            rend.sharedMaterial = materials[3];
        }
        else if (selectable)
        {
            rend.sharedMaterial = materials[2];
        }
        else
        {
            rend.sharedMaterial = materials[0];
        }
    }

    public void Reset()
    {
        adjacencyList.Clear();

        current = false;
        target = false;
        selectable = false;

        visited = false;
        parent = null;
        distance = 0;

        f = g = h = 0;

    }

    public void FindNeighbors(Tile target)
    {
        Reset();

        CheckTile(Vector3.forward, target);
        CheckTile(-Vector3.forward, target);
        CheckTile(Vector3.right, target);
        CheckTile(-Vector3.right, target);
    }

    public void CheckTile(Vector3 direction, Tile target)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0, 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtents);

        foreach (Collider item in colliders)
        {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable)
            {
                RaycastHit hit;

                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1) || tile == target)
                {
                    adjacencyList.Add(tile);
                }
            }
        }
    }
}
