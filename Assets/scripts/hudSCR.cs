using TMPro;
using UnityEngine;

public class hudSCR : MonoBehaviour
{
    //odkay na gameObject textu, potreba priradit v inspectoru
    public TextMeshProUGUI TextScore;
    //verejna promena score, static pro jednodusi pristup v ostatnich scriptech
    public static int score = 0;

    //zobrazovani textu score pri startu hry
    void Start()
    {
        TextScore.text=score.ToString();
    }

    //zmena textu kazdej frame
    void Update()
    {
        TextScore.text = score.ToString();
    }
}