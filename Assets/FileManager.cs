using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public static List<GameObject> GetChildrenList(GameObject parent) //Children 정보 받아오기
    {
        List<GameObject> childrenList = new List<GameObject>();

        foreach (Transform childTransform in parent.transform)
        {
            GameObject childGameObject = childTransform.gameObject;
            childrenList.Add(childGameObject);
        }

        return childrenList;
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
