using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyAudioManager : MonoBehaviour
    {
        public List<AudioClip> enemySounds;
        public List<AudioClip> enemyFootSounds;

        public AudioSource leftFootAudioSource;
        public AudioSource rightFootAudioSource;

        public void PlayLeftFootSound()
        {
            int randomSound = Random.Range(0, enemyFootSounds.Count);

            leftFootAudioSource.clip = enemyFootSounds[randomSound];
            leftFootAudioSource.Play();
        }
        
        public void PlayRightFootSound()
        {
            int randomSound = Random.Range(0, (int)enemyFootSounds.Count);

            rightFootAudioSource.clip = enemyFootSounds[randomSound];
            rightFootAudioSource.Play();
        }
    }
}
