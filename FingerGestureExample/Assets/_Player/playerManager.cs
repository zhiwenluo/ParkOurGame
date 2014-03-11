using UnityEngine;
using System.Collections;

public class playerManager : MonoBehaviour {
	internal GameObject PlatformCreator; 
	internal GameController Gamecontroller; 
	

	public float NormalSpeed = 0.1f;
	internal float Speed = 0f; //player当前速度,
	public float MaxSpeed = 0.2f; //player最大速度,
	public float Acceleration = 0.001f; //加速度,
	public Transform SpeedEffect; //高速效果,
	
	internal float middRoadXPosition = 0f;//中间路的初始坐标,
	internal float positionRange = 0;//每次移动的距离,即两条路线间的间隔,
	public float turningSpeed  = 0f;//移动的速度,
	internal float InitTurningSpeed;//保存速度,

	internal bool OnTheLeftRoad = false;//是否处于左路,
	internal bool OnTheRightRoad = false;//是否处于右路,
	private bool turningLeft = false;//是否正在移向左,
	private bool turningRight = false;//是否正在移向右,

	internal float lastXPosition;//移动前的位置,

	internal bool pressToSpeedUp = true;//是否处于长按加速状态,

	public Transform TrailEffect; //轨迹效果,
	private Transform TrailEffectCopy; //轨迹效果复制物体,
	
	internal string HitAnimation = "default";//碰撞动画name,
	internal float HitAnimationTime = 0;//碰撞动画时间,
	internal bool AffectSightOfPlayer = false;//影响player的视野，获取可以通过另一个脚本实现，需要时enable=true,
	internal bool AffectDirectionOfPlayer = false;//影响player左右混乱,

	void OnEnable(){
		//=================================启动时调用，注册手势操作的事件==================================
		//不用插件自带的发送message到OnSwipe OnlongPress方法了,因为效率奇低,
		SwipeRecognizer swipeRecognizer = this.GetComponent<SwipeRecognizer>();
		swipeRecognizer.OnGesture += MySwipeEventHandler;
		//不监听时用 -= MySwipeEventHandler

		LongPressRecognizer longPressRecognizer = this.GetComponent<LongPressRecognizer>();
		longPressRecognizer.OnGesture += MyLongPressEventHandler;

		FingerUpDetector fingerUpDetector = this.GetComponent<FingerUpDetector>();
		fingerUpDetector.OnFingerUp += MyFingerUpEventHandler;
	}

	void OnDisable(){
		Debug.Log ("onDisable");

	}

	void Start(){
		Gamecontroller = GameObject.FindWithTag ("GameController").GetComponent<GameController>();//GameController
		PlatformCreator = GameObject.FindWithTag ("PlatformCreator");//platformCreator
		InitTurningSpeed = turningSpeed;//保存速度，因为后来会改变他达到加速度的效果,

		positionRange = PlatformCreator.GetComponent<PlatformCreator>().PlatformRoadsOffset[2];
		Speed = NormalSpeed;
	}

	void Update(){
		if (pressToSpeedUp){
			if(Speed <= MaxSpeed){
				Speed += Acceleration;
			}else{
				Speed = MaxSpeed;
			}
		}else{
			if(Speed >= NormalSpeed){
				Speed -= Acceleration;
			}
			else{
				Speed = NormalSpeed;
			}
		}

		
//		Add to the distance value in the game controller
		Gamecontroller.TotalDistance += Speed;

		//===========================跳向左==========================================
		if (turningLeft == true) {
			
						if (transform.position.x >= (lastXPosition - positionRange)) {
								//加速度向左移动,
								//这种运动效果可以用iTween插件来实现很多种效果,
								//或者用自带的characterControl脚本，然后调用controller.move,这里仅告诉你,要移动,在此执行,
								transform.Translate (Vector3.right * (-turningSpeed) * Time.deltaTime, Space.World);
								turningSpeed += 1.0f;
		
						} else {
								transform.position = new Vector3 (lastXPosition - positionRange, transform.position.y, transform.position.z);
								//				已换路完毕,初始化变量,
								turningSpeed = InitTurningSpeed;
								turningLeft = false;
								if (OnTheRightRoad) {
										OnTheRightRoad = false;
								} else {
										OnTheLeftRoad = true;
								}
						}
				}
		//===========================跳向右=================================================
		if (turningRight) {
			if(transform.position.x <= (lastXPosition + positionRange)){
				//向右的加速度,
				Debug.Log(transform.position.x);
				transform.Translate(Vector3.right * turningSpeed * Time.deltaTime ,Space.World);
				turningSpeed += 1.0f;
				//此处播放个动画,
			}else{
				//已换路完毕,初始化变量,
				transform.position = new Vector3(lastXPosition + positionRange , transform.position.y , transform.position.z);
				turningSpeed = InitTurningSpeed;
				turningRight = false;
				if (OnTheLeftRoad) {
					OnTheLeftRoad = false;
				}
				else {
					OnTheRightRoad = true;
				}
			}
		}

		//========================================高速效果=====================================================
		if (Speed > 0.5f) {
			SpeedEffect.particleEmitter.emit = true; //turn on the high speed effect
		} 
		else {
			SpeedEffect.particleEmitter.emit = false; //turn off the high speed effect
		}
		//========================================各种动画效果==================================================
		if (HitAnimation != "" && HitAnimationTime > 0) { //If we have a hit animation set, play it
						
//			animation.CrossFade (HitAnimation); //play the hit animation
 
			HitAnimationTime -= Time.deltaTime; //reduce from the hit animation time
 
						
			if (HitAnimation.Equals("Default"))
			{
				Instantiate (TrailEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
			}
		}
		else{
//			Debug.Log("other animation here");
		}

	}
	
	//==========================================手势===============================================
	//=============================================================================================
	//=============================================================================================
	
	/*手势用到的是FingerGestures插件,导入后拖拽assets下fingergestrues目录prefab下的FingerGestures到Hierarchy下即可,
	 *以下两个方法的由来：
	 * 选中一个object,add Component——FingerGestures——Gestures——选择Recognizer,
	 * 此时可以看到有recognizer属性了,
	 */
	void MySwipeEventHandler( SwipeGesture gesture )
	{
		Debug.Log ("滑动: " + gesture.Direction);
		//变向,
		if (gesture.Direction == FingerGestures.SwipeDirection.Left) {
			if(AffectDirectionOfPlayer && ! OnTheRightRoad)
				turningRight = true;
			else if ( !AffectDirectionOfPlayer && !OnTheLeftRoad )
				turningLeft = true;
		}
		
		if (gesture.Direction == FingerGestures.SwipeDirection.Right) {
			if(AffectDirectionOfPlayer && !OnTheLeftRoad)
				turningLeft = true;
			else if( !AffectDirectionOfPlayer && !OnTheRightRoad )
				turningRight = true;
		}
		//保存目前位置,
		lastXPosition = transform.localPosition.x;
	}
	
	//长按事件,
	void MyLongPressEventHandler(LongPressGesture gesture) {
		Debug.Log ("long press" );
		//进入加速状态,我没改变进入此方法的时间，默认1秒,
		pressToSpeedUp = true;
		//播放动画,
	}
	//手指抬起事件,因为长按事件就是按了x秒就算一次长按,
	//需要监听开始长按到手指抬起的这段时间,在此加速,
	//我想不出别的法子了,
	void MyFingerUpEventHandler(FingerUpEvent e) {
		Debug.Log ("finger up");
		//如果手指抬起时,pressToSpeedUp为true,说明此前处于加速态,此时需要去掉加速效果,
		//恢复原始动作及速度,
		if (pressToSpeedUp == true) {
			pressToSpeedUp = false;
		}
	}
	//==========================================触发===============================================
	//=============================================================================================
	//=============================================================================================

//
//	//触发信息检测,前提——勾选了is trigger,此时能直接穿过物体
//	//现在设置好的是触发Trigger方法
//	//操作,比如此时代码实现让character减速,
//	void OnTriggerEnter(Collider collider){
//		//		Debug.Log("player OnTriggerEnter：" + collider.gameObject.name);
//
//	}

}
