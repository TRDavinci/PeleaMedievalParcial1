using UnityEngine;
using Fusion;


public class PlayerCombat : NetworkBehaviour
{
    public HandSlot leftHand;
    public HandSlot rightHand;
    Health myHealth;

    ItemWorld nearItem;

    public override void Spawned()
    {
        if (!HasStateAuthority) return;
        myHealth = GetComponent<Health>();
        myHealth.OnProcessDamage = DamageReduction;
    }
   

    private void Update()
    {
        if (!HasInputAuthority) return;
        if (!rightHand.IsBlocking())
        {
            leftHand.ProcessInput(0);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            leftHand.ActionUp();
        }
        
        if (!leftHand.IsBlocking())
        {
            rightHand.ProcessInput(1);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rightHand.ActionUp();
        }

        if (Input.GetKeyDown(KeyCode.F) && nearItem != null)
        {
            TryPick(nearItem);
        }
    }

    

    void TryPick(ItemWorld item)
    {
        WeaponsData newWeapon = item.weaponData;

        PickWeapon(newWeapon);
        item.Collect();      
        
        nearItem= null;
    }

    void PickWeapon(WeaponsData newWeapon)
    {
        if (newWeapon.twoHands)
        {
            if (!leftHand.IsEmpty()) leftHand.Drop();
            if (!rightHand.IsEmpty()) rightHand.Drop();
            leftHand.Pickup(newWeapon);
            leftHand.SetHandVisible(false);
            rightHand.SetHandVisible(false);
        }
        else
        {

            if (!leftHand.IsEmpty() && leftHand.GetCurrentData().twoHands)
            {
                leftHand.Drop();
                leftHand.SetHandVisible(true);
                rightHand.SetHandVisible(true);
            }
            if (leftHand.IsEmpty())
            {
                leftHand.Pickup(newWeapon);
                leftHand.SetHandVisible(false);
            }
            else if (rightHand.IsEmpty())
            {
                rightHand.Pickup(newWeapon);
                rightHand.SetHandVisible(false);
            }
            else
            {
                rightHand.Drop();
                rightHand.Pickup(newWeapon);
                rightHand.SetHandVisible(false);
            }
        }
        
    }

    float DamageReduction(float dmg, Vector2 attackerPosition)
    {
        if (leftHand.IsBlocking() || rightHand.IsBlocking())
        {
            Vector2 dirToAttacker = (attackerPosition - (Vector2)transform.position).normalized;
            
            float dot = Vector2.Dot(transform.right, dirToAttacker);

            //Si el dot es mayor a 0, el enemigo está mirando a nosotros (en un cono de 180°)
            if (dot > 0)
            {
                Debug.Log("Bloqueo");
                return 0;
            }            
        }

        return dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            nearItem = collision.GetComponent<ItemWorld>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            
            if (nearItem != null && collision.gameObject == nearItem.gameObject)
            {
                nearItem = null;
            }
        }
    }
}
