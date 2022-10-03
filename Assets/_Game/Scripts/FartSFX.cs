using AlmenaraGames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(FartManager))]
public class FartSFX : MonoBehaviour
{
    public AudioObject lowSFX = null;
    public AudioObject mediumSFX = null;
    public AudioObject highSFX = null;

    public float highThreshold = 10.0f;

    private FartManager fartManager = null;

    private void Awake()
    {
        fartManager = GetComponent<FartManager>();
        fartManager.onFart.AddListener(x => 
        {
            Debug.Log("Fart power : " + x.FartMultiplier);

            if (x.FartMultiplier < fartManager.breakGlassValue)
                MultiAudioManager.PlayAudioObject(lowSFX, transform);
            else if (x.FartMultiplier >= fartManager.breakGlassValue && x.FartMultiplier < highThreshold)
                MultiAudioManager.PlayAudioObject(mediumSFX, transform);
            else
                MultiAudioManager.PlayAudioObject(highSFX, transform);
        });
    }
}
