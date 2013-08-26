using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour {

    ////////////////////////////////////////
    //For now I am just making a fucking list of skills with prices, FUCK IT
    ///////////////////////////////////////

    //public bool display { get; set; }
    private static Dictionary<string, Skill> purchasedSkills =  new Dictionary<string,Skill>();
    public List<Skill> skillsList;
    public static List<Skill> skills;
	// Use this for initialization
	void Start () {
        skills = skillsList;
	}
	
	// Update is called once per frame
	void Update () {
	    
     
	}

    void OnGUI()
    {
        /* For now the display is handled in the store
        if (!display) {
            return;
        }*/
    }

    /// <summary>
    /// This function will return true only if the Player has already purchased the prequisites for the skill and has enough XP to purchase the skill
    /// </summary>
    /// <param name="skillToCheck"></param>
    /// <returns></returns>
    public static bool CanPurchase(Skill skillToCheck)
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

    public static bool IsPurchased(string skillName)
    {
        return purchasedSkills.ContainsKey(skillName);
    }

    public static bool PurchaseSkill(Skill skillToPurchase)
    {
        if (CanPurchase(skillToPurchase))
        {
            PlayerCharacter.s_singleton.experiencePoints -= skillToPurchase.cost;
            purchasedSkills.Add(skillToPurchase.skillName, skillToPurchase);
            Debug.Log("Just purchased skill: " + skillToPurchase.skillName);
            return true;
        }
        return false;
    }
}
