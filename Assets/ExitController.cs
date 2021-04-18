using UnityEngine;

public class ExitController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var currentRoom = gameObject.GetComponentInParent<RoomProperties>();
        Debug.Log(currentRoom.MapX + " " + currentRoom.MapY);
        GameController.CurrentRoom = currentRoom;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var currentRoom = gameObject.GetComponentInParent<RoomProperties>();
        currentRoom.CloseExits();
    }
}
