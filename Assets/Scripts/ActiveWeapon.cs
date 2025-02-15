using System.Collections;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon {get; private set; }
    private PlayerControl playercontrol;
    private float timeBetweenAttacks;
    private bool attackButtonDown, isAttacking = false;
    protected override void Awake()
    {
        base.Awake();
        playercontrol = new PlayerControl();
    }
    private void OnEnable() {
        playercontrol.Enable();
    }
    private void Start()
    {
        playercontrol.Combat.Attack.started += _ => StartAttacking();
        playercontrol.Combat.Attack.canceled += _ => StopAttacking();
        AttackCooldown();
    }
    private void Update()
    {
        Attack();   
    }
    public void NewWeapon(MonoBehaviour newWeapon) {
        CurrentActiveWeapon = newWeapon;
        attackButtonDown = false;
        isAttacking = false;
        AttackCooldown();
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }
    public void WeaponNull() {
        CurrentActiveWeapon = null;
    }
    private void AttackCooldown() {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(timeBetweenAttacksRoutine());
    }
    private IEnumerator timeBetweenAttacksRoutine() {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }
    private void StartAttacking() {
        attackButtonDown = true;
    }
    private void StopAttacking() {
        attackButtonDown = false;
    }
    private void Attack() {
        if(attackButtonDown && !isAttacking) {
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }
}
