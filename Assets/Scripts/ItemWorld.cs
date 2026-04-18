using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public WeaponsData weaponData;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        
        if (weaponData != null)
        {
            SetWeapon(weaponData);
        }
    }
    public WeaponsData GetWeapon()
    {
        Destroy(gameObject);
        return weaponData;
    }

    public void SetWeapon(WeaponsData data)
    {
        weaponData = data;
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = data.sprite;
    }

    public void Collect()
    {
        
        // if(Object.HasStateAuthority) Runner.Despawn(Object);
        Destroy(gameObject);
    }

    public void SetUpDrop(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * 1.5f, ForceMode2D.Impulse);
        }
    }
}
