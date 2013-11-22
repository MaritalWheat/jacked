using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill : MonoBehaviour {

    public bool m_passive;
    public Skill m_preReq;
    public string m_name;
    public int m_cost;
    public List<Modifier> m_mods;
	public Texture m_icon;
	
	public int Level {get; set;}
	
	//Do not use start and update since this is currently not on a gameobject it will not be called
	
	public int GetNextCost() 
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
