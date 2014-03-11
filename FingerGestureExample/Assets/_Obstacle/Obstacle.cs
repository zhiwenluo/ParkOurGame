using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	internal GameObject Player;

	public float SpeedChange = 0.8f;//碰撞时对player速度的改变,
	public string PlayerHitAnimation = "default";//碰撞中player所发生的动画,
	public float PlayerHitAnimationTime = 0;//碰撞中player发生动画的持续时间,
	public bool AffectSightOfPlayer = false;//影响player的视野,
	public bool AffectDirectionOfPlayer = false;//让player左右混乱,反向操作

	public bool AnimateObstacle = false;//碰撞时是否自动播放障碍物的碰撞动画,
	public Transform DisperseEffect;//消失动画,
	public float RotateSpeed = 5f;//the rotate speed of obstacle
	public Vector3 Rotation = new Vector3 (0, 0, 0);//创建障碍物时初始角度,
	public Vector3 Offset = new Vector3 (0, 0, 0);//初始偏移量,

	public bool isRunWithPlayer = false;//障碍物 属性 是否是黏在player身上一段时间,
	internal bool isStickingPlayer = false;//是否是黏在player上的状态,
	public float StickTime = 0;//障碍物要作用在player上多久,

	public float RewardDistance = 1;//极限躲避所要求的距离,
	internal bool HitPlayer = false;
	public AudioClip HitSound;//碰撞时的音效,
	public AudioClip RewardSound;//极限躲避时的音效,

	public bool LookAtPlayer = false;

	// Use this for initialization
	void Start () {
		
		Player = GameObject.FindWithTag ("Player");

		transform.Translate (Offset , Space.Self);
		transform.eulerAngles = Rotation;
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.value * 360, transform.eulerAngles.z);//给每个障碍物一个随即旋转角度，使看起来不一样，比如多把椅子，每把角度都不一样,
	}
	
	// Update is called once per frame
	void Update () {
//		transform.eulerAngles = new Vector3(transform.eulerAngles.x , transform.eulerAngles.y , RotateSpeed * Time.deltaTime);
		transform.Rotate(Vector3.forward * RotateSpeed * Time.deltaTime, Space.World);
		//检测和player的距离,判断是否有极限闪避奖励（未碰撞前）,
		if (!isStickingPlayer && Vector3.Distance (Player.transform.position, transform.position) < RewardDistance ) 
		{
//			Debug.Log("极限躲避奖励！");
			//增加player的速度或者什么的..
//			audio.PlayOneShot(RewardSound);//播放奖励音效 （暂定欢呼）,
				
		}

		if (isRunWithPlayer == true) //如果是依附性障碍,
		{
			if (StickTime > 0 && isStickingPlayer == true) //.如果stickTime大于0且此时正依附在player上,
			{
				StickTime -= Time.deltaTime; //递减stickTime,
				
				if (Player)
					transform.position = Player.transform.position; //让障碍物跟着player跑,
			}
			else if (isStickingPlayer == true) //如果时间走完了，障碍物依然依附在player上,
			{
				isStickingPlayer = false; //停止依附,
				
				if (DisperseEffect)
					Instantiate(DisperseEffect, transform.position, Quaternion.identity); //产生一个消失动画,

				if(AffectSightOfPlayer)
					Player.GetComponent<playerManager>().AffectSightOfPlayer = false; //remove the effect on the player
				if(AffectDirectionOfPlayer)
					Player.GetComponent<playerManager>().AffectDirectionOfPlayer = false; 

				Destroy(gameObject); //销毁障碍物,
			}
		}
		
		if (LookAtPlayer == true && Player) //如果设置为LookAtPlayer
		{
			transform.LookAt(Player.transform.position); 
			transform.eulerAngles = new Vector3(0.0f, 0.0f, transform.eulerAngles.z);
		}
	}

	void OnTriggerEnter(Collider collision)
	{
		//如果碰到player
		if (collision.tag == "Player" && HitPlayer == false && Player)
		{
			Debug.Log("OnTriggerEnter");
			HitPlayer = true; //置为true,目的是为了一个障碍物只与player碰撞一次,
			
//			if (isRunWithPlayer == false) audio.PlayOneShot(HitSound); //如果不是依附性障碍物，则只播放一次音效,
//			else audio.Play(); //如果是依附性障碍物，则循环 播放音效，for example :碰到饮料瓶 ,持续播放吹风机短路的声音,
			
			Player.GetComponent<playerManager>().Speed *= SpeedChange; //改变player速度,
			Player.GetComponent<playerManager>().HitAnimation = PlayerHitAnimation; //设置player碰撞时的动画,
			Player.GetComponent<playerManager>().HitAnimationTime = PlayerHitAnimationTime; //设置player碰撞动画的时间,
			if(AffectSightOfPlayer)
				Player.GetComponent<playerManager>().AffectSightOfPlayer = AffectSightOfPlayer; //设置是否影响player视野,
			if(AffectDirectionOfPlayer)
				Player.GetComponent<playerManager>().AffectDirectionOfPlayer = AffectDirectionOfPlayer; //设置是否让palyer的方向混乱,

			
			if (isRunWithPlayer == true)
			{
				isStickingPlayer = true;
				transform.parent = Player.transform.parent; //让依附性障碍依附于plaer上,
			}
			
			if (AnimateObstacle == true)
			{
				transform.animation.Play(); //播放障碍物的动画,
			}
		}
	}
	
	void OnTriggerExit(Collider collision)
	{
		Debug.Log("OnTriggerExit");
		if (Player)
						;
//			Player.GetComponent<playerManager>().JumpState = 4; //If we finished hitting an obstacle, assuming he was bounce some distance. Automatically set the jump state to "falling after a double jump"
	}
}
