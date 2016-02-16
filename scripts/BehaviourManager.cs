using UnityEngine;
using System.Collections;

[System.Serializable]
public class Utterance
{
	public string name;
	public string message;
	public string animation;
	public float  speechThreshold;
	public AudioClip audioClip;
}

public class BehaviourManager : MonoBehaviour {

	public Utterance [] utterances;
	public Animator anim;
	public GameObject [] GUIprops;

	public PatientModel patientModel;
	//public float speechThreshold = 0.3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void PlayUtterance(string val){
		Debug.Log("Play Utterance " + val);
		for(int i = 0; i < utterances.Length; i++){
			if(utterances[i].name == val){
				Debug.Log(utterances[i].message);
				if (utterances[i].animation != ""){
					Debug.Log("animation " + utterances[i].animation);
					anim.SetTrigger(utterances[i].animation);
				}
				if (patientModel.level_1b_LOC_answer_verbal <= utterances[i].speechThreshold 
					 &&  utterances[i].audioClip != null){
					GetComponent<AudioSource>().clip= utterances[i].audioClip;
					GetComponent<AudioSource>().Play();
				}
				return;
			}
		}
	}
}
