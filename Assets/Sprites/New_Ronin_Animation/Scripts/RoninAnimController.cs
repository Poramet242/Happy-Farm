using System.Collections.Generic;
using UnityEngine;

public class RoninAnimController : MonoBehaviour
{
    public bool isTest = false;
    public List<SpriteRenderer> _renderers = new List<SpriteRenderer>();
    public bool isfacingleft;
    public int currentState = 0;
    private Animator animController
    {
        get
        {
            if(_animator == null)
                _animator = GetComponent<Animator>();
            return _animator;
        }
        set { _animator = value; }
    }
    private Animator _animator;
    private void Start()
    {
        _renderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }
    void Update() 
    {
        // if(!isTest) return;
        // if(Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     Idle();
        // }
        // if(Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     Walk();
        // }
        // if(Input.GetKeyDown(KeyCode.Alpha3))
        // {
        //     Attack();
        // }
        foreach (SpriteRenderer spriteRenderer in _renderers)
        {
            spriteRenderer.sortingOrder = Mathf.FloorToInt(transform.position.y * -100);
        }
        if (isfacingleft)
        {
            transform.GetChild(0).localScale = Vector3.one;
        }
        else
        {
            transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
        }

    }

    public void SetSpeed(float speed)
    {
        animController.speed = speed;
    }

    public bool IsAttack()
    {
        return animController.GetCurrentAnimatorStateInfo(0).IsName("Attack");
    }    
    public bool IsIdle()
    {
        return animController.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }

    public void Attack()
    {
        animController.SetTrigger("Attack");
    }

    public void Walk()
    {
        UpdateState(1);
    }

    public void Idle()
    {
        UpdateState(0);
    }

    private void UpdateState(int index)
    {
        if(animController == null){
            Debug.Log("animController is NULL");
            animController = GetComponent<Animator>();
        }
        animController.SetInteger("State",index);
    }
}
