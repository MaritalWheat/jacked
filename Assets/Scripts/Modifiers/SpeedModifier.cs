using UnityEngine;
using System.Collections;

public class HealthModifier : Modifier {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Execute()
    {
        PlayerCharacter.s_singleton.m_characterSpeed += 10;
    }
}
