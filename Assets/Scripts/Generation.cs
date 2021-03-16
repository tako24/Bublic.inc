using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Generation : MonoBehaviour
{
    void Start()
    {
        GameObject newRoom = new GameObject();
        var tilemap = GetComponent<Tilemap>();
        var room = GameObject.Find("Room 1");
        var roomPos = room.transform.position;

        for (int i = 0; i < 21; i++)
        {
            var rand = Random.Range(1, 5);

            switch (rand)
            {
                case 1:
                    {
                        newRoom = Instantiate(room, new Vector3(roomPos.x + 2, roomPos.y + 1, 0), Quaternion.identity);
                        break;
                    }
                case 2:
                    {
                        newRoom = Instantiate(room, new Vector3(roomPos.x - 2, roomPos.y - 1, 0), Quaternion.identity);
                        break;
                    }
                case 3:
                    {
                        newRoom = Instantiate(room, new Vector3(roomPos.x + 2, roomPos.y - 1, 0), Quaternion.identity);
                        break;
                    }
                case 4:
                    {
                        newRoom = Instantiate(room, new Vector3(roomPos.x - 2, roomPos.y + 1, 0), Quaternion.identity);
                        break;
                    }
                default: 
                    break;
            }

            roomPos = newRoom.transform.position;
            newRoom.transform.parent = transform;
        }
    }

    void Update()
    {
        
    }
}
