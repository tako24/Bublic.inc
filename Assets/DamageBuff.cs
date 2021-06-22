using UnityEngine;

public class DamageBuff : MonoBehaviour, IModuleEffect
{
    public int DamageIncrease;

    public void ActivateEffect(bool activate)
    {
        if (activate)
            GameController.DamageBonus += DamageIncrease;
        else
            GameController.DamageBonus -= DamageIncrease;
    }
}
