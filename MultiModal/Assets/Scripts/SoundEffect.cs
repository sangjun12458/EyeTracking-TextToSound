using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Text;
using FMODUnity;
using Tobii.G2OM;
using Tobii.XR;


public class SoundEffect : MonoBehaviour, IGazeFocusable
{
    private TextMeshProUGUI textMeshPro;
    public Text indexText;
    static public List<string> quotes2 = new List<string> { "" };
    static public List<string> quotes = new List<string>
    {
        "어린왕자",
        // 어린왕자에서...
        "내 생활은 단조롭단다." + " " +
        "나는 닭을 쫓고 사람들은 나를 쫓지." + " " +
        "닭은 모두 그게 그거고, 사람들도 모두 그래." + " " +
        "그래서 난 좀 따분하지." + " " +
        "그러나 네가 나를 길들인다면 내 생활은 환해질 거야." + " " +
        "많은 발자국과 다른 발자국 소리를 알게 되겠지." + " " +
        "다른 발자국 소리에 나는 땅 밑으로 기어들겠지만 네 발자국 소리는 나를 굴 밖으로 불러내겠지!",
        "그리고 한 번 봐!" + " " +
        "저기 밀밭 보이지?" + " " +
        "난 빵은 먹지 않지." + " " +
        "밀은 내겐 아무 쓸모 없지." + " " +
        "밀밭을 보아도 아무 생각 없지." + " " +
        "그래서 서글퍼!" + " " +
        "그런데 네 머리칼은 금빛이지." + " " +
        "그러니 네가 나를 길들이면 정말 신나겠지!" + " " +
        "밀도 금빛이지." + " " +
        "너를 생각하게 되겠지." + " " +
        "그럼 난 밀밭을 스치는 바람 소리를 사랑하게 되겠지…",
        // 윤동주 서시...
        "서시" + " - 윤동주\n\n" +
        "죽는 날까지 하늘을 우러러\r\n한 점 부끄럼이 없기를,\r\n잎새에 이는 바람에도\r\n나는 괴로워했다.\r\n별을 노래하는 마음으로\r\n모든 죽어 가는 것을 사랑해야지\r\n그리고 나한테 주어진 길을\r\n걸어가야겠다.\r\n\r\n오늘 밤에도 별이 바람에 스치운다.\r\n",
        "테스트 문장",
        "작은 종소리가 멀리서 들려왔다. 그 소리는 점점 가까워지더니, 결국 내 바로 옆에서 울리는 것 같았다.",
        "폭포 소리는 가까워졌다가 멀어졌다가, 마치 나를 따라다니며 주변을 맴도는 듯한 느낌을 주었다.",
        "내가 숲을 거닐 때, 새들의 지저귐이 앞에서, 뒤에서, 양옆에서 들려왔다. 그 소리들은 마치 나를 따라다니는 듯했다.",
/*        // 유명한 문장
        "어른들은 누구나 처음에는 어린이였다. 그러나 그것을 기억하는 어른은 많지 않다.",
        "가장 중요한 것은 눈에 보이지 않는다.",
        "넌 영원히 네가 길들인 것에 대해 책임이 있어.",
        "별이 아름다운 것은 보이지 않는 꽃 한 송이 때문이야.",
        // 기본 소리 문장
        "별들은 너무나도 아름답고, 나는 별을 보면서 웃는다. 그러면 너도 별을 보면서 웃겠지. 그래서 우리 둘은 모두 웃게 될 거야!",
        "내가 별들 중 하나에서 웃고 있을 테니, 밤마다 너는 하늘의 모든 별을 보고 웃는 것처럼 느낄 거야. 그러면 너에게 있어 별들은 모두 웃는 별이 될 거야!",
        "너는 내가 사랑했던 꽃들을 밤하늘의 별처럼 볼 거야. 나는 하늘의 별들을 하나하나 듣고 있을 거야. 그럼 나는 네 목소리를 듣는 것 같을 거야.",
        "네 장미꽃은 그 많은 장미꽃들 중의 하나일 뿐이야. 그러나 내가 그 장미꽃에 물을 주었기 때문에, 네 장미꽃이 특별한 거야. 내가 네 장미꽃을 돌보며 들은 모든 소리가 그 장미꽃을 특별하게 만든 거야.",
        // 소리가 멀어지고 가까워지는 문장
        "작은 종소리가 멀리서 들려왔다. 그 소리는 점점 가까워지더니, 결국 내 바로 옆에서 울리는 것 같았다.",
        "그가 부르는 노래는 처음에는 바람을 타고 멀리서 들려왔지만, 나에게 점점 다가와 내 마음 속 깊이 스며들었다.",
        "폭포 소리는 가까워졌다가 멀어졌다가, 마치 나를 따라다니며 주변을 맴도는 듯한 느낌을 주었다.",
        // 소리가 주위를 맴도는 문장
        "별들이 조용히 속삭이는 소리가 밤하늘에서 나의 주위를 맴돌았다. 그 소리들은 마치 춤추듯이 나를 에워싸며 여기저기서 들려왔다.",
        "바람이 불 때마다 나뭇잎들이 부딪히는 소리가 좌우에서 들려왔고, 그 소리는 마치 나를 에워싸고 도는 것 같았다.",
        "내가 숲을 거닐 때, 새들의 지저귐이 앞에서, 뒤에서, 양옆에서 들려왔다. 그 소리들은 마치 나를 따라다니는 듯했다.",
        "그의 목소리는 공중에서 울려 퍼지며, 좌우로, 앞뒤로 움직였다. 그 소리는 마치 바람을 타고 나를 에워싸는 듯했다.",
        // 기타 소리와 관련된 문장
        "사람들이 별을 보고 무엇을 찾으려고 할 때, 별들은 그들에게 다른 의미로 다가온다. 별이 멀리서 희미하게 빛나는 소리를 들을 때, 그 소리는 그들에게 소중한 누군가의 목소리로 들릴 것이다.",
        "사막이 아름다운 것은, 그것이 어딘가에 우물을 감추고 있기 때문이야.",*/
    };
    static public int numOfQuotes = -1;
    public int indexOfQuote = 0;


    public AudioSource audioSource;
    public AudioSource bgmSource;
    public AudioSource continuousSoundEffect;
    public Transform orbitObject;
    private bool isContinuousSoundPlaying = false;
    private Dictionary<string, AudioClip> soundDictionary;
    private Dictionary<string, string> effectDictionary;

    private Coroutine currentCoroutine;
    private string currentCoroutineName;

    private StringBuilder csvContent;
    private float[] weights;
    private bool weightOn = false;

    private bool onlyOnce = true;

    public RectTransform canvasRectTransform; // 캔버스의 RectTransform
    public RectTransform imageRectTransform;  // 이동할 이미지의 RectTransform

    [FMODUnity.EventRef]
    public string fmodEventPath;

    //FMOD.Studio.EventInstance eventInstance;

    void Awake()
    {
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = quotes[indexOfQuote];
        indexText.text = indexOfQuote.ToString();
        numOfQuotes = quotes.Count;

        soundDictionary = new Dictionary<string, AudioClip>
        {
            { "종소리", Resources.Load<AudioClip>("TestSounds/bell") },
            { "바람", Resources.Load<AudioClip>("TestSounds/wind1") },
            { "우물", Resources.Load<AudioClip>("TestSounds/well") },
            { "폭포", Resources.Load<AudioClip>("TestSounds/waterfall") },
            { "지저귐", Resources.Load<AudioClip>("TestSounds/bird") },
            { "책", Resources.Load<AudioClip>("TestSounds/book_page") },
            { "닭", Resources.Load<AudioClip>("TestSounds/chicken") },
            { "발자국", Resources.Load<AudioClip>("TestSounds/footfall") },

            { "bgm0", Resources.Load<AudioClip>("TestSounds/bgm_0") },
            { "bgm1", Resources.Load<AudioClip>("TestSounds/bgm_1") },
            { "sad", Resources.Load<AudioClip>("TestSounds/sad_bgm") },
            { "night", Resources.Load<AudioClip>("TestSounds/ngiht_nature") },
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        continuousSoundEffect.Stop();
        // FMOD 이벤트 인스턴스 생성
        //eventInstance = RuntimeManager.CreateInstance(fmodEventPath);
        // 이벤트 재생
        //eventInstance.start();
    }

    void OnDestroy()
    {
        // 이벤트 정지 및 인스턴스 해제
        //eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        //eventInstance.release();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            audioSource.Stop();
            audioSource.volume = 1.0f;
            audioSource.clip = soundDictionary["책"];
            audioSource.loop = false;
            audioSource.Play();

            indexOfQuote = (indexOfQuote - 1 + numOfQuotes) % numOfQuotes;

            PlayBGM(indexOfQuote);

            textMeshPro.text = quotes[indexOfQuote];
            indexText.text = indexOfQuote.ToString();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            audioSource.Stop();
            audioSource.volume = 1.0f;
            audioSource.clip = soundDictionary["책"];
            audioSource.loop = false;
            audioSource.Play();

            indexOfQuote = (indexOfQuote + 1) % numOfQuotes;

            PlayBGM(indexOfQuote);

            textMeshPro.text = quotes[indexOfQuote];
            indexText.text = indexOfQuote.ToString();
        }

        // 눈 추적 데이터를 세계 공간에서 가져오기
        var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);

        CheckWord("");
        // GazeRay가 유효한지 확인
        if (false)//(eyeTrackingData.GazeRay.IsValid)
        {
            // GazeRay의 원점(시선의 위치)과 방향 벡터 가져오기
            var rayOrigin = eyeTrackingData.GazeRay.Origin;
            var rayDirection = eyeTrackingData.GazeRay.Direction;
            Vector3 mousePosition = Camera.main.WorldToScreenPoint(rayOrigin);
            int wordIndex = TMP_TextUtilities.FindIntersectingWord(textMeshPro, mousePosition, Camera.main);
            if (wordIndex != -1)
            {
                string word = textMeshPro.textInfo.wordInfo[wordIndex].GetWord();

                CheckWord(word);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                weightOn = !weightOn;
                if (weightOn)
                {
                    Debug.Log("Attention on");
                    weights = new float[textMeshPro.textInfo.wordCount];
                }
            }
            if (weightOn)
            {
                AccumulateWeight();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveAll();
            }

            MoveImageToMousePosition();
        }


    }

    void MoveImageToMousePosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        // 스크린 좌표를 캔버스 로컬 좌표로 변환
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, Camera.main, out localPoint))
        {
            // 이미지의 위치를 마우스 위치로 설정
            imageRectTransform.localPosition = localPoint + new Vector2(0, -50);
        }
    }

    public void CheckWord(string word)
    {
        foreach (var entry in soundDictionary)
        {
            if (word.Contains(entry.Key))
            {
                audioSource.clip = entry.Value;
                if (!audioSource.isPlaying)
                {
                    audioSource.volume = 0.5f;
                    audioSource.loop = false;
                    if (entry.Key != "종소리")
                        audioSource.Play();
                    if (entry.Key == "폭포")
                        audioSource.loop = true;
                }

            }
        }
       
        if (word.Contains("폭포"))
        {
            audioSource.clip = soundDictionary["폭포"];
            if (!audioSource.isPlaying)
            {
                audioSource.volume = 0.5f;
                audioSource.Play();
                audioSource.loop = true;
            }
        }
        if (word.Contains("지저귐"))
        {
            audioSource.clip = soundDictionary["지저귐"];
            if (!audioSource.isPlaying)
            {
                audioSource.volume = 1.0f;
                audioSource.Play();
                audioSource.loop = true;
            }
        }

        if (word.Contains("들려왔"))
        {
            audioSource.clip = soundDictionary["종소리"];
            if (!audioSource.isPlaying)
            {
                audioSource.volume = 0.1f;
                audioSource.Play();
                audioSource.loop = true;
            }
        }
        else if (word.Contains("불어올") || word.Contains("가까워"))
        {
            if (currentCoroutine != null)
            {
                return;
            }
            StartCoroutine(FadeIn(audioSource, 1.0f));
            audioSource.loop = false;
        }
        else if (word.Contains("멀어졌"))
        {
            if (currentCoroutine != null)
            {
                return;
            }
            StartCoroutine(FadeOut(audioSource, 1.0f));

        }
        else if (word.Contains("좌우")) //앞 뒤 좌 우 옆 주위 맴돌 에워싸 돌
        {
            // 좌에서 우로 이동
            if (currentCoroutine != null)
            {
                return;
            }
            currentCoroutine = StartCoroutine(MoveSoundEffect(audioSource, new Vector3(-7f, 0, 0), new Vector3(7f, 0, 0), 2.0f));
            // 우에서 좌로 이동
            //effectManager.TriggerMovementEffect(new Vector3(5f, 0, 0), new Vector3(-5f, 0, 0), 5f);

        }
        else if (word.Contains("앞"))
        {
            // 앞에서 뒤로 이동
            if (currentCoroutine != null)
            {
                return;
            }
            currentCoroutine = StartCoroutine(MoveSoundEffect(audioSource, new Vector3(0, 0, -15f), new Vector3(0, 0, 15f), 3.0f));
            // 뒤에서 앞으로 이동
            //effectManager.TriggerMovementEffect(new Vector3(0, 0, 5f), new Vector3(0, 0, -5f), 5f);

        }
        else if (word.Contains("에워싸") || word.Contains("맴도"))
        {
            if (currentCoroutine != null)
            {
                return;
            }
            StartCoroutine(OrbitAround(audioSource, 3.0f));

        }

        if (word.Contains("닭") )
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = soundDictionary["닭"];
                audioSource.volume = 0.5f;
                audioSource.Play();
                audioSource.loop = false;
            }
        }
        else if (word.Contains("발자국") || word.Contains("걸어"))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = soundDictionary["발자국"];
                audioSource.volume = 0.5f;
                audioSource.Play();
                audioSource.loop = false;
            }
        }
        else if (word.Contains("밀밭") )
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = soundDictionary["바람"];
                audioSource.volume = 1.0f;
                audioSource.Play();
                audioSource.loop = false;
            }
        }
        else if (word.Contains("별") )
        {
            if (onlyOnce)
            {
                onlyOnce = false;
                bgmSource.clip = soundDictionary["sad"];
                bgmSource.volume = 0.2f;
                bgmSource.Play();
                bgmSource.loop = true;
            }
        }
    }

    private void PlayBGM(int index)
    {
        bgmSource.Stop();
        bgmSource.volume = 0.1f;
        bgmSource.loop = true;
        if (index == 1)
        {
            bgmSource.clip = soundDictionary["bgm0"];
        }
        else if (index == 2)
        {
            bgmSource.clip = soundDictionary["bgm1"];
        }
        else if (index == 3)
        {
            bgmSource.clip = soundDictionary["night"];
        }
        bgmSource.Play();
    }

    public void StartFadeEffectSequence(string sequenceName)
    {
        // 같은 코루틴이 실행 중이면 새 코루틴을 시작하지 않음
        if (currentCoroutine != null && currentCoroutineName == sequenceName)
        {
            return;
        }

        // 다른 코루틴이 실행 중이면 중단
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // 새로운 코루틴 시작
        switch (sequenceName)
        {
            case "Sequence1":
                currentCoroutine = StartCoroutine(FadeIn(audioSource, 2.0f));
                currentCoroutineName = "Sequence1";
                break;
            case "Sequence2":
                currentCoroutine = StartCoroutine(FadeOut(audioSource, 2.0f));
                currentCoroutineName = "Sequence2";
                break;
        }
    }

    public void TriggerSoundEffect()
    {
        audioSource.Play();
    }

    public void TriggerFadeEffect(bool fadeIn, float duration)
    {
        if (fadeIn)
        {
            StartCoroutine(FadeIn(audioSource, duration));
        }
        else
        {
            StartCoroutine(FadeOut(audioSource, duration));
        }
    }

    public void TriggerOrbitEffect()
    {
        StartCoroutine(OrbitAround(audioSource, 2.0f));
    }

    public void StartContinuousSoundEffect(float duration)
    {
        if (!isContinuousSoundPlaying)
        {
            continuousSoundEffect.Play();
            StartCoroutine(FadeIn(continuousSoundEffect, duration));
            isContinuousSoundPlaying = true;
        }
    }

    public void StopContinuousSoundEffect(float duration)
    {
        if (isContinuousSoundPlaying)
        {
            StartCoroutine(FadeOut(continuousSoundEffect, duration));
        }
            isContinuousSoundPlaying = false;
    }

    public void TriggerMovementEffect(Vector3 startPos, Vector3 endPos, float duration)
    {
        // 이전 코루틴이 실행 중이면 중지
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        // 새로운 코루틴 시작
        currentCoroutine = StartCoroutine(MoveSoundEffect(audioSource, startPos, endPos, duration));

    }

    /*    private IEnumerator OrbitAround()
        {
            float duration = 5f; // Orbit for 5 seconds
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                orbitObject.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }*/
    private IEnumerator OrbitAround(AudioSource audioSource, float duration)
    {
        float elapsedTime = 0f;
        Transform audioTransform = audioSource.transform;
        Vector3 center = Vector3.zero; // Center of the orbit
        float radius = 5f; // Radius of the orbit
        float speed = 60f; // Speed of rotation

        while (elapsedTime < duration || audioSource.isPlaying)
        {
            float angle = elapsedTime * speed;
            float radian = angle * Mathf.Deg2Rad;
            audioTransform.position = new Vector3(center.x + Mathf.Cos(radian) * radius, center.y, center.z + Mathf.Sin(radian) * radius);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the position to center after orbiting
        audioTransform.position = center;
        
        currentCoroutine = null;
    }

    private IEnumerator MoveSoundEffect(AudioSource audioSource, Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0f;
        Transform audioTransform = audioSource.transform;

        audioTransform.position = startPos;
        audioSource.Play();

        while (elapsedTime < duration)
        {
            audioTransform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the audio source reaches the end position
        audioTransform.position = endPos;
        currentCoroutine = null;
    }
    /*    private IEnumerator PanLeftToRight(AudioSource audioSource, float duration)
        {
            float elapsedTime = 0f;
            float startPan = -1f; // Fully left
            float endPan = 1f; // Fully right

            audioSource.panStereo = startPan;
            audioSource.Play();

            while (elapsedTime < duration)
            {
                audioSource.panStereo = Mathf.Lerp(startPan, endPan, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the panStereo reaches the end value
            audioSource.panStereo = endPan;
        }*/

    private IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 1, currentTime / duration);
            yield return null;
        }

        audioSource.volume = 1;

        currentCoroutine = null;
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float startVolume = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / duration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();

        currentCoroutine = null;
    }

    void CheckWordInQuote(string word)
    {
        if (word.Contains("바람"))
        {
            TriggerSoundEffect();
        }

        if (word.Contains("작은 종소리가 멀리서 들려왔다"))
        {
            TriggerFadeEffect(true, 2.0f);
        }

        if (word.Contains("그가 부르는 노래는 처음에는 바람을 타고"))
        {
            TriggerOrbitEffect();
        }

        if (word.Contains("폭포 소리는 가까워졌다가 멀어졌다가"))
        {
            StartContinuousSoundEffect(2.0f);
        }

        if (word.Contains("사막이 아름다운 것은"))
        {
            StopContinuousSoundEffect(2.0f);
        }
    }

    private void AccumulateWeight()
    {
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        Vector2 mousePosition = Input.mousePosition;
        int wordIndex = TMP_TextUtilities.FindIntersectingWord(textMeshPro, mousePosition, Camera.main);
        if (wordIndex == -1) return;
        weights[wordIndex] += Time.deltaTime;
    }

    private void SaveAll()
    {
        string csvFileName = "true_attention.csv"; // CSV 파일 이름

        // 텍스트 메쉬 프로에서 텍스트의 모든 단어와 단어 좌표를 가져오기
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        int wordCount = textInfo.wordCount; // 텍스트의 단어 수

        // CSV 파일에 쓸 데이터를 저장할 StringBuilder 초기화
        csvContent = new StringBuilder();
        csvContent.AppendLine("Word,PositionX,PositionY,Attention");

        // 모든 단어 반복
        for (int i = 0; i < wordCount; i++)
        {
            TMP_WordInfo wordInfo = textInfo.wordInfo[i];

            // 단어의 첫 번째 문자와 마지막 문자의 좌표 가져오기
            Vector3 wordWorldPositionStart = textInfo.characterInfo[wordInfo.firstCharacterIndex].bottomLeft;
            Vector3 wordWorldPositionEnd = textInfo.characterInfo[wordInfo.lastCharacterIndex].topRight;

            // 단어의 중앙 좌표를 계산합니다.
            Vector3 wordWorldPosition = (wordWorldPositionStart + wordWorldPositionEnd) / 2f;

            // 단어의 화면 좌표를 가져옵니다.
            Vector3 wordScreenPosition = Camera.main.WorldToScreenPoint(wordWorldPositionStart);

            // CSV 데이터에 추가
            string csvLine = $"{textMeshPro.text.Substring(wordInfo.firstCharacterIndex, wordInfo.characterCount)},{wordScreenPosition.x},{wordScreenPosition.y},{weights[i]}";
            csvContent.AppendLine(csvLine);
        }

        // CSV 파일에 데이터 쓰기
        string filePath = Path.Combine(Application.dataPath, csvFileName);
        File.WriteAllText(filePath, csvContent.ToString());

        Debug.Log("CSV 파일이 저장되었습니다: " + filePath);
    }

    public void GazeFocusChanged(bool hasFocus)
    {
        //If this object received focus, fade the object's color to highlight color
        if (hasFocus)
        {
 
        }
        //If this object lost focus, fade the object's color to it's original color
        else
        {

        }
    }
}
