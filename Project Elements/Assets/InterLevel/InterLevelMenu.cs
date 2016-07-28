using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InterLevelMenu : MonoBehaviour {

    public string[] storyPaths;
    public Texture2D buttonBG;
    public Font font;

    private GUIStyle buttonStyle;
    private GUIStyle headerStyle;

	void Start ()
    {
        buttonStyle = new GUIStyle();
        headerStyle = new GUIStyle();

        buttonStyle.font = font;
        buttonStyle.fontSize = 20;
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle.normal.background = buttonBG;
        buttonStyle.normal.textColor = Color.yellow;
        buttonStyle.fixedWidth = 200;
        buttonStyle.fixedHeight = 64;

        headerStyle.font = font;
        headerStyle.fontSize = 32;
        headerStyle.alignment = TextAnchor.MiddleCenter;
        headerStyle.normal.textColor = Color.white;
        headerStyle.fixedHeight = 64;

        string story = File.ReadAllText(Application.dataPath + "/Resources/" + storyPaths[GameSceneLevelLoading.levelNumber]);

        int x = Display.main.renderingWidth / 2;
        int y = Display.main.renderingHeight / 100 * 65;

        GameObject o = new GameObject();
        o.transform.parent = transform;

        RectTransform rect = o.AddComponent<RectTransform>();

        rect.offsetMax = new Vector2(300, 400);
        rect.offsetMin = new Vector2(-300, 0);

        rect.anchorMin = new Vector2(0.5f, 0.65f);
        rect.anchorMax = new Vector2(0.5f, 0.65f);
        o.AddComponent<CanvasRenderer>();
        Text text = o.AddComponent<Text>();
        text.color = Color.white;
        text.text = story;
        text.font = font;
        text.fontSize = 16;
        text.alignment = TextAnchor.LowerCenter;
        text.transform.position = new Vector3(x, y, 0);
    }
	void Update () {}

    void OnGUI()
    {
        int x = Display.main.renderingWidth / 2;
        int y = Display.main.renderingHeight / 2 + 128;

        if (GUI.Button(new Rect(x - 128, y + 64, 256, 32), "Continue"))
        {
            SceneManager.LoadScene("GameScene");
        }
        if(Inventory.shopTokens > 0)
        {
            if (GUI.Button(new Rect(x - 128, y + 104, 256, 32), "Shop (portals left: " + Inventory.shopTokens + ")"))
            {
                SceneManager.LoadScene("ShopScene");
            }
        }
    }
}
