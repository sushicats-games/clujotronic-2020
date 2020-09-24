using TMPro;
using UnityEngine;

public class ObjectiveMessageAdapter : MonoBehaviour
{
    public TMP_Text text;

    string strCurrent = null;
    string strNext = null;
    float timeout = 0;

    // Start is called before the first frame update
    void Start()
    {
        text.text = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (strNext != null)
        {
            timeout -= Time.deltaTime;
            if (timeout <= 0)
            {
                timeout = 0;
                text.color = new Color(1.0f, 1.0f, 1.0f);
                text.text = strNext;
                strCurrent = strNext;
                strNext = null;
            }
        }
    }

    internal void Completed(string objectiveMessage)
    {
        text.color = new Color(.7f, 1.0f, .6f);
        text.text = objectiveMessage;
        strCurrent = objectiveMessage;
    }

    internal void NewObjective(string objectiveMessage)
    {
        strNext = objectiveMessage;
        timeout = strCurrent == null ? 0 : 0.5f;
    }
}
