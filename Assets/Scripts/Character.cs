using UnityEngine;

[System.Serializable]
public class Character
{
    public string characterName;
    public Sprite characterSprite;
    public RuntimeAnimatorController animatorController;
    public Vector2 colliderSize;
    public Vector2 colliderOffset;
    public Vector2 crouchColliderSize;
    public Vector2 crouchColliderOffset;

    public bool useCustomCollider;
}
