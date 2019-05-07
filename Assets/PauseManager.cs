using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public void setPauseEnabled(bool inPauseEnabled) {
        //TODO: Implement pause
        //NB: Settings up timeScale for objects with physics make
        // FixedUpdate to rare (each 3-4 seconds, for example)
        // witch make updating of Overlaps caches too rare too
        // witch, for example, make buildings placement tests
        // update to rare...
        // In other hand, "timeScale = 0" looks like the only one
        // easy way to pause physic based worlds.
        //
        // Maybe, use separate physics scene for objects that
        // overlaps should be updated in this case

        //Time.timeScale = inPauseEnabled ? 0.0f : 1.0f;
    }
}
