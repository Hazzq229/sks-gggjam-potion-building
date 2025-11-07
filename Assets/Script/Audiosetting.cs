using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Audiosetting : MonoBehaviour
{
    [SerializeField] AudioMixer audiomixxer;
    [SerializeField] Slider audioslide;
    [SerializeField] Slider sfxslide;
   
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Musicvolume"))
        {
            loadvolume();
        }
        if (PlayerPrefs.HasKey("sfxvolume"))
        {
            loadsfx();
        }
        else
        {

        }
        setsfx();
        setvolume();
    }
    public void setvolume()
    {
        float volume = audioslide.value;
        audiomixxer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Musicvolume", volume);

    }

    public void setsfx()
    {
        float volume = audioslide.value;
        audiomixxer.SetFloat("sfx", Mathf.Log10(volume) * 20);
           PlayerPrefs.SetFloat("sfxvolume", volume);

    }
    private void loadvolume()
    {
        audioslide.value = PlayerPrefs.GetFloat("MusicVolume");
        setvolume();

    }
    private void loadsfx()
    {
        
        audioslide.value = PlayerPrefs.GetFloat("sfxvolume");
        setsfx();

    
}
    // Update is called once per frame
    void Update()
    {
        
    }
}
