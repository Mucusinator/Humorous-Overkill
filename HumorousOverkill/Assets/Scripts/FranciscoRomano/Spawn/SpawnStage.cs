using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FR
{
    [System.Serializable]
    public class SpawnStage
    {
        // :: variables
        public int index;
        public SpawnWave wave;
        public List<SpawnWave> waves;
        // :: initializers
        public SpawnStage() : this(0, new List<SpawnWave>()) {}
        public SpawnStage(SpawnStage other) : this(other.index, new List<SpawnWave>(other.waves)) {}
        public SpawnStage(int index, List<SpawnWave> waves)
        {
            // initialize
            this.wave = null;
            this.index = index;
            this.waves = waves;
        }
        // :: class functions
        public void Reset()
        {
            // reset stage
            index = 0;
            wave = new SpawnWave(waves[0]);
        }
        public void NextWave()
        {
            // check if depleted
            if (IsStageEmpty()) return;
            // change current wave
            wave = new SpawnWave(waves[index++]);
        }
        public bool IsWaveEmpty()
        {
            // check if depleted
            return wave.IsEmpty();
        }
        public bool IsStageEmpty()
        {
            // check if depleted
            return IsWaveEmpty() && (index + 1) >= waves.Count;
        }
        public GameObject CreateUnit(Quaternion rotation, Transform parent)
        {
            // create unit
            return CreateUnit(wave, rotation, parent);
        }
        public GameObject CreateUnit(SpawnWave wave, Quaternion rotation, Transform parent)
        {
            // create unit
            GameObject instance = wave.CreateUnit(rotation, parent);
            // return instance
            return instance;
        }
    }
}