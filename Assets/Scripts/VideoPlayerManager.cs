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
                mPlayBtn.GetComponentInChildren<Text>().text = "Play";
            }
            else if (player.GetCurrentState() == MediaPlayerCtrl.MEDIAPLAYER_STATE.PAUSED)
            {
                player.Play();
                mPlayBtn.GetComponentInChildren<Text>().text = "Pause";
            }
        }));
        mQuitBtn.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
        {
            player.UnLoad();
        }));

        player.OnEnd += OnEnd;
        player.OnReady += OnReady;
        player.OnVideoFirstFrameReady += OnFirstFrameReady;
    }

    void OnEnd()
    {
        Debug.Log("OnEnd!");
    }

    /// <summary>
    /// 视频加载完成后启动播放（自动播放模式下不需要此操作）
    /// </summary>
    void OnReady()
    {
        Debug.Log("OnReady!");
        player.Play();
    }

    void OnFirstFrameReady()
    {
        Debug.Log("OnFirstFrameReady!");
    }
}
