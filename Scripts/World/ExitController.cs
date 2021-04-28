using UnityEngine;
using Pathfinding;

public class ExitController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var currentRoom = gameObject.GetComponentInParent<RoomProperties>();
        GameController.CurrentRoom = currentRoom;
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var currentRoom = gameObject.GetComponentInParent<RoomProperties>();
        currentRoom.CloseExits();
        var ai = currentRoom.GetComponentInChildren<EnemyLogic>();
        if (ai != null)
            if (!currentRoom.IsCleared)
                ai.Atack();
            else
                ai.StopAtack();
    }
}
