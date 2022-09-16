namespace Output{
    public interface IOutputStruct
    {
        public OutputType GetOutputType();
    }

    // 试卷的结构体
    public struct TestPaper : IOutputStruct
    {
        public OutputType Type;
        public Grade Grade;
        public String FileName;
        public String FileType;
        public String TeacherName;
        public List<String> FileContent;


        /// <summary>
        /// 试卷构造函数
        /// </summary>
        /// <param name = "grade">年级</param>
        /// <param name = "filename">文件名</param>
        /// <param name = "fileType">文件类型</param>
        /// <param name = "teacherName">老师姓名</param>
        /// <param name = "type">输出类型,默认为TestPaper</param>
        public TestPaper(Grade grade,string filename,string fileType,string teacherName,OutputType type=OutputType.TestPaper)
        {
            Type = type;
            Grade = grade;
            FileName = filename;
            FileType = fileType;
            TeacherName = teacherName;
            FileContent = new List<string>();
        }
        public TestPaper()
        {
            Type = OutputType.TestPaper;
            Grade = Grade.None;
            FileName = "";
            FileType = "";
            TeacherName = "";
            FileContent = new List<string>();
        }

        // 获得输出类型
        public OutputType GetOutputType()
        {
            return Type;
        }
    }
}
