using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : MonoBehaviour
{
    public int SortingOrder;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshRenderer>().sortingOrder = SortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
