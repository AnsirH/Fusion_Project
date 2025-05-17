using Fusion;

public class Ball : NetworkBehaviour
{
    // [Networked]�� ǥ�õ� �ʵ�� Fusion���� ����ȭ �ڵ带 ������ �� ����ϴ� �Ӽ��̰� {get; set;} ������ �־�� �մϴ�. �׻� �� ������ ��������.
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
