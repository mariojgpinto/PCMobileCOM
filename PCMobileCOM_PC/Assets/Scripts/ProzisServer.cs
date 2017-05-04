using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProzisServer : MonoBehaviour {
	public UnityEngine.UI.Text textUI;
	float waitTime = 2f;
	SocketServer server = null;

	//List<KeyValuePair<int, int>> count = new List<KeyValuePair<int, int>>();
	// Use this for initialization
	void Start () {
		Log.AddToLog("Init Server");

		server = new SocketServer();
		server.StartServer();
	}
	
	// Update is called once per frame
	void Update () {
		if (server.infoReceived.Count > 0) {
			KeyValuePair<int, COMData> data = server.infoReceived.Dequeue();

			if (data.Value.type == COMData.TYPE.TEXT) {
				COMData_text text = (COMData_text)data.Value;

				//Log.AddToLog(data.Key + "|Message Received: " + text.GetText());

				string txt = text.GetText();
				textUI.text = txt;

				Log.AddToLog(txt);
				int count = 0;
				try {
					count = System.Convert.ToInt32(txt);
				}
				catch (System.Exception) {
					Debug.Log("ERROR ON MESSAGE - " + txt);
				}
				

				StartCoroutine(SendMenssage(data.Key, "" + (count + 1), waitTime));
				//StartCoroutine(SendMenssage("1", waitTime));
			}
		}
	}

	IEnumerator SendMenssage(int id, string txt, float delay) {
		yield return new WaitForSeconds(delay);

		server.SendMessage(id, txt);
	}

	void OnApplicationQuit() {
		Debug.Log("ApplicationQuit");
		if (server != null) {
			server.Close();
		}
		Debug.Log("ApplicationQuit agora a serio");
	}
}
