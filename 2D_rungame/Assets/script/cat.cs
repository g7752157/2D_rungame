using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections;
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
    [Header("傷害")]
    public float damage = 20f;
    private Transform CatTransform, cam;
    private Animator ani;
    private CapsuleCollider2D c2d;
    private Rigidbody2D r2d;
    public AudioClip JumpSfx;
    public AudioClip SlideSfx;
    public Image hpbar;
    public float hp = 100;
    private float maxhp;
    AudioSource audioSource;
    SpriteRenderer sr;
    [Header("拼接地圖")]
    public Tilemap TileProp;
    public Text DimondText;
    public int DimondCount,CherryCount;
    public float HpLoseRate=80;
    public GameObject Final;
    public Text FinalCherryScore, FinalDimondScore, FinalTimeScore, FinalTotalScore;
    //cherry dimond time total
    public int[] Score = new int[4];
    //public int CherryScore, DimondScore, TimeScore;
    #endregion

    private void Start()
    {
        DimondCount = 0;
        maxhp = hp;
        ani = GetComponent<Animator>();
        c2d = GetComponent<CapsuleCollider2D>();
        CatTransform = GetComponent<Transform>();
        cam = GameObject.Find("Main Camera").GetComponent<Transform>();
        r2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
     
    }

    private void Update()
    {
        MoveCat();
        MoveCamera();
        losehp();
        
    }
    /// <summary>
    /// 隨時間扣血
    /// </summary>
    void losehp()
    {
        hp -= Time.deltaTime * HpLoseRate;
        hpbar.fillAmount = hp / maxhp;
        dead();

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
            if (hp <= 0) return;
            print("jump");
             ani.SetBool("jump switch",true);
            r2d.AddForce(new Vector2(0, Jump));
            audioSource.PlayOneShot(JumpSfx, 1.0f);

        }
    }
    /// <summary>
    /// slide
    /// </summary>
    public void CatSlide()
    {
        if (hp <= 0) return;
        print("slide");
        ani.SetBool("slide switch",true);
        c2d.offset=new Vector2 (0.05f, -0.72f);
        c2d.size=new Vector2 (0.85f, 0.85f);
        audioSource.PlayOneShot(SlideSfx, 1.0f);
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
        if (col.gameObject.name=="地板")
        {
            IsGround = true;
            
        }

        if (col.gameObject.name == "道具")
        {
            eatcherry(col);
            
        }
        

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "障礙物")
        {
            Damage();
        }
        if (col.tag == "鑽石")
        {
            eatdimond(col);
        }
        if (col.name == "DeadZone")
        {
            hp = 0;
            dead();
        }
    }
    /// <summary>
    /// damage
    /// </summary>
    void Damage()
    {
        Debug.Log("hitten");
        hp -= damage;
        hpbar.fillAmount = hp / maxhp;
        sr.enabled = false;
        Invoke("SRenable",.3f);
        dead();
    }
    void SRenable()
    {
        sr.enabled = true;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name == "地板" )
        {
            IsGround = false;
        }
    }
    void eatcherry(Collision2D col)
    {
        Debug.Log("item");
        Vector3 HitPoint = col.contacts[0].point;
        Debug.Log(HitPoint);
        Vector3 pos = Vector3.zero;
        Vector3 normal = col.contacts[0].normal;
        pos.x = HitPoint.x - 0.01f * normal.x;
        pos.y = HitPoint.y - 0.01f * normal.y;
        TileProp.SetTile(TileProp.WorldToCell(pos), null);
    }
    /// <summary>
    /// 鑽石
    /// </summary>
    /// <param name="col"></param>
    void eatdimond(Collider2D col)
    {
        DimondCount += 1;
        DimondText.text = DimondCount + "";
        Destroy(col.gameObject);
    }
    /// <summary>
    /// DIE
    /// </summary>
    void dead()
    {
        if (hp <= 0)
        {
            ani.SetBool("death switch", true);
            speed = 0;
            FinalScreen();
            
        }
    }
    /// <summary>
    /// 結算
    /// </summary>
    void FinalScreen()
    {
        if(Final.activeInHierarchy==false)
        {
            Final.SetActive(true);
            StartCoroutine(FinalScore(DimondCount,0,100,FinalDimondScore));
            StartCoroutine(FinalScore(CherryCount, 1, 100, FinalCherryScore,DimondCount*0.1f));
            int time = (int)Time.timeSinceLevelLoad;
            StartCoroutine(FinalScore(time, 2, 100, FinalTimeScore, (DimondCount+CherryCount) * 0.1f));
        }
    }
    IEnumerator FinalScore(int count,int ScoreIndex,int addscore,Text FinalItemScore,float wait=0)
    {
        yield return new WaitForSeconds(wait);
        while (count>0)
        {
            Score[ScoreIndex] += addscore;
            FinalItemScore.text = Score[ScoreIndex] + "";
            count--;
            yield return new WaitForSeconds(0.1f);
            
        }
        if (ScoreIndex != 3) { Score[3] = Score[0] + Score[1] + Score[2]; }
        if (ScoreIndex == 2)
        {
            int total = Score[3] / 100;
            Score[3] = 0;
            StartCoroutine(FinalScore(total,3,100,FinalTotalScore,0.05f));
        }
    }
    
}