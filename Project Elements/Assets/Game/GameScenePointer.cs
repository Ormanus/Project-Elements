using UnityEngine;
using System.Collections;

public struct ElementColors
{
    public static Color Fire = new Color(1.0f, 0.7f, 0.2f);
    public static Color Ice = new Color(0.3f, 0.75f, 1.0f);
    public static Color Air = new Color(0.8f, 1.0f, 0.4f);
};

public class GameScenePointer : MonoBehaviour {

    public Transform target;

    private LineRenderer line1;
    private LineRenderer line2;
    private Transform circle;

    // Use this for initialization
    void Start () {
        line1 = GameObject.Find("Line1").GetComponent<LineRenderer>();
        line2 = GameObject.Find("Line2").GetComponent<LineRenderer>();
        circle = GameObject.Find("Circle").transform;
        setColor(new Vector2(0, 1.0f));
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position += new Vector3(0, 0, 1);

        Vector3 circlePos = target.position;
        Vector2 delta = transform.position - circlePos;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, delta);

        Vector3 line = (delta / 4.0f);

        line1.SetPosition(0, transform.position);
        line1.SetPosition(1, transform.position - line);

        line2.SetPosition(0, circlePos);
        line2.SetPosition(1, circlePos + line);

        //circle.rotation = transform.rotation;
        circle.position = circlePos;
        circle.localEulerAngles = new Vector3(60, 0, 180 * Mathf.Atan2(delta.y, delta.x) / Mathf.PI);
    }

    public void setColor(Vector2 data)
    {
        line1 = GameObject.Find("Line1").GetComponent<LineRenderer>();
        line2 = GameObject.Find("Line2").GetComponent<LineRenderer>();
        circle = GameObject.Find("Circle").transform;

        Color color;
        switch((int)data.x)
        {
            case 0:
                color = ElementColors.Fire;
                break;
            case 1:
                color = ElementColors.Ice;
                break;
            case 2:
                color = ElementColors.Air;
                break;
            default:
                color = ElementColors.Fire;
                break;
        }
        color *= new Color(data.y, data.y, data.y);
        GetComponent<SpriteRenderer>().color = color;
        circle.gameObject.GetComponent<SpriteRenderer>().color = color;

        Color zeroAlpha = color * new Color(1, 1, 1, 0);

        line2.SetColors(color, zeroAlpha);
        line1.SetColors(color, zeroAlpha);
    }
}
