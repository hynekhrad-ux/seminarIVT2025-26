using TMPro;
using UnityEngine;

public class hudSCR : MonoBehaviour
{
    //odkay na gameObject textu, potreba priradit v inspectoru
    public TextMeshProUGUI TextScore;
    public TextMeshProUGUI playerHealthTXT;
    //verejna promena score, static pro jednodusi pristup v ostatnich scriptech
    public static int score = 0;

    //zobrazovani textu score pri startu hry
    void Start()
    {
        playerHealthTXT.text=player.playerHealth.ToString() + "/100";
        TextScore.text=score.ToString();
    }

    //zmena textu kazdej frame
    void Update()
    {
        playerHealthTXT.text=player.playerHealth.ToString() + "/100";
        TextScore.text = score.ToString();
    }
}