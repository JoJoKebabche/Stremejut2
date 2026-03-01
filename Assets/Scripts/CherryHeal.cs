using UnityEngine;

public class CherryHeal : MonoBehaviour
{
    public int healAmount = 25;

    public void HealPlayer()
    {
        HealthBar hp = FindFirstObjectByType<HealthBar>();

        if (hp != null)
        {
            hp.Heal(healAmount);
        }
    }
}

