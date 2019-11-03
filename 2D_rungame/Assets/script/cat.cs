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
    private Transform CatTransform, cam;
    private Animator ani;
    private CapsuleCollider2D c2d;
    private Rigidbody2D r2d;
    #endregion

    private void Start()
    {
        ani = GetComponent<Animator>();
        c2d = GetComponent<CapsuleCollider2D>();
        CatTransform = GetComponent<Transform>();
        cam = GameObject.Find("Main Camera").GetComponent<Transform>();
        r2d = GetComponent<Rigidbody2D>();
     
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
        if (IsGround==true)
        {
             
             print("jump");
             ani.SetBool("jump switch",true);
            r2d.AddForce(new Vector2(0, Jump));
        }
    }
    /// <summary>
    /// slide
    /// </summary>
    public void CatSlide()
    {
        print("slide");
        ani.SetBool("slide switch",true);
        c2d.offset=new Vector2 (0.05f, -0.72f);
        c2d.size=new Vector2 (0.85f, 0.85f);
    }
    ///
    /// rest ani.
    ///
    public void ResetAnimator()
    {
        ani.SetBool("slide switch", false);
        ani.SetBool("jump switch", false);
        c2d.offset= new Vector2 (0.05f,-0.18f);
        c2d.size= new Vector2 (0.85f,1.95f);
        
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name=="地板"|| col.gameObject.name == "障礙物")
        {
            IsGround = true;
            
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name == "地板" || col.gameObject.name == "障礙物")
        {
            IsGround = false;
        }
    }
}