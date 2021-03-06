﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChargeable : ItemHandler {

    protected override void Awake() {
        base.Awake();

        _CanUseWith = item.CanUseWith;
        item.CanUseWith = CanUseWith;
    }

    public void ChargeUp(float charge)
    {
        this.GetComponent<InteractiveComponent>().ChargeUp(charge);
    }

    public override void OnUseWith(InteractiveComponent with) {
        InteractiveCharger charger = with.GetComponent<InteractiveCharger>();
        if (charger == null)
            return;
        
        charger.Container.PickupItem(item);
    }

    private Func<PlayerController, InteractiveComponent, bool> _CanUseWith;
    public bool CanUseWith(PlayerController player, InteractiveComponent with) {
        InteractiveCharger charger = with.GetComponent<InteractiveCharger>();
        if (charger != null && name.Contains(charger.ChargingName))
            return true;

        if (_CanUseWith != null)
            return _CanUseWith(player, with);
        return false;
    }

}
