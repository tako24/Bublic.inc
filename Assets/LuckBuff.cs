using UnityEngine;

public class LuckBuff : MonoBehaviour, IModuleEffect
{
    public int LuckIncrease;

    public void ActivateEffect(bool activate)
    {
        if (activate)
            GameController.LuckBonus += LuckIncrease;
        else
            GameController.LuckBonus -= LuckIncrease;
    }
}
