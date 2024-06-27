using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance != null)
                {
                    GameObject obj = new GameObject("AudioManager");
                    instance = obj.AddComponent<AudioManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    public string audioFilePath = "Assets/Texts/TheLittlePrince_sounds.txt";
    private AudioSource audioSource;
    private List<AudioClip> audioClips = new List<AudioClip>();
    private Dictionary<string, AudioClip> audioClipsDict = new Dictionary<string, AudioClip>(); // 오디오 클립을 저장할 딕셔너리
    //private int totalAudioClips;

    public AudioMixer audioMixer;
    public UnityEvent OnSoundComplete;

    public AudioSource[] audioSources;
    private int currentSourceIndex = 0;

    //private bool isPlaying = false;

    public float minVolume = 0f;
    public float maxVolume = 1f;

    public string mixerParameter0;
    public string mixerParameter1;
    public string mixerParameter2;
    public string mixerParameter3;
    public string exposedParameterName = "TotalVolume";
    private bool isMuted = false;


    private float[] allWeights;
    private int[] sentencesTracked = new int[4];
    private float[] volumes = new float[4];


    private void Awake()
    {
        // 기존 인스턴스가 있으면 이 오브젝트를 제거
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 유지되도록 설정
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        LoadAudioClips();
    }
    // Start is called before the first frame update
    void Start()
    {
        allWeights = new float[audioClips.Count];
        for (int i = 0; i < audioClips.Count; i++)
            allWeights[i] = 0;

        for (int i = 0; i < 4; i++)
        {
            volumes[i] = 0;
            sentencesTracked[i] = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isPlaying && !audioSource.) { }
        //OnSoundComplete.Invoke();
        //UpdateMixerVolumes();



    }

    void LoadAudioClips()
    {
        string[] lines = File.ReadAllLines(audioFilePath);

        foreach (string line in lines) 
        {
            string path = "Sounds/" + Path.GetFileNameWithoutExtension(line);
            //string path = "Assets/Sounds/" + line.Trim();
            AudioClip clip = Resources.Load<AudioClip>(path);
            if (clip != null)
            {
                audioClips.Add(clip);
                //audioClips[path] = clip;
            }
            else
            {
                Debug.LogWarning("오디오 클립을 로드할 수 없습니다: " + path);
            }
        }
    }

    public void PlaySound(int index)
    {
        if (audioSources == null) Debug.Log("There is no source");
        else
        {
            currentSourceIndex = (currentSourceIndex + 1) % audioSources.Length;
            audioSources[currentSourceIndex].PlayOneShot(audioClips[index]);
            Debug.Log(index);
        }
    }

    // 상호작용 텍스트로 부터 사운드 재생할 인덱스와 가중치를 부여받음
    public void PassParameter(int[] indices, float[] weights)
    {
        for (int i = 0; i < indices.Length; i++)
        {
            int newIndex = indices[i] + FileReader.targetIndex;
            volumes[i] = weights[i];

            if (newIndex != sentencesTracked[i])
            {
                sentencesTracked[i] = newIndex;
                PlayAudioClip(i, newIndex);
            }
        }
    }

    // 기존의 사운드 1개 재생 함수
    private void PlayAudioClip(int sourceIndex, int clipIndex)
    {
        Debug.Log("클립인덱스: " + clipIndex.ToString());
        Debug.Log("개수: " + audioClips.Count.ToString());
        if (clipIndex < audioClips.Count)
        {
            Debug.Log("소스인덱스: " + sourceIndex.ToString());
            AudioClip clip = audioClips[clipIndex];
            audioSources[sourceIndex].clip = clip;
            if (!audioSources[sourceIndex].isPlaying)
                audioSources[sourceIndex].Play();
            Debug.Log("오디오 재생: " + clipIndex);
        }
        else
        {
            Debug.LogWarning("해당 번호의 오디오 클립을 찾을 수 없습니다: " + clipIndex);
        }
    }

    // 기존의 사운드 여러개 재생 함수
    public void PlayAudioClipDict(string path)
    {
        if (audioClipsDict.ContainsKey(path))
        {
            AudioClip clip = audioClipsDict[path];
            audioSource.clip = clip;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            Debug.LogWarning("해당 경로의 오디오 클립을 찾을 수 없습니다: " + path);
        }
    }

    // 가중치에 따른 볼륨 조절 함수
    private void AdjustVolume(float[] weights)
    {
        // 각 AudioSource에 대해 가중치 값을 이용하여 볼륨 조절
        for (int i = 0; i < Mathf.Min(weights.Length, 4); i++)
        {
            volumes[i] = weights[i];
        }
    }

    // 각 믹서 채널에 대한 볼륨 계산
    private void UpdateMixerVolumes()
    {
        float volume0 = Mathf.Clamp01(volumes[0]);
        float volume2 = Mathf.Clamp01(volumes[1]);
        float volume1 = Mathf.Clamp01(volumes[2]);
        float volume3 = Mathf.Clamp01(volumes[3]);

        audioMixer.SetFloat(mixerParameter0, Mathf.Log10(volume0) * 20); // 데시벨로 변환
        audioMixer.SetFloat(mixerParameter1, Mathf.Log10(volume1) * 20);
        audioMixer.SetFloat(mixerParameter2, Mathf.Log10(volume2) * 20);
        audioMixer.SetFloat(mixerParameter3, Mathf.Log10(volume3) * 20);
    }

    // 음소거 기능. 사운드 재생을 멈추는 방식이 아닌 소리를 0으로 만드는 방식
    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            // 뮤트를 켭니다.
            audioMixer.SetFloat(exposedParameterName, -80f); // 뮤트 파라미터 값을 -80으로 설정하여 소리를 끔
        }
        else
        {
            // 뮤트를 끕니다.
            audioMixer.SetFloat(exposedParameterName, 0f); // 뮤트 파라미터 값을 0으로 설정하여 소리를 켬
        }
    }
}
