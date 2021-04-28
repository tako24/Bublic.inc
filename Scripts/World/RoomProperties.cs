using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomProperties : MonoBehaviour
{
    public float Size;
    public int MapX;
    public int MapY;

    public GameObject TopExit;
    public GameObject BottomExit;
    public GameObject RightExit;
    public GameObject LeftExit;

    public bool IsCleared;

    public List<GameObject> SpawnedExits = new List<GameObject>();

    public void SpawnExit(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                TopExit.GetComponent<EdgeCollider2D>().isTrigger = true;
                SpawnedExits.Add(TopExit);
                break;
            case Direction.Down:
                BottomExit.GetComponent<EdgeCollider2D>().isTrigger = true;
                SpawnedExits.Add(BottomExit);
                break;
            case Direction.Right:
                RightExit.GetComponent<EdgeCollider2D>().isTrigger = true;
                SpawnedExits.Add(RightExit);
                break;
            case Direction.Left:
                LeftExit.GetComponent<EdgeCollider2D>().isTrigger = true;
                SpawnedExits.Add(LeftExit);
                break;
        }
    }

    public void OpenExits()
    {
        foreach (var exit in SpawnedExits)
        {
            exit.GetComponent<TilemapRenderer>().enabled = false;
            exit.GetComponent<EdgeCollider2D>().isTrigger = true;
        }

        IsCleared = true;
    }

    public void CloseExits()
    {
        if (IsCleared) return;

        foreach (var exit in SpawnedExits)
            {
                exit.GetComponent<TilemapRenderer>().enabled = true;
                exit.GetComponent<EdgeCollider2D>().isTrigger = false;
            }

        Debug.Log("Encounter Started (Press E to clear)");
    }
}
