using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace _01_Works.HS.Code.Test
{
    public class Test : MonoBehaviour
    {
        string url = 
            "https://script.google.com/macros/s/AKfycbxye8ZGDCeM-g5MntYYu1KeHiXYth-xzzwWXA1yzBqFO3X6Q3OyO_ZibIa7Xt-XGzUChg/exec";

        void Start()
        {
            StartCoroutine(LoadCSV());
        }

        IEnumerator LoadCSV()
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    string csvText = www.downloadHandler.text;
                    Debug.Log(csvText);
                    // 여기서 csvText를 직접 파싱해서 배열/딕셔너리로 변환하면 됨
                }
            }
        }
    }
}