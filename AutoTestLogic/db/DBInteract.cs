using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTestLogic.Event;
using AutoTestLogic.Encryption;
using AutoTestLogic.Json;
using AutoTestEntity;

namespace AutoTestLogic.Database {
    public class DBInteract {
        private static DBInteract instance = null;
        private LogicAccount logicAccount = new LogicAccount();
        private LogicQuestion logicQuestion = new LogicQuestion();


        private DBInteract() {

        }
        public static DBInteract GetInstance() {
            if (instance == null) {
                instance = new DBInteract();
            }
            return instance;
        }

        /// <summary>
        /// 尝试登录用户
        /// </summary>
        /// <param name = "username">用户名</param>
        /// <param name = "password">密码</param>
        public bool TryLogin(string username, string password) {
            Account account = logicAccount.GetAccount(username);
            if (password.StringToMD5_UTF8() == account.Password) {
                EventCenter.GetInstance().EventTrigger(EventType.LoginSuccess);
                return true;
            }
            // 通过事件中心发布登陆失败的信息
            EventCenter.GetInstance().EventTrigger(EventType.LoginFail);
            return false;
        }

        /// <summary>
        /// 增加用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="email">邮箱</param>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        public bool AddAccount(string username, string password, string email = null, string phone = null) {
            Account account = new Account();
            account.Username = username;
            account.Password = password.StringToMD5_UTF8();
            account.Email = email;
            account.Phone = phone;
            if(logicAccount.InsertAccount(account)) {
                EventCenter.GetInstance().EventTrigger(EventType.RegisterSuccess);
                return true;
            }
            EventCenter.GetInstance().EventTrigger(EventType.RegisterFail);
            return false;
        }


        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool ChangeAccount(string username, string oldpwd,string newpwd) {
            Account account = logicAccount.GetAccount(username);
            if(account.Password != oldpwd.StringToMD5_UTF8()) {
                return false;
            }
            account.Password = newpwd.StringToMD5_UTF8();
            if (logicAccount.UpdateAccount(account)) {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 通过邮箱寻找用户
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <returns>是否寻找到</returns>
        public bool GetAccountByEmal(string email) {
            if (logicAccount.GetAccount(email, email) != null) {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 通过用户名寻找用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>是否寻找到</returns>
        public bool GetAccountByUsername(string username) {
            if (logicAccount.GetAccount(username) != null) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取试卷
        /// </summary>
        /// <param name="grade">年级</param>
        /// <param name="count">题目数量</param>
        /// <returns>json列表</returns>
        public List<string> GetTestPaper(int grade,int count) {
            // 获取该年级题目的列表
            List<Question> tempList = new List<Question>();
            tempList.AddRange(logicQuestion.GetQuestionList(grade));
            // 随机排序列表
            System.Random random = new System.Random();
            List<Question> randList = new List<Question>();
            foreach (Question question in tempList) {
                randList.Insert(random.Next(randList.Count),question);
            }
            // 截取到适当数量题目的列表
            List<Question> tempList2 = new List<Question>();
            tempList2 = randList.GetRange(0, count);
            List<string> jsonList = new List<string>();
            foreach (Question temp in tempList2) {
                // 选项
                int[] option = { 1, 2, 3, 4 };
                /*
                将这四个数打乱，四位分别代表abcd四个选项
                其中对应位置是1的就是正确答案，
                对应位置是2的是正确答案加一个数
                对应位置是3的是正确答案减一个数
                对应位置是4的是正确答案乘以一个数
                 */
                // 打乱数组
                for (int i = 0; i < option.Length; i++) {
                    int t = option[i];
                    int index = random.Next(option.Length);
                    option[i] = option[index];
                    option[index] = t;
                }
                int ans = 0;
                // 四个选项的字符串
                string[] op = new string[4];
                for (int i = 0; i < option.Length; i++) {
                    switch (option[i]) {
                        case 1:
                            string[] t = temp.Answer.Split('.');
                            op[i] = t[0];
                            ans = i;
                            break;
                        case 2:
                            op[i] =  random.Next(temp.Answer.Length*10).ToString();
                            break;
                        case 3:
                            op[i] = (random.Next(temp.Answer.Length)-3).ToString();
                            break;
                        case 4:
                            op[i] = (random.Next(temp.Answer.Length)*0.5).ToString();
                            break ;
                    }
                }
                // 生成单个题目json
                JsonQuestion ques = new JsonQuestion(temp.Stem, op[0], op[1], op[2], op[3], ans);
                jsonList.Add(ques.ToJsonCon());
            }
            return jsonList;
        }

        public class JsonQuestion {
            public string question { get; set; }
            public string a { get; set; }
            public string b { get; set; }
            public string c { get; set; }
            public string d { get; set; }
            public int ans { get; set; }

            public JsonQuestion(string question, string a, string b, string c, string d, int ans) {
                this.question = question;
                this.a = a;
                this.b = b;
                this.c = c;
                this.d = d;
                this.ans = ans;
            }
        }

    }

}
