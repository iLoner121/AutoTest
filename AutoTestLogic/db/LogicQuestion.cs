using AutoTestEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestLogic.Database {
    public class LogicQuestion {
        /// <summary>
        /// 获取题目列表
        /// </summary>
        /// <param name="keyWord">题目关键词</param>
        /// <returns></returns>
        public List<Question> GetQuestionList(string keyWord = null) {
            ADOEntity adoEntity = new ADOEntity();
            var data = adoEntity.Question.ToList();
            if (string.IsNullOrEmpty(keyWord)) {
                data = data.Where(p => p.Stem.Contains
                    (keyWord)).ToList();
            }
            return data;
        }
        /// <summary>
        /// 获取题目列表
        /// </summary>
        /// <param name="grade">年级</param>
        /// <returns></returns>
        public List<Question> GetQuestionList(int grade = 0) {
            ADOEntity adoEntity = new ADOEntity();
            var data = adoEntity.Question.ToList();
            data = data.Where(p => p.Grade ==
                grade).ToList();
            return data;
        }

        /// <summary>
        /// 删除单个题目
        /// </summary>
        /// <param name="ID">题目ID</param>
        /// <returns></returns>
        public bool DelQuestion(int ID) {
            ADOEntity adoEntity = new ADOEntity();
            var data = adoEntity.Question.Where(p => p.ID ==
                ID).FirstOrDefault();
            if (data != null) {
                adoEntity.Question.Remove(data);
                adoEntity.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取单个题目
        /// </summary>
        /// <param name="ID">题目ID</param>
        /// <returns></returns>
        public Question GetQuestion(int ID) {
            ADOEntity adoEntity = new ADOEntity();
            var data = adoEntity.Question.Where(p => p.ID ==
                ID).FirstOrDefault();
            return data;
        }

        /// <summary>
        /// 修改单个题目
        /// </summary>
        /// <param name="question">需修改的题目信息</param>
        /// <returns></returns>
        public bool UpdateQuestion(Question question) {
            ADOEntity adoEntity = new ADOEntity();
            var data = adoEntity.Question.Where(p => p.ID ==
                question.ID).FirstOrDefault();
            if (data == null) {
                return false;
            }
            data.Stem = question.Stem;
            data.Answer = question.Answer;
            data.Grade = question.Grade;
            adoEntity.SaveChanges();
            return true;
        }

        /// <summary>
        /// 增加单个题目
        /// </summary>
        /// <param name="question">待增添的题目信息</param>
        /// <returns></returns>
        public bool InsertQuestion(Question question) {
            if (question == null) {
                return false;
            }
            ADOEntity adoEntity = new ADOEntity();
            adoEntity.Question.Add(question);
            adoEntity.SaveChanges();
            return true;
        }

    }
}
