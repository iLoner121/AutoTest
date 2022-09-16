using System.Text;

// 阅读指令类
public class CommandReader
{
    // 单例
    private static CommandReader? instance = null;
    // 状态管理类，处理主要业务逻辑
    private StateManager _stateManager = new StateManager();
    

    private CommandReader()
    {
        // do nothing
    }
    public static CommandReader GetInstance()
    {
        if(instance==null)
        {
            instance = new CommandReader();
        }
        return instance;
    }

    // 从控制条读取指令
    public bool CommandGet()
    {
        string? command = Console.ReadLine();
        if(command==null||command=="")
        {
            return true;
        }
        return CommandRead(command);
    }

    // 初步处理指令
    private bool CommandRead(string command)
    {
        // 内建命令：kill 在任意阶段终止程序
        if(command.Contains("kill"))
        {
            _stateManager.SaveHistory();
            Console.WriteLine("program terminated");
            return false;
        }
        // 将命令发送给业务逻辑处理类
        _stateManager.CommandRecv(command);
        return true;
    }
}