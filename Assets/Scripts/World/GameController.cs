using UnityEngine;

public class GameController : MonoBehaviour
{
    public static RoomProperties CurrentRoom;
    public static GameObject Player;

    private void Start()
    {
        Player = GameObject.Find("Player");
    }
}
