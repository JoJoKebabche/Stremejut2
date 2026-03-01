using UnityEngine;

public class GemLife : MonoBehaviour
{
    public void AddExtraLife()
    {
        Player player = FindFirstObjectByType<Player>();
        player.extraLives++;
    }
}
