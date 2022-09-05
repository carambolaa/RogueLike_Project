using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform canvas;
    [SerializeField] private TextMeshProUGUI text;
    private int price;
    private Transform player;

    private void Start()
    {
        price = Random.Range(20, 100);
        text.text = price.ToString();
        player = GameObject.Find("Player").transform;
    }

    private void Spawn()
    {
        var currentItem = itemPrefabs[Random.Range(0,itemPrefabs.Count - 1)];
        Instantiate(currentItem, spawnPoint.position, Quaternion.identity);
    }

    private void OnTriggerStay(Collider other)
    {
        canvas.gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        canvas.gameObject.SetActive(false);
    }
}
