using System.Collections.Generic;
using UnityEngine;

public static class TransformExt {

    public static void SetLayerDeep(this Transform root, int layer) {
        root.gameObject.layer = layer;
        foreach (Transform child in root)
            child.SetLayerDeep(layer);
    }

    public static int SetLayerByRegion(this Transform root, string prefix = "Player") {
        int number;
        // Determine in which part of the level we spawned in.
        if (Physics.CheckSphere(root.position, 0.1f, LayerMask.GetMask("Invisible Wall 1"))) {
            // We spawned inside the invisible walls of player 1, thus are player 2.
            number = 2;
        } else {
            // We spawned inside the invisible walls of player 2, thus are player 1.
            number = 1;
        }

        root.SetLayerDeep(LayerMask.NameToLayer(prefix + " " + number));

        return number;
    }

}
