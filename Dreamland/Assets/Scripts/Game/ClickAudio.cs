using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAudio : MonoBehaviour {

    private ManagerVars vars;
    private AudioSource audioSource;

    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        audioSource = GetComponent<AudioSource>();

        EventCenter.AddListener(EventDefine.PlayClickAudio, PlayClickAudio);
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.PlayClickAudio, PlayClickAudio);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
    }

    /// <summary>
    /// 播放按钮点击音效
    /// </summary>
    private void PlayClickAudio()
    {
        audioSource.PlayOneShot(vars.btn);
    }

    /// <summary>
    /// 音效是否开启
    /// </summary>
    /// <param name="value"></param>
    private void IsMusicOn(bool value)
    {
        audioSource.mute = !value;
    }
}
