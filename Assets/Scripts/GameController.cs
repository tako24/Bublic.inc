using UnityEngine;

public class GameController : MonoBehaviour
{
    public static RoomProperties CurrentRoom;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CurrentRoom.OpenExits();
        }
    }
}
