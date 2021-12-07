using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterControllerScripte : MonoBehaviour
{
    public CharacterController Controller;
    public float AccelerationFacto=0.1f;
    public float SlowSpeed;
    public float Walkspeed;
    public float RunSpeed;
    public float TurnSpeed;
    
    public Animator Animator;
    
    public float Gravity =-9.81f;
    public Transform GroundCheck;
    public float GroundDistance=0.4f;
    public LayerMask GroundMask;

    public float JumpHeight = 1f;
    [Header("IKMainGauche")]
    public Transform AimRaycaster;
    public Transform HandTarget;
    public Rig IKTwoBown;
    public float IKRaycastRange;
    public float IKHandFullInfluanceRange;
    public float IKHandInfluenceRAnge;
    public float OffSettToWall;

    [Header("IK Raycat foot")]
    public Transform RightFoot;
    public Transform LeftFoot;
    public Rig IKRightFoot;
    public Rig IKLeftFoot;
    public Transform IKRightFootTarget;
    public Transform IKLeftFootTarget;
    
    private float _actualeSpeed=0;
    private bool _isGRounded;
    private Vector3 _velocity;
    void Update()
    {
        Debug.DrawLine(AimRaycaster.position,AimRaycaster.position+AimRaycaster.forward*IKRaycastRange);
        RaycastHit hit;
        if (Physics.Raycast(AimRaycaster.position, AimRaycaster.forward, out hit)) {
            HandTarget.position = hit.point-(hit.point-AimRaycaster.position).normalized*OffSettToWall;
            HandTarget.forward = -hit.normal;
            Debug.Log(hit.distance);
            if (IKHandInfluenceRAnge > hit.distance&&IKHandFullInfluanceRange<hit.distance)
            {
                IKTwoBown.weight = 1-(hit.distance / IKHandInfluenceRAnge);
            }
            else if (IKHandFullInfluanceRange > hit.distance){
                IKTwoBown.weight = 1;
            }
            else
            {
                IKTwoBown.weight = 0;
            }
        }
        else
        {
            IKTwoBown.weight = 0;
        }
        
        
        
        _isGRounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if (_isGRounded && _velocity.y < 0) {
            _velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && _isGRounded) {
            _velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }
        _velocity.y += Gravity * Time.deltaTime;

        float rotactionValue = Input.GetAxisRaw("Horizontal");
        transform.Rotate(Vector3.up ,rotactionValue*TurnSpeed*Time.deltaTime );

        Vector3 moveVector = transform.forward * Input.GetAxis("Vertical");
        moveVector = moveVector.normalized;

        float targetSpeed;
        if (moveVector.magnitude < 1) targetSpeed = 0;
        else if (Input.GetKey(KeyCode.LeftShift)) targetSpeed = RunSpeed;
        else if (Input.GetKey(KeyCode.LeftControl)) targetSpeed = SlowSpeed;
        else targetSpeed = Walkspeed;

        _actualeSpeed = Mathf.Lerp(_actualeSpeed, targetSpeed, AccelerationFacto * Time.deltaTime);
        moveVector = moveVector * (Time.deltaTime * _actualeSpeed);
        //Vector3 moveVector = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));
        Controller.Move(moveVector);
        Controller.Move(_velocity * Time.deltaTime);

        float senfInfp = _actualeSpeed;
        if (Input.GetAxisRaw("Vertical") < 0) senfInfp = -_actualeSpeed;
        Animator.SetFloat("WalkingSpeed",senfInfp/RunSpeed);
        Animator.SetFloat("Turn",Input.GetAxisRaw("Horizontal"));
        Animator.SetBool("IsGrounded", _isGRounded);
        Animator.SetFloat("Y",_velocity.y);
        FootIK();
    }

    private void FootIK()
    {
        Debug.DrawLine(LeftFoot.position+Vector3.up , LeftFoot.position+Vector3.down,Color.green);
        Debug.DrawLine(RightFoot.position+Vector3.up ,RightFoot.position+ Vector3.down,Color.green);
        RaycastHit hit;
        if (Physics.Raycast(LeftFoot.position + Vector3.up, Vector3.down * 2, out hit))
        {
            IKLeftFootTarget.position = hit.point;
            IKLeftFootTarget.forward = hit.normal;

            IKLeftFoot.weight = Animator.GetFloat("LeftFootIK") * 20;
        }
        if (Physics.Raycast(RightFoot.position + Vector3.up, Vector3.down * 2, out hit))
        {
            IKRightFootTarget.position = hit.point;
            IKRightFootTarget.forward = hit.normal;

            IKRightFoot.weight = Animator.GetFloat("RightFootIK") * 20;
        }
        Debug.Log(Animator.GetFloat("LeftFootIK"));
    }
}
