using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    [SerializeField] private MonoBehaviour currentActiveWeapon;
    private PlayerControl playercontrol;
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
    }
    private void Update()
    {
        Attack();   
    }
    public void ToggleIsAttacking(bool value) {
        isAttacking = value;
    }
    private void StartAttacking() {
        attackButtonDown = true;
    }
    private void StopAttacking() {
        attackButtonDown = false;
    }
    private void Attack() {
        if(attackButtonDown && !isAttacking) {
            isAttacking = true;
            (currentActiveWeapon as IWeapon).Attack();
        }
    }
}
