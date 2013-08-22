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
    private Dictionary<string, Skill> purchasedSkills =  new Dictionary<string,Skill>();
    public List<Skill> skills;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(PurchaseSkill(skills[0]));
        }
	}

    void OnGUI()
    {
        if (!display) {
            return;
        }
    }

    /// <summary>
    /// This function will return true only if the Player has already purchased the prequisites for the skill and has enough XP to purchase the skill
    /// </summary>
    /// <param name="skillToCheck"></param>
    /// <returns></returns>
    public bool CanPurchase(Skill skillToCheck)
    {
        if (skillToCheck.preReq != null)
        {
            return IsPurchased(skillToCheck.preReq.skillName) && skillToCheck.cost <= PlayerCharacter.s_singleton.experiencePoints;
        }
        else
        {
            return skillToCheck.cost <= PlayerCharacter.s_singleton.experiencePoints;
        }
    }

    public bool IsPurchased(string skillName)
    {
        return purchasedSkills.ContainsKey(skillName);
    }

    public bool PurchaseSkill(Skill skillToPurchase)
    {
        if (CanPurchase(skillToPurchase))
        {
            PlayerCharacter.s_singleton.experiencePoints -= skillToPurchase.cost;
            purchasedSkills.Add(skillToPurchase.skillName, skillToPurchase);
            return true;
        }
        return false;
    }
}
