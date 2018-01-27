using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSounds : ItemHandler {

    [Range(0f, 1f)]
    public float Vol;

    public AudioClip ClipPickup;
    [Range(0f, 1f)]
    public float ClipPickupVol;
    public AudioClip ClipDrop;
    [Range(0f, 1f)]
    public float ClipDropVol;
    public AudioClip ClipUse;
    [Range(0f, 1f)]
    public float ClipUseVol;
    public AudioClip ClipUseWith;
    [Range(0f, 1f)]
    public float ClipUseWithVol;

    public AudioSource Source;

    protected override void Awake() {
        base.Awake();

        if (Source == null)
            Source = GetComponent<AudioSource>();
        if (Source == null)
            Source = gameObject.AddComponent<AudioSource>();
    }

    public override void OnPickup() {
        if (ClipPickup != null)
            Source.PlayOneShot(ClipPickup, ClipPickupVol * Vol);
    }

    public override void OnDrop() {
        if (ClipDrop != null)
            Source.PlayOneShot(ClipDrop, ClipDropVol * Vol);
    }

    public override void OnUse() {
        if (ClipUse != null)
            Source.PlayOneShot(ClipUse, ClipUseVol * Vol);
    }

    public override void OnUseWith(InteractiveComponent with) {
        if (ClipUseWith != null)
            Source.PlayOneShot(ClipUseWith, ClipUseWithVol * Vol);
    }

}
