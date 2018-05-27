using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static Player playerInstance;

    public static bool PlayerExists()
    {
        return playerInstance != null;
    }

}
