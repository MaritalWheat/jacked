using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour {

    ////////////////////////////////////////
    //For now I am just making a fucking list of skills with prices, FUCK IT
    ///////////////////////////////////////

    //public bool display { get; set; }
	public static SkillManager m_singleton;
    private static Dictionary<string, Skill> skillsDict =  new Dictionary<string,Skill>();
    public List<Skill> skillsList;  //This is just the class of the skills to use, use the dict for the actual instance
	// Use this for initialization
	void Start () {
		if (m_singleton == null) {
			m_singleton = this;
		}

        foreach (Skill s in skillsList) {
			s.Level = 0; //This should be changed it we ever mplement a way to save a game.
			skillsDict.Add(s.m_name, s);	
		}
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
    public static bool CanPurchase(string skillToCheck)
    {
    	return skillsDict[skillToCheck].GetNextCost() <= PlayerCharacter.s_singleton.skillPoints;
    }

	/// <summary>
	/// Returns the level of the skill provided, or -1 if the skill is not found
	/// </summary>
	/// <param name='skillName'>
	/// Skill name.
	/// </param>
	public static int getSkillLevel(string skillName) {
		Skill skillToCheck = skillsDict[skillName];
		
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
            PlayerCharacter.s_singleton.skillPoints -= skillsDict[skillToPurchase].GetNextCost();
			Debug.Log("Pruchaseing skill : " + skillToPurchase);
			skillsDict[skillToPurchase].Level++;
			PlayerCharacter.s_singleton.newSkillAdded(skillsDict[skillToPurchase]);
            return true;
        }
        return false;
    }
	
	public static List<Skill> getSkills() {
		return new List<Skill>(skillsDict.Values);
	}
	
	public static void fireSkill(string skillName) {
	
		//Check if we have the skill
		Skill skillToFire = skillsDict[skillName];
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

	IEnumerator Activated() {
		while (true) {
			PlayerCharacter.s_singleton.SlowHeartRate(1);
			yield return new WaitForSeconds(0.1f);
		}
	}
}
