## BusTimeCheckApp

**BusTimeCheckApp**은 특정 정류장의 실시간 버스 도착 정보를 확인할 수 있는 Unity 기반 앱입니다.  
공공 데이터를 활용하여 현재 정류장에 도착 예정인 버스 정보를 표시합니다.
<p align="center">
  <img src="https://github.com/user-attachments/assets/8e2e0dab-e307-436b-bee3-c2f2059e018c" width="400">
</p>

---

## 주요 기능

- 특정 버스 정류장의 실시간 도착 정보 조회
- 버스 번호 및 도착 예정 상태 표시
- 공공 데이터 API 활용
- 간단한 UI로 직관적인 정보 제공

---

## 기술 스택

- **Unity** (C#)
- **SimpleJSON** (JSON 파싱)
- **TextMeshPro** (UI 텍스트 출력)
- **UnityWebRequest** (HTTP 요청)
- **공공데이터포털 API** (실시간 버스 정보 제공)

---

## 핵심 코드 설명

```csharp
string url = $"http://apis.data.go.kr/6270000/dbmsapi01/getRealtime?serviceKey={apiKey}&bsId={busStopId}";
```
- 공공데이터포털 API를 호출하여 버스 정류장의 실시간 데이터를 가져옵니다.

```csharp
using (UnityWebRequest request = UnityWebRequest.Get(url))
{
    yield return request.SendWebRequest();
    if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
    {
        Debug.LogError($"Error fetching bus data: {request.error}");
    }
}
```
- API 요청을 보내고 응답을 받아 처리합니다.

```csharp
if (route["arrList"] != null && route["arrList"].Count > 0)
{
    string arrState = route["arrList"][0]["arrState"];
    BusInfo[BusNum].text = $"도착 예정: {arrState}";
}
```
- JSON 데이터를 파싱하여 버스 도착 정보를 UI에 표시합니다.

---

## 🔗 참고 자료

- [공공데이터포털 실시간 버스 API](https://www.data.go.kr/)
- [UnityWebRequest 문서](https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.html)
- [SimpleJSON 사용법](https://wiki.unity3d.com/index.php/SimpleJSON)

