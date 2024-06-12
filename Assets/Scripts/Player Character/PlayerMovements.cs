using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_body;
    [SerializeField] private BoxCollider2D m_grounCheck;
    [SerializeField] private LayerMask m_groundMask;
    [SerializeField] private Transform m_wallCheck;
    [SerializeField] private LayerMask m_wallLayerMask;
    [SerializeField] private Animator m_animator;

    [SerializeField] private float m_groundSpeed = 2;
    [SerializeField] private float m_jumpHeight = 3;
    [SerializeField] private float m_acceleration;
    private int m_doubleJump = 1;
    private int m_doubleJumpCount = 1;

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
   
    [Range(0f,1f)]
    [SerializeField] private float m_groundDecay = .9f;
    [SerializeField] private bool m_grounded;

    private float m_xInput;
    private float m_yInput;



    // Update is called once per frame
    void Update()
    {
        CheckInput();
        Jump();    
        
    }
    private void FixedUpdate()
    {
        CheckGround();
        ApplyFriction();
        XMovement();
    }
    void CheckInput()
    {
        m_xInput = Input.GetAxis("Horizontal");
        m_yInput = Input.GetAxis("Vertical");
        
    }
    void XMovement()
    {
       
        if (Mathf.Abs(m_xInput) > 0)
        {
            //increment velocity by acceleration then clamp
            var increment = m_xInput * m_acceleration;
            var newspeed = Mathf.Clamp(m_body.velocity.x + increment, -m_groundSpeed, m_groundSpeed);
           
            m_body.velocity = new Vector2(newspeed, m_body.velocity.y);
                       
            FacingInput();
            
            
        }
        m_animator.SetFloat("Speed", Mathf.Abs(m_xInput));
    }
    void FacingInput()
    {
        var direction = Mathf.Sign(m_xInput);
        transform.localScale = new Vector3(direction, 1, 1);
    }
    void ApplyFriction()
    {
        if (m_grounded && m_xInput == 0 && m_body.velocity.y <= 0)
        {
            m_body.velocity *= m_groundDecay;
        }
    }
    private void Jump()
    {
        
        if (m_grounded == true) 
        {
            m_doubleJump = m_doubleJumpCount;
            m_animator.SetBool("IsJumping", false);
        
        }
        else
        {
            m_animator.SetBool("IsJumping", true);
        }
        if (Input.GetButtonDown ("Jump") && m_grounded == true)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            m_body.velocity = new Vector2(m_body.velocity.x, m_jumpHeight);


        }
        if (Input.GetButton("Jump") && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                m_body.velocity = new Vector2(m_body.velocity.x, m_jumpHeight);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }

        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        else if (Input.GetButtonDown("Jump") && m_doubleJump > 0)
        {
            m_body.velocity = new Vector2(m_body.velocity.x, m_jumpHeight+1);
            m_doubleJump--;
            
        }   
        

    }
    
    private void CheckGround()
    {
        m_grounded = Physics2D.OverlapAreaAll(m_grounCheck.bounds.min, m_grounCheck.bounds.max, m_groundMask).Length > 0;
    }

   
}
