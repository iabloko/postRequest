using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MyBehavior : MonoBehaviour {
	public string _url;
	
	void Update () {
		if (Input.GetKeyUp (KeyCode.Q))
			StartCoroutine (_WebRequest ());
		if (Input.GetKeyUp (KeyCode.E))
			StartCoroutine (_WWWForm ());
	}

	IEnumerator _WebRequest () {
		List<IMultipartFormSection> formData = new List<IMultipartFormSection> ();
		formData.Add (new MultipartFormDataSection ("field1=foo&field2=bar"));
		formData.Add (new MultipartFormFileSection ("my file data", "myfile.txt"));

		UnityWebRequest www = UnityWebRequest.Post (_url, formData);
		yield return www.SendWebRequest ();

		if (www.isNetworkError || www.isHttpError) {
			Debug.Log (www.error);
		} else {
			Debug.Log ("Form upload complete!" + "       ");
		}
		www.Dispose ();
	}

	IEnumerator _WWWForm () {
		WWWForm form = new WWWForm ();
		form.AddField ("action", "ssss");
		form.AddField ("deviceName", "north_entrance");
		form.AddField ("param1", "param1value");

		UnityWebRequest www = UnityWebRequest.Post (_url, form);
		yield return www.SendWebRequest ();

		if (www.isNetworkError || www.isHttpError) {
			Debug.Log (www.error);
		} else {
			Debug.Log ("Form upload complete!");
		}
		www.Dispose ();
	}
}