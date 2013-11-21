using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill : MonoBehaviour {

    public bool passive;
    public Skill preReq;
    public string skillName;
    public int cost;
    public List<Modifier> mods;
	public Texture icon;
	
	public int level {get; set;}
	
	//Do not use start and update since this is currently not on a gameobject it will not be called
	
	public int getNextCost() 
	{
	//Maybe this should be based on level later
		return 1;
	}
	
	/// <summary>
	/// Execute the skill, this can be overriden by children for special skills
	/// </summary>
	public virtual void Execute() 
	{
		Debug.Log ("BLAM!!!");
	}
}
