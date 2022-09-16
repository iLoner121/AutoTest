using EventSystem;
using System.IO;
using System.Text;
using Encryption;

// 装填管理器环境类
public class StateManager
{
    // 当前生效的状态
    private IState _state ;
    // 当前登录的用户名
    public string Username;
    // 该用户的出题历史
    public HashSet<string> UserTestHistory = new HashSet<string>();
    // 该用户的出题总数
    private int _count=0;

    // 可能存在的四种状态
    private LoginState _loginState = new LoginState();
    private PrimaryState _primaryState = new PrimaryState();
    private JuniorState _juniorState = new JuniorState();
    private SeniorState _seniorState = new SeniorState();


    // 构造函数
    public StateManager()
    {
        // 初始状态为待登录状态
        _state = _loginState;
        _state.Handle(this);
        EventCenter.GetInstance().EventSubscribe<Account>(
            EventType.LoginSuccess,
            (account) => LoginSuccess(account)
        );
    }
    // 切换当前状态
    public void StateSwitch(Grade type)
    {
        switch(type)
        {
            case Grade.None:
                _state = _loginState;
                break;
            case Grade.Primary:
                _state = _primaryState;
                break;
            case Grade.Junior:
                _state = _juniorState;
                break;
            case Grade.Senior:
                _state = _seniorState;
                break;
            default:
                break;
        }
        _state.Handle(this);
    }

    /// <summary>
    /// 主要操作类接收指令的入口
    /// </summary>
    /// <param name = "command">命令行读取入的指令</param>
    public void CommandRecv(string command)
    {
        _state.CommandRecv(command);
    }

    // 登录成功后的处理
    public void LoginSuccess(Account account){
        Username = account.UserName;
        // 检查是否已经存在对应用户文件并进行创建
        if( Directory.Exists(@".\File\"+Username) == true)
        {
            // 存在用户文件夹
            if(!File.Exists(@".\File\"+Username+@"\"+Username+".txt"))
            {
                FileStream fs = new FileStream(@".\File\"+Username+@"\"+Username+".txt", FileMode.Append);
                StreamWriter wr = null;
                wr = new StreamWriter(fs);
                wr.WriteLine("0");
                wr.Close();
            }
        }
        else
        {
            // 创建一个记录为0条的用户出题历史
            Directory.CreateDirectory(@".\File\"+Username);
            FileStream fs = new FileStream(@".\File\"+Username+@"\"+Username+".txt", FileMode.Append);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            wr.WriteLine("0");
            wr.Close();
        }

        // 读取用户出题数据
        StreamReader rd = File.OpenText(@".\File\"+Username+@"\"+Username+".txt");
        string s = rd.ReadLine();
        // 出题历史的总条数
        _count = int.Parse(s);

        for (int i = 0; i < _count; i++)  //读入数据并赋予数组
        {
            string line = rd.ReadLine();
            UserTestHistory.Add(line);
        }          
        rd.Close();
    }

    // 保存出题历史
    public void SaveHistory()
    {
        System.IO.File.WriteAllText(@".\File\"+Username+@"\"+Username+".txt", string.Empty);
        FileStream fs = new FileStream(@".\File\"+Username+@"\"+Username+".txt",FileMode.Append);
        StreamWriter wr = null;
        wr = new StreamWriter(fs);
        wr.WriteLine(_count);
        foreach(string test in UserTestHistory)
        {
            wr.WriteLine(test);
        }
        wr.Close();
    }

    /// <summary>
    /// 检查试题是否重复
    /// </summary>
    public bool AddTestHistory(string test)
    {
        string code = MD5.GetInstance().StringToMD5_UTF8(test);
        if(UserTestHistory.Contains(code))
        {
            return false;
        }
        _count++;
        UserTestHistory.Add(code);
        return true;
    }


}