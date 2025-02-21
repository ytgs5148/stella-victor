using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public GameObject ending1; //emperor
    public GameObject ending2;
    public GameObject ending3;
    public GameObject ending4;
    public GameObject ending5; //savior

    public void OnButtonClick()
    {
        DisableAllEndings();

        float endingBar = PlayerData.Instance.endingBar;

        if (endingBar >= 3.5f)
            ending5.SetActive(true);
        else if (endingBar >= 1.5f && endingBar < 3.5f)
            ending4.SetActive(true);
        else if (endingBar > -1.5f && endingBar < 1.5f)
            ending3.SetActive(true);
        else if (endingBar > -3.5f && endingBar <= -1.5f)
            ending2.SetActive(true);
        else if (endingBar <= -3.5f)
            ending1.SetActive(true);
        else
            Debug.LogWarning("Invalid endingBar value");
    }

    private void DisableAllEndings()
    {
        ending1.SetActive(false);
        ending2.SetActive(false);
        ending3.SetActive(false);
        ending4.SetActive(false);
        ending5.SetActive(false);
    }
}
