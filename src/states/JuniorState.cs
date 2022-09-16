using Output;
using System.Text;

// 初中状态
public class JuniorState: IState
{
    // 环境类
    private StateManager? _stateManager = null;

    // 转换为当前状态
    public void Handle(StateManager stateManager)
    {
        this._stateManager = stateManager;
        Console.WriteLine("准备生成初中数学题目，请输入生成题目数量（输入-1将退出当前用户，重新登录）：");
    }
    // 接受指令
    public void CommandRecv(string command)
    {
        string[] getCommand = command.Split(" ",StringSplitOptions.RemoveEmptyEntries);
        if(getCommand[0].Contains("切换为"))
        {
            if(getCommand[0].Contains("小学"))
            {
                _stateManager.StateSwitch(Grade.Primary);
            }
            else if(getCommand[0].Contains("初中"))
            {
                _stateManager.StateSwitch(Grade.Junior);
            }
            else if(getCommand[0].Contains("高中"))
            {
                _stateManager.StateSwitch(Grade.Senior);
            }
            else
            {
                Console.WriteLine("请输入小学、初中和高中三个选项中的一个");
            }
            return;
        }
        int total;
        int.TryParse(command,out total);
        if(total == -1)
        {
            // 转换为登录状态
            _stateManager.StateSwitch(Grade.None);
            return;
        }
        else if(total <10||total>30)
        {
            Console.WriteLine("题目数量不适宜");
            return;
        }
        TestGenerate(total);
    }
    
    /// <summary>
    /// 试卷生成方法
    /// </summary>
    /// <param name = "total">题目总数</param>
    private void TestGenerate(int total)
    {
        TestPaper testPaper = new TestPaper(
            Grade.Junior,
            System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"),
            ".txt",
            _stateManager.Username
        );
        for(int i=0;i<total;++i)
        {
            testPaper.FileContent.Add(QuestionGenerate());
        }
        _stateManager.SaveHistory();
        Output(testPaper);
        Console.WriteLine(total+@"道题目已输出完成，位于程序文件根目录\File\出题人姓名");
    }
    
    // 单个试题生成
    private string QuestionGenerate()
    {
        string[] op = { "+", "-", "*", "/" };
        string[] power = { "^2","^(1/2)"};
        Random random = new Random();
        StringBuilder build = new StringBuilder();
        int count = (int) (random.NextDouble() * 2) + 2;
        int start = 0;
        int n1 = (int) (random.NextDouble() * 99) + 1;
        build.Append(n1);
        bool hasPower = false;
        if(random.NextDouble()>0.5)
        {
            build.Append(power[random.NextDouble()>0.7?1:0]);
            hasPower= true;
        }
        while (start <= count){
            int oper = (int) (random.NextDouble() * 3); 
            int n2 = (int) (random.NextDouble() * 99) + 1;
            build.Append(op[oper]).Append(n2);
            start ++;
            if(random.NextDouble()>0.55)
            {
                build.Append(power[random.NextDouble()>0.8?1:0]);
                hasPower=true;
            }
        }
        if(!hasPower)
        {
            build.Append("^2");
        }
        build.Append("=");
        string test = build.ToString();
        if(_stateManager.AddTestHistory(test)){
            return test;
        }
        else
        {
            // 递归直到算出一个没有重复的公式
            return QuestionGenerate();
        }
    }

    // 将试卷输出
    private void Output(TestPaper stuff)
    {
        OutputManager.GetInstance().Output(stuff);
    }


}