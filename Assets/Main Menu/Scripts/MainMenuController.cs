using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private static NewMonoBehaviourScript instance;
    private float alpha = 0f;
    private float fadeSpeed = 1f;
    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private string sceneToLoad = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne deðiþiminde kaybolmasýn
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnGUI()
    {
        if (alpha > 0f)
        {
            Color oldColor = GUI.color;
            GUI.color = new Color(0, 0, 0, alpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = oldColor;
        }
    }

    private void Update()
    {
        if (isFadingOut)
        {
            alpha += Time.deltaTime * fadeSpeed;
            if (alpha >= 1f)
            {
                alpha = 1f;
                isFadingOut = false;
                SceneManager.LoadScene(sceneToLoad);
                StartCoroutine(FadeIn());
            }
        }
        else if (isFadingIn)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            if (alpha <= 0f)
            {
                alpha = 0f;
                isFadingIn = false;
            }
        }
    }

    public void startgame()
    {
        FadeToScene("SampleScene");
    }

    public void options()
    {
        FadeToScene("Settings");
    }

    public void xd()
    {
        Application.Quit();
    }

    public void FadeToScene(string sceneName)
    {
        sceneToLoad = sceneName;
        isFadingOut = true;
    }

    private IEnumerator FadeIn()
    {
        yield return new WaitForEndOfFrame();
        isFadingIn = true;
    }
}
