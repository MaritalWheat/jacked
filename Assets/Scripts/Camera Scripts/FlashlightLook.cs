using UnityEngine;
using System.Collections;

public class FlashlightLook : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 worldPos = InputManager.s_singleton.PlayerToMouse();
		Vector3 lookTo = new Vector3(worldPos.x, 0, worldPos.y);
		gameObject.transform.rotation = Quaternion.LookRotation(lookTo);
	}
}
