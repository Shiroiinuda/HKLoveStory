using EasierLocalization;
using UnityEngine;
using I2.Loc;
using TMPro;

namespace EasierLocalization
{
    public class Localization : MonoBehaviour
    {
        private const string PrefDubNum = "DUBNumber";
        public static void ParametersText(TextMeshProUGUI textUI, string parameter, string value)
        {
            LocalizationParamsManager paramsManager = textUI.gameObject.GetComponent<LocalizationParamsManager>();
            paramsManager.SetParameterValue(parameter, value);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public static string GetString(string id)=> LocalizationManager.GetTranslation(id);
        public static void SetLanguage(string language)
        {
            string tmpLang = language switch
            {
                "tchinese" => "繁體中文",
                "schinese" => "簡體中文",
                "簡體中文" => "簡體中文",
                "简体中文" => "簡體中文",
                _ => "繁體中文"
            };
            if (!LocalizationManager.HasLanguage(tmpLang)) return;
            Debug.Log(tmpLang);
            LocalizationManager.CurrentLanguage = tmpLang;
            GameControl.instance.UserLanguage = tmpLang;

        }
        public static string CurrentLan()
        {
            return LocalizationManager.CurrentLanguage;
        }

        public static string NpcVoiceLan()
        {
            if (!PlayerPrefs.HasKey(PrefDubNum)) PlayerPrefs.SetInt(PrefDubNum, 0);
            if(PlayerPrefs.GetInt(PrefDubNum) ==0)
            {
                return CurrentLan() switch
                {
                    "簡體中文" => "CN",
                    _ => "HK"
                };
            }

            return PlayerPrefs.GetInt(PrefDubNum) switch
            {
                1 => "HK",
                2 => "CN",
                _ => "HK"
            };
        }
    }
}

namespace SoundControl
{
    public class SoundManager : MonoBehaviour
    {
        public static AudioClip GetSfx(string clipID)
        {
            AudioClip clip = Resources.Load<AudioClip>($"Sounds/sfx/{clipID}");
            if (clip is null)
            {
                Debug.Log($"<color=#FF0000>Error:</color> Clip '{clipID}' could not be found.");
                return null;
            }
            return clip;
        }

        public static AudioClip GetBgm(string clipID)
        {
            AudioClip clip = Resources.Load<AudioClip>($"Sounds/BGM/{clipID}");
            if (clip is null)
            {
                Debug.Log($"<color=#FF0000>Error:</color> Clip '{clipID}' could not be found.");
                return null;
            }
            return clip;
        }
        /// <summary>
        /// Path: Sounds/NPCVoice/ChengWing2/Language/<paramref name="soundFileName"/>/<paramref name="tmpvoice"/>
        /// </summary>
        public static AudioClip GetCSound(string soundFileName, string tmpvoice)
        {
            AudioClip npcVoice = Resources.Load<AudioClip>(
                    $"Sounds/NPCVoice/ChengWing2/{Localization.NpcVoiceLan()}/{soundFileName}/{tmpvoice}");
//            Debug.Log($"Sounds/NPCVoice/ChengWing2/{Localization.NpcVoiceLan()}/{soundFileName}/{tmpvoice}");
            if (npcVoice == null)
            {
                //fallback in case no other language
                npcVoice = Resources.Load<AudioClip>($"Sounds/NPCVoice/ChengWing2/HK/{soundFileName}/{tmpvoice}");
                Debug.Log("fall back to HK");
            }

            return npcVoice;
        }
        /// <summary>
        /// Path: Sounds/sfx/<paramref name="clipID"/>
        /// </summary>
        public static void PlaySfx(string clipID)
        {
            if (GetSfx(clipID) is null) return;
            AudioManager.Instance.PlaySFX(GetSfx(clipID));
        }
        /// <summary>
        /// Path: Sounds/BGM/<paramref name="clipID"/>
        /// </summary>
        public static void PlayBgm(string clipID)
        {
            if (GetBgm(clipID) == null) return;
            AudioManager.Instance.PlayMusic(GetBgm(clipID));
        }
        /// <summary>
        /// Path: Sounds/PlayCSound/<paramref name="clipID"/>
        /// </summary>
        public static void PlayCSound(string soundFileName, string tmpvoice)
        {
            Debug.Log($"Sounds/NPCVoice/ChengWing2/{Localization.NpcVoiceLan()}/{soundFileName}/{tmpvoice}");
            if (GetCSound(soundFileName,tmpvoice) == null) AudioManager.Instance.StopCSound();
            AudioManager.Instance.PlayCSound(GetCSound(soundFileName,tmpvoice));
        }
    }
}