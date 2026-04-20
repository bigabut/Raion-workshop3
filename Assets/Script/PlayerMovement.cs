using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField]private float speed = 5f;
    private Vector2 vector2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        
    }

    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        vector2 = new Vector2(moveX, moveY);
        vector2 = vector2.normalized;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position = rb.position + speed * vector2 *Time.deltaTime;
    }
}
