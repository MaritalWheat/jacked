using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    private float m_normalFOV;
    private PlayerCharacter m_player;
    private MotionBlur m_motionBlur;
    private Vignetting m_vignette;

    public static CameraController s_singleton;


	// Use this for initialization
	void Start () {
        if (!s_singleton)
        {
            s_singleton = this;
        }
        m_player = PlayerCharacter.s_singleton;
        m_normalFOV = Camera.main.fov;
        m_motionBlur = gameObject.GetComponent<MotionBlur>();
        m_vignette = gameObject.GetComponent<Vignetting>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Camera.main.fov > m_normalFOV)
        {
            Camera.main.fov--;
        }

        m_motionBlur.blurAmount = (float)m_player.m_heartRate / (float)m_player.GetMaxHeartRate();
       
        m_vignette.intensity = (10.0f)*((float)(90 + m_player.GetMinHeartRate() - m_player.m_heartRate)) / (90 + m_player.GetMinHeartRate());
        

        m_vignette.chromaticAberration = m_vignette.intensity / 1.4f;
        m_vignette.blur = m_vignette.intensity / 10.0f;

        if (m_player.m_heartRate > m_player.GetMaxHeartRate() * 6.0f / 13.0f)
        {
            m_motionBlur.enabled = true;
            m_vignette.enabled = false;
        }
        else if (m_player.m_heartRate < 90)
        {
            m_motionBlur.enabled = false;
            m_vignette.enabled = true; ;
        }else
        {
            m_motionBlur.enabled = false;
            m_vignette.enabled = false;
        }

        Vector3 pos = this.transform.position;
        pos.x = m_player.transform.position.x;
        pos.z = m_player.transform.position.z;
        this.transform.position = pos;
	}


    /// <summary>
    /// Increases the cameras fov by the intensity
    /// </summary>
    /// <param name="intensity">Amount to increase fov by</param>
    public void PulseCamera(int intensity)
    {
        Camera.main.fov = m_normalFOV + intensity;
    }
}
