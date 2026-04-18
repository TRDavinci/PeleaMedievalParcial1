using UnityEngine;

public interface IAttack
{
    void Attack();
    void ActionHold();
    void ActionUp();

    float GetDamage();
}

public interface IItemData
{
    void SetData(WeaponsData data);
    WeaponsData GetData();  
}
