using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Log : MonoBehaviour {
	#region VARIABLES
	public static Log instance;
	
	public Text logText;
	Queue<string> newText = new Queue<string>();
	#endregion
	
	public static void AddToLog(string text){
		instance.newText.Enqueue(text);
		//		if(instance.logText != null){
		//			instance.logText.text = 
		//				"[" + System.DateTime.Now.ToString("yyyy/mm/dd HH:mm:ss") + "] "  + 
		//			   	text +
		//			   	"\n" + 
		//			   	instance.logText.text;
		//		}
	}

	public static void AddToDebug(string text){
		Debug.Log(text);
	}
	
	#region UNITY_CALLBACKS
	// Use this for initialization
	void Awake () {
		instance = this;
	}

	int limit = 50;
	
	void FixedUpdate(){
		if(newText.Count != 0){
			if(instance.logText != null){
				while(newText.Count != 0){
					logText.text = 
						"[" + System.DateTime.Now.ToString("yyyy/mm/dd HH:mm:ss") + "] "  + 
							newText.Dequeue() +
							"\n" + 
							logText.text;

					string[] logs = logText.text.Split(new string[]{"\n"}, System.StringSplitOptions.RemoveEmptyEntries);
					if(logs.Length > limit){
						string t = "";
						for(int i = 0; i < limit ; ++i){
							t+=logs[i] + "\n";
						}

						logText.text = t;
					}
				}
			}
		}
	}
	#endregion
}
