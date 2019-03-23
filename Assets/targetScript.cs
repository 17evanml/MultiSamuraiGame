using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetScript : MonoBehaviour
{
    int locationIndex;
    private bool dragging = false;
    // Start is called before the first frame update
    void Start()
    {
       // locationIndex = index;
    }

    private void OnMouseDown()
    {
        print("clicked");
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }


    private void LateUpdate()
    {
        if(dragging)
        {
            Vector2 normalizedMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 tempLoc = new Vector2(normalizedMouse.x, normalizedMouse.y);
            gameObject.transform.position = tempLoc;
        }
    }
}
