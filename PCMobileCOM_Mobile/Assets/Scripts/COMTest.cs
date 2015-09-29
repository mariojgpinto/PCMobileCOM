using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class COMTest : MonoBehaviour {
	SocketClient client;

	Texture2D textureTemp = null;
	WebCamTexture webCam = null;
	byte[] imageConverted = new byte[10000000];

	#region BUTTON_CALLBACKS
	public void OnButtonPressed(int id){
		switch(id){
		case 0 :
			Application.Quit();
			break;
		case 1 :
			
			break;
		case 2 :
			string txt = GameObject.Find("InputField").GetComponent<InputField>().text;
			
			if(txt != ""){
				client.SendInfo_text(txt);
			}
			break;
		case 3 :
			Debug.Log("IMAGE");

			if(webCam == null)
				webCam = GameObject.Find("Main Camera").GetComponent<WebCam>().cam;

			if(textureTemp == null)
				textureTemp = new Texture2D(webCam.width, webCam.height);

			textureTemp.SetPixels32(webCam.GetPixels32());



//			Debug.Log ((GameObject.Find("RawImageCam").GetComponent<RawImage>().mainTexture).width + "," + (GameObject.Find("RawImageCam").GetComponent<RawImage>().mainTexture).height);
//			IntPtr pointer = webCam.GetNativeTexturePtr();
//			textureTemp.UpdateExternalTexture(GameObject.Find("RawImageCam").GetComponent<RawImage>().mainTexture.GetNativeTexturePtr());
//			textureTemp.Apply();

			imageConverted = textureTemp.EncodeToPNG();

			client.SendInfo_image(imageConverted, textureTemp.width, textureTemp.height);

			break;
		default: break;
		}
	}
	#endregion

	// Use this for initialization
	void Start () {
		client = new SocketClient("192.168.21.83");

		client.TryToConnect();
	}
	
	// Update is called once per frame
	void Update () {
		if(client.infoReceived.Count > 0){
			COMData data = client.infoReceived.Dequeue();

			if(data.type == COMData.TYPE.TEXT){
				COMData_text text = (COMData_text)data;

				Log.AddToLog("Message Received: " + text.GetText());
			}
			else
			if(data.type == COMData.TYPE.IMAGE){
				COMData_image image = (COMData_image)data;

				Texture2D texture = new Texture2D(image.imageWidth, image.imageHeight);
				texture.LoadImage(image.data);
				texture.Apply();

				Log.AddToLog("Image Received: " + texture.width + " x " + texture.height);

				GameObject.Find("RawImage").GetComponent<RawImage>().texture = texture;
			}
			else
			if(data.type == COMData.TYPE.AUDIO){
				
			}
		}
	}

	void OnApplicationQuit(){
		Debug.Log("OnApplicationQuit");
		client.Close();
		Debug.Log("OnApplicationQuit agora a serio");
	}
}
