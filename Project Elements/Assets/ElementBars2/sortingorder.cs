using UnityEngine;
using System.Collections;

public class sortingorder : MonoBehaviour
{

    private MeshRenderer rend;
    public string layerName;
    public int order;

    void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        rend.sortingLayerName = layerName;
        rend.sortingOrder = order;
    }
    // Use this for initialization
    void Start()
    {
        //transform.position = GetComponent<Camera> ().ScreenToWorldPoint (screenPosition);
    }


    // Update is called once per frame
    void Update()
    {

        if (rend.sortingLayerName != layerName)
            rend.sortingLayerName = layerName;
        if (rend.sortingOrder != order)
            rend.sortingOrder = order;


    }
    public void OnValidate()
    {
        rend = GetComponent<MeshRenderer>();
        rend.sortingLayerName = layerName;
        rend.sortingOrder = order;
    }


}
