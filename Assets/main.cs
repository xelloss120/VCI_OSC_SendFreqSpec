using UnityEngine;
using TMPro;

public class main : MonoBehaviour
{
    [SerializeField] TMP_Dropdown Device;
    [SerializeField] TMP_InputField LengthSec;
    [SerializeField] TMP_InputField Frequency;
    [SerializeField] AudioSource AudioSource;
    [SerializeField] LineRenderer LineRenderer;

    const int LEN = 4096;
    float[] Spectrum = new float[LEN];

    void Start()
    {
        foreach (var device in Microphone.devices)
        {
            var option = new TMP_Dropdown.OptionData();
            option.text = device;
            Device.options.Add(option);
        }

        for (int i = 0; i < LEN; i++)
        {
            var vec3 = new Vector3(Mathf.Log(i + 1), 0, 0);
            LineRenderer.SetPosition(i, vec3);
        }

        Apply();
    }

    void Update()
    {
        AudioSource.GetSpectrumData(Spectrum, 0, FFTWindow.Rectangular);
        for (int i = 0; i < LEN; i++)
        {
            var vec3 = LineRenderer.GetPosition(i);
            vec3.y = Spectrum[i] == 0 ? 0 : Mathf.Log(Spectrum[i], 10000);
            LineRenderer.SetPosition(i, vec3);
        }
    }

    void Apply()
    {
        string device;
        int lengthSec;
        int frequency;

        device = Device.options[Device.value].text;
        int.TryParse(LengthSec.text, out lengthSec);
        int.TryParse(Frequency.text, out frequency);

        AudioSource.clip = Microphone.Start(device, true, lengthSec, frequency);
        while (Microphone.GetPosition(null) <= 0) { }
        AudioSource.Play();
    }

    public void OnClick()
    {
        Apply();
    }
}
