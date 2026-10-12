using UnityEngine;
using UnityEngine.AI;

public class ClientOrder : MonoBehaviour
{
    [SerializeField] private ClientTypes clientType;
    [SerializeField] private int minTotalPacience = 60;
    [SerializeField] private int maxTotalPacience = 120;
    [SerializeField] private NavMeshAgent agent;

    private OrderCounter orderCounter;

    private float currentPacience;
    private int totalPacience;
    private PotionTypes requestedPotions;
    
    private bool reachedCounter;
    private bool isLeaving;
    private bool hasBeenRegistered;
    private ClientSpawner clientSpawner;

    public bool ReachedCounter => reachedCounter;

    private void Start()
    {
        GenerateClientType();
        GenerateOrder();

        totalPacience = Random.Range(minTotalPacience, maxTotalPacience + 1);
        currentPacience = totalPacience;

        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        orderCounter = FindFirstObjectByType<OrderCounter>();

        if (agent == null || orderCounter == null)
        {
            Debug.Log("Falta el NavMesh o el mostrador");
            enabled = false;
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.Log("El cliente no está sobre el NavMesh");
            enabled = false;
            return;
        }

        orderCounter.RegisterClient(this);
    }

    private void Update()
    {
        if (agent == null || orderCounter == null)
        {
            return;
        }

        if (isLeaving)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && agent.velocity.sqrMagnitude < 0.01f)
            {
                Destroy(gameObject);
            }

            return;
        }
        if (!reachedCounter && orderCounter.IsFirstClient(this))
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.03f)
            {
                reachedCounter = true;
                agent.isStopped = true;

                Debug.Log(clientType + " ha llegado al mostrador.");
            }
        }

        currentPacience -= Time.deltaTime;

        if (currentPacience <= 0)
        {
            int randomNumber = Random.Range(1, 11);

            if (randomNumber == 1)
            {
                Debug.Log(clientType + " abandona la cola.");

                orderCounter.RemoveClient(this);
                Leave(orderCounter.GetExitPosition());
            }
            else
            {
                currentPacience = totalPacience;
                Debug.Log(clientType + " sigue esperando.");
            }
        }
    }

    private void GenerateClientType()
    {
        ClientTypes[] clients = {ClientTypes.Knight, ClientTypes.Witch, ClientTypes.Orc, ClientTypes.Spider, ClientTypes.Dragon};

        clientType = clients[Random.Range(0, clients.Length)];
    }

    private void GenerateOrder()
    {
        PotionTypes[] possiblePotions;

        switch (clientType)
        {
            case ClientTypes.Knight:
                possiblePotions = new PotionTypes[]
                {
                    PotionTypes.HealPotion,
                    PotionTypes.DamagePotion,
                    PotionTypes.ResistancePotion,
                    PotionTypes.StaminaPotion
                };
                break;

            case ClientTypes.Witch:
                possiblePotions = new PotionTypes[]
                {
                    PotionTypes.HealPotion,
                    PotionTypes.DamagePotion,
                    PotionTypes.ResistancePotion,
                    PotionTypes.StaminaPotion,
                    PotionTypes.MagicPotion,
                    PotionTypes.PoisonPotion,
                    PotionTypes.BeautyPotion,
                    PotionTypes.AmnesiaPotion,
                    PotionTypes.FirePotion,
                    PotionTypes.InvisibilityPotion
                };
                break;

            case ClientTypes.Orc:
                possiblePotions = new PotionTypes[]
                {
                    PotionTypes.HealPotion,
                    PotionTypes.BeautyPotion,
                    PotionTypes.InvisibilityPotion
                };
                break;

            case ClientTypes.Spider:
                possiblePotions = new PotionTypes[]
                {
                    PotionTypes.HealPotion,
                    PotionTypes.DamagePotion,
                    PotionTypes.StaminaPotion,
                    PotionTypes.PoisonPotion,
                    PotionTypes.AmnesiaPotion
                };
                break;

            case ClientTypes.Dragon:
                possiblePotions = new PotionTypes[]
                {
                    PotionTypes.HealPotion,
                    PotionTypes.DamagePotion,
                    PotionTypes.StaminaPotion,
                    PotionTypes.MagicPotion,
                    PotionTypes.FirePotion
                };
                break;

            default:
                return;
        }

        requestedPotions = possiblePotions[Random.Range(0, possiblePotions.Length)];
    }

    public CustomerOrder CreateOrder()
    {
        CustomerOrder order = new CustomerOrder();

        order.clientType = clientType;
        order.potionType = requestedPotions;
        order.totalPatience = totalPacience;
        order.currentPatience = currentPacience;

        return order;
    }

    public void Leave(Vector3 exitPosition)
    {
        isLeaving = true;
        reachedCounter = false;

        agent.isStopped = false;
        agent.SetDestination(exitPosition);
    }

    public void MoveToQueuePosition(Vector3 position)
    {
        if (agent == null || !agent.isOnNavMesh || isLeaving)
        {
            return;
        }

        if (Vector3.Distance(agent.destination, position) <= 0.1f)
        {
            return;
        }

        reachedCounter = false;
        agent.isStopped = false;
        agent.SetDestination(position);
    }

    public void SetSpawner(ClientSpawner spawner)
    {
        clientSpawner = spawner;
    }

    public void OnDestroy()
    {
        if (clientSpawner != null)
        {
            clientSpawner.RemoveClient();
        }
    }
}
