using System.Collections;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class textOut : MonoBehaviour {
	private string JsonDataString;
	public Text _Text;
	public GameObject _IPOBJ;
	public string _url;
	[HideInInspector] public bool MenuActive = false;

	void Awake () {
		_url = PlayerPrefs.GetString ("_url", _url);
	}

	void Start () {
		_IPOBJ.SetActive (false);
	}
	void Update () {
		if (Input.GetKeyUp (KeyCode.Q))
			StartCoroutine (POST ());
		if (Input.GetKeyUp (KeyCode.E))
			StartCoroutine (GET ());
		if (Input.GetKeyUp (KeyCode.W))
			StartCoroutine (_UnityWebRequest ());
		if (Input.GetKeyUp (KeyCode.Tab))
			_MenuActive (_IPOBJ);
	}

	public IEnumerator POST () {
		_Text.text = "Post";
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
		_Text.text = "GET";
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
		_Text.text = "_UnityWebRequest";
		using (UnityWebRequest www = UnityWebRequest.Get (_url)) {
			yield return www.SendWebRequest ();

			if (www.isNetworkError || www.isHttpError) {
				Debug.Log (www.error);
			} else {
				//_Text.text = www.downloadHandler.text;
				byte[] results = www.downloadHandler.data;
				JSONNode jsonNode = SimpleJSON.JSON.Parse (www.downloadHandler.text);
				_Text.text = jsonNode["country"].ToString ().ToUpper ();
			}
			www.Dispose ();
		}
	}

	private void _MenuActive (GameObject _InputField) {

		if (_InputField.activeInHierarchy == false)
			_IPOBJ.SetActive (true);
		else
			_IPOBJ.SetActive (false);
	}

	public void SaveChange () {
		InputField _IFfield = _IPOBJ.GetComponent<InputField> () as InputField;
		_url = _IFfield.text;

		PlayerPrefs.SetString ("_url", _IFfield.text);
		Debug.Log ("IPField.text" + "              " + _url);
	}
}