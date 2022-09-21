using AutoTestLogic.Json;
using Microsoft.AspNetCore.Mvc;
using MVCAutoTest.Models;
using System.Diagnostics;
using AutoTestLogic.Database;
using System.Net.Mail;

namespace MVCAutoTest.Controllers {


    public class HomeController : BaseController {
        private readonly ILogger<HomeController> _logger;

        private string _username;
        private string _password;
        private string _email;
        private string _captcha;


        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 登录的接口
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult login(string username, string password) {
            _username = username;
            _password = password;
            if (DBInteract.GetInstance().TryLogin(username, password)) {
                return Content(new JsonReturn(ResultType.LoginSuccess, "login successful").ToJsonCon());
            }
            return Content(new JsonReturn(ResultType.LoginFail, "Incorrect username or password").ToJsonCon());
        }


        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="email">用户邮箱</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult get_vc(string email) {
            _email = email;
            System.Random random = new System.Random();
            _captcha = random.Next(100000, 1000000).ToString();
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add(email);
            msg.From = new System.Net.Mail.MailAddress("114514@qq.com","田所先生",System.Text.Encoding.UTF8);

            msg.Subject = "验证码";
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = _captcha;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = false;
            msg.Priority = System.Net.Mail.MailPriority.High;

            SmtpClient client = new SmtpClient();
            client.Host = "localhost";
            object userState = msg;
            try {
                client.SendAsync(msg, userState);
                return Content(new JsonReturn(ResultType.Captcha, _captcha).ToJsonCon());
            }
            catch (System.Net.Mail.SmtpException ex) {
                return Content(new JsonReturn(ResultType.Captcha, "发送邮件出错").ToJsonCon());
            }
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="email">用户邮箱</param>
        /// <param name="username">用户名</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult get_vc(string email, string username, string password) {
            _username = username;
            _password = password;
            _email = email;
            if (DBInteract.GetInstance().AddAccount(username,password,email)) {
                return Content(new JsonReturn(ResultType.RegisterSuccess, "register successful").ToJsonCon());
            }
            return Content(new JsonReturn(ResultType.RegisterFail, "Incorrect username or password").ToJsonCon());
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="oldpwd">老密码</param>
        /// <param name="newpwd">新密码</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult change_pwd(string username, string oldpwd, string newpwd) {
            if (DBInteract.GetInstance().ChangeAccount(username, oldpwd, newpwd)) {
                return Content(new JsonReturn(ResultType.PasswordSuccess, "change successful").ToJsonCon());
            }
            return Content(new JsonReturn(ResultType.PasswordFail, "change failed").ToJsonCon());
        }

        /// <summary>
        /// 检查邮箱是否已使用
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult get_by_email(string email) {
            if (DBInteract.GetInstance().GetAccountByEmal(email)) {
                return Content(new JsonReturn(ResultType.EmailUsed, "邮箱已被使用").ToJsonCon());
            }
            return Content(new JsonReturn(ResultType.EmailAvail, "邮箱未被使用").ToJsonCon());
        }


        /// <summary>
        /// 检查用户名是否已使用
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult get_by_username(string username) {
            if (DBInteract.GetInstance().GetAccountByUsername(username)) {
                return Content(new JsonReturn(ResultType.UsernameUsed, "用户名已被使用").ToJsonCon());
            }
            return Content(new JsonReturn(ResultType.UsernameAvail, "用户名未被使用").ToJsonCon());
        }


        /// <summary>
        /// 获取试卷
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult get_letter(int grade, int num) {
            return Content(new JsonTest(DBInteract.GetInstance().GetTestPaper(grade, num)).ToJsonCon());
        }

    }

    public class JsonReturn {
        public object resultType { get; set; }
        public string message { get; set; }
        public object data { get; set; }

        public JsonReturn (ResultType resultType,string message = null ,object data = null) {
            this.resultType = resultType;
            this.message = message;
            this.data = data;
        }
    }

    public class JsonTest {
        public List<string> detail { get; set; }

        public JsonTest(List<string> detail) {
            this.detail = new List<string>();
            this.detail = detail;
        }
    }


    /// <summary>
    /// 返回状态码
    /// </summary>
    public enum ResultType {
        // 登陆成功
        LoginSuccess = 200,
        // 登陆失败
        LoginFail = 400,
        // 验证码
        Captcha = 111,
        // 注册成功
        RegisterSuccess = 201,
        // 注册失败
        RegisterFail = 401,
        // 修改密码成功
        PasswordSuccess = 202,
        // 修改密码失败
        PasswordFail = 402,
        // 邮箱未被注册
        EmailAvail = 203,
        // 邮箱已被注册
        EmailUsed = 403,
        // 用户名未被注册
        UsernameAvail = 204,
        // 用户名已被注册
        UsernameUsed = 404
    }


}