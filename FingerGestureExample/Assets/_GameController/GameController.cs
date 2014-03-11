using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	internal GameObject Player;
	internal GameObject PlatformCreatorObject;
	internal PlatformCreator PlatformCreatorScript;

	public int CurrentLevel = 1;
	public int DistanceToLevelUp = 100;

	public string[] LevelUpText;
	internal string CurrentLevelUpText;
	internal bool LevelUp = false;

//	public int LevelUpRumble = 200 ;//shake screen

	public float MaxSpeedChange = 0.1f;
//	public float PlatformWidthChange;
//	public float PlatformLengthChange;
	public float PriceRateChange = 0.05f;
	public float ObstacleRateChange = 0.05f;

	public GUISkin GUIskin;
	public Texture2D Prices;

	internal float LevelUpPosX = -Screen.width;

	internal float TotalDistance = 0;
	internal float LastLevelDistance = 0;
	internal float TotalPrices = 0;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindWithTag ("Player");
		PlatformCreatorObject = GameObject.FindWithTag ("PlatformCreator");
		PlatformCreatorScript = PlatformCreatorObject.GetComponent<PlatformCreator>();

	}
	
	// Update is called once per frame
	void Update () {
		if (LevelUp == false && TotalDistance >= DistanceToLevelUp * CurrentLevel + LastLevelDistance) 
		{
			LevelUp = true;

			LastLevelDistance = TotalDistance;

			CurrentLevel ++;
			LevelUpPosX = Screen.width;
			CurrentLevelUpText = LevelUpText[Random.Range(0,LevelUpText.Length)];

//			Camera.main.GetComponent<Shake>().shake = LevelUpRumble;

			PlatformCreatorScript.PriceObjectRate += PriceRateChange;
			PlatformCreatorScript.ObstacleRate += ObstacleRateChange;

		}

			if(Input.GetKey(KeyCode.Escape)){
				Application.Quit();
			}
	}

	public void EndLevel(){
		PlayerPrefs.SetInt ("TotalDistance" , (int) TotalDistance);
		PlayerPrefs.SetInt ("TotalPrices" , (int) TotalPrices);

//		StartCoroutine (WaitAndDelay(2.0f));

//		Application.LoadLevel ("end");
	}

//	IEnumerator WaitAndDelay(float waitTime){
//		yield return WaitForSeconds (waitTime);
//	}


	void OnGUI(){
		GUI.skin = GUIskin;

		GUI.Label(new Rect(Screen.width * 0.98f, Screen.height * 0.01f, 0, 0), Mathf.Round(TotalDistance).ToString() + " M"); //Place the distance count on the top right of the screen
		
		GUI.Label(new Rect(Screen.width * 0.93f, Screen.height * 0.1f, 0f, 0f), TotalPrices.ToString()); //Place the gems count on the top right of the screen

		GUI.DrawTexture(new Rect(Screen.width * 0.94f, Screen.height * 0.1f, 32f, 32f), Prices); //Place the gem image beside the gems count on the top right of the screen

		if (LevelUp == false && LevelUpPosX > -Screen.width) 
		{
			GUI.Label(new Rect(LevelUpPosX , Screen.height * 0.85f , 200f , 50f ),CurrentLevelUpText , "LevelUp");		
			LevelUpPosX -= 10;
		}
		else if(LevelUp == true)
		{
			LevelUp = false;
		}
	}
}
