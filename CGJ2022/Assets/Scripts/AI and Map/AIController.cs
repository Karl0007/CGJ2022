using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AIController : MonoBehaviour
{

    Vector2Int[] privatePath;
    Vector2[] newPath;
    public bool canWalk = false;
    float[] currentPath;
    float[] targetPath;
    int pathIndex=1;
    int maxPathIndex = 1;
    float moveSpeed=0;
    float unit=0.5f;//和场景转换的单位 代码*unit=场景
    public Rigidbody rb;

    float currentDirection;
    float nextDirection;
 
    public Transform transform_ui;
    public float rotateAngle;//角色旋转角度

    public GameObject playerFall;
    public GameObject playerSuccess;
    bool isChanging = false;
    public Vector2 originalPos;
    void Start()
    {
        Debug.Log("getRB");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canWalk)
        {
            moveSpeed = 0.5f;
            //已经到了拐点,此处拐点应该多取0.5，具体以场景中坐标为准
            if (Mathf.Abs(transform.position.x - targetPath[0]  ) <0.1f && Mathf.Abs(transform.position.z - targetPath[1]) < 0.1f && isChanging!=true)
            {
                //固定一下位置
                transform.position = new Vector3(targetPath[0] , transform.position.y, targetPath[1] );
                //索引后移
                pathIndex++;
                Debug.Log("index:" + pathIndex);
                //如果索引越界（没到终点）
                if(pathIndex>= maxPathIndex)
                {
                    transform_ui.GetComponent<gameController>().playerFail();//失败
                    //Time.timeScale = 0;
                    canWalk=false;
                }

                currentPath[0] = targetPath[0];
                currentPath[1] = targetPath[1];
                newPath[pathIndex] = BlockManager.GetWorldPos(privatePath[pathIndex]);
                targetPath[0] = newPath[pathIndex].x;
                targetPath[1] = newPath[pathIndex].y;

                Debug.Log("target0:" + targetPath[0]);
                Debug.Log("current0" + currentPath[0]);

                nextDirection = (targetPath[0] - currentPath[0]) == 0? -(targetPath[1] - currentPath[1]) : (targetPath[0] - currentPath[0]);
                //开启转身
                //Debug.Log(nextDirection);
                rotateAngle = ((currentDirection * nextDirection) / Mathf.Abs(currentDirection * nextDirection)) * 90;
                Debug.Log("rotate" + rotateAngle);
                transform.Rotate(0f, rotateAngle, 0f, Space.Self);
                //StartCoroutine(RotateTheHero());
                Invoke("ChangingBool", 0.5f);

            }


            //rb.velocity = new Vector3(targetPath[0] - currentPath[0], 0, targetPath[1] - currentPath[1])*moveSpeed*unit;
            Debug.Log(new Vector3(targetPath[0] - currentPath[0], 0, targetPath[1] - currentPath[1]).normalized);
            rb.velocity = new Vector3(targetPath[0] - currentPath[0], 0, targetPath[1] - currentPath[1]).normalized *moveSpeed*20;


        }
        else
        {
            //Debug.Log("speed=0");
            moveSpeed = 0;
            rb.velocity = Vector3.zero;
        }
        //Debug.Log(canWalk);
    }

    void ChangingBool()
    {
        isChanging = false;
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.CompareTag("Hurt"))
        {
            Debug.Log("1234");
            transform_ui.GetComponent<gameController>().hurtHelth();
        }
        if (other.gameObject.CompareTag("Win"))
        {
            transform_ui.GetComponent<gameController>().playerFail();
        }
    }

	//碰到了伤害
	private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Hurt"))
        {
            transform_ui.GetComponent<gameController>().hurtHelth();

            //返回伤害坐标：
            //collision.gameObject.transform.position.x;
            //collision.gameObject.transform.position.z;
        }
        if(collision.gameObject.CompareTag("Win"))
        {
            transform_ui.GetComponent<gameController>().playerFail();
        }
        
    }

    IEnumerator RotateTheHero()
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / 0.25f;
            transform.Rotate(0f, Time.deltaTime / 0.25f * rotateAngle,0f, Space.Self);

            yield return null;
        }
    }


    //接收路径
    public void updatePath(Vector2Int[] path)
    {
        
        currentPath = new float[2];
        targetPath = new float[2];
        privatePath = path;

        maxPathIndex = privatePath.Length;
        newPath = new Vector2[path.Length];
        newPath[0] = BlockManager.GetWorldPos(privatePath[0]);
        Debug.Log("path");
        currentPath[0] = newPath[0].x;
        currentPath[1] = newPath[0].y;
        originalPos = privatePath[0];
        if (path.Length <= 1)
        {
            transform_ui.GetComponent<gameController>().playerFail();
        }
        newPath[1] = BlockManager.GetWorldPos(privatePath[1]);
        targetPath[0] = newPath[1].x;
        targetPath[1] = newPath[1].y;

        currentDirection = (targetPath[0] - currentPath[0]) == 0? (targetPath[1] - currentPath[1]) : (targetPath[0] - currentPath[0]);
        
        transform.position = new Vector3(currentPath[0], transform.position.y, currentPath[1]);
    }

    public void setMove()
    {
        canWalk = true;
    }
}
