using UnityEngine;
using Pathfinding;
using System.Linq;

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
        var ai = currentRoom.GetComponentsInChildren<EnemyLogic>();
        if (ai != null)
            if (!currentRoom.IsCleared)
                foreach (var x in ai) x.Atack();
            else
                foreach (var x in ai) x.StopAtack();
    }
}
