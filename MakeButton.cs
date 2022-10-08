using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MakeButton : MonoBehaviour
{

    public UnityEvent buttonEvent = new UnityEvent();
    public GameObject button;
    string x = "";

    // Start is called before the first frame update
    void Start()
    {
        button = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var c = GameController.NameToPos(button.name);
        x = ThreeDimensionGameController.board[c.i,c.j,c.k];

        if(x == "" && ThreeDimensionGameController.gameState == "play"){

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Input.GetMouseButtonDown(0)){
                if(Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject){
                    buttonEvent.Invoke();
                }
            }

            if(Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject){
                button.GetComponent<Renderer>().enabled = true;
            } else{
                button.GetComponent<Renderer>().enabled = false;
            }
        } 
    }
}
