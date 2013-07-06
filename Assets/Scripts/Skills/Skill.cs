using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill : MonoBehaviour {

    public bool passive;
    public Skill preReq;
    public string skillName;
    public int cost;
    public List<Modifier> mods;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
