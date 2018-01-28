using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

	public static GameManager instance;

    [SyncVar]
    private float hitPoints = 10;

    public void TakeDamage(float dmg)
    {
        if (!isServer) return;
        hitPoints -= dmg;
        CmdTakeDamage(dmg);

        Debug.Log("WE TOOK DAMAGE: " + dmg);

        if (hitPoints <= 0)
            GameManager.instance.EndGame();
    }

    public void TakeDamageInRange(float min, float max)
    {
        TakeDamage(Random.Range(5, 10));
    }

    [Command]
    public void CmdTakeDamage(float damage)
    {
        RpcTakeDamageFeedback(damage);
    }

    [ClientRpc]
    public void RpcTakeDamageFeedback(float damage)
    {
        ScreenShakeController.Instance.Trigger(Camera.main.transform, 0.5f, damage);
    }

    private void Awake()
	{
		instance = this;
	}

	public void EndGame()
	{
        if (isServer) CmdEndGame();
    }

    [Command]
    public void CmdEndGame()
    {
        RpcEndGame();
    }

    [ClientRpc]
    public void RpcEndGame()
    {
        Debug.Log("GAME OVER");
    }
}
