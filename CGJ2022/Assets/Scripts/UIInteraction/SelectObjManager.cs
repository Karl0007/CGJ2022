using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SelectObjManager : MonoBehaviour
{
	private static SelectObjManager _instance;
	public static SelectObjManager Instance
	{
		get { return _instance; }
	}

	//物体z轴距摄像机的长度
	public LayerMask groundLayerMask;
	private bool isChooseSuccess = false;
	public bool canPlace= false;
	public GameObject CurrentObj;
	public GameObject AIPrefab;
	
	public int dangerTime;
	public int outerTime;


	void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		if(canPlace)MoveCurrentPlaceObj();
		if (canPlace&&isChooseSuccess && Input.GetMouseButton(0))
		{
			CurrentObj.GetComponent<Glass>().ChoosePanelObj.SetActive(true);
			canPlace = false;
			SelectTrickPanel.Instacne.MarkActiveFalse();

		}
		if (Input.GetMouseButton(0))
		{
			TryGetObj();
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			RunAI();
		}
	}

	public void RunAI()
	{
		Instantiate(AIPrefab, BlockManager.GetWorldVec3(BlockManager.stPos), Quaternion.identity);
		var pm = FindObjectOfType<PathManagement>();
		pm.conveyLevel(BlockManager.stPos,BlockManager.edPos, PathManagement.Dir.Left);
		for (int i = 0; i < GameManager.Instance.DangerList.Length; i++)
		{
			if (GameManager.Instance.DangerList[i] == true)
			{
				pm.blacklist[pm.listp++] = i;
			}
		}
		var path = FindObjectOfType<PathManagement>().Pathfinder(BlockManager.intMap, new Vector2Int[1]);
		FindObjectOfType<AIController>().updatePath(path);
		FindObjectOfType<AIController>().setMove();
		for (int i = 0; i < path.Length; i++)
		{
			Debug.Log(path[i]);
		}

	}

	void TryGetObj()
	{
		Vector3 point;
		Vector3 ScreenPosition;
		ScreenPosition = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay(ScreenPosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, 1000, groundLayerMask))
		{
			CurrentObj = hitInfo.collider.gameObject;
			Debug.Log(CurrentObj.GetComponent<BlockManager>().pos);
			isChooseSuccess = true;
		}
		else
		{
			isChooseSuccess = false;
		}
	}

	 void MoveCurrentPlaceObj()
	{

	}

	 
}