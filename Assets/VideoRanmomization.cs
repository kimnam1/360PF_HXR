using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class VideoRanmomization : MonoBehaviour
{
    [SerializeField] private VideoClip v1 = null;
    [SerializeField] private VideoClip v2 = null;
    [SerializeField] private VideoClip v3 = null;
    [SerializeField] private VideoClip v4 = null;
    [SerializeField] private VideoClip v5 = null;
    [SerializeField] private VideoClip v6 = null;
    [SerializeField] private VideoClip v7 = null;
    [SerializeField] private VideoClip v8 = null;
    [SerializeField] private VideoClip v9 = null;
    [SerializeField] private VideoClip v10 = null;
    [SerializeField] private VideoClip v11 = null;
    [SerializeField] private VideoClip v12 = null;
    [SerializeField] private VideoClip v13 = null;
    [SerializeField] private VideoClip v14 = null;
    [SerializeField] private VideoClip v15 = null;
    [SerializeField] private VideoClip v16 = null;
    [SerializeField] private VideoClip v17 = null;
    [SerializeField] private VideoClip v18 = null;
    [SerializeField] private VideoClip v19 = null;
    [SerializeField] private VideoClip v20 = null;
    [SerializeField] private VideoClip v21 = null;
    [SerializeField] private VideoClip v22 = null;
    [SerializeField] private VideoClip v23 = null;
    [SerializeField] private VideoClip v24 = null;

    // 2���� �迭
    private VideoClip[,] VideoArray = new VideoClip[4, 6];
    public VideoClip[,] ReorderedClips = null;
    public VideoClip[] FinalClips = null;

    // ���� 4�� * �� 7���� ����� �� * ���� 2�� ���� = 56��
    public static GameObject[] videos = new GameObject[56];
    public GameObject VideoPlayer;

    public List<List<string>> DataList = new List<List<string>>();
    public static List<List<int>> Combination = new List<List<int>>();
    private List<int> OneList = new List<int>();
    private List<List<int>> CombinationList = new List<List<int>>();

    private string csvDirectoryName = "Survey";
    public string csvFileName;
    public static string SceneFullName = null;

    // Start is called before the first frame update
    private void Awake()
    {
        VideoArray[0, 0] = v1;
        VideoArray[0, 1] = v2;
        VideoArray[0, 2] = v3;
        VideoArray[0, 3] = v4;
        VideoArray[0, 4] = v5;
        VideoArray[0, 5] = v6;
        VideoArray[1, 0] = v7;
        VideoArray[1, 1] = v8;
        VideoArray[1, 2] = v9;
        VideoArray[1, 3] = v10;
        VideoArray[1, 4] = v11;
        VideoArray[1, 5] = v12;
        VideoArray[2, 0] = v13;
        VideoArray[2, 1] = v14;
        VideoArray[2, 2] = v15;
        VideoArray[2, 3] = v16;
        VideoArray[2, 4] = v17;
        VideoArray[2, 5] = v18;
        VideoArray[3, 0] = v19;
        VideoArray[3, 1] = v20;
        VideoArray[3, 2] = v21;
        VideoArray[3, 3] = v22;
        VideoArray[3, 4] = v23;
        VideoArray[3, 5] = v24;

        for (int i = 0; i < 4; i++) // ���� 4�� 4�� ����ȭ
        {
            // ���� ��� ���� (Resources ���� ���� ������ ��� ��η� ���� ����) Resources ������ Unity 3D Assets ���� ������.
            string filePath = "DataCombination";

            // CSV ������ �о List<List<string>>�� �����ϴ� �޼��� ȣ��
            List<List<string>> DataList = ReadCSVFile(filePath); // to do

            // string List ���� int List ������ ��ȯ & int List ���� ����
            List<List<int>> Combination = SelectedValues(String_to_int(DataList)); // to do

            // List<int>�� ��ȯ
            OneList = FlattenList(Combination);

            // List<List<Int>>�� ��ġ��
            CombinationList.Add(OneList);

        }

        // CombinationLIst ���� ���� CSV
        SceneFullName = SceneManager.GetActiveScene().name;
        csvFileName = "Result" + SceneFullName + ".csv";
        CreateCSV();// to do

        // �޾ƿ� List<List<int>>�� ���� ���� �迭
        ReorderedClips = ReorderedArray(CombinationList, VideoArray);

        // VideoClip[]���� ����
        FinalClips = ReorderedClips.Cast<VideoClip>().ToArray();

        // VideoClip[] Shuffle
        FinalClips = ShuffleVideoClips(FinalClips); // to do

        List<GameObject> children = GetChildrenList(VideoPlayer);

        // �������� ��迭�� ���� ������ �˸°� video �� ���� ��迭
        for (int i = 0; i < FinalClips.Length; i++)
        {
            videos[i] = children[i];
        }
        for (int video_num = 0; video_num < videos.Length; video_num++)
        {
            videos[video_num].GetComponent<VideoPlayer>().clip = FinalClips[video_num];
        }
    }

    public static List<GameObject> GetChildrenList(GameObject parent)
    {
        List<GameObject> childrenList = new List<GameObject>();

        foreach (Transform childTransform in parent.transform)
        {
            GameObject childGameObject = childTransform.gameObject;
            childrenList.Add(childGameObject);
        }

        return childrenList;
    }

    public static List<List<int>> String_to_int(List<List<string>> Strings) // String to Int 
    {
        List<List<int>> New_List = new List<List<int>>();
        foreach (List<string> strList in Strings)
        {
            List<int> intList = new List<int>();

            foreach (string str in strList)
            {
                int num = int.Parse(str);
                intList.Add(num);
            }
            New_List.Add(intList);
        }

        return New_List;
    }

    public static List<List<int>> SelectedValues(List<List<int>> Integers) // ���� �� �迭 ������
    {
        ShuffleList(Integers);
        List<List<int>> OrderList = new List<List<int>>();
        System.Random random = new System.Random();

        foreach (var listInt in Integers)
        {
            // 0 or 1 random
            int randomIdx = random.Next(2);

            // ���õ� index�� ���� �� ����
            int selectedValue1 = listInt[randomIdx * 2];
            int selectedValue2 = listInt[randomIdx * 2 + 1];

            List<int> selectedValues = new List<int> { selectedValue1, selectedValue2 };

            OrderList.Add(selectedValues);
        }

        return OrderList;
    }

    public static void ShuffleList(List<List<int>> lst) // ���� ���� ������
    {
        int n = lst.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + UnityEngine.Random.Range(0, n - i);
            List<int> temp = lst[r];
            lst[r] = lst[i];
            lst[i] = temp;
        }
    }

    public static List<int> FlattenList(List<List<int>> lsts) // ���� ���� �ϳ��� ������
    {
        List<int> flattened = new List<int>();

        foreach (var sublist in lsts)
        {
            flattened.AddRange(sublist);
        }

        return flattened;
    }

    public static VideoClip[,] ReorderedArray(List<List<int>> orderList, VideoClip[,] videoClips) // List<List<int>> orderList ���� �ִ� ������ VideoClip[,] vidoeClips �� ���� ���� ��迭
    {
        VideoClip[,] reordered = new VideoClip[4, 14];
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 14; j++)
            {
                int newIdx = orderList[i][j];
                reordered[i, j] = videoClips[i, newIdx];
            }
        }

        return reordered;

    }

    public VideoClip[] ShuffleVideoClips(VideoClip[] videoClips) // VideoClip[] videoClips �� 1���� �� ���� 2�� �� ��� ���� ��迭(����)
    {
        List<VideoClip[]> paired = new List<VideoClip[]>();

        for(int i = 0; i < videoClips.Length; i += 2)
        {
            if (i+1 < videoClips.Length)
            {
                VideoClip[] pair = new VideoClip[2];
                pair[0] = videoClips[i];
                pair[1] = videoClips[i + 1];
                paired.Add(pair);
            }
        }

        // �����ϰ� ����
        System.Random rng = new System.Random();
        paired = paired.OrderBy(pair => rng.Next()).ToList();

        // ��迭
        List<VideoClip> shuffled = new List<VideoClip>();

        foreach(var pair in paired)
        {
            shuffled.Add(pair[0]);
            shuffled.Add(pair[1]);
        }

        // �迭�� ��ȯ�ؼ� ����
        return shuffled.ToArray();
    }

    public static List<List<string>> ReadCSVFile(string fileName)
    {
        // CSV ������ �� �پ� �б����ؼ� TextAsset ���???
        TextAsset CSVFile = Resources.Load<TextAsset>(fileName);
        StringReader reader = new StringReader(CSVFile.text);

        List<List<string>> lsts = new List<List<string>>();

        string line;
        while ((line = reader.ReadLine()) != null)
        {
            // CSV ������ �� ������ ','���� �и��Ͽ� ���ڿ� �迭�� ����
            string[] values = line.Split(',');

            List<string> lineList = new List<string>(values);

            // List<List<string>>�� �� ���� List<string> �߰�
            lsts.Add(lineList);
        }

        // ���ҽ� ����
        Resources.UnloadAsset(CSVFile);

        return lsts;
    }

    string GetDirectoryPath()
    {
        return Application.dataPath + "/" + csvDirectoryName;
    }

    string GetFilePath()
    {
        return GetDirectoryPath() + "/" + csvFileName;
    }

    void VerifyDirectory()
    {
        string dir = GetDirectoryPath();
        
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    void VerifyFile()
    {
        string file = GetFilePath();

        if (!File.Exists(file))
        {
            Debug.Log("Fail to make file");
        }
    }

    public void CreateCSV()
    {
        VerifyDirectory();
        VerifyFile();

        using(StreamWriter sw = File.CreateText(GetFilePath()))
        {
            for (int i = 0; i < CombinationList.Count; i++)
            {
                List<int> sampleList = CombinationList[i];

                foreach (int numb in sampleList)
                {
                    sw.Write(numb);
                    sw.Write(",");
                }

                sw.Write("\r\n");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}