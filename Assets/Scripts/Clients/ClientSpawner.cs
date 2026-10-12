using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxClients = 5;

    private float currentTime;
    private int currentClients;

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= spawnInterval)
        {
            if (currentClients < maxClients)
            {
                GameObject client = Instantiate(clientPrefab, spawnPoint.position, spawnPoint.rotation);
                currentClients++;
                ClientOrder clientOrder = client.GetComponent<ClientOrder>();
                clientOrder.SetSpawner(this);
            }
            currentTime = 0;
        }
    }

    public void RemoveClient()
    {
        currentClients--;
    }
}
