using AutoTestEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestLogic.Database {
    public class LogicAccount {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="keyWord">用户名关键词</param>
        /// <returns></returns>
        public List<Account> GetAccountList(string keyWord = null) {
            ADOEntity adoEntity = new ADOEntity();
            var data = adoEntity.Account.ToList();
            if (string.IsNullOrEmpty(keyWord)) {
                data = data.Where(p => p.Username.Contains
                    (keyWord)).ToList();
            }
            return data;
        }

        /// <summary>
        /// 删除单个用户
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <returns></returns>
        public bool DelAccount(int ID) {
            ADOEntity adoEntity = new ADOEntity();
            var data = adoEntity.Account.Where(p => p.ID ==
                ID).FirstOrDefault();
            if(data != null) {
                adoEntity.Account.Remove(data);
                adoEntity.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取单个用户信息
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <returns></returns>
        public Account GetAccount(int ID) {
            ADOEntity adoEntity = new ADOEntity();
            var data = adoEntity.Account.Where(p => p.ID ==
                ID).FirstOrDefault();
            return data;
        }
        /// <summary>
        /// 获取单个用户信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public Account GetAccount(string username,string email = null) {
            if (email != null) {
                ADOEntity entity = new ADOEntity();
                var dat = entity.Account.Where(p => p.Email == 
                    email).FirstOrDefault();
                return dat;
            }
            ADOEntity adoEntity = new ADOEntity();
            var data = adoEntity.Account.Where(p => p.Username ==
                username).FirstOrDefault();
            return data;
        }

        /// <summary>
        /// 修改单个账户
        /// </summary>
        /// <param name="account">需修改的账户信息</param>
        /// <returns></returns>
        public bool UpdateAccount(Account account) {
            ADOEntity adoEntity = new ADOEntity();
            var data = adoEntity.Account.Where(p => p.Username ==
                account.Username).FirstOrDefault();
            if(data == null) {
                return false;
            }
            data.Username = account.Username;
            data.Password = account.Password;
            data.Email = account.Email;
            data.Phone = account.Phone;
            adoEntity.SaveChanges();
            return true;
        }

        /// <summary>
        /// 增加单个账户
        /// </summary>
        /// <param name="account">待增添的用户信息</param>
        /// <returns></returns>
        public bool InsertAccount(Account account) {
            if(account == null||account.Username==null||account.Password==null) {
                return false;
            }
            ADOEntity adoEntity = new ADOEntity();
            adoEntity.Account.Add(account);
            adoEntity.SaveChanges();
            return true;
        }
    }
}
