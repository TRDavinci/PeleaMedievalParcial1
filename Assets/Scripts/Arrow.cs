using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;
    public float lifeTime = 3f;
    public GameObject owner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (owner != null && collision.gameObject == owner) return;

        Debug.Log("Chocando con: " + collision.name);
        if (collision.CompareTag("Player"))
        {
            Health targetHealth = collision.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage,transform.position);
                Destroy(gameObject);
            }
            else if (collision.CompareTag("Border"))
            {
                Destroy(gameObject);
            }
        }       
    }
}
