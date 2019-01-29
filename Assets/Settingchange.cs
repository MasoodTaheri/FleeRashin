using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settingchange : MonoBehaviour
{
    public Animator Musicanim;
    public Animator SFXanim;
    public bool Music;
    public bool SFX;
    public AudioMixerGroup AMGmusic;
    public AudioMixerGroup sfx;
    float MusicMinValue = -80;
    float MusicMaxValue = -7;
    float sfxMinValue = -80;
    float sfxMaxValue = -10;


    // Use this for initialization
    void Start()
    {
        init();
    }

    void OnEnable()
    {
        init();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Musicchange()
    {
        Music = !Music;
        if (Musicanim != null)
            SettingChange(Musicanim, "on", Music);
        PlayerPrefs.SetInt("music", Music ? 1 : 0);

    }
    public void Sfxchange()
    {
        SFX = !SFX;
        if (SFXanim != null)
            SettingChange(SFXanim, "on", SFX);
        PlayerPrefs.SetInt("sfx", SFX ? 1 : 0);
    }

    public void init()
    {
        Music = PlayerPrefs.GetInt("music",1) == 1;
        SFX = PlayerPrefs.GetInt("sfx", 1) == 1;
        //if (Musicanim != null)
            SettingChange(Musicanim, "on", Music);

        //if (SFXanim != null)
            SettingChange(SFXanim, "on", SFX);

    }


    void SettingChange(Animator anim, string id, bool value)
    {
        if (anim != null)
            anim.SetBool(id, value);
        AMGmusic.audioMixer.SetFloat("MusicVol", Music ? MusicMaxValue : MusicMinValue);
        sfx.audioMixer.SetFloat("SfxVol", SFX ? sfxMaxValue : sfxMinValue);
    }

}
