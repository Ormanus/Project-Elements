using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageUnfill : MonoBehaviour
{
    [SerializeField]
    Image CircleImage;

    public int fillOrigin;

    void Start()
    {
        CircleImage.type = Image.Type.Filled;
        CircleImage.fillMethod = Image.FillMethod.Radial180;
        CircleImage.fillOrigin = fillOrigin;
    }

    void Update()
    {
        CircleImage.fillAmount = Mathf.Max(PlayerHealth.Playerhealth, 0.002f);
    }
}
