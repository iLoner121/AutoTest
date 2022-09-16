// 状态模式接口
public interface IState
{
    public void CommandRecv(string command);
    public void Handle(StateManager stateManager);
}