using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    /*Wyœwietlenie kart 
     * Trzeba zrobiæ pule kart, z której bêdzie sie losowaæ
     * 
     */
    [SerializeField] private GameObject _cardPrizePanel;



    public void ShowVictoryScreen()
    {
        // Wywo³ywane po pokonaniu wroga
        gameObject.SetActive(true);
    }
}
