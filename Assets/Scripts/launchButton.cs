using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class launchButton : MonoBehaviour
{
    private Vector3 origScale;
    private Vector3 bigScale;
    private bool inButton = false;
    private bool clicking = false;

    // Start is called before the first frame update
    void Start()
    {
        origScale = this.transform.parent.localScale;
        var scaleAmt = 1.1f;
        bigScale = new Vector3(origScale.x * scaleAmt, origScale.y * scaleAmt, origScale.z * scaleAmt);
    }

    // Update is called once per frame
    void Update()
    {
        if(inButton && !clicking){
            this.transform.parent.localScale = bigScale;
        }
        else{
            this.transform.parent.localScale = origScale;
        }
    }

    void OnMouseOver() {
        inButton = true;
    }

    void OnMouseExit() {
        inButton = false;
    }

    void OnMouseDown() {
        clicking = true;
        SceneManager.LoadScene(1);
    }

    void OnMouseUp() {
        clicking = false;
    }
}
