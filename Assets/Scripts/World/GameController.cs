using Pathfinding;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static RoomProperties CurrentRoom;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CurrentRoom.OpenExits();
            var ai = CurrentRoom.GetComponentInChildren<AIPath>();
            ai.canSearch = false;
        }
    }
}
