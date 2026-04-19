using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject shootingItem;
    public Transform shootingPoint;
    public bool canShoot = true;

    public void Update()
    {
        if (InputManager.instance.ShootPressed())
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (!canShoot)
            return;
        GameObject si = Instantiate(shootingItem, shootingPoint);
        si.transform.parent = null;
    }
}
