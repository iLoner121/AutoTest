using EventSystem;

public class LoginState: IState
{
    // 状态管理器
    private StateManager? _stateManager;
    // 是否第一次打开（确保事件注册不重复）
    private bool _start = true;


    // 接收指令
    public void CommandRecv(string command)
    {
        string[] str = command.Split(" ",StringSplitOptions.RemoveEmptyEntries);
        if(str.Length==1)
        {
            System.Console.WriteLine("Wrong Format");
            return;
        }
        string username = str[0];
        string password = str[1];
        // 尝试登录
        Login.GetInstance().TryLogin(username,password);
    }

    // 转换为此状态
    public void Handle(StateManager stateManager)
    {
        this._stateManager = stateManager;
        if(_start)
        {
            _start = false;
            DoSubscribe();
        }
        Console.WriteLine("请输入用户名、密码");
    }
    
    // 在第一次打开时订阅登陆成功的事件
    private void DoSubscribe()
    {
        // 订阅登陆信息
        EventCenter.GetInstance().EventSubscribe<Account>(
            EventType.LoginSuccess,
            account => _stateManager.StateSwitch(account.Grade)
        );
        EventCenter.GetInstance().EventSubscribe(EventType.LoginFailed,
            () => Console.WriteLine("请输入正确的用户名、密码")
        );
    }


}
