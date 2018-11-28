using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	// Use this for initialization
	[SerializeField] private float lifeTime = 3f;
	private float timeOfDestruction;
	void Start () {
		timeOfDestruction = Time.time + lifeTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time>=timeOfDestruction)
			Destroy(gameObject);
	}
}
