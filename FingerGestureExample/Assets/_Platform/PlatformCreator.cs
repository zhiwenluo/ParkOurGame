using UnityEngine;
using System.Collections;

public class PlatformCreator : MonoBehaviour {
	internal GameObject Player;
	
	public int NumberOfPlatforms = 3;//最初初始化平台的个数,只产生一次,以后的销毁与再造行为由他们自己来执行.在platform平台object里的platform.cs脚本执行,每一个平台有多节组成，每节都有长度,平台长度为 节数*节长度
	internal int PlatformIndex = 0;//平台的序号

	public Transform NewPlatform;//传递进来的platfrom预设物体
	internal Transform NewPlatformCopy;//初始化预设物体

	public int SectionLength = 3;//平台每一节的长度

//	public Transform[] SectionEdge;//平台边缘模型
	public Transform[] SectionMiddle;//单节平台中心模型
	internal int SectionIndex;//节序号
	internal Transform SectionCopy;//初始化平台每节的预设物体

	public Vector2 PlatformWidthRange = new Vector2(0,0);//平台宽度范围,    .x输入min .y输入max   避免创建两个变量存储最大最小，方便
	internal float PlatformWidth = 0;
	public int[] PlatformRoadsOffset;

	public Vector2 PlatformLengthRange = new Vector2 (0,0);//长度范围,指有多少节平台中心模型
	internal int PlatformOldLength = 0;
	internal int PlatformLength = 0;

	public Vector2 PlatformHeightRange = new Vector2(0,0);//高度范围
	internal int PlatformHeight = 0;

	public Vector2 PlatformRotateRange = new Vector2(0,0);//旋转角范围
	internal int PlatformRotate = 0;

//	public Vector2 PlatformGapRange = new Vector2(0,0);//间隔范围
//	internal int PlatformGap = 0;
//
//	public Vector2 PlatformShiftRange = new Vector2(0,0);//偏移量范围
//	internal int PlatformShift = 0;

	public Transform[] PriceObjects;//奖励物品数组，例如金币或者道具等等
	internal Transform PriceObjectCopy;
	internal int PriceObjectIndex = 0;
	internal int PriceObjectShift = 0;//物品的偏移量，指在左中右那条路上
	public float PriceObjectRate = 0.5f;//奖励出现的几率,随等级在GameController里增加
	internal int PriceTrail = 0;//奖励的轨迹，例如连续的一列金币
	public Vector2 PriceTrailRange = new Vector2 (0,0);//奖励轨迹的范围,例如连续一列x个金币

	public Transform[] Obstacles;//障碍物数组
	internal Transform ObstacleCopy;
	public float ObstacleRate = 0.5f;//出现障碍物的几率,随等级在GameController里增加

	internal float PlatformTotalLength = 0;//平台已产生的总长度


	// Use this for initialization
	void Start () {
		Player = GameObject.FindWithTag ("Player");


		for(PlatformIndex = 0 ; PlatformIndex < NumberOfPlatforms ; PlatformIndex ++)
		{
			CreatPlatform(PlatformTotalLength);
			PlatformTotalLength += PlatformLength * SectionLength;
		}
	}
	

	public void CreatPlatform(float offset)
	{
		NewPlatformCopy = Instantiate (NewPlatform , transform.position , Quaternion.identity) as Transform;

		PlatformWidth = Random.Range (PlatformWidthRange.x , PlatformWidthRange.y);
//		if (PlatformWidth < 0) {
//		}
		PlatformOldLength = PlatformLength;

		PlatformLength = (int) Random.Range (PlatformLengthRange.x , PlatformLengthRange.y);
//		if (PlatformLength < 5) {
//		}
//		for(SectionIndex = 0 ; SectionIndex < PlatformLength ; SectionIndex ++){
//			SectionCopy = Instantiate(SectionMiddle[Random.Range(0,SectionMiddle.Length)] , transform.position , Quaternion.identity) as Transform;
//			SectionCopy.Translate(Vector3.forward * SectionIndex * SectionLength , Space.World);
//			SectionCopy.localScale = new Vector3(PlatformWidth , SectionCopy.localScale.y , SectionCopy.localScale.z);
//			SectionCopy.transform.parent = NewPlatformCopy.transform;
//			SectionCopy.Rotate(Vector3.right * 0 , Space.World);
//		}
		if(PlatformOldLength > 0)
		{
			CreatPriceAndObstacle();
		}
		//Set platform Height
//		PlatformHeight = (int)Random.Range(PlatformHeightRange.x, PlatformHeightRange.y); //Choose a random value within the minimum and maximum of PlatformHeightRange
//		NewPlatformCopy.Translate(Vector3.up * PlatformHeight, Space.World); //Move the platform either up or down by the value of PlatformHeight
		NewPlatformCopy.Translate (Vector3.forward * offset , Space.World);
	}
	
	public void CreatPriceAndObstacle()
	{
		for( PriceObjectIndex = 0 ; PriceObjectIndex < PlatformLength * SectionLength ; PriceObjectIndex += 15 ){
			if( PriceObjectRate > Random.value)
			{
				PriceObjectCopy = Instantiate(PriceObjects[Random.Range(0,PriceObjects.Length)] , NewPlatformCopy.transform.position , Quaternion.identity) as Transform;
				PriceObjectCopy.Translate(Vector3.forward * PriceObjectIndex , Space.World);
//				PriceObjectCopy.Translate(Vector3.up * 0.5f, Space.World);
				if(PriceTrail > 0){
					PriceTrail--;
				}
				else 
				{
					PriceObjectShift = PlatformRoadsOffset[Random.Range(0,PlatformRoadsOffset.Length)];
					PriceTrail = (int) Random.Range(PriceTrailRange.x , PriceTrailRange.y);
				}
				PriceObjectCopy.Translate(Vector3.right * PriceObjectShift , Space.World);
				PriceObjectCopy.transform.parent = NewPlatformCopy.transform;
			}

			if( ObstacleRate > Random.value )
			{
				ObstacleCopy = Instantiate(Obstacles[Random.Range(0,Obstacles.Length)] , NewPlatformCopy.transform.position , Quaternion.identity) as Transform ;
				ObstacleCopy.Translate(Vector3.forward * PriceObjectIndex ,Space.World);
				ObstacleCopy.Translate(Vector3.right * PlatformRoadsOffset[Random.Range(0,PlatformRoadsOffset.Length)] , Space.World);
				ObstacleCopy.transform.parent = NewPlatformCopy.transform;
			}
		}
	}
}
