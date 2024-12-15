using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAniTest : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator ani;

    float moveSpeed = 5;
    float jumpPower = 5;
    bool isJumping = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            move = Vector3.left;
            ani.SetBool("Walk", true);
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            move = Vector3.right;
            ani.SetBool("Walk", true);
        }

        transform.position += move * moveSpeed * Time.deltaTime;

        if (move == Vector3.zero)
        {
            ani.SetBool("Walk", false);
        }
    }

    void Jump()
    {
        if(!isJumping)
        {
            return;
        }

        rigid.velocity = Vector2.zero;

        Vector2 jump = new Vector2(0, jumpPower);
        rigid.AddForce(jump, ForceMode2D.Impulse);

        isJumping = false;
    }
}
