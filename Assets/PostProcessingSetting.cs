using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostProcessingSetting : MonoBehaviour
{
    public Toggle EnableToggle;
    public GameObject ModeBox;
    public TextMeshProUGUI currentFilter;
    public Button leftButton;
    public Button rightButton;
    public TMP_Dropdown postProcessingDropdown;
    private const string PrefPPIndex = "PostProcessProfileIndex";
    private const string PrefDisable = "PPDisable";
    private const string PrefIsManual = "PPIsManual";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(PrefDisable)) PlayerPrefs.SetInt(PrefDisable, 0);
        if (!PlayerPrefs.HasKey(PrefPPIndex)) PlayerPrefs.SetInt(PrefPPIndex, 0);
        if (!PlayerPrefs.HasKey(PrefIsManual)) PlayerPrefs.SetInt(PrefIsManual, 0);
        PostProcessingController.Instance?.ChangeFilter(PlayerPrefs.GetInt(PrefPPIndex));
        postProcessingDropdown.value = PlayerPrefs.GetInt(PrefPPIndex);
        /*leftButton.onClick.AddListener(() => OnValueChanging(-1));
        rightButton.onClick.AddListener(() => OnValueChanging(1));*/
        EnableToggle.onValueChanged.AddListener(OnToggleChange);

        bool isDisabled = PlayerPrefs.GetInt(PrefDisable, 0) == 1;
        bool isManual = PlayerPrefs.GetInt(PrefPPIndex, 0) != 0;

        EnableToggle.isOn = !isDisabled;
        ModeBox.SetActive(!isDisabled);

        if (!PostProcessingController.Instance) return;
        PostProcessingController.Instance.isManual = isManual;
        PostProcessingController.Instance.disabled = isDisabled;
        int currentIndex = PlayerPrefs.GetInt(PrefPPIndex);
        postProcessingDropdown.onValueChanged.AddListener((arg0)=>PostProcessingController.Instance.ChangeFilter(arg0));
    }

    /*private void OnValueChanging(int direction)
    {
        if (PostProcessingController.Instance)
        {
            int currentIndex = PlayerPrefs.GetInt(PrefPPIndex, -1);
            int profileCount = PostProcessingController.Instance.postProcessProfiles.Count;


            currentIndex += direction;

            if (currentIndex < -1) 
                currentIndex = profileCount - 1;
            else if (currentIndex >= profileCount) 
                currentIndex = -1; 


            PlayerPrefs.SetInt(PrefPPIndex, currentIndex);
            currentFilter.text = PostProcessingController.Instance.ChangeFilter(currentIndex);
        }
    }*/

    private void OnToggleChange(bool isEnabled)
    {
        if (PostProcessingController.Instance)
        {
            PostProcessingController.Instance.disabled = !isEnabled;
            PlayerPrefs.SetInt(PrefDisable, isEnabled ? 0 : 1);
            ModeBox.SetActive(isEnabled);
        }
    }
}
