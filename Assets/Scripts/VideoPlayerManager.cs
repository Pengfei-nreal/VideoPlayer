using UnityEngine;
using UnityEngine.UI;

public class VideoPlayerManager : MonoBehaviour
{
    [SerializeField]
    Button mLoadBtn;
    [SerializeField]
    Button mPlayBtn;
    [SerializeField]
    Button mQuitBtn;
    [SerializeField]
    MediaPlayerCtrl player;

    void Start()
    {
        mLoadBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
        {
            player.Load("EasyMovieTexture.mp4");
        }));
        mPlayBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
        {
            if (player.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING)
            {
                player.Stop();
            }
            else if (player.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED)
            {
                player.Play();
            }
        }));
        mQuitBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
        {

        }));

        player.OnEnd += OnEnd;

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 100, 100), "Load"))
        {

        }

        if (GUI.Button(new Rect(50, 200, 100, 100), "Play"))
        {

        }

        if (GUI.Button(new Rect(50, 350, 100, 100), "stop"))
        {

        }

        if (GUI.Button(new Rect(50, 500, 100, 100), "pause"))
        {
            player.Pause();
        }

        if (GUI.Button(new Rect(50, 650, 100, 100), "Unload"))
        {
            player.UnLoad();
        }
    }

    void OnEnd()
    {

    }
}
