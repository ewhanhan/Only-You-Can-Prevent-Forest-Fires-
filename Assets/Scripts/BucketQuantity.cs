using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketQuantity : MonoBehaviour
{
    float currentTime;
    public Text bucketQuantityText;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bucketQuantityText.text = "Bucket Quantity - " + player.bucketWaterQuantity.ToString();
    }
}
