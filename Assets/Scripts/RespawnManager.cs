using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private Vector3 playerStartPosition;
    private Player player;

    void Start()
    {
        player = FindFirstObjectByType<Player>();
        playerStartPosition = player.transform.position;
    }

    public void RespawnAtStart()
    {
        player.transform.position = playerStartPosition;
        player.ResetPlayer();
    }

    public void RespawnAtPosition(Vector3 position)
    {
        player.transform.position = position;
        player.ResetPlayer();
    }
}
