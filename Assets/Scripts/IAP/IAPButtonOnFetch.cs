using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;

public class IAPButtonOnFetch : MonoBehaviour
{
    public TMP_Text price;

    public void OnProductFetched(Product product)
    {
        if (price != null)
        {
            price.text = product.metadata.localizedPriceString;
        }
    }
}
