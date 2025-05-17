using Fusion;

public class Ball : NetworkBehaviour
{
    // [Networked]로 표시된 필드는 Fusion에서 직렬화 코드를 생성할 때 사용하는 속성이고 {get; set;} 스텁이 있어야 합니다. 항상 이 패턴을 따르세요.
    [Networked] private TickTimer life { get; set; }

    public void Init()
    {
        life = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Runner.Despawn(Object);
        }

        transform.position += 5 * transform.forward * Runner.DeltaTime;
    }
}
