using UnityEngine;

public class FrogSound : MonoBehaviour
{
  [Header("Audio Sources")]
  [SerializeField] private AudioSource _audioSource;

  [Header("Audio Clips")]
  [SerializeField] private AudioClip[] _clips;

  [Header("Audio Settings")]
  [SerializeField] private float _sfxVolume = 1f;
  [SerializeField] private float _sfxRandomness = 0.2f;

  public void PlayRandomSound()
  {
    AudioClip randomClip = _clips[Random.Range(0, _clips.Length)];
    float randomPitch = Random.Range(_sfxVolume - _sfxRandomness, _sfxVolume + _sfxRandomness);
    _audioSource.PlayOneShot(randomClip, randomPitch);
  }
}