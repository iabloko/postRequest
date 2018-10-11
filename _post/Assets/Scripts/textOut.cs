using System.Collections;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class textOut : MonoBehaviour {
	private string JsonDataString;
	public Text _Text;
	public string _url;

	void Start () {

	}
	void Update () {
		if (Input.GetKeyUp (KeyCode.Q))
			StartCoroutine (POST ());
		if (Input.GetKeyUp (KeyCode.E))
			StartCoroutine (GET ());
		if (Input.GetKeyUp (KeyCode.W))
			StartCoroutine (_UnityWebRequest ());

	}

	public IEnumerator POST () {
		Debug.Log ("Post");
		var Data = new WWWForm ();
		Data.AddField ("action", "check");
		Data.AddField ("deviceName", "north_entrance");
		Data.AddField ("param1", "param1value");

		var Query = new WWW (_url, Data);
		yield return Query;
		if (Query.error != null) {
			Debug.Log ("Server does not respond : " + Query.error);
		} else {
			if (Query.text == "Testss") { //ответ сервера
				Debug.Log ("Server responded correctly");
			} else {
				Debug.Log ("Server responded : " + Query.text);
				JSONNode jsonNode = SimpleJSON.JSON.Parse (Query.text);
				_Text.text = jsonNode["country"].ToString ().ToUpper ();
			}
		}
		Query.Dispose ();
	}

	public IEnumerator GET () {
		Debug.Log ("Get");
		//string data1 = "Текст 1";
		//string data2 = "Текст 2";
		WWW Query = new WWW (_url); //+ data1 + "&variable2=" + data2);
		yield return Query;
		if (Query.error != null) {
			Debug.Log ("Server does not respond : " + Query.error);
		} else {
			if (Query.text == "Test") {
				Debug.Log ("Server responded correctly");
			} else {
				Debug.Log ("Server responded : " + Query.text);
				JSONNode jsonNode = SimpleJSON.JSON.Parse (Query.text);
				_Text.text = jsonNode["country"].ToString ().ToUpper ();
			}
		}
		Query.Dispose ();
	}

	IEnumerator _UnityWebRequest () {
		Debug.Log ("_UnityWebRequest");
		using (UnityWebRequest www = UnityWebRequest.Get (_url)) {
			yield return www.SendWebRequest ();

			if (www.isNetworkError || www.isHttpError) {
				Debug.Log (www.error);
			} else {
				Debug.Log (www.downloadHandler.text);
				_Text.text = www.downloadHandler.text;
				byte[] results = www.downloadHandler.data;
			}
			www.Dispose ();
		}
	}
}