using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Material skyboxMaterial_2K;
    [SerializeField] private Material skyboxMaterial_4K;
    [SerializeField] private Material skyboxMaterial_8K;

    [SerializeField] private RenderTexture RenderTexture;
    [SerializeField] private RenderTexture RenderTexture_2K;
    [SerializeField] private RenderTexture RenderTexture_4K;
    [SerializeField] private RenderTexture RenderTexture_8K;

    [SerializeField] private GameObject[] Players;

    private GameObject player;
    private float playing_time = 10.0f;
    private int VidNumb = 0;
    public GameObject[] Survey;
    private uint VidWidth = 0;
    private uint VidHeight = 0;
    public static bool PlayAccept = false;

    private void OnEnable()
    {
        // 해당 스크립트가 들어간 오브젝트의 Transform 정보를 parentTransform에 할당
        Transform parentTransform = transform;

        // Player 배열 크기를 빈 오브젝트의 자식 개수로 조정.
        Players = new GameObject[parentTransform.childCount];

        // 빈 오브젝트의 개수만큼 반복
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            GameObject childGameObject = childTransform.gameObject;

            // Players 배열의 해당 인덱스에 자식 오브젝트를 할당.
            Players[i] = childGameObject;
        }

        // Players의 VideoNumb에 해당하는 리스트 정보는 player에 할당
        player = Players[VidNumb];

        // GetVidSource 함수 확인
        GetVideoSource(player);

        if (skyboxMaterial == true && RenderTexture == true)
        {
            RenderSettings.skybox = skyboxMaterial;
            player.GetComponent<VideoPlayer>().targetTexture = RenderTexture;

            player.GetComponent<VideoPlayer>().playOnAwake = false;
            player.GetComponent<VideoPlayer>().Play();
            PlayAccept = true;
        }
    }

    private void FixedUpdate()
    {
        Transform parentTransform = transform;

        //player가 재생될 때
        if (player.GetComponent<VideoPlayer>().isPlaying == true)
        {
            playing_time += Time.deltaTime;

            if (playing_time >= 30.0f) // 30초보다 길거나 짧은 영상이 있음. 5초에서 10초 정도 텀이 필요할 수도 있음.
            {
                if (VidNumb < 56)
                {
                    VidNumb++; // 0~55 -> 비디오 순번으로 설문 문항 실행. -> 비디오 수 변경되면 수정필요.
                    Debug.Log("VideoNumber : " + VidNumb);
                }

                if (VidNumb % 2 == 0) // 두 개 영상 시청 후, 설문조사 진행 -> 비디오 수 변경되면 수정 필요.
                {
                    PlayAccept = false;
                    Debug.Log("Play Accept : " + PlayAccept);
                }

                player.GetComponent<VideoPlayer>().Pause();
                player.SetActive(false);
                SliderManager.SurveyFinish = false;
            }
        }

        // player의 재생이 종료되면 실행
        if (player.GetComponent<VideoPlayer>().isPlaying == false)
        {
            if (PlayAccept == true)
            {
                SliderManager.SurveyFinish = false;
                if (SliderManager.FinalEnd == false && SliderManager.SurveyFinish == false)
                {
                    player = Players[VidNumb];
                    GetVideoSource(player);
                    RenderSettings.skybox = skyboxMaterial;
                    player.GetComponent<VideoPlayer>().targetTexture = RenderTexture;
                    player.SetActive(true);
                    player.GetComponent<VideoPlayer>().playOnAwake = false;
                    playing_time = 0.0f;
                    player.GetComponent<VideoPlayer>().Play();
                }
            }
            else if (PlayAccept == false && VidNumb % 2 == 0)
            {
                Debug.Log("Questionare Number : " + (VidNumb / 2));
                Survey[VidNumb / 2].SetActive(true); // 비디오 수 변경되면 수정 필요.
            }
        }
        // 설문 종료 후 이전 영상 끄고 PlayAccept 변환
        if (SliderManager.SurveyFinish == true)
        {
            Survey[VidNumb / 2].SetActive(false); // 비디오 수 변경되면 수정 필요
            SliderManager.SaveTrigger = false;
            StartCoroutine(DelayTime(0.5f));
            PlayAccept = true;
        }

    }

    public static IEnumerator DelayTime(float delay) // delay 시간만큼 나중에 실행
    {
        yield return new WaitForSeconds(delay);
        SliderManager.SurveyFinish = false;
    }

    void GetVideoSource(GameObject Vid) // 영상 해상도에 따라서 material과 rendertexture 설정
    {
        VideoPlayer videoPlayer = Vid.GetComponent<VideoPlayer>();

        if (videoPlayer != null && videoPlayer.clip != null)
        {
            VidWidth = videoPlayer.width;
            VidHeight = videoPlayer.height;

            if (VidWidth == 2560 && VidHeight == 1440)
            {
                skyboxMaterial = skyboxMaterial_2K;
                RenderTexture = RenderTexture_2K;
            }
            if (VidWidth == 3840 && VidHeight == 2160)
            {
                skyboxMaterial = skyboxMaterial_4K;
                RenderTexture = RenderTexture_4K;
            }
            if (VidWidth == 7680 && VidHeight == 4320)
            {
                skyboxMaterial = skyboxMaterial_8K;
                RenderTexture = RenderTexture_8K;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
