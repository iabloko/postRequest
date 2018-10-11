using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class textOut : MonoBehaviour {

	public Text _Text;

	void Start () {
		//StartCoroutine(Upload());
	}
	void Update () {
		if (Input.GetKeyUp (KeyCode.Q))
			StartCoroutine (POST ());
		if (Input.GetKeyUp (KeyCode.E))
			StartCoroutine (GET ());
	}

	public IEnumerator POST () {
		Debug.Log("Post_");
		var Data = new WWWForm ();
		Data.AddField ("«action»:", "check");
		Data.AddField ("«deviceName»:", "north_entrance");
		Data.AddField ("barcode", "1234560000123");

		var Query = new WWW ("vk.com", Data);
		yield return Query;
		if (Query.error != null) {
			Debug.Log ("Server does not respond : " + Query.error);
		} else {
			if (Query.text == "Testss") {//ответ сервера
				Debug.Log ("Server responded correctly");
			} else {
				Debug.Log ("Server responded : " + Query.text);
			}
		}
		Query.Dispose ();
	}

	public IEnumerator GET () {
		Debug.Log("_GET");
		string data1 = "Текст 1";
		string data2 = "Текст 2";
		WWW Query = new WWW ("vk.com"); //+ data1 + "&variable2=" + data2);
		yield return Query;
		if (Query.error != null) {
			Debug.Log ("Server does not respond : " + Query.error);
		} else {
			if (Query.text == "Test") {
				Debug.Log ("Server responded correctly");
			} else {
				Debug.Log ("Server responded : " + Query.text);
			}
		}
		Query.Dispose ();
	}
}