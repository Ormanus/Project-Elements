using UnityEngine;
using UnityEngine.SceneManagement;

public class InterLevelMenu : MonoBehaviour {

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
    }
	void Update () {}

    void OnGUI()
    {
        int x = Display.main.renderingWidth / 2;
        int y = Display.main.renderingHeight / 2;

        if (GUI.Button(new Rect(x - 100, y + 32, 200, 64), "Continue"))
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
