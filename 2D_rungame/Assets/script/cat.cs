using UnityEngine;

public class cat : MonoBehaviour
{
    #region 欄位
    [Header("跳躍次數")][Range(1, 10)][Tooltip("連跳次數")]
    public int JumpCount;
    [Header("跳躍高度")]
    public int Jump = 100;
    [Header("跑速")]
    [Range(1, 10)]
    public float speed = 1.5f;
    [Header("地面判定")]
    public bool IsGround;
    [Header("狗名")]
    public string CatName = "cat";
    public Transform CatTransform, cam;
    #endregion
    private void Start()
    {
     
    }

    private void Update()
    {
        MoveCat();
        MoveCamera();
    }
    /// <summary>
    /// move cat
    /// </summary>
    private void MoveCat()
    {
        //deltaTime= time passed per frame
        CatTransform.Translate(speed*Time.deltaTime, 0, 0);

    }

    /// <summary>
    /// move camera
    /// </summary>
    private void MoveCamera()
    {
        cam.position = new Vector3(CatTransform.position.x, cam.position.y, cam.position.z);

    }
    /// <summary>
    /// jump
    /// </summary>
    public void CatJump()
    {
        print("jump");
    }
    /// <summary>
    /// slide
    /// </summary>
    public void CatSlide()
    {
        print("slide");
    }
}