using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using PubSubCommunication;
using PubSubMessages;
using TMPro;

public class SceneManagementComponent : GenericPubSubComponent
{
    #region public fields

    public static SceneManagementComponent instance;

    public string StartScreen = "";
    public string Level1 = "";

    public float FadeSpeed = 1f;

    public Image FadeOutUIImage;

    public TextMeshProUGUI OverlayTextObj;

    public enum FadeDirection
    {
        In, //Alpha = 1
        Out // Alpha = 0
    }

    #endregion

    #region private variables

    private Coroutine loadSceneCoroutine = null;

    #endregion

    #region MonoBehavior Functions

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        PubSubServerInstance.Subscribe(typeof(StartGameMessage), OnStartGame);
        PubSubServerInstance.Subscribe(typeof(SuccessMessage), OnSuccess);

        Debug.Log("IN AWAKE OF SCENE MANAGER");
    }

    private void OnDestroy()
    {
        PubSubServerInstance.Unsubscribe(typeof(StartGameMessage), OnStartGame);
        PubSubServerInstance.Unsubscribe(typeof(SuccessMessage), OnSuccess);
    }

    #endregion

    #region Scene Management Component Functions

    private IEnumerator Fade(FadeDirection fadeDirection)
    {
        float alpha = (fadeDirection == FadeDirection.Out) ? 1 : 0;

        float fadeEndValue = (fadeDirection == FadeDirection.Out) ? 0 : 1;

        if (fadeDirection == FadeDirection.Out)
        {
            while (alpha >= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
            FadeOutUIImage.enabled = false;
        }
        else
        {
            FadeOutUIImage.enabled = true;
            while (alpha <= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
        }
    }

    /// <summary>
    /// This will: 
    ///  1. Fade in an overlay
    ///  2. Unload Scene
    ///  3. Load Scene
    ///  4. Fade out overlay (Can be overwritten with param)
    /// </summary>
    /// <param name="sceneToLoad">This is the scene to load</param>
    /// <param name="holdOverlay">
    /// This allows the function to leave the overlay up until later call to fade it out.
    /// </param>
    /// <returns></returns>
    public IEnumerator FadeAndLoadScene(string sceneToLoad, string infoText = "", int waitTime = 0, bool holdOverlay = false, Sprite sprite = null)
    {
        if (holdOverlay)
        {
            FadeOutUIImage.color = Color.white;
        }
        else
        {
            FadeOutUIImage.color = Color.black;
        }

        yield return new WaitForSeconds(waitTime);

        // Fade in stuff
        // Wait until all faded in
        FadeOutUIImage.gameObject.SetActive(true);
        yield return Fade(FadeDirection.In);

        if (infoText != string.Empty)
        {
            OverlayTextObj.text = infoText;
            OverlayTextObj.gameObject.SetActive(true);
        }

        // Scene load blocking
        SceneManager.LoadScene(sceneToLoad);

        // yield return null
        yield return null;

        // Resources unused assets <<< yield here
        yield return Resources.UnloadUnusedAssets();

        // yield return null
        yield return null;

        if (!holdOverlay)
        {
            // Fade Out.
            yield return FadeOutAfterHold();
        }

        // Nullify this to unblock future scene loads
        loadSceneCoroutine = null;
    }

    public IEnumerator FadeOutAfterHold()
    {
        OverlayTextObj.text = string.Empty;
        OverlayTextObj.gameObject.SetActive(false);

        yield return Fade(FadeDirection.Out);
        FadeOutUIImage.gameObject.SetActive(false);
    }

    private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
    {
        FadeOutUIImage.color = new Color(FadeOutUIImage.color.r, FadeOutUIImage.color.g, FadeOutUIImage.color.b, alpha);
        alpha += Time.deltaTime * (1.0f / FadeSpeed) * ((fadeDirection == FadeDirection.Out) ? -1 : 1);
    }

    #endregion

    #region PubSub functions

    private void OnStartGame(BaseMessage m)
    {
        if (loadSceneCoroutine == null)
        {
            IEnumerator coroutine = FadeAndLoadScene(Level1, string.Empty, 0, false);
            loadSceneCoroutine = StartCoroutine(coroutine);
        }

        LevelManager.Instance.winScore = 1;
    }

    private void OnSuccess(BaseMessage m)
    {
        SuccessMessage successMessage = m as SuccessMessage;

        if (loadSceneCoroutine == null)
        {
            IEnumerator coroutine = FadeAndLoadScene(successMessage.nextLevel, string.Empty, 2, false);
            loadSceneCoroutine = StartCoroutine(coroutine);
        }

        PopupManager.instance.SuccessText.SetActive(false);
    }

    private void OnFailure(BaseMessage m)
    {
        if (loadSceneCoroutine == null)
        {
            IEnumerator coroutine = FadeAndLoadScene(SceneManager.GetActiveScene().name, string.Empty, 2, false);
            loadSceneCoroutine = StartCoroutine(coroutine);
        }

        PopupManager.instance.FailureText.SetActive(false);
    }

    #endregion
}
