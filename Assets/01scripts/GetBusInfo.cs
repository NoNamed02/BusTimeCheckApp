using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;

public class GetBusInfo : MonoBehaviour
{
    private string apiKey;
    public string busStopId;

    public TextMeshProUGUI[] BusInfo;
    public TextMeshProUGUI[] RoutNum;

    void Awake()
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
    }

    void Start()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("BusApiData");
        if (textAsset != null)
        {
            apiKey = textAsset.text.Trim();
        }
        else
        {
            Debug.LogError("API Key file not found!");
            return;
        }
        StartCoroutine(GetBusArrivalInfo());
    }

    IEnumerator GetBusArrivalInfo()
    {
        string url = $"http://apis.data.go.kr/6270000/dbmsapi01/getRealtime?serviceKey={apiKey}&bsId={busStopId}";

        int BusNum = 0;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error fetching bus data: {request.error}");
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log($"Raw Bus Data: {jsonResponse}");

                var json = JSON.Parse(jsonResponse);
                var items = json["body"]["items"];

                if (items == null || items.Count == 0)
                {
                    Debug.Log("버스 도착 정보가 없습니다.");
                    yield break;
                }
                foreach (var route in items.Children)
                {
                    string routeNo = route["routeNo"];
                    Debug.Log($"{routeNo}번 버스");
                    RoutNum[BusNum].text = $"{routeNo}번 버스";

                    if (route["arrList"] != null && route["arrList"].Count > 0)
                    {
                        string arrState = route["arrList"][0]["arrState"];
                        Debug.Log($"도착 예정: {arrState}");
                        BusInfo[BusNum].text = $"도착 예정: {arrState}";
                    }
                    else
                    {
                        Debug.Log("도착 정보 없음");
                        BusInfo[BusNum].text = "도착 정보 없음";
                    }
                    BusNum++;
                }
            }
        }
    }
}
