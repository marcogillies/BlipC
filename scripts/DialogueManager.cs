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
	public string text;      // text for triggering via speech or text input (can include regular expression syntax)
	public string key;       // key for keyboard control
	public Regex regex;      // a regular expression created from the text
	public string message;   // the message to send (i.e the name of the function to call on another component)
	public string val;       // the value to pass to that function
}

public class DialogueManager : MonoBehaviour {

	public DialogueResponse [] responses;
	private string inputString = "";


	// Use this for initialization
	void Start () {
        // create regular expressions for all of the responses
        // so they can be recognised from text
		for(int i = 0; i < responses.Length; i++){
			responses[i].regex = new Regex(responses[i].text);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// check if they keyboard key associated with a response has been 
        // pressed 
		for(int i = 0; i < responses.Length; i++){
		if (Input.GetKeyDown (responses[i].key)){
				doResponse(i);
			}
		}
	}

    // trigger a response by number
	void doResponse(int i){
		DialogueResponse response = responses[i];
		//Debug.Log(response.val);
		gameObject.SendMessage(response.message, response.val);
	}

    // respond to a speech input 
    // (which is recognised as a regex)
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

    // some GUI code if you want to use for quick debugging
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
