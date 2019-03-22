using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigContext : BaseContext {
    public ConfigContext() : base(UIType.Config) {

    }
}

public class ConfigView : AnimatorView {
    public Slider musicVolumeSlider;
    public Text text;

    public override void OnEnter() {
        base.OnEnter();
    }
    public override void OnExit() {
        base.OnExit();
    }
    public override void OnPause(BaseContext nextContext) {
        base.OnPause(nextContext);
    }
    public override void OnResume() {
        base.OnResume();
    }

    public void BackCallBack() {
        Pop();
    }

    public void SetValueCallBack() {
        float value = musicVolumeSlider.value;
        text.text = value.ToString();
        SetMusicVolume(value / musicVolumeSlider.maxValue);
    }

    public void SetMusicVolume(float volume) {
        FullMusicManager.Instance.SetVolume(volume);
    }

}
