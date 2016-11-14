using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {
    [SerializeField] private string AndroidId = "1181015";

    private void Start()
    {
        Advertisement.Initialize(AndroidId, true);
    }

    public void DisplayAdVideo()
    {
        if (Advertisement.IsReady("video") && !Advertisement.isShowing)
        {
            Advertisement.Show("video", new ShowOptions { resultCallback = HandleClosingOfAd });
        }
    }

    private void HandleClosingOfAd(ShowResult result)
    {
        Debug.Log(string.Format("{0}", result.ToString()));
    }
}
