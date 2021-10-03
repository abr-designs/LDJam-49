using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRepeater : MonoBehaviour
{
    [SerializeField]
    private AudioController.EFFECT soundEffect;
    [SerializeField]
    private float delayTime;

    [SerializeField]
    private int playCount;

    private int _countdown;
    private bool _playForever;

    private float _timer;
    // Start is called before the first frame update
    private void Start()
    {
        _timer = delayTime;

        _playForever = playCount == 0;
        _countdown = playCount;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
            return;
        }

        _timer = delayTime;
        AudioController.Instance.PlaySoundEffect(soundEffect);

        if (_playForever) 
            return;
        _countdown--;
            
        if (_countdown > 0) 
            return;
            
        _countdown = playCount;
        enabled = false;
    }
}
