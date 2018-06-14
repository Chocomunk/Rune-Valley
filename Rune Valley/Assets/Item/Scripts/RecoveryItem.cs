using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recovery Item", menuName = "Inventory/Usable/Recovery Item")]
public class RecoveryItem : UsableItem {

    public int healthRecovery;
    public int energyRecovery;

    public override void Use()
    {
        PlayerManager.instance.playerStats.RecoverHealth(healthRecovery);
        PlayerManager.instance.playerStats.RecoverEnergy(energyRecovery);
    }

}
