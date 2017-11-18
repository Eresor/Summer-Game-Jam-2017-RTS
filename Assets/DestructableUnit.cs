using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableUnit : MonoBehaviour {

    public float healthPoints = 100f;
    public float attackPoints = 25;
    public float attackRange = 7f;
    public float hp = 100;
    bool canAttack = true;

    enum AttackState
    {
        attack,
        chase,
        idle
    }

    public enum Weapon
    {
        laser
    }

    public bool isAttacking = false;
    AttackState currentState;
     public DestructableUnit lockedEnemy;

    public void AttackEnemy(DestructableUnit unit)
    {
        lockedEnemy = unit;
        if(!isAttacking)
        StartCoroutine("ShotC");
        
        currentState = AttackState.attack;
        isAttacking = true;
    }
    public void StopAttacking()
    {
        currentState = AttackState.idle;
        isAttacking = false;
        StopAllCoroutines();
    }

    private void Chase()
    {
        currentState = AttackState.chase;
        isAttacking = false;
    }




    IEnumerator ShotC()
    {
        Shot(Weapon.laser);
        yield return new WaitForSeconds(2);
        StartCoroutine(ShotC());
    }



    public void Shot(Weapon weapon)
    {
                var value = Time.time.ToString() + Random.Range(0, float.MaxValue);
                string hash = Hash128.Parse(value).ToString();


                Vector3 targetDir = lockedEnemy.GetComponent<Transform>().position - GetComponent<Transform>().position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, 1000, 0.0F);


                var go = PhotonNetwork.Instantiate("laserPrefab", GetComponent<Transform>().position, Quaternion.LookRotation(newDir), 0, new object[] { hash });
                go.GetComponent<Rigidbody>().velocity = targetDir * 4;
                go.GetComponent<ShotBehavior>().target = lockedEnemy.gameObject;
                lockedEnemy.GetComponent<DestructableUnit>().hp -= attackRange;
    }


    private bool InAttackRange()
    {
        if (lockedEnemy == null)
            return false ;

        var distance = Vector3.ProjectOnPlane(lockedEnemy.GetComponent<Transform>().position - GetComponent<Transform>().position, Vector3.up).magnitude;
        Debug.Log("distance" + distance + " attack range: " + attackRange);
        if (distance <= attackRange)
        {
            Debug.Log("Im in attack range :)");
            return true;
        }
        return false;
    }
    void IsDestroyed()
    {
        if (this.hp <= 0)
        {
            var value = Time.time.ToString() + Random.Range(0, float.MaxValue);
            string hash = Hash128.Parse(value).ToString();
            var go = PhotonNetwork.Instantiate("BigExplosionEffectPref", GetComponent<Transform>().position, Quaternion.identity, 0, new object[] { hash });
            if (this.gameObject != null)
            {
                PhotonView.Destroy(this.gameObject);
            }
                
        }
    }
	// Use this for initialization
	void Start () {
        currentState = AttackState.idle;
    }

	// Update is called once per frame
	void Update () {
        
        if(!canAttack)
        {
            StopAttacking();
            return;
        }


        if (currentState == AttackState.attack)

        {
            if (!InAttackRange())
            {
                Chase();
            }
        }
        else if(currentState ==AttackState.chase)
        {


            if (!InAttackRange())
            {
                PlayerController.instance.MoveUnit(this, lockedEnemy.transform.position);
            }else
            {
                AttackEnemy(lockedEnemy);
            }

           



            }

        IsDestroyed();


    }


}
