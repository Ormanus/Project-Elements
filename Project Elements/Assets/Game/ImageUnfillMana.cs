using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageUnfillMana : MonoBehaviour
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
        CircleImage.fillAmount = Mathf.Max(PlayerHealth.Playermana / Inventory.maxMana, 0.002f);
    }
}
