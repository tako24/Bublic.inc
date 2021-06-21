using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameObject Player;
    public static int CoinsCount;
    public static MeleeWeapon CurrentWeapon;

    public static InventoryScript Inventory;

    public static StageGeneration Stage;
    public static RoomProperties CurrentRoom;

    public static int RoomsCleared;

    private void Start()
    {
        Player = GameObject.Find("Player");
        Stage = GameObject.Find("GameWorld").GetComponent<StageGeneration>();
        Inventory = GameObject.Find("Inventory").GetComponent<InventoryScript>();
    }

    public static void IncreaseClearedRoomsCount()
    {
        RoomsCleared++;
        if (RoomsCleared == Stage.BossSpawnDelay)
            Stage.GenerateBossRoom();
    }
}
