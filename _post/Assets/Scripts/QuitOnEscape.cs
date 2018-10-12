using System.Collections;
using UnityEngine;

public class QuitOnEscape : MonoBehaviour {

    private bool quit = false;
    public textOut _textOut;
    void Start () {
        quit = false;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp (KeyCode.Escape)) {
            Debug.Log("EXIT");
            Application.Quit ();
        }
    }
}