using UnityEngine;

public class EndlessCity : MonoBehaviour
{
    [SerializeField] Transform playerCarTransform;
    [SerializeField] Transform otherCityTransform;
    [SerializeField] float halfLength = 80f;
    [SerializeField] float spawnOffset = 10f; // buffer

    float lastPlayerZ;

    void Start()
    {
        lastPlayerZ = playerCarTransform.position.z;
    }

    void Update()
    {
        float playerZ = playerCarTransform.position.z;
        float direction = playerZ - lastPlayerZ;

        // NAPRIJED (+Z)
        if (direction > 0 &&
            playerZ > transform.position.z + halfLength + spawnOffset)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                otherCityTransform.position.z + halfLength * 2
            );
        }

        // NAZAD (-Z)
        if (direction < 0 &&
            playerZ < transform.position.z - halfLength - spawnOffset)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                otherCityTransform.position.z - halfLength * 2
            );
        }

        lastPlayerZ = playerZ;
    }
}
