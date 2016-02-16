using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

//[System.Serializable]
//public class ResponseOption
//{
//	public float val;
//	public string action;
//}

/*
 *  A class for representing possible dialogue items 
 *  and responses to them
 */
[System.Serializable]
public class DialogueResponse
{
	public string text;
	public string key;
	public Regex regex;
	public string message;
	public string val;
}

public class DialogueManager : MonoBehaviour {

	public DialogueResponse [] responses;
	private string inputString = "";


	// Use this for initialization
	void Start () {
		for(int i = 0; i < responses.Length; i++){
			responses[i].regex = new Regex(responses[i].text);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log("dialogue manager");
		for(int i = 0; i < responses.Length; i++){
		if (Input.GetKeyDown (responses[i].key)){
				doResponse(i);
			}
		}
	}

	void doResponse(int i){
		DialogueResponse response = responses[i];
		//Debug.Log(response.val);
		gameObject.SendMessage(response.message, response.val);
	}

	public bool SpeechInput(string text)
	{	
		for(int i = 0; i < responses.Length; i++){
			if(responses[i].regex.IsMatch(text)){
				Debug.Log(responses[i].text);
				inputString = responses[i].text;
				doResponse(i);
				return true;
			}
		}
		return false;
	}

	// void OnGUI(){
	// 	string newText = GUI.TextField (new Rect (10, 10, 200, 20), inputString, 25);
	// 	if(newText != inputString){
	// 		//Debug.Log(newText);
	// 		inputString = newText;
	// 		if(SpeechInput(newText)){
	// 			inputString = "";
	// 		}
	// 	}
	// }


}
