using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

	public static GameManager instance;

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
