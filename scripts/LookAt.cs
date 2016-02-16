using UnityEngine;
using System.Collections;

[System.Serializable]
public class GazeCondition
{
	public string name;
	public float threshold;
	public Transform lookAtTarget;
	public bool startLooking = true;
	public float lookAtTime = 0.5f;
	public float lookAwayTime = 0.3f;
	public float headLookProb = 1.0f;
	public float eyeLookProb = 1.0f;
}

public class LookAt : MonoBehaviour {

	public GazeCondition [] gazeConditions;
	public GazeCondition currentGazeCondition;
	//public PatientModel patientModel;

	private Animator anim;
	public Transform lookAtTarget;
	public Transform [] lookAwayTargets;
	//public float lookAtTime = 0.5f;
	//public float lookAwayTime = 0.3f;
	public float transitionSpeed = 1f;

	//public float headLookProb = 1.0f;
	//public float eyeLookProb = 1.0f;

	public float currentHeadWeight = 0.0f;
	public float currentEyeWeight = 0.0f;
	public float targetHeadWeight = 0.0f;
	public float targetEyeWeight = 0.0f;

	public Transform head;

	public float timer = 0.0f;
	public bool looking = false;
	//public Transform target;
	public Vector3 lookAtPos;

	public Transform eye;

	public string initialCondition;

	// hwd working
	// not elegant but either tracks the face or looks sideways for the GUi
	public bool trackface = true;
	


	// hwd working
	//public float timeLook = 6.35f;
	//public bool allowLook = false;


	// Use this for initialization
	void Start () {
		
		anim = GetComponent<Animator>();
		// hwd set to full tracking for the initial demo
	//	currentGazeCondition = gazeConditions[2];
		//int i = (int)Random.Range(0, lookAwayTargets.Length);
		//target = lookAwayTargets[i];
		//target = lookAtTarget;
		
		// hwd changed from the line below to rotate brain when swipe dissolving 
		// lookAtPos = lookAtTarget.position;
		
		//lookAtPos = lookAwayTargets[0].position; // lookat users face first

		// Debug.Log("In start: lookAtPos is "+ lookAtTarget.position);
		timer = 0;
		// hwd set the initial loook at behaviour
		LookAtCondition(initialCondition);
		// hwd workingout how to talk to the dialouge manager
	
		

	}

	 void Update()
     {
     	timer -= Time.deltaTime;
     	//Debug.Log("trackface: " + trackface);
         //timeLook -= Time.deltaTime;
         //Debug.Log("timeLook is: " + timeLook + " allowLook is: "+ allowLook);
         //Debug.Log("position " + head.rotation);
         //if(timeLook < 0)
         //{
         //    allowLook = true;
         //}
         //head.LookAt(target.position);
     }
	
	void OnAnimatorIK (){
		//Debug.Log("in animator IK");
		//Debug.Log(timer);
		if(timer <= 0f)
		{
			if(looking)
			{
				//int i = (int)Random.Range(0, lookAwayTargets.Length);
				//target = lookAwayTargets[i];
				timer = currentGazeCondition.lookAwayTime;
				//Debug.Log("weights " + 0 + " " + 0);
				// global, body, head, eyes, clamp (where 0 is unrestrained / 1 is full clamp)
				targetHeadWeight = 0;
				targetEyeWeight = 0;
				looking = false;
				// Debug.Log("hwd 1 "+ lookAtTarget.position + targetHeadWeight + targetEyeWeight + looking);
			}
			else
			{

				//lookAtPos = Vector3.Lerp(lookAtPos, lookAtTarget.position, transitionSpeed * Time.deltaTime);
				
				targetHeadWeight = 0;
				if(Random.value < currentGazeCondition.headLookProb){
					targetHeadWeight =  1.0f;
				}
				targetEyeWeight = 0;
				if(Random.value < currentGazeCondition.eyeLookProb){
					targetEyeWeight =  1.0f;
				}
				
				// global, body, head, eyes, clamp (where 0 is unrestrained / 1 is full clamp)
				

				//target = lookAtTarget;
				timer = currentGazeCondition.lookAtTime;
				// Debug.Log("hwd 2 "+ timer);
				looking = true;
			}
		}
		
		// lookAtPos = Vector3.Lerp(lookAtPos, lookAtTarget.position, transitionSpeed * Time.deltaTime);
		// hwd changed from above line to allow smooth brain rotation

    	// if(condition == "GUI"){
    	// Debug.Log("tuesGUI");
    		
    	//	} 		
    	//if(condition == "full_tracking"){
    	//	Debug.Log("tuesfull_tracking");
    	//	} 
    	/// hwd to do just need to get current gaze conditionn in a boolean
		// if (currentGazeCondition = "GUI"){
		// Debug.Log("tues: " + GUI);
		// }	
		// hwd look at face
		//if (trackface == true){
		//	lookAtPos = Vector3.Lerp(lookAtPos, lookAwayTargets[0].position, transitionSpeed * Time.deltaTime);	
		//	}

		// hwd look at GUI
		//if (trackface == false){
		//	lookAtPos = Vector3.Lerp(lookAtPos, lookAwayTargets[1].position, transitionSpeed * Time.deltaTime);	
		//	}

		lookAtPos = Vector3.Lerp(lookAtPos, currentGazeCondition.lookAtTarget.position, transitionSpeed * Time.deltaTime);	
		
		//Debug.Log(lookAtPos);
		//Debug.Log(lookAtTarget.position);
		
		//Vector3 direction = lookAtPos - eye.position;
		Vector3 direction = eye.InverseTransformPoint(lookAtPos);

		
		currentHeadWeight = Mathf.Lerp(currentHeadWeight, targetHeadWeight, transitionSpeed * Time.deltaTime);
		currentEyeWeight = Mathf.Lerp(currentEyeWeight, targetEyeWeight, transitionSpeed * Time.deltaTime);
		
		//Debug.Log("weights " + currentHeadWeight + " " + currentEyeWeight);
		

		anim.SetLookAtWeight(1.0f,0.0f,currentHeadWeight,currentEyeWeight,0.0f);
		//if (allowLook == true)
		//	{

				//Debug.Log("lookatpos " + lookAtPos);
				//Debug.Log("lookatpos " + lookAtPos);
        anim.SetLookAtPosition (lookAtPos);
        		// anim.SetLookAtWeight(1.0f,1.0f,1.0f,1.0f,0.0f); // global, body, head, eyes, clamp (where 0 is unrestrained / 1 is full clamp)
 				//anim.SetLookAtWeight(0.5f,0.0f,1.0f,1.0f,0.0f); // global, body, head, eyes, clamp (where 0 is unrestrained / 1 is full clamp)
 
 	   	//	}
    }		

    void LookAtCondition(string condition){
    	Debug.Log("looking at " + condition);
		if (condition == "full_tracking"){ // look at user
				trackface = true;
			}
		
		if (condition == "GUI"){ // look left to show brain
				trackface = false; 
			}	
		
	
    	for(int i = 0; i < gazeConditions.Length; i++){
    		if(condition == gazeConditions[i].name){
    			currentGazeCondition = gazeConditions[i];
	    		timer = 0f;
	    		// set looking to not startlooking because it
	    		// will be flipped immediately on the next frame
	    		looking = !currentGazeCondition.startLooking;
    		}
    	}

    	/*
    	if(condition == "neutral"){
    		if(patientModel.level_1a_LOC_responsiveness < 0.5){
    			currentGazeCondition = gazeConditions[1];
    		} else {
    			currentGazeCondition = gazeConditions[0];
    		}
    	} else if(condition == "listening"){
    		if(patientModel.level_1a_LOC_responsiveness < 1.5){
    			currentGazeCondition = gazeConditions[1];
    		} else {
    			currentGazeCondition = gazeConditions[0];
    		}
    	}  else if(condition == "physicalStimulus"){
    		if(patientModel.level_1a_LOC_responsiveness < 2.5){
    			currentGazeCondition = gazeConditions[1];
    		} else {
    			currentGazeCondition = gazeConditions[0];
    		}
    	} else if(condition == "full_tracking"){
    		Debug.Log("full tracking");
    		if(patientModel.level_1a_LOC_responsiveness < 1.5){
    			currentGazeCondition = gazeConditions[2];
    		} else {
    			currentGazeCondition = gazeConditions[0];
    		}
    	}
		*/

    	//for(int i = 0; i < gazeConditions.Length; i++){
		//	if(gazeConditions[i].name == condition){
		//		currentGazeCondition = gazeConditions[i];
		//		timer = 0;
		//	}
		//}
    }
}
