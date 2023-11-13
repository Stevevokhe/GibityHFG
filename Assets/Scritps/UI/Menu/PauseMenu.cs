using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Pause() =>
        Time.timeScale = 0;

    public void UnPause() => 
        Time.timeScale = 1;
}
