using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancingConstant : MonoBehaviour {

    public static GameManager instance;

    public static float ENEMYAPPROACH_TIME = 20f;
    public static int ENEMYAPPROACH_ATTACKS = 3;

    public static float TURBULENCES_TIME = 5f;

    public static float AIRPLANEFALL_TIME = 20f;

    public static float EVADE_BUTTONOVERLAPTIME = 3f;
    public static float EVADE_TIME = 20f;
    public static int EVADE_DAMAGE = 2;
}
