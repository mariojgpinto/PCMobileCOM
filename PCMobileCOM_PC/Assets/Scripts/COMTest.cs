using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class COMTest : MonoBehaviour {
	SocketServer server = null;

	public Texture2D imageToSend;

	#region BUTTON_CALLBACKS
	public void OnButtonPressed(int id){
		switch(id){
		case 1 :
			string txt = GameObject.Find("Input_SendText").GetComponent<InputField>().text;
			
			if(txt != ""){
				Log.AddToLog("Send Message: " + txt);
				server.BroadcastMessage(txt);
			}
			break;
		case 2 :
			Debug.Log("Send Image");

			byte[] textureBytes = imageToSend.EncodeToPNG();

			server.BroadcastImage(textureBytes, imageToSend.width, imageToSend.height);

			break;
		case 100 :
			//			ThreadedServer.CloseAllConnections();
			break;
		case 101 :
			Application.Quit();
			break;
		default: break;
		}
	}
	#endregion

	// Use this for initialization
	void Start () {
		server = new SocketServer();
		server.StartServer();
	}
	
	// Update is called once per frame
	void Update () {
		if(server.infoReceived.Count > 0){
			KeyValuePair<int, COMData> data = server.infoReceived.Dequeue();
			
			if(data.Value.type == COMData.TYPE.TEXT){
				COMData_text text = (COMData_text)data.Value;
				
				Log.AddToLog(data.Key + "|Message Received: " + text.GetText());
			}
			else
			if(data.Value.type == COMData.TYPE.IMAGE){
				COMData_image image = (COMData_image)data.Value;
				
				Texture2D texture = new Texture2D(image.imageWidth, image.imageHeight);
				texture.LoadImage(image.data);
				texture.Apply();

				Log.AddToLog("Image Received: " + texture.width + " x " + texture.height);
				
				GameObject.Find("RawImage").GetComponent<RawImage>().texture = texture;
			}
			else
			if(data.Value.type == COMData.TYPE.AUDIO){

			}
		}
	}

	void OnApplicationQuit(){
		Debug.Log("ApplicationQuit");
		if(server != null){
			server.Close();
		}
		Debug.Log("ApplicationQuit agora a serio");
	}
}
