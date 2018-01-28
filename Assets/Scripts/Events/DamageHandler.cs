using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour {

    InteractiveComponent interactive;
    GameObject broken;

    private void Awake()
    {
        interactive = GetComponent<InteractiveComponent>();
    }

    public void DoDamage(float damage)
    {
        interactive.TakeDamage(damage);
    }

    public void DoDestroy()
    {
        if (broken == null)
        {
            Debug.Log("BOOOM!!!");
            GameManager.instance.TakeDamage(1f);
            
            broken = Instantiate(Resources.Load<GameObject>("Broken"), this.transform.position, Quaternion.identity, this.transform);
        }
    }
}
