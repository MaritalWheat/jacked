using UnityEngine;
using System.Collections;

public class Modifier : MonoBehaviour {

    public bool isReversable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void Execute() {}

    public virtual void Reverse() { }
}
