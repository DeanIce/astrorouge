using System.Collections;
using Managers;
using UnityEngine;

public class HandleDeath : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Death Script Enabled");

        AudioManager.Instance.PlayDeathSound();
        StartCoroutine(StartRecap());

        GetComponent<PlayerDefault>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        // TODO: Handle everything that happens after death.
    }

    public Texture2D toTexture2D(RenderTexture rTex)
    {
        var tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture old_rt = RenderTexture.active;
        RenderTexture.active = rTex;

        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        RenderTexture.active = old_rt;
        return tex;
    }

    public IEnumerator StartRecap()
    {
        yield return new WaitForSeconds(2);
        GameObject ui = GameObject.Find("UIManager");
        ui.SetActive(false);
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;

        var tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        EventManager.Instance.deathCam = tex;
        // EventManager.Instance.deathCam = ScreenCapture.CaptureScreenshotAsTexture();


        yield return new WaitForSeconds(2);
        EventManager.Instance.Recap();
    }
}