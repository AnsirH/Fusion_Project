using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _cc;

    [SerializeField] private Ball _prefabBall;
    [SerializeField] private PhysxBall _prefabPhysxBall;

    private Vector3 _forward = Vector3.forward;

    [Networked] private TickTimer delay { get; set; }

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        _forward = transform.forward;
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);

            if (data.direction.sqrMagnitude > 0) 
                _forward = data.direction;

            if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
            {
                if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON0))
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    Runner.Spawn(_prefabBall,
                        transform.position + _forward, Quaternion.LookRotation(_forward),
                        Object.InputAuthority, (runner, o) =>
                        {
                            // Initialize the Ball before synchronizing it
                            o.GetComponent<Ball>().Init();
                        });
                }
                else if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON1))
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    Runner.Spawn(_prefabPhysxBall,
                        transform.position + _forward, Quaternion.LookRotation(_forward),
                        Object.InputAuthority,
                        (runner, o) =>
                        {
                            o.GetComponent<PhysxBall>().Init(10 * _forward);
                        });
                }
            }            
        }
    }
}
