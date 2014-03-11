using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

	internal GameObject Player;
	internal GameObject PlatformCreatorObject;

	internal float PlatformSpeed;

	public bool CreatedPlatform = false ;

	internal playerManager ScriptplayerManger;
	internal PlatformCreator ScriptPlatformCreator;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindWithTag ("Player");
		ScriptplayerManger = Player.transform.GetComponent<playerManager>();
		PlatformCreatorObject = GameObject.FindWithTag ("PlatformCreator");
		ScriptPlatformCreator = PlatformCreatorObject.GetComponent<PlatformCreator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Player)
			PlatformSpeed = -1 * ScriptplayerManger.Speed;

		transform.Translate (Vector3.forward * PlatformSpeed , Space.World);

		if(transform.position.z < (PlatformCreatorObject.transform.position.z - ScriptPlatformCreator.PlatformLength * ScriptPlatformCreator.SectionLength) && CreatedPlatform == false )
		{

			ScriptPlatformCreator.CreatPlatform(ScriptPlatformCreator.NewPlatformCopy.position.z + ScriptPlatformCreator.PlatformLength * ScriptPlatformCreator.SectionLength);
			CreatedPlatform = true;
		}

		if(transform.position.z < PlatformCreatorObject.transform.position.z - ScriptPlatformCreator.PlatformLength * ScriptPlatformCreator.SectionLength - 0)
		{
			Destroy(gameObject);
		}
	}
}
