using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPeer : MonoBehaviour {

    public List<AudioSource> audioSources = new List<AudioSource>();
    public bool useBuffer = true;

    private List<float[]> samplesList = new List<float[]>();
    private float[] freqBands = new float[8];
    private float[] freqBandBuffers = new float[8];
    private float[] bufferDecrease = new float[8];

    private float[] freqBandsHighest = new float[8];
    private float[] audioBands = new float[8];
    private float[] audioBandBuffers = new float[8];

    // 볼륨 변수를 RGB 값으로 저장
    public Vector3 soundVolume = Vector3.one;
    public float temp;
    public Slider xSlider; // X 축 슬라이더
    public Slider ySlider; // Y 축 슬라이더
    public Slider zSlider; // Z 축 슬라이더

    private void Start()
    {
        // 각 슬라이더의 값을 변경할 때마다 볼륨을 업데이트하기 위한 이벤트 추가
        xSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        ySlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        zSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }

    void Update() {
        if (audioSources.Count == 0) {
            return;
        }
        ReadSamples();
        CalculateFrequencyBands();
        if (useBuffer) {
            BufferFrequencyBands();
        }
        CalculateAudioBands();

        SetSoundVolume(soundVolume);
    }

    private void ReadSamples() {
        int sourceIndex = 0;
        foreach (AudioSource audioSource in audioSources) {
            float[] samples;
            if (samplesList.Count < sourceIndex + 1) {
                samples = new float[512];
                samplesList.Add(samples);
            } else {
                samples = samplesList[sourceIndex];
            }
            sourceIndex++;
            audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
        }
    }
    private float GetMaxSampleAtPosition(int sampleIndex) {
        float maxSample = 0;
        foreach(float[] samples in samplesList) {
            float sample = samples[sampleIndex];
            if (sample > maxSample) {
                maxSample = sample;
            }
        }
        return maxSample;
    }

    public void CalculateFrequencyBands() {
        int totalSampleCount = 0;
        for (int freqIndex = 0; freqIndex < freqBands.Length; freqIndex++) {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, freqIndex) * 2;

            if (freqIndex == 7) {
                sampleCount += 2;
            }
            for (int sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++) {
                average += GetMaxSampleAtPosition(totalSampleCount) * (totalSampleCount + 1);
                totalSampleCount++;
            }

            average /= totalSampleCount;

            freqBands[freqIndex] = average;
        }
    }

    private void BufferFrequencyBands() {
        for (int freqIndex = 0; freqIndex < freqBands.Length; freqIndex++) {
            // New Maximum reached
            if (freqBands[freqIndex] > freqBandBuffers[freqIndex]) {
                freqBandBuffers[freqIndex] = freqBands[freqIndex];
                bufferDecrease[freqIndex] = 0.005f;
            } else if (freqBands[freqIndex] < freqBandBuffers[freqIndex]) {
                freqBandBuffers[freqIndex] -= bufferDecrease[freqIndex];
                bufferDecrease[freqIndex] *= 1.2f;
            }
        }
    }

    private void CalculateAudioBands() {
        for (int freqIndex = 0; freqIndex < freqBands.Length; freqIndex++) {
            if (freqBands[freqIndex] > freqBandsHighest[freqIndex]) {
                freqBandsHighest[freqIndex] = freqBands[freqIndex];
            }
            audioBands[freqIndex] = freqBands[freqIndex] / freqBandsHighest[freqIndex];
            audioBandBuffers[freqIndex] = freqBandBuffers[freqIndex] / freqBandsHighest[freqIndex];
        }
    }

    public float GetSample(int sampleIndex) {
        return GetMaxSampleAtPosition(sampleIndex);
    }
    public float GetFrequencyBand(int freqIndex) {
        return useBuffer ? freqBandBuffers[freqIndex] : freqBands[freqIndex];
    }

    // Normalized frequency band between 0 and 1
    public float GetAudioBand(int freqIndex) {
        return audioBands[freqIndex];
    }

    // 각 사운드의 볼륨 변수를 반환
    public Vector3 GetSoundVolume()
    {
        return soundVolume;
    }

    // 각 사운드의 볼륨 변수를 설정
    public void SetSoundVolume(Vector3 volume)
    {
        soundVolume = volume;
        // 설정된 볼륨을 모든 오디오 소스에 적용
        int i = 0;
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = volume[i];
            i++;
        }
    }

    void UpdateVolume()
    {
        // X, Y, Z 축의 슬라이더 값을 이용하여 볼륨을 업데이트
        soundVolume = new Vector3(xSlider.value, ySlider.value, zSlider.value);
        // 설정된 볼륨을 모든 오디오 소스에 적용
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = (soundVolume.x + soundVolume.y + soundVolume.z) / 3f; // 평균 값을 사용
        }
    }
}
