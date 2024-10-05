using UnityEngine;
using Object = UnityEngine.Object;

namespace LiquorStore;

public class Radio : MonoBehaviour
{
    public AudioSource speaker;
    public AudioSource ch;

    private void Start()
    {
        this.ch = ((IEnumerable<AudioSource>) Resources.FindObjectsOfTypeAll<AudioSource>()).First<AudioSource>((Func<AudioSource, bool>) (x => ((Object) ((Component) x).gameObject).name == "Channel1"));
        this.speaker.clip = this.ch.clip;
        this.speaker.Play();
    }

    private void Update()
    {
        if (!(((Object) this.speaker.clip).name != ((Object) this.ch.clip).name))
            return;
        this.speaker.clip = this.ch.clip;
        this.speaker.Play();
        this.speaker.timeSamples = this.ch.timeSamples;
    }
}