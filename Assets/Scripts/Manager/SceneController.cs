using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

	[SerializeField]
	Airplane airplane;

	[SerializeField]
	GameObject enemyAttackLamp;

	void Start(){
		//EnemyApproach enemyApproach = new EnemyApproach (enemyAttackLamp);
		EnemyAttack ea = new EnemyAttack (airplane);
	}
}
