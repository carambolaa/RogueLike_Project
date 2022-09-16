using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> itemPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI text;
    private int price;
    private Transform player;

    private Animator myAnimator;
    private bool boxOpened;

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        if (myAnimator == null)
        {
            myAnimator = GetComponentInParent<Animator>();
        }

        player = GameObject.Find("Player").transform;

        price = Random.Range(20, 100);
        text.text = price.ToString();
    }

    public void Spawn()
    {
        if (!boxOpened && CharacterManager.Instance.GetGold() >= price)
        {
            CharacterManager.Instance.MinusGold(price);
            objectClicked();
            Destroy(canvas.gameObject);
            Invoke("SpawnLogic", 0.8f);
        }
    }

    private void SpawnLogic()
    {
        var currentItem = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
        Instantiate(currentItem, spawnPoint.position, Quaternion.identity);
        //Destroy(this);
    }

    private void objectClicked()
    {
        myAnimator.Play("Open", 0, 0.0f);
        boxOpened = true;
    }

    private void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        if(canvas == null)
        {
            return;
        }
        if(Vector3.Distance(player.position, transform.position) < 5)
        {
            canvas.gameObject.SetActive(true);
            return;
        }
        canvas.gameObject.SetActive(false);
    }
}
