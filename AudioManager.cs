using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static readonly string FX_VOL = "EffectsVolume",
        BGM_VOL = "MusicVolume",
        AMB_VOL = "AmbianceVolume";
    public static AudioManager instance;
    private Settings settings;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource ambiancePlayer;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioSource fxPlayer;

    private bool isChasing = false;
    public static List<AudioClip> availableFX = new List<AudioClip>();
    public static List<AudioClip> availableMusic = new List<AudioClip>();
    public static List<AudioClip> availableAmb = new List<AudioClip>();

    public void SwitchToChaseMusic()
    {
        if (isChasing == true) return;
        isChasing = true;
        var chaseClip = availableMusic.Find(x => x.name.Equals("bossChase"));
        if (chaseClip == null) throw new UnityException("Cannot find boss chase music");
        musicPlayer.PlayOneShot(chaseClip, 0.5f);
    }

    IEnumerator PlayRandomAmbiance()
    {
        if (isChasing) yield break;
        var randomClip = availableAmb[Random.Range(0, availableAmb.Count)];
        ambiancePlayer.PlayOneShot(randomClip);
        Debug.Log("Playing random ambiance");
        yield return new WaitForSeconds(Random.Range(2f, 10f));
    }



    void Awake()
    {
        settings = GameSettings.ReadSettingsFromFile();
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        availableFX = new List<AudioClip>(Resources.LoadAll<AudioClip>("Sounds/Effects"));
        availableMusic = new List<AudioClip>(Resources.LoadAll<AudioClip>("Sounds/Music"));
        availableAmb = new List<AudioClip>(Resources.LoadAll<AudioClip>("Sounds/Ambiance"));
        StartCoroutine(PlayRandomAmbiance());
    }

    public static float A(int i)
    {
        float f = (float)i / 100f;
        return (Mathf.Log10(f) * 20);
    }

    void Update()
    {
        GameSettings.UpdateSettings(out this.settings);
        var res1 = audioMixer.SetFloat(FX_VOL, A(settings.fxVolume));
        var res2 = audioMixer.SetFloat(BGM_VOL, A(settings.musicVolume));
        var res3 = audioMixer.SetFloat(AMB_VOL, A(settings.ambianceVolume));


        var chaseClip = availableMusic.Find(x => x.name.Equals("bossChase"));
        if (chaseClip == null) throw new UnityException("Cannot find boss chase music");
        if (musicPlayer.clip == chaseClip)
        {
            if (!isChasing) musicPlayer.Stop();
        }
    }
}
