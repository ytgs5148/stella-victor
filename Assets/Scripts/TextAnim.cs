using System.Collections;
using UnityEngine;
using TMPro;

public class TextAnim : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;

    public string[] stringArray;

    [SerializeField] float timeBtwnChars;
    [SerializeField] float timeBtwnWords;

    int i = 0;

    void Start()
    {
        EndCheck();
    }

    public void EndCheck()
    {
        if (i <= stringArray.Length - 1)
        {
            _textMeshPro.text = stringArray[i];
            StartCoroutine(TextVisible());
        }
        else
        {
            PlayerData.Instance.endingBar = 0f;
            FindFirstObjectByType<LoadingScreenManager>().LoadScene(0);
        }
    }

    private IEnumerator TextVisible()
    {
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            _textMeshPro.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                i += 1;
                Invoke("EndCheck", timeBtwnWords);
                break;
            }

            counter += 1;
            yield return new WaitForSeconds(timeBtwnChars);
        }
    }
}
