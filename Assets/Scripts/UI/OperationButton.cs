using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperationButton : MonoBehaviour
{
    public RightMenu rightMenu;
    public string operation;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerThis()
    {
        rightMenu.TriggerOperation(this.GetComponent<Image>(), operation);

    }
}
