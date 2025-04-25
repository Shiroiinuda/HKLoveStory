using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasierLocalization;
using I2.Loc;
using MyBox;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    [Foldout("Script", true)] public GameControl gameControl;
    public AudioManager audioManager;
    [Foldout("Script")] public LoadSceneManager loadSceneManager;

    [ReadOnly] public AudioClip smashselect;
    [SerializeField] private TMP_Dropdown lanauageDropdown;
    [SerializeField] Slider musicSlider;

    [SerializeField] Slider sfxSlider;

    [SerializeField] Slider cSoundSlider;

    private float musicValue;
    private float sfxValue;
    private float cSoundValue;
    public GameObject confirmQuitPanel;
    public GameObject screenSettingPanel;
    private const string PrefDubNum = "DUBNumber";
    [SerializeField] private TMP_Dropdown dubSlider;
    public List<GameObject> tabs;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(PrefDubNum)) PlayerPrefs.SetInt(PrefDubNum, 0);
        //lanauageDropdown.value = lanauageDropdown.options.FindIndex(option => option.text.Equals(Localization.CurrentLan()));

        string currentLanguage = Localization.CurrentLan();

        if (currentLanguage.Equals("简体中文") || currentLanguage.Equals("簡體中文"))        
            lanauageDropdown.value = 1;
        else
            lanauageDropdown.value = 0;

        lanauageDropdown.onValueChanged.AddListener((arg0) => StartCoroutine(OnLanguageChange(arg0)));
        dubSlider.value = PlayerPrefs.GetInt(PrefDubNum);
        dubSlider.onValueChanged.AddListener((arg0) => ChangeDUB((int)arg0));
        musicSlider.onValueChanged.AddListener((arg0)=>ChangeMusicVolume());
        sfxSlider.onValueChanged.AddListener((arg0)=>ChangeSFXVolume());
        cSoundSlider.onValueChanged.AddListener((arg0)=>ChangeCSoundVolume());
        musicValue = AudioManager.Instance.musicVolume;
        sfxValue = AudioManager.Instance.sfxVolume;
        cSoundValue = AudioManager.Instance.cSoundVolume;

        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;
        cSoundSlider.value = cSoundValue;
        smashselect = Resources.Load<AudioClip>("Sounds/sfx/smashselect");
#if UNITY_ANDROID || UNITY_IOS
        screenSettingPanel.SetActive(false);
#endif
    }

    private void OnEnable()
    {
        foreach (var tab in tabs)
        {
            tab.SetActive(false);
        }
        tabs[0].SetActive(true);
    }

    private IEnumerator OnLanguageChange(int arg0)
    {
        audioManager.PlayButtonSFX(smashselect);
        LoadSceneManager.instance.GetCurrentSceneThenFade();
        yield return new WaitForSeconds(0.75f);
        Localization.SetLanguage(lanauageDropdown.options[arg0].text);
    }

    private void ChangeDUB(int value)
    {
        PlayerPrefs.SetInt(PrefDubNum, value);
    }
    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat(AudioManager.MIXER_MUSIC,musicSlider.value);
        AudioManager.Instance.mixer.SetFloat(AudioManager.MIXER_MUSIC, musicSlider.value);
        audioManager.LoadMusic();
    }

    public void ChangeSFXVolume()
    {
        PlayerPrefs.SetFloat(AudioManager.MIXER_SFX, sfxSlider.value);
        AudioManager.Instance.mixer.SetFloat(AudioManager.MIXER_SFX, sfxSlider.value);
        audioManager.LoadSFX();
    }

    public void ChangeCSoundVolume()
    {
        PlayerPrefs.SetFloat(AudioManager.MIXER_CSOUND, cSoundSlider.value);
        AudioManager.Instance.mixer.SetFloat(AudioManager.MIXER_CSOUND, cSoundSlider.value);
        audioManager.LoadCSound();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(AudioManager.MIXER_MUSIC, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MIXER_SFX, sfxSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MIXER_CSOUND, cSoundSlider.value);
    }

    private void OnDisable()
    {
        Save();

        Debug.Log("saved Quit");
    }
    public void OnMenuClicked()
    {
        audioManager.PlayButtonSFX(smashselect);
        loadSceneManager.FadeToLevel(2);
    }
    public void OnQuitClicked()
    {
        audioManager.PlayButtonSFX(smashselect);
        confirmQuitPanel.SetActive(true);
    }

    public void ConfirmQuitClicked()
    {
        audioManager.PlayButtonSFX(smashselect);
        Application.Quit();
    }

    public void OnBackClicked(GameObject closeObj)
    {
        AudioClip backbutton = Resources.Load<AudioClip>("Sounds/sfx/backbutton");
        audioManager.PlayButtonSFX(smashselect);

        closeObj.SetActive(false);
    }

    public void OnButtonClickSFX()
    {
        audioManager.PlayButtonSFX(smashselect);
    }

}