using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapons", menuName = "Weapons")]
public class WeaponsData : ScriptableObject
{
    public string name;
    public GameObject prefab;
    public Sprite sprite;
    public float damage;
    public float rateAttack;
    public bool twoHands;

    [Header("Animations")]
    public RuntimeAnimatorController leftOverride;
    public RuntimeAnimatorController rightOverride;

        
    public float hitRadius = 0.5f;
}
