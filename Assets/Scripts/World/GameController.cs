using UnityEngine;

public class GameController : MonoBehaviour
{
    public static RoomProperties CurrentRoom;
    public static GameObject Player;
    public static int CoinsCount;
    public static MeleeWeapon CurrentWeapon;

    private void Start()
    {
        Player = GameObject.Find("Player");
    }
}
