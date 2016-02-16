using UnityEngine;
using System.Collections;

[System.Serializable]
public class GazeCondition
{
	public string name;
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

	private Animator anim;
	public Transform lookAtTarget;
	public float transitionSpeed = 1f;
    

	public float currentHeadWeight = 0.0f;
	public float currentEyeWeight = 0.0f;
	public float targetHeadWeight = 0.0f;
	public float targetEyeWeight = 0.0f;
    

	public float timer = 0.0f;
	public bool looking = false;
	public Vector3 lookAtPos;

	public Transform eye;

	public string initialCondition;


	// Use this for initialization
	void Start () {
		
		anim = GetComponent<Animator>();
		
		timer = 0;
		LookAtCondition(initialCondition);
		

	}

	 void Update()
     {
     	timer -= Time.deltaTime;
     }
	
	void OnAnimatorIK (){
		if(timer <= 0f)
		{
			if(looking)
			{
				timer = currentGazeCondition.lookAwayTime;
				
				targetHeadWeight = 0;
				targetEyeWeight = 0;
				looking = false;
			}
			else
			{

				
				targetHeadWeight = 0;
				if(Random.value < currentGazeCondition.headLookProb){
					targetHeadWeight =  1.0f;
				}
				targetEyeWeight = 0;
				if(Random.value < currentGazeCondition.eyeLookProb){
					targetEyeWeight =  1.0f;
				}
				
				
				timer = currentGazeCondition.lookAtTime;
				looking = true;
			}
		}
		


		lookAtPos = Vector3.Lerp(lookAtPos, currentGazeCondition.lookAtTarget.position, transitionSpeed * Time.deltaTime);	
		
		Vector3 direction = eye.InverseTransformPoint(lookAtPos);

		
		currentHeadWeight = Mathf.Lerp(currentHeadWeight, targetHeadWeight, transitionSpeed * Time.deltaTime);
		currentEyeWeight = Mathf.Lerp(currentEyeWeight, targetEyeWeight, transitionSpeed * Time.deltaTime);
		

		anim.SetLookAtWeight(1.0f,0.0f,currentHeadWeight,currentEyeWeight,0.0f);
		
        anim.SetLookAtPosition (lookAtPos);
    }		

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
