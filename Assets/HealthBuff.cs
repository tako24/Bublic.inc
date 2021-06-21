using UnityEngine;

public class HealthBuff : MonoBehaviour, IModuleEffect
{
    public int HealthIncrease;

    public void ActivateEffect(bool activate)
    {
        if (activate)
        {
            GameController.Player.GetComponent<HPBar>()._maxHP += HealthIncrease;
            GameController.Player.GetComponent<HPBar>().Heal(HealthIncrease);
        }
        else
        {
            GameController.Player.GetComponent<HPBar>()._maxHP -= HealthIncrease;
            GameController.Player.GetComponent<HPBar>().TakeDamage(HealthIncrease);
        }
    }
}
