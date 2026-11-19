using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;
using Yarn.Unity;
using System.Collections;
using System.Collections.Generic;

public class dialogueshit : MonoBehaviour
{
    public character portraitView;
    public DialogueRunner lineshow;
    public Volume bwVolume;

    static bool bwOn;                       
    public static dialogueshit current;

    static Dictionary<string, float> savedF;
    static Dictionary<string, string> savedS;
    static Dictionary<string, bool> savedB;

    readonly List<GameObject> shown = new List<GameObject>();
    Coroutine bwFade;

 void Awake()
{
    if (bwVolume == null) bwVolume = FindFirstObjectByType<Volume>();
    current = this;

    
    lineshow.AddCommandHandler("turnBW", () => SetBW(true));
    lineshow.AddCommandHandler("bwOff", () => SetBW(false));
    lineshow.AddCommandHandler<string>("play", PlayMoment);
    lineshow.AddCommandHandler<string>("show", ShowMoment);
    lineshow.AddCommandHandler("clear", ClearMoments);
    lineshow.AddCommandHandler<string>("loadscene", LoadYourScene);
    lineshow.AddCommandHandler<string>("hideobj", hideobject);
    lineshow.AddCommandHandler("quit", QuitGame);

    lineshow.AddCommandHandler<string>("audio", PlayAudio);
    lineshow.AddCommandHandler<string>("audiostop", StopAudio);

    if (portraitView != null)
    {
        lineshow.AddCommandHandler<string>("portrait", portraitView.ShowPortrait);
        lineshow.AddCommandHandler("hideportrait", portraitView.HidePortrait);
    }

    
    if (bwVolume != null) bwVolume.weight = bwOn ? 1f : 0f;

    
    var vs = lineshow.VariableStorage as InMemoryVariableStorage;
    if (vs != null && savedB != null)
        vs.SetAllVariables(savedF, savedS, savedB);
}

    void Start()
    {
        var vs = lineshow.VariableStorage as InMemoryVariableStorage;
        if (vs != null && savedB != null)
            vs.SetAllVariables(savedF, savedS, savedB);
    }

    void OnDestroy()
    {
        if (lineshow == null) return;
        var vs = lineshow.VariableStorage as InMemoryVariableStorage;
        if (vs == null) return;
        var all = vs.GetAllVariables();
        savedF = all.Item1;
        savedS = all.Item2;
        savedB = all.Item3;
    }

    
    public void SetBW(bool on)
    {
        Debug.Log($"[BW] SetBW({on}) volume={(bwVolume ? bwVolume.name : "NULL")}");
        bwOn = on;
        if (bwVolume == null) return;
        if (bwFade != null) StopCoroutine(bwFade);
        bwFade = StartCoroutine(FadeBW(on ? 1f : 0f, 1.5f));
    }

    public static void ForceNormal() { if (current != null) current.SetBW(false); }

    IEnumerator FadeBW(float target, float duration)
    {
        float start = bwVolume.weight, t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            bwVolume.weight = Mathf.Lerp(start, target, t / duration);
            yield return null;
        }
        bwVolume.weight = target;
    }

   
    GameObject Spawn(string momentName)
{
    var prefab = Resources.Load<GameObject>("Moments/" + momentName);
    if (prefab == null)
    {
        Debug.LogWarning("No moment prefab at Resources/Moments/" + momentName);
        return null;
    }
    var go = Instantiate(prefab);
    var vp = go.GetComponentInChildren<VideoPlayer>();
    if (vp != null && vp.renderMode == VideoRenderMode.CameraNearPlane)
        vp.targetCamera = Camera.main;

#if UNITY_WEBGL && !UNITY_EDITOR
    if (vp != null)
    {
        vp.source = VideoSource.Url;
        vp.url = Application.streamingAssetsPath + "/Videos/" + momentName + ".mp4";
        vp.Play();
    }
#endif

    return go;
}

    void ShowMoment(string momentName)
    {
        var go = Spawn(momentName);
        if (go != null) shown.Add(go);
    }

    void ClearMoments()
    {
        foreach (var g in shown) if (g != null) Destroy(g);
        shown.Clear();
    }

    AudioSource FindAudio(string objName)
{
    foreach (var a in FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        if (a.name == objName) return a;
    Debug.LogWarning("No AudioSource named: " + objName);
    return null;
}

void PlayAudio(string objName)
{
    var a = FindAudio(objName);
    if (a == null) return;
    a.gameObject.SetActive(true);
    a.enabled = true;
    a.Play();
}

void QuitGame()
{
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;     
#elif UNITY_WEBGL
    UnityEngine.SceneManagement.SceneManager.LoadScene("TheEnd");   
#else
    Application.Quit();                                  
#endif
}

void StopAudio(string objName)
{
    var a = FindAudio(objName);
    if (a != null) a.Stop();
}

    IEnumerator PlayMoment(string momentName)
    {
        var go = Spawn(momentName);
        if (go == null) yield break;

        var vp = go.GetComponentInChildren<VideoPlayer>();
        if (vp != null)
        {
            vp.isLooping = false;
            bool done = false;
            vp.loopPointReached += _ => done = true;
            vp.errorReceived += (_, msg) => { Debug.LogError("[moment] " + msg); done = true; };

            float timeout = (vp.clip != null ? (float)vp.clip.length : 30f) + 2f;
            float t = 0f;
            while (!done && t < timeout && !Input.GetKeyDown(KeyCode.Escape))
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }
        }
        Destroy(go);
    }

  
    public void hideobject(string objname)
    {
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.name == objname && obj.scene.IsValid())
            {
                obj.SetActive(false);
                return;
            }
        }
        Debug.LogWarning("Could not find: " + objname);
    }

    public void LoadYourScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}