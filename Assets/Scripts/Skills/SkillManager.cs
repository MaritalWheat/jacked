using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour {

	public static SkillManager m_singleton;
    private static Dictionary<string, Skill> m_skillsDict =  new Dictionary<string, Skill>();
    public List<Skill> m_skillsList;  //This is just the class of the skills to use, use the dict for the actual instance

	void Start () {
		if (m_singleton == null) {
			m_singleton = this;
		}

        foreach (Skill s in m_skillsList) {
			s.Level = 0; //This should be changed it we ever mplement a way to save a game.
			s.Locked = false; //Unity bug that saves property values, make sure we reset all properties at start of game
			s.Cooldown = false;
			s.Activated = false;
			m_skillsDict.Add(s.m_name, s);	
		}
	}
	
    // This function will return true only if the Player has already purchased the prequisites for the skill and has enough XP to purchase the skill
    public static bool CanPurchase(string skillToCheck)
    {
    	return m_skillsDict[skillToCheck].GetNextCost() <= PlayerCharacter.s_singleton.skillPoints;
    }
	
	// Returns the level of the skill provided, or -1 if the skill is not found
	public static int GetSkillLevel(string skillName) {
		Skill skillToCheck = m_skillsDict[skillName];
		
		if (skillToCheck) {
			return skillToCheck.Level;
		} else {
			return -1;	
		}
	}
	
    public static bool PurchaseSkill(string skillToPurchase)
    {
        if (CanPurchase(skillToPurchase))
        {
            PlayerCharacter.s_singleton.skillPoints -= m_skillsDict[skillToPurchase].GetNextCost();
			Debug.Log("Pruchaseing skill : " + skillToPurchase);
			m_skillsDict[skillToPurchase].Level++;
			PlayerCharacter.s_singleton.newSkillAdded(m_skillsDict[skillToPurchase]);
            return true;
        }
        return false;
    }
	
	public static List<Skill> getSkills() {
		return new List<Skill>(m_skillsDict.Values);
	}
	
	public static void FireSkill(string skillName) {
	
		//Check if we have the skill
		Skill skillToFire = m_skillsDict[skillName];
		if (skillToFire == null) {
			Debug.LogError("Trying to fire a non-existent skill: " + skillName);
			return;
		}
		//Make sure we can use it, is it active, has it cooled down
		if (skillToFire.m_passive == true) {
			return;	
		}
		
		//Do the action
		skillToFire.Execute();
	}
}
