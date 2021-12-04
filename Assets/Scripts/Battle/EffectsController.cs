using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    public bool burnedCard;
    public bool playedCard;
    public GameObject burnedCardConvoyerPrefabHand, burnedCardConvoyerPrefabDeck;

    // Start is called before the first frame update
    void Start()
    {
        burnedCard = false;
        playedCard = false;
    }

    public void InstantiateBurnedCardConvoyerFromHand(GameObject card)
    {
        GameObject burnedCardConvoy;
        burnedCardConvoy = Instantiate(burnedCardConvoyerPrefabHand, Vector3.zero, Quaternion.identity);
        burnedCardConvoy.transform.SetParent(this.transform, true);
        card.transform.SetParent(burnedCardConvoy.transform);
    }

    public void InstantiateBurnedCardConvoyerFromDeck(GameObject card)
    {
        GameObject burnedCardConvoy;
        burnedCardConvoy = Instantiate(burnedCardConvoyerPrefabDeck, Vector3.zero, Quaternion.identity);
        burnedCardConvoy.transform.SetParent(this.transform, true);
        card.transform.SetParent(burnedCardConvoy.transform);
    }
}
