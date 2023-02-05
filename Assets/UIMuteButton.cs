using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIMuteButton : MonoBehaviour
{
    private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private AudioMixer _mixer;
    private AudioMixerSnapshot[] _snapshots;
    private float[][] _weights;
    private int _muteState = 0;

    [SerializeField] private Sprite soundOnIcon;
    [SerializeField] private Sprite muteAllIcon;
    [SerializeField] private Sprite muteMusicIcon;

    private Sprite[] _buttonIcons;
    // Start is called before the first frame update
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Click);
    }

    private void Start()
    {
        var narOnly = _mixer.FindSnapshot("NarrationOnly");
        var muteAll = _mixer.FindSnapshot("MuteAll");
        var both = _mixer.FindSnapshot("Snapshot");
        _snapshots = new[]
        {
            both,
            narOnly,
            muteAll
        };
        _weights = new[]
        {
            new[] { 1f, 0, 0 },
            new[] { 0, 1f, 0 },
            new[] { 0, 0, 1f }
        };
        _buttonIcons = new[]
        {
            soundOnIcon,
            muteMusicIcon,
            muteAllIcon
        };
    }

    void Click()
    {
        Debug.Log("Cycle Mute State");
        _muteState++;
        if (_muteState >= _snapshots.Length)
        {
            _muteState = 0;
        }

        _image.sprite = _buttonIcons[_muteState];
        _mixer.TransitionToSnapshots(_snapshots,_weights[_muteState],0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
