using System.Collections;
using System.Numerics;
using MyBox;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Vector3 = UnityEngine.Vector3;

public class BackgroundManager : MonoSingleton<BackgroundManager>
{
    [Separator("Script")]
    public Dialogue currentDialogue;
    public GameControl gameControl;
    public GameObject vP;
    [Separator("Video")] public VideoPlayer videoPlayer;
    [Separator("SpriteRenderer")]
    public SpriteRenderer backGround;
    public SpriteRenderer cG;
    [Separator("Camera")]
    public Camera backgroundCamera;
    public GameObject skipVideoButton;
    [Separator("Prop")] public Image propImg;
    public Animator propAnim;
    protected  void Start()
    {
        gameControl ??= GameControl.instance;
    }

    public Vector3 ShowVideo()
    {

        StartCoroutine(PlayVideo());
        // if (videoPlayer.clip != video)
        // {
        //     videoPlayer.Prepare();
        //
        //     videoPlayer.StepForward();
        //     
        // }

        //Debug.Log(bg.sprite);
        return videoPlayer.gameObject.transform.position;
        
    }

    private IEnumerator PlayVideo()
    {
        VideoClip video = Resources.Load<VideoClip>(
            $"Backgrounds/{currentDialogue.backgroundData.file.Trim()}/{currentDialogue.backgroundData.image.Trim()}");
        GameObject oldvideo = null;
        if (videoPlayer != null)
            if (videoPlayer.clip != video)
            {
                oldvideo= videoPlayer.gameObject;
                videoPlayer = null;
            }
        if(videoPlayer == null)
        {
            videoPlayer = Instantiate(vP).GetComponent<VideoPlayer>();
            videoPlayer.transform.SetParent(backGround.transform);
            videoPlayer.transform.localPosition = Vector3.zero;
            videoPlayer.transform.localScale = Vector3.one;
            videoPlayer.clip = Resources.Load<VideoClip>(
                $"Backgrounds/{currentDialogue.backgroundData.file.Trim()}/{currentDialogue.backgroundData.image.Trim()}");
        }
        cG.gameObject.SetActive(false);
        videoPlayer.gameObject.SetActive(true);
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }
        if (oldvideo == null) yield return null;
        


            yield return new WaitForSeconds(0.25f);
        Destroy(oldvideo);
    }
    public void ShowImage(Dialogue dialogue)
    {
        //Debug.Log(dialogue.backgroundData.file.Trim());
        Vector3 temp;
        currentDialogue = dialogue;
        if(videoPlayer != null)
         if(dialogue.backgroundData.file.Trim() != "Video")
             Destroy(videoPlayer.gameObject);

        if (!string.IsNullOrEmpty(currentDialogue.cgData.cG) && (currentDialogue.callFuncs.func1.Trim() == "HideCGImage" || currentDialogue.callFuncs.func2.Trim() == "HideCGImage"))
        {
            temp = dialogue.backgroundData.file.Trim() == "Video" ? ShowVideo() : ShowBgImage();
            if (!gameControl.CG.Contains(currentDialogue.cgData.cG.Trim()))
                gameControl.CG.Add(currentDialogue.cgData.cG.Trim());
        }
        else if (string.IsNullOrEmpty(currentDialogue.cgData.cG))
        {
            temp = dialogue.backgroundData.file.Trim() == "Video" ? ShowVideo() :ShowBgImage();
        }
        else
        {
            temp =  ShowCgImage();
        }

        backgroundCamera.transform.position = temp;
        ShowImgEffect();
        ShowPropAnimation();
        ShowAnimation();
        
    }
    private Vector3 ShowBgImage()
    {
        cG.gameObject.SetActive(false);
        backGround.sprite = Resources.Load<Sprite>(
            $"Backgrounds/{currentDialogue.backgroundData.file.Trim()}/{currentDialogue.backgroundData.image.Trim()}");
        //Debug.Log(bg.sprite);
        return backGround.transform.position;
    }
    private Vector3 ShowCgImage()
    {
        cG.gameObject.SetActive(true);
        cG.sprite = Resources.Load<Sprite>($"CG/{currentDialogue.cgData.file.Trim()}/{currentDialogue.cgData.cG.Trim()}");
        if (!gameControl.CG.Contains(currentDialogue.cgData.cG.Trim()))
            gameControl.CG.Add(currentDialogue.cgData.cG.Trim());
        return cG.transform.position;
    }
    private void ShowImgEffect()
    {
        if (string.IsNullOrEmpty(currentDialogue.imgEffect))
            return;
        //Debug.Log(currentDialogue.imgEffect);
        ImgEffectControl.Instance.ShowImgEffect(currentDialogue.imgEffect);
    }
    private void ShowPropAnimation()
    {
        if (string.IsNullOrEmpty(currentDialogue.prop.anim))

            return;
        //return if not have animations no need run whole things
        Sprite propSprite = Resources.Load<Sprite>($"SmallProps/{currentDialogue.prop.name.Trim()}");
        propImg.sprite = propSprite;

        switch (currentDialogue.prop.anim)
        {
            case "In":
                propAnim.SetTrigger("In");
                break;

            case "Out":
                propAnim.SetTrigger("Out");
                break;
        }
    }
    private void ShowAnimation()
    {
        var animations = currentDialogue.animation.Trim();
        if (string.IsNullOrEmpty(animations))
            return;
        //if (gameControl.isTester == true || gameControl.stageClear.Contains("End4") ||
        //    gameControl.stageClear.Contains("End5") || gameControl.stageClear.Contains("End7"))
        //{
        //    skipVideoButton?.SetActive(animations == "FongComesBack" ||
        //                              animations == "Trailer" ||
        //                              animations == "WifeGhostCutScene" ||
        //                              animations == "FongToJadeNecklace" ||
        //                              animations == "CreditVideo" ||
        //                              animations == "JadeRingBreakVideo");
        //}
        ImgEffectControl.Instance.PlayEffect(animations);
    }
}
