using UnityEngine;
using System.Collections;

public class HeartRateModifier : Modifier {

    public int heartRateChange;

    public override void Execute()
    {
        PlayerCharacter.s_singleton.IncreaseHeartRate(heartRateChange);
    }

    public override void Reverse()
    {
        PlayerCharacter.s_singleton.IncreaseHeartRate(-heartRateChange);
    }
}
