using System.Collections;
using I2.Loc;
using MyBox;
using SoundControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArgueGameBtn : MonoBehaviour
{
    public string text;
    private TestArgueGame testArgueGame;
    [SerializeField][AutoProperty(AutoPropertyMode.Asset)]private Localize i2Localize;
    // private int fogClickTime = 6;
    // private int currentClickTime = 0;
    [SerializeField]private Button btn;
    public int scoreAmount = 0;
    // [SerializeField]private Image fog;
    public float screenWidth = 1920f;
    public TextMeshProUGUI MeshProUGUI; 
    
    public RectTransform buttonRect; 
    public float MoveSpd = 20f;

    public bool clicked;
    public Vector3 targetPosition;
    private bool isRight;
    
    private void Start()
    {
        MeshProUGUI.fontSize = Random.Range(55, 80);
        MoveSpd = Random.Range(100, 250);
        clicked = false;
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ClickedButton);
        buttonRect = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (clicked) return;

        Vector3 position = buttonRect.localPosition;

        // Move left or right based on isRight
        if (isRight)
        {
            position.x += MoveSpd * Time.deltaTime;

            // Check if the button is out of the screen on the right
            if (position.x > screenWidth / 2 + buttonRect.rect.width/2)
            {
                // Reappear on the left side of the screen
                position.x = -screenWidth / 2 - buttonRect.rect.width/2;
            }
        }
        else
        {
            position.x -= MoveSpd * Time.deltaTime;

            // Check if the button is out of the screen on the left
            if (position.x < -screenWidth / 2 - buttonRect.rect.width/2)
            {
                // Reappear on the right side of the screen
                position.x = screenWidth / 2 + buttonRect.rect.width/2;
            }
        }

        // Apply the updated position
        buttonRect.localPosition = position;
    }

    public void Init(TestArgueGame script,string dialogeText,AudioClip clip, int score,Vector3 target,bool isRighty)
    {
        isRight = isRighty;
        targetPosition = target;
        Debug.Log(script);
        GetComponent<AudioSource>().clip = clip;
        testArgueGame = script;
        testArgueGame.argueBtns.AddListener(()=>Destroy(this.gameObject));
        text = dialogeText;
        MeshProUGUI.text = dialogeText;
        scoreAmount = score;
    }
    private void ClickedButton()
    {
        /*currentClickTime++;
        if (fogClickTime-1 < currentClickTime)
        {
            fog.enabled = false;
        } 

        if (fogClickTime < currentClickTime)
        {
            if(!clicked)
            StartCoroutine(Attack());
        }
        if(!clicked)*/
            StartCoroutine(Attack());
            SoundManager.PlaySfx("select1");
    }
    private float elapsedTime = 0f;
    public float moveDuration = 5f;
    IEnumerator Attack()
    {
        if (clicked) yield return null;
        clicked = true;
        MeshProUGUI.color = Color.yellow;
        var initialScale = buttonRect.localScale;
        var targetScale = new Vector3(0.3f, 0.3f, 0.3f);
        Vector3 startPosition = buttonRect.localPosition;
        testArgueGame.GetComponent<AudioSource>().clip = GetComponent<AudioSource>().clip;
        testArgueGame.GetComponent<AudioSource>().Play();
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / moveDuration;
            buttonRect.localScale = Vector3.Lerp(initialScale, targetScale, progress);
            // Move towards the target position
            buttonRect.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);

            yield return null; // Wait for the next frame
        }
        SoundManager.PlaySfx("fireball_hit");
        testArgueGame.womenAnim.SetTrigger("Hurt");

        testArgueGame.ChangeScore(scoreAmount);
        testArgueGame.EndArgue();
        
    }
    
}
