using UnityEngine;
using System.Collections;

public class SpeedModifier : Modifier {

    public int speedIncrease;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Execute()
    {
        PlayerCharacter.s_singleton.m_maxCharacterSpeed += speedIncrease;
    }

    public override void Reverse()
    {
        PlayerCharacter.s_singleton.m_maxCharacterSpeed -= speedIncrease;
    }
}
