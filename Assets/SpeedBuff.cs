using UnityEngine;

public class SpeedBuff : MonoBehaviour, IModuleEffect
{
    public float SpeedIncrease;

    public void ActivateEffect(bool activate)
    {
        if (activate)
            GameController.Player.GetComponent<PlayerController>().Speed += SpeedIncrease;
        else
            GameController.Player.GetComponent<PlayerController>().Speed -= SpeedIncrease;
    }
}
