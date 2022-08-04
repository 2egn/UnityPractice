using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();   
        spriteRenderer = GetComponent<SpriteRenderer>();   
        anim = GetComponent<Animator>();
    }
    //단발적인 업데이트들은 Update에 쓰는것을 추천
    void Update()
    {
        //Jump
        if(Input.GetButtonDown("Jump") && !anim.GetBool("isJumping")){
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            //anim.SetBool("isKeyJumping", true);
        }

        //slip prevent
        if(Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.000000000001f, rigid.velocity.y);
        }
        //Change Facing direction
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        //Walking Animation
        if (rigid.velocity.normalized.x == 0)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
        /*//Float Animation
         if(rigid.velocity.y < 0 && !anim.GetBool("isKeyJumping")) {
            anim.SetBool("isJumping", true);
         }
         else anim.SetBool("isJumping", false); */

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        //Max Speed
        if(rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if(rigid.velocity.x < maxSpeed*(-1))
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
        //Landing Detection
        if(rigid.velocity.y < 0) {//대충점프할때
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0)); //대충레이캐스트하는거
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));//대충 Platform이라는 레이어에 맞는지확인하는거인듯??
            if (rayHit.collider != null){ //만약맞았으면
                if (rayHit.distance < 0.5f) //0.5의 float변수(0.5길이만큼의 레이에맞았으면)
                    anim.SetBool("isJumping", false);//점프변수를 false로바꿔서 다시 대기나 달리기 애니메이션으로 돌림
                    //anim.SetBool("isKeyJumping", false); //레비테이팅 확인용
            }
        }
    }
}
