using Output;
// 试卷输出类
public class TestPaperOutput : IFileWrite
{
    // 试卷输出方法
    public void Output(IOutputStruct testpaper)
    {
        TestPaper tp = (TestPaper)testpaper;
        FileStream fs = new FileStream(@".\File\"+tp.TeacherName+ @"\" +tp.FileName + tp.FileType,FileMode.Append);
        StreamWriter wr = new StreamWriter(fs);
        for(int i=1;i<tp.FileContent.Count+1;i++)
        {
            wr.WriteLine(i+". "+tp.FileContent[i-1]);
            wr.WriteLine(" ");
        }
        wr.Close();
    }
}