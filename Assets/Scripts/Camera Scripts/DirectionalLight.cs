using UnityEngine;
using System.Collections;



public class DirectionalLight : MonoBehaviour {
	
	public bool day;
	public bool night;
	public Vector3 dayRot;
	public Vector3 nightRot;
	GameObject directionalLight;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(day) gameObject.transform.Rotate(dayRot * Time.deltaTime);
		if(night) gameObject.transform.rotation.SetLookRotation(nightRot);
	}
}
