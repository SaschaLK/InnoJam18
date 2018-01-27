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
        interactive.health -= damage;

        ScreenShakeController.Instance.Trigger(interactive.transform, 1f, damage/10);

        if (interactive.health <= 0)
        {
            interactive.health = 0f;
            interactive.OnDestroy.Invoke();
        }
    }

    public void DoDestroy()
    {
        if (broken == null)
        {
            Debug.Log("BOOOM!!!");
            broken = Instantiate(Resources.Load<GameObject>("Broken"), this.transform.position, Quaternion.identity, this.transform);
        }
    }
}
