using UnityEngine;

public class CoinMananager : MonoBehaviour
{
    public static CoinMananager singleton;
    public GameObject coinPrefab;

    private void Awake()
    {
        singleton = this;
    }

    public void CreateCoins(int points, Vector3 instanciationPoint)
    {
        //Performance
        if (points > 25) points = 25;
        for (int i=0; i < points; i++)
        {
            GameObject go = Instantiate(coinPrefab, 
                new Vector3(
                    Random.Range(instanciationPoint.x-5, instanciationPoint.x+5),
                    Random.Range(instanciationPoint.y + 10, instanciationPoint.y + 15),
                    instanciationPoint.z),
                Quaternion.identity);
            Coin c = go.GetComponent<Coin>();
            c.MoveToPlayer();
        }
    }
}
