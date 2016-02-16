using UnityEngine;
using System.Collections;

/*
 *  a small class to respresent looking states
 *  a looking state is a setting for the parameters
 *  of the lookat component, making it easy to
 *  transition to new looking behaviour
 *  (should probably just be replaced with
 *   mecanim state machine scripting)
 */
[System.Serializable]
public class GazeCondition
{
	public string name;                 // name of the state for display in the editor
	public Transform lookAtTarget;      // where to look at
	public bool startLooking = true;    // whether to look straight away when entering the state
	public float lookAtTime = 0.5f;     // how long to look for
	public float lookAwayTime = 0.3f;   // how long to look away for
	public float headLookProb = 1.0f;   // probability of turning the head
	public float eyeLookProb = 1.0f;    // probability of turning the eyes
}

public class LookAt : MonoBehaviour {

	public GazeCondition [] gazeConditions;
	public GazeCondition currentGazeCondition;

	private Animator anim;                // the animator we are controlling
	public Transform lookAtTarget;        // the current target to look at
	public float transitionSpeed = 1f;    // how fast to transition between looking at different targets (in seconds)
    

	public float currentHeadWeight = 0.0f;  // how much the head is turned to the target
	public float currentEyeWeight = 0.0f;   // how much the eyes are turned to the target
    public float targetHeadWeight = 0.0f;   // the final target for how much the head is turned to the target
    public float targetEyeWeight = 0.0f;    // the final target for how much the eyes are turned to the target


    public float timer = 0.0f;    // a timer to know when to stop looking
	public bool looking = false;  // whether we are looking at the moment
	public Vector3 lookAtPos;     // where we are looking at 

	public Transform eye;         // a transform for one of the character's eyes

	public string initialCondition; // which gaze condition we start in


	// Use this for initialization
	void Start () {
		
		anim = GetComponent<Animator>(); 
		
		timer = 0;
		LookAtCondition(initialCondition);
    }

	 void Update()
     {
        // update the timer each frame
     	timer -= Time.deltaTime;
     }
	
	void OnAnimatorIK (){
        // when the timer reaches 0 we switch from looking to not looking
        // and vice versa
		if(timer <= 0f)
		{
			if(looking)
			{
                // set to not looking

                // reset the timer
				timer = currentGazeCondition.lookAwayTime;
				
                // set the weights to 0 so the character is not looking
				targetHeadWeight = 0;
				targetEyeWeight = 0;
				looking = false;
			}
			else
			{
                // set the head and eye weights to looking with 
                // probabilities given by the current gaze condition
				targetHeadWeight = 0;
				if(Random.value < currentGazeCondition.headLookProb){
					targetHeadWeight =  1.0f;
				}
				targetEyeWeight = 0;
				if(Random.value < currentGazeCondition.eyeLookProb){
					targetEyeWeight =  1.0f;
				}
				
				// reset the timer
				timer = currentGazeCondition.lookAtTime;
				looking = true;
			}
		}
		
        // interpolate the look at position from the current position to the one we should be looking at
        // this ensures a smooth transition
		lookAtPos = Vector3.Lerp(lookAtPos, currentGazeCondition.lookAtTarget.position, transitionSpeed * Time.deltaTime);	
		
        // set the direction from the eye
		Vector3 direction = eye.InverseTransformPoint(lookAtPos);

		// interpolate the head and eye weights to ensure a smooth transition
		currentHeadWeight = Mathf.Lerp(currentHeadWeight, targetHeadWeight, transitionSpeed * Time.deltaTime);
		currentEyeWeight = Mathf.Lerp(currentEyeWeight, targetEyeWeight, transitionSpeed * Time.deltaTime);
		
        // set the look at weight and position in the animator
		anim.SetLookAtWeight(1.0f,0.0f,currentHeadWeight,currentEyeWeight,0.0f);
        anim.SetLookAtPosition (lookAtPos);
    }		

    // set a new look at condition
    void LookAtCondition(string condition){
    	
    	for(int i = 0; i < gazeConditions.Length; i++){
    		if(condition == gazeConditions[i].name){
    			currentGazeCondition = gazeConditions[i];
	    		timer = 0f;
	    		// set looking to not startlooking because it
	    		// will be flipped immediately on the next frame
	    		looking = !currentGazeCondition.startLooking;
    		}
    	}
        
    }
}
