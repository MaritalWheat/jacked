using UnityEngine;
using System.Collections;

public class DeathAnimationScript : MonoBehaviour {
    //public int m_duration
    public SpriteAnimation m_deathAnimation;

    private bool m_fadeOutDestroy;
    private SpriteAnimationManager m_spriteManager;

	// Use this for initialization
	void Start () {
        m_spriteManager = GetComponent<SpriteAnimationManager>();
        m_spriteManager.SetSpriteAnimation(m_deathAnimation);
	}
	
	// Update is called once per frame
	void Update () {
        if (m_fadeOutDestroy) {
            float alpha = renderer.material.color.a;
            alpha = Mathf.Lerp(renderer.material.color.a, 0.0f, Time.deltaTime);
            Color newColor = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alpha);
            renderer.material.color = newColor;

            if (alpha < 0.1f) {
                Destroy(gameObject);
            }
        }
	}

    public void FadeOutToDestroy() {
        m_fadeOutDestroy = true;
    }
}
