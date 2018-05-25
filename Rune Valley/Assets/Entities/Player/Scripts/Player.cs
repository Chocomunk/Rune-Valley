using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInfo))]
public class Player : MonoBehaviour {

    private PlayerInfo info;

	// Use this for initialization
	void Start () {
        info = this.gameObject.GetComponent<PlayerInfo>();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
