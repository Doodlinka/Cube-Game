using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public float size = 3;
    public float speed = 15;

    void Update()
    {
        transform.localScale = transform.localScale + Vector3.one * speed * Time.deltaTime;
        if (transform.localScale.x > size) {
            Destroy(gameObject);
        }
    }
}