namespace Output{
    // 输出管理器，管理所有文件输出
    public class OutputManager
    {
        private static OutputManager? instance = null;
        
        private TestPaperOutput? _testPaperOutput;


        private OutputManager()
        {
            // do nothing
        }
        public static OutputManager GetInstance()
        {
            if(instance == null)
            {
                instance = new OutputManager();
            }
            return instance;
        }

        /// <summary>
        /// 输出文件的API
        /// </summary>
        public void Output(IOutputStruct stuff)
        {
            switch(stuff.GetOutputType())
            {
                case OutputType.TestPaper:
                    TestPaperOutput(stuff);
                    break;
            }
        }

        // 输出试卷
        private void TestPaperOutput(IOutputStruct stuff)
        {
            TestPaperOutput op = new TestPaperOutput();
            op.Output(stuff);
        }

    }
}