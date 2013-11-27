using UnityEngine;
using System;
using System.Collections;

public class Spiritwalker : Skill {
	private bool m_activated;
	private Vector4 m_originalColor;
	private float m_timer = 0.0f;
	public float m_duration;

	public override void Execute() 
	{
		if (!m_activated) {
			m_activated = true;
			m_timer = 0.0f;
			m_originalColor = PlayerCharacter.s_singleton.gameObject.renderer.material.GetColor("_Color");
			Color newColor = new Color(m_originalColor.x, m_originalColor.y, m_originalColor.z, 0.25f);
			PlayerCharacter.s_singleton.gameObject.renderer.material.SetColor("_Color", newColor);
			PlayerCharacter.Invulnerable = true;
			Physics.IgnoreLayerCollision(8, 10, true);

			Func<int, int> executable = Timer;
			CoroutineHandler.StartCoroutine(executable);
			//SkillManager.m_singleton.StartCoroutine("Activated");
		} else {
			m_activated = false;
			PlayerCharacter.s_singleton.gameObject.renderer.material.SetColor("_Color", m_originalColor);
			PlayerCharacter.Invulnerable = false;
			Physics.IgnoreLayerCollision(8, 10, false);
			//SkillManager.m_singleton.StopCoroutine("Activated");
		}
	}

	public int Timer(int arg) {
		m_timer += Time.deltaTime;
		Debug.Log(m_timer);
		if (m_timer > m_duration) {
			Execute();
			return 1;
		}
		return 0;
	}
}
