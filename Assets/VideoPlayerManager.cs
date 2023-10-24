using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class NewBehaviourScript : MonoBehaviour
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
    private float playing_time = 0.0f;
    private int VidNumb = 0;
    public GameObject[] Survey;
    private uint VidWidth = 0;
    private uint VidHeight = 0;
    public static bool PlayAccept = 0;

    private void OnEnable()
    {
        // �ش� ��ũ��Ʈ�� �� ������Ʈ�� Transform ������ parentTransform�� �Ҵ�
        Transform parentTransform = transform;

        // Player �迭 ũ�⸦ �� ������Ʈ�� �ڽ� ������ ����.
        Players new GameObject[parentTransform.childCount];

        // �� ������Ʈ�� ������ŭ �ݺ�
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            GameObject childGameObject = childTransform.gameObject;

            // Players �迭�� �ش� �ε����� �ڽ� ������Ʈ�� �Ҵ�.
            Players[i] = childGameObject;
        }

        // Players�� VideoNumb�� �ش��ϴ� ����Ʈ ������ player�� �Ҵ�
        player = Players[VidNumb];

        // GetVidSource �Լ� Ȯ��
        GetVidSource(player);

        if (skyboxMaterial == true && RenderTexture == true)
        {
            RenderSetting.skybox = skyboxMaterial;
            player.GetComponent<VideoPlayer>().targetTexture = RenderTexture;

            player.GetComponent<VideoPlayer>().playOnAwake = false;
            player.GetCOmponenet<VideoPlayer>().Play();
            PlayAccept = true;
        }
    }

    private void FixedUpdate()
    {
        Transform parentTransform = transform;

        //player�� ����� ��
        if (player.GetComponent<VideoPlayer>().isPlaying == true)
        {
            playing_time += Time.deltaTime;

            if (playing_time >= 10.0f) // 30�ʺ��� ��ų� ª�� ������ ����. 5�ʿ��� 10�� ���� ���� �ʿ��� ���� ����.
            {
                if (VidNumb < 70)
                {
                    VidNumb++; // 0~70 -> ���� �������� ���� ���� ����. -> ���� �� ����Ǹ� �����ʿ�.
                    Debug.Log(VidNumb);
                }

                if (VidNumb % 2 == 0) // �� �� ���� ��û ��, �������� ���� -> ���� �� ����Ǹ� ���� �ʿ�.
                {
                    PlayAccept = false;
                    // Debug.Log(PlayAccept);
                }

                player.GetComponent<VideoPlayer>().Pause();
                player.SetActive(false);
                NewSliderController.SurveyFinish = false; // �̸� ���� �ʿ�.
            }
        }

        // player�� ����� ����Ǹ� ����
        if (player.GetComponent<VideoPlayer>().isPlaying == false)
        {
            if (PlayAccept == true)
            {
                NewSliderController.SurveyFinish = false;
                if (NewSliderController.FinalEnd == false && NewSliderConroller.SurveyFinishi == false)
                {
                    player = Players[VidNumb];
                    GetVidSource(player);
                    RenderSettings.skybox = skyboxMaterial;
                    player.GetComponent<VideoPlayer>().targetTexture = RenderTexture;
                    player.SetActive(true);
                    player.GetComponent<VideoPlayer>().playOnAwake = false;
                    playing_time = 0.0f
                    player.GetComponent<VideoPlayer>().Play();
                }
            }
            else if (PlayAccept == false && VidNumb % 2 == 0)
            {
                Survey[(VidNumb / 2) - 1].SetActive(true); // ���� �� ����Ǹ� ���� �ʿ�.
            }
        }
        // ���� ���� �� ���� ���� ���� PlayAccep ��ȯ
        if (NewSliderController.SurveyFinish == true)
        {
            Survey[(VidNumb / 2) - 1].SetActive(false); // ���� �� ����Ǹ� ���� �ʿ�
            NewSliderController.SaveTrigger = false;
            StartCoroutine(DelayTime(0.5f));
            PlayAccept = true;
        }

    }

    public static IEnumerator DelayTime(float delay) // delay �ð���ŭ ���߿� ����
    {
        yield return new WaitForSeconds(delay);
        NewSliderController.SurveyFinish = false;
    }

    public static List<GameObject> GetChildrenList(GameObject parent) // Children ���� �޾ƿ���
    {
        List<GameObject> childrenList = new List<GameObject>();

        foreach (Transform childTransform in parent.transform)
        {
            GameObject childGameObject = childTransform.gameObject;
            childrenList.Add(childGameObject);
        }
    }

    void GetVidSource(GameObject Vid) // ���� �ػ󵵿� ���� Material�� RenderTexture ����
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
                RenderTexture = RenderTexture_2K;
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