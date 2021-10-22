using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    
    public Text score; 
    public Text winText;
    public Text lives;
    public Text debug1;

    private int countobj = 0;
    private int countbadobj = 0;
    private int scoreValue = 0;
    private int livesValue = 3;

    private bool facingRight = true;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        countobj = 0;
        lives.text = "Lives: " + livesValue.ToString();
        debug1.text = "";
        winText.text = "";
    }

    void Update()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        // Player looking direction, and ground check
        {
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
           Flip();
        }

        if (hozMovement > 0 && facingRight == true)
        {
            Debug.Log ("Facing Right");
            //debug1.text = "Facing Right";
        }

        if (hozMovement < 0 && facingRight == false)
        {
            Debug.Log ("Facing Left");
        }

        if (vertMovement > 0 && isOnGround == false)
        {
            Debug.Log ("Jumping");
        }

        if (vertMovement < 0 && isOnGround == true)
        {
            Debug.Log ("Not Jumping");
        }

        }
    }

        // Collection Of Coins, Hitting Enemy, Win & Lose Conditions
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Player Collects A Coin
        {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            countobj = countobj += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        // Player Hits An Enemy
        if (collision.collider.tag == "Enemy")
        {
            scoreValue -= 1;
            livesValue -= 1;
            countbadobj += 1;
            score.text = "Score: " + scoreValue.ToString();
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        // Player Loses All Lives
        if (countbadobj == 3)
        {
            winText.text = "You Lose! Game Created by Junior Rojas";
            Destroy (gameObject);
        }

        // Player Collects All Coins
        if (countobj == 4)
        {
            winText.text = "You Win! Game Created by Junior Rojas";
            rd2d.isKinematic = true;
            Destroy(rd2d);
        }
        }
    }

        // Jumping
    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 4), ForceMode2D.Impulse);
            }
        }
    }   

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
