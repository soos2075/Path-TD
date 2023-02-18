using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private static SceneChange _instance;
    public static SceneChange Instance { get { Initialize(); return _instance; } }

    private static void Initialize()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<SceneChange>();
            if (_instance == null)
            {
                GameObject go = new GameObject { name = "@SceneManager" };
                go.AddComponent<SceneChange>();
                _instance = go.GetComponent<SceneChange>();
            }
        }
    }

    public string PreScene; //? 이전
    public string NowScene; //? 현재
    //public Scene NextScene;


    private void Awake()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "#3_Stage")
            {
                return;
            }

            var quit = FindObjectOfType<UI_ApplicationQuit>();
            if (quit == null)
            {
                SoundManager.Instance.PlaySound("UI/UIPopup");
                GameManager.m_UI.ShowPopUp<UI_ApplicationQuit>("QuitPopup");
            }
        }
    }


    private void Start()
    {
        
    }
    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        PreScene = NowScene;
        NowScene = SceneManager.GetActiveScene().name;
    }





    public void Change(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }




}
