using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private Animator animator;

    public float runSpeed = 1;
    public bool isRunning = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        mouseLook.SetCursorLock(!PlayerManager.instance.viewingMenu);
        if (PlayerManager.instance.viewingMenu)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("Running", true);
        } else if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("Running", false);
        }
	}

    public void OnAnimatorMove()
    {

    }
}
