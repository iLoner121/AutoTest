using System;

// 程序入口
public class Program
{
    public static void Main(string[] args)
    {
        // 调整命令行编码
        Console.InputEncoding = System.Text.Encoding.Unicode;
        Console.WriteLine("Hello world");
        Console.WriteLine("如果确认输入了正确的密码仍失败，请尝试将cmd活动代码页更改为65001（chcp 65001)");
        // 实现控制台输入接收
        while(CommandReader.GetInstance().CommandGet());
    }
}