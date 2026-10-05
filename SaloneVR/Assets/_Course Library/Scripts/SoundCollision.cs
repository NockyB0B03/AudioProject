using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollision : MonoBehaviour {
	public bool DebugTag;
	public bool DebugVelocity;
	public FMODUnity.EventReference CollisionSoundEvent;
	public string TagParameterName = "TagMaterial";
	public string VelocityParameterName= "Velocity";

	public float MaxCollisionForce=20;
	//public AudioSource CollisionAudioSource;
	//public AudioClip[] CollisionRandomSounds;
	private float VelNorm;
	private FMOD.Studio.EventInstance CollInstance;



	[TagSelector] public string[] CollisionTag;





	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision Col)
	{
		if (DebugTag) Debug.Log(Col.gameObject.tag);		
		VelNorm = Mathf.InverseLerp(0f, MaxCollisionForce, Col.relativeVelocity.magnitude)*100f;
		if (DebugVelocity) Debug.Log("Velocity: " + Col.relativeVelocity.magnitude + " Parameter: " + VelNorm);

		//CollisionInstance = FMODUnity.RuntimeManager.CreateInstance (CollisionEvent);
		//CollisionInstance.setParameterByName ("Collision", Col.relativeVelocity.magnitude);

		for (int i = 0; i < CollisionTag.Length; i++)
		{
			if (Col.gameObject.tag == CollisionTag[i])
			{
				if (!CollisionSoundEvent.IsNull)
				{
					CollInstance = FMODUnity.RuntimeManager.CreateInstance(CollisionSoundEvent);
					CollInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
					CollInstance.setParameterByName(TagParameterName, (float)i);
					CollInstance.setParameterByName(VelocityParameterName, VelNorm);
					CollInstance.start();
					CollInstance.release();
				}
			}
		}

	}
}
