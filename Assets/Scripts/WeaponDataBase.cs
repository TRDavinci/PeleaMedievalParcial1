using System.Collections.Generic;
using UnityEngine;

public class WeaponDataBase : MonoBehaviour
{
    public static WeaponDataBase Instance;
    public List<WeaponsData> allWeapons;

    private void Awake() { Instance = this; }
}
