using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
	public const float Size = 5.2f;
	public const float Gap = 0.1f;
	public const float SumSize = Size + Gap;

	public List<GameObject> safeBlocks;
	public List<GameObject> dangerBlocks;
	public GameObject wallBlock;
	public static Dictionary<Vector2Int, GameObject> map = new Dictionary<Vector2Int, GameObject>();
	public static Vector2Int LeftDown = new Vector2Int(9999, 9999);
	public static Vector2Int RightUp = new Vector2Int(-9999, -9999);
	public static int[,] intMap;
	public static Vector2Int stPos;
	public static Vector2Int edPos;

	public enum BlockType
	{
		Wall = -1,
		Null = 0,
		Type2 = 1,
		Type3 = 2,
		Type4 = 3,
		Type5 = 4,
		Type6 = 5,
	}

	public GameObject safe;
	public GameObject danger;
	public GameObject cube;
	public bool isEmpty;
	public bool isDanger;
	public bool isStart;
	public bool isEnd;
	public BlockType type;
	public Vector2Int pos;

	public void DestroyChildren(GameObject obj)
	{
		for (int i = 0; i < obj.transform.childCount; i++)
		{
			Destroy(obj.transform.GetChild(i).gameObject);
		}
	}

	public void ChangeType(BlockType t)
	{
		if (t == BlockType.Wall || type == BlockType.Wall)
		{
			Debug.Log("dont change wall");
			return;
		}
		type = t;
		DestroyChildren(safe);
		DestroyChildren(danger);
		if (t != BlockType.Null)
		{
			//Instantiate(safeBlocks[(int)t - 1],safe.transform);
			//Instantiate(dangerBlocks[(int)t - 1], danger.transform);
		}
	}


	public void SetDanger(bool d)
	{
		isDanger = d;
	}

	public static Vector2 GetWorldPos(Vector2Int pos)
	{
		return ((pos + LeftDown + Vector2.one * 0.5f) * SumSize);
	}

	public void Awake()
	{
		//cube.SetActive(false);
		pos = new Vector2Int(Mathf.RoundToInt((transform.position.x - SumSize / 2) / SumSize), Mathf.RoundToInt((transform.position.z - SumSize / 2) / SumSize));
		map[pos] = gameObject;
		LeftDown.x = Mathf.Min(pos.x, LeftDown.x);
		LeftDown.y = Mathf.Min(pos.y, LeftDown.y);
		RightUp.x = Mathf.Max(pos.x, RightUp.x);
		RightUp.y = Mathf.Max(pos.y, RightUp.y);
		Debug.Log(LeftDown);
		Debug.Log(RightUp);
		if (isStart) stPos = pos - LeftDown;
		if (isEnd) edPos = pos - LeftDown;
	}

	public void Start()
	{
		if (intMap == null)
		{
			intMap = new int[RightUp.x - LeftDown.x + 1 , RightUp.y - LeftDown.y + 1];
		}
		intMap[pos.x - LeftDown.x, pos.y - LeftDown.y] = (int)type;
		//ChangeType(BlockType.Type2);
		FindObjectOfType<PathManagement>().conveyLevel(stPos,edPos,PathManagement.Dir.Left);
	}
}
