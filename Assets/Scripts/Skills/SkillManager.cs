using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour {

    ////////////////////////////////////////
    // What I need to fucking add
    // -a way to spend points on skills
    // -skills
    // -a skill tree
    ///////////////////////////////////////

    public bool display { get; set; }
    private Dictionary<string, Skill> purchasedSkills;
    public List<Skill> skills;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (!display) {
            return;
        }
    }

    public bool IsUnlocked(Skill skillToCheck)
    {
        return IsPurchased(skillToCheck.preReq.skillName);
    }

    public bool IsPurchased(string skillName)
    {
        return purchasedSkills.ContainsKey(skillName);
    }

    public void PurchaseSkill(Skill skillToPurchase)
    {
        purchasedSkills.Add(skillToPurchase.skillName, skillToPurchase);
    }
}
