using EventSystem;
using Encryption;

public class Login: ILogin
{
    private static Login? instance =null;
    // 用户管理字典（在没有数据库时暂时这样使用）
    private Dictionary<string,Account> _users =  new Dictionary<string, Account>();


    private Login()
    {
        // 仅用作个人项目中。此时未添加数据库功能。
        AddAccount("张三1","123",Grade.Primary);
        AddAccount("张三2","123",Grade.Primary);
        AddAccount("张三3","123",Grade.Primary);
        AddAccount("李四1","123",Grade.Junior);
        AddAccount("李四2","123",Grade.Junior);
        AddAccount("李四3","123",Grade.Junior);
        AddAccount("王五1","123",Grade.Senior);
        AddAccount("王五2","123",Grade.Senior);
        AddAccount("王五3","123",Grade.Senior);
    }
    public static Login GetInstance()
    {
        if(instance==null)
        {
            instance = new Login();
        }
        return instance;
    }

    /// <summary>
    /// 尝试登录用户
    /// </summary>
    /// <param name = "username">用户名</param>
    /// <param name = "password">密码</param>
    public void TryLogin(string username, string password)
    {
        if(_users.ContainsKey(username))
        {
            string code = MD5.GetInstance().StringToMD5_UTF8(password);
            if(_users[username].PassWord==code)
            {
                // 通过事件中心发布登陆成功的信息
                EventCenter.GetInstance().EventTrigger<Account>(EventType.LoginSuccess,_users[username]);
                return;
            }
        }
        // 通过事件中心发布登陆失败的信息
        EventCenter.GetInstance().EventTrigger(EventType.LoginFailed);
    }

    /// <summary>
    /// 添加账户
    /// </summary>
    /// <param name = "username">用户名</param>
    /// <param name = "password">密码</param>
    /// <param name = "grade">年级</param>
    public bool AddAccount(string username, string password, Grade grade)
    {
        if(_users.ContainsKey(username))
        {
            Console.WriteLine("Duplicate Username");
            // 通过事件中心发布添加失败的信息
            EventCenter.GetInstance().EventTrigger(EventType.DuplicateUsername);
            return false;
        }
        string code = MD5.GetInstance().StringToMD5_UTF8(password);
        _users.Add(username,new Account(username,code,grade));
        // 通过事件中心发布添加成功的信息
        EventCenter.GetInstance().EventTrigger(EventType.AddAccountSuccess);
        return true;
    }

}

// 账户结构体
public struct Account
{
    public string UserName;
    public string PassWord;
    public Grade Grade;
        
    public Account(string username,string password,Grade grade)
    {
        UserName=username;
        PassWord=password;
        Grade=grade;
    }
}