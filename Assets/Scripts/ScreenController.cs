using UnityEngine;
using System;

public class ScreenController : MonoBehaviour
{
    public enum ScreenType
    {
        /// <summary>
        /// 左右3D银幕
        /// </summary>
        MovieLeftRight3D,

        /// <summary>
        /// 右左3D银幕
        /// </summary>
        MovieRightLeft3D,

        /// <summary>
        /// 上下3D银幕
        /// </summary>
        MovieTopDown3D,

        /// <summary>
        /// 下上3D银幕
        /// </summary>
        MovieDownTop3D,

        /// <summary>
        /// 普通银幕
        /// </summary>
        MovieNormal,

        /// <summary>
        /// 普通全景
        /// </summary>
        PanoNormal,

        /// <summary>
        /// 上下3D全景
        /// </summary>
        PanoTopDown3D,

        /// <summary>
        /// 下上3D全景
        /// </summary>
        PanoDownTop3D,

        /// <summary>
        /// 左右3D全景
        /// </summary>
        PanoLeftRight3D
    }

    public bool isPano
    {
        get
        {
            if (screenType == ScreenType.PanoNormal ||
                screenType == ScreenType.PanoDownTop3D ||
                screenType == ScreenType.PanoLeftRight3D ||
                screenType == ScreenType.PanoTopDown3D
                )
                return true;
            else
                return false;
        }
    }
    public bool is3D
    {
        get
        {
            if (screenType == ScreenType.MovieDownTop3D ||
                screenType == ScreenType.MovieLeftRight3D ||
                screenType == ScreenType.MovieRightLeft3D ||
                screenType == ScreenType.MovieTopDown3D ||
                screenType == ScreenType.PanoDownTop3D ||
                screenType == ScreenType.PanoLeftRight3D ||
                screenType == ScreenType.PanoTopDown3D
                )
                return true;
            else
                return false;
        }
    }

    //左右3D的材质设置
    private readonly Vector2 LEFT_RIGHT_TEXTURE_SCALE = new Vector2(0.5f, 1f);
    private readonly Vector2 LEFT_TEXTURE_OFFSET = new Vector2(0.0f, 0.0f);
    private readonly Vector2 RIGHT_TEXTURE_OFFSET = new Vector2(0.5f, 0.0f);

    //上下3D的材质设置
    private readonly Vector2 TOP_DOWN_TEXTURE_SCALE = new Vector2(1.0f, 0.5f);
    private readonly Vector2 TOP_TEXTURE_OFFSET = new Vector2(0.0f, 0.5f);
    private readonly Vector2 DOWN_TEXTURE_OFFSET = new Vector2(0.0f, 0.0f);

    //普通材质设置
    private readonly Vector2 NORMAL_TEXTURE_SCALE = new Vector2(1.0f, 1.0f);
    private readonly Vector2 NORMAL_TEXTURE_OFFSET = new Vector2(0.0f, 0.0f);

    private Transform thisTransform;

    //电影屏幕和全景屏幕的左右眼材质
    [SerializeField]
    public MeshRenderer movieLeftScreenRenderer;
    [SerializeField]
    public MeshRenderer movieRightScreenRenderer;
    [SerializeField]
    public MeshRenderer panoLeftScreenRenderer;
    [SerializeField]
    public MeshRenderer panoRightScreenRenderer;

    private Material movieLeftScreenMaterial = null;
    private Material movieRightScreenMaterial = null;
    private Material panoLeftScreenMaterial = null;
    private Material panoRightScreenMaterial = null;

    //准备画面
    public Texture movieReadyTexture;
    //public Texture movie3DReadyTexture;
    public Texture panoReadyTexture;
    //public Texture pano3DReadyTexture;

    //视频贴图，也即是显示播放器渲染的画面
    public Texture moviePlayTexture;
    public Texture panoPlayTexture;

    public GameObject movieScreen;
    public GameObject panoScreen;
    public GameObject Environment;

    [SerializeField]
    public ScreenType screenType;


    /// <summary>
    /// 屏幕的宽高比
    /// </summary>
    [Obsolete]
    public float Ratio
    {
        get
        {
            return ScreenWidth / ScreenHeight;
        }
    }

    /// <summary>
    /// 屏幕的宽度，单位为米（以Unity中一个unit为1米为准）
    /// </summary>
    [Obsolete]
    public float ScreenWidth
    {
        get
        {
            return thisTransform.localScale.x;
        }
        set
        {
            thisTransform.localScale = new Vector3(value, ScreenHeight, 1);
        }
    }

    /// <summary>
    /// 屏幕的高度，单位为米（以Unity中一个unit为1米为准）
    /// </summary>
    [Obsolete]
    public float ScreenHeight
    {
        get
        {
            return thisTransform.localScale.y;
        }
        set
        {
            thisTransform.localScale = new Vector3(ScreenWidth, value, 1);
        }
    }

    // Use this for initialization
    void Awake()
    {
        thisTransform = transform;

        movieLeftScreenMaterial = movieLeftScreenRenderer.material;
        movieRightScreenMaterial = movieRightScreenRenderer.material;
        panoLeftScreenMaterial = panoLeftScreenRenderer.material;
        panoRightScreenMaterial = panoRightScreenRenderer.material;
    }

    /// <summary>
    /// 切换屏幕类型
    /// </summary>
    /// <param name="type">屏幕的枚举类型</param>
    public void ChangeScreenType(ScreenType type)
    {
        this.screenType = type;
        switch (screenType)
        {
            case ScreenType.MovieLeftRight3D:
                ActiviateMovieScreen();
                break;
            case ScreenType.MovieRightLeft3D:
                ActiviateMovieScreen();
                break;
            case ScreenType.MovieTopDown3D:
                ActiviateMovieScreen();
                break;
            case ScreenType.MovieDownTop3D:
                ActiviateMovieScreen();
                break;
            case ScreenType.MovieNormal:
                ActiviateMovieScreen();
                break;
            case ScreenType.PanoNormal:
                ActiviatePanoScreen();
                break;
            case ScreenType.PanoTopDown3D:
                ActiviatePanoScreen();
                break;
            case ScreenType.PanoDownTop3D:
                ActiviatePanoScreen();
                break;
            case ScreenType.PanoLeftRight3D:
                ActiviatePanoScreen();
                break;
        }
    }

    // 激活电影屏幕
    private void ActiviateMovieScreen()
    {
        movieScreen.SetActive(true);
        panoScreen.SetActive(false);
        Environment.SetActive(true);
    }

    // 激活全景屏幕
    private void ActiviatePanoScreen()
    {
        movieScreen.SetActive(false);
        panoScreen.SetActive(true);
        Environment.SetActive(false);
    }

    //切换电影屏幕材质的Scale和Offset，其中leftOffset表示左眼屏幕材质的Offset，rightOffset表示右眼屏幕材质的Offset
    private void ChangeMovieScreenTextureScaleAndOffset(Vector2 scale, Vector2 leftOffset, Vector2 rightOffset)
    {
        if (movieLeftScreenMaterial == null)
        {
            return;
        }
        movieLeftScreenMaterial.mainTextureScale = scale;
        movieRightScreenMaterial.mainTextureScale = scale;
        movieLeftScreenMaterial.mainTextureOffset = leftOffset;
        movieRightScreenMaterial.mainTextureOffset = rightOffset;
    }

    //切换全景屏幕的Scale和Offset
    private void ChangePanoScreenTextureScaleAndOffset(Vector2 scale, Vector2 leftOffset, Vector2 rightOffset)
    {
        if (panoLeftScreenMaterial == null)
        {
            return;
        }
        panoLeftScreenMaterial.mainTextureScale = scale;
        panoRightScreenMaterial.mainTextureScale = scale;
        panoLeftScreenMaterial.mainTextureOffset = leftOffset;
        panoRightScreenMaterial.mainTextureOffset = rightOffset;
    }

    /// <summary>
    /// 重置材质的纹理偏移至Ready状态
    /// </summary>
    private void ResetTexScaleAndOffSet()
    {
        switch (screenType)
        {
            case ScreenType.MovieNormal:
            case ScreenType.MovieLeftRight3D:
            case ScreenType.MovieRightLeft3D:
            case ScreenType.MovieTopDown3D:
            case ScreenType.MovieDownTop3D:
                ChangeMovieScreenTextureScaleAndOffset(NORMAL_TEXTURE_SCALE, NORMAL_TEXTURE_OFFSET, NORMAL_TEXTURE_OFFSET);
                break;
            case ScreenType.PanoNormal:
            case ScreenType.PanoTopDown3D:
            case ScreenType.PanoDownTop3D:
            case ScreenType.PanoLeftRight3D:
                ChangePanoScreenTextureScaleAndOffset(NORMAL_TEXTURE_SCALE, NORMAL_TEXTURE_OFFSET, NORMAL_TEXTURE_OFFSET);
                break;
        }
    }

    /// <summary>
    /// 切换材质的纹理偏移至Play状态
    /// </summary>
    private void SwitchTexScaAndOffToPlayState()
    {
        Debug.Log("[ScreenController]: switch to PlayState " + screenType.ToString());
        switch (screenType)
        {
            case ScreenType.MovieLeftRight3D:
                ChangeMovieScreenTextureScaleAndOffset(LEFT_RIGHT_TEXTURE_SCALE, LEFT_TEXTURE_OFFSET, RIGHT_TEXTURE_OFFSET);
                break;
            case ScreenType.MovieRightLeft3D:
                ChangeMovieScreenTextureScaleAndOffset(LEFT_RIGHT_TEXTURE_SCALE, RIGHT_TEXTURE_OFFSET, LEFT_TEXTURE_OFFSET);
                break;
            case ScreenType.MovieTopDown3D:
                ChangeMovieScreenTextureScaleAndOffset(TOP_DOWN_TEXTURE_SCALE, TOP_TEXTURE_OFFSET, DOWN_TEXTURE_OFFSET);
                break;
            case ScreenType.MovieDownTop3D:
                ChangeMovieScreenTextureScaleAndOffset(TOP_DOWN_TEXTURE_SCALE, DOWN_TEXTURE_OFFSET, TOP_TEXTURE_OFFSET);
                break;
            case ScreenType.MovieNormal:
                ChangeMovieScreenTextureScaleAndOffset(NORMAL_TEXTURE_SCALE, NORMAL_TEXTURE_OFFSET, NORMAL_TEXTURE_OFFSET);
                break;
            case ScreenType.PanoNormal:
                ChangePanoScreenTextureScaleAndOffset(NORMAL_TEXTURE_SCALE, NORMAL_TEXTURE_OFFSET, NORMAL_TEXTURE_OFFSET);
                break;
            case ScreenType.PanoTopDown3D:
                ChangePanoScreenTextureScaleAndOffset(TOP_DOWN_TEXTURE_SCALE, TOP_TEXTURE_OFFSET, DOWN_TEXTURE_OFFSET);
                break;
            case ScreenType.PanoDownTop3D:
                ChangePanoScreenTextureScaleAndOffset(TOP_DOWN_TEXTURE_SCALE, DOWN_TEXTURE_OFFSET, TOP_TEXTURE_OFFSET);
                break;
            case ScreenType.PanoLeftRight3D:
                ChangePanoScreenTextureScaleAndOffset(LEFT_RIGHT_TEXTURE_SCALE, LEFT_TEXTURE_OFFSET, RIGHT_TEXTURE_OFFSET);
                break;
        }
    }

    public GameObject[] GetActiveScreen()
    {
        MeshRenderer[] meshList = null;
        GameObject[] screenlist = null;
        if (movieScreen.activeInHierarchy)
        {
            meshList = movieScreen.GetComponentsInChildren<MeshRenderer>();
        }
        else if (panoScreen.activeInHierarchy)
        {
            meshList = panoScreen.GetComponentsInChildren<MeshRenderer>();
        }

        if (meshList != null)
        {
            screenlist = new GameObject[meshList.Length];

            for (int i = 0; i < meshList.Length; i++)
            {
                screenlist[i] = meshList[i].gameObject;
            }
        }

        return screenlist;
    }
}