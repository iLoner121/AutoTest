using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestLogic.Event {
    public interface IEventCallBack { }

    // 回调函数事件的两个重载
    public class EventCallBack : IEventCallBack {
        public Action actions;

        public EventCallBack(Action action) {
            actions += action;
        }
    }
    public class EventCallBack<T> : IEventCallBack {
        public Action<T> actions;

        public EventCallBack(Action<T> action) {
            actions += action;
        }
    }

    /// <summary>
    /// 事件中心系统
    /// </summary>
    public class EventCenter {
        // 单例
        private static EventCenter instance = null;

        // 事件管理字典
        private Dictionary<EventType, IEventCallBack> _events =
            new Dictionary<EventType, IEventCallBack>();


        private EventCenter() {
            // do nothing
        }
        public static EventCenter GetInstance() {
            if (instance == null) {
                instance = new EventCenter();
            }
            return instance;
        }


        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="action">以委托形式传递的回调函数</param>
        public void EventSubscribe(EventType type, Action action) {
            if (_events.ContainsKey(type)) {
                (_events[type] as EventCallBack).actions += action;
            } else {
                _events.Add(type, new EventCallBack(action));
            }
        }
        public void EventSubscribe<T>(EventType type, Action<T> action) {
            if (_events.ContainsKey(type)) {
                (_events[type] as EventCallBack<T>).actions += action;
            } else {
                _events.Add(type, new EventCallBack<T>(action));
            }
        }

        /// <summary>
        /// 退订事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="action">以委托形式传递的回调函数</param>
        public void EventUnsubscribe(EventType type, Action action) {
            if (_events.ContainsKey(type)) {
                (_events[type] as EventCallBack).actions -= action;
            }
        }
        public void EventUnsubscribe<T>(EventType type, Action<T> action) {
            if (_events.ContainsKey(type)) {
                (_events[type] as EventCallBack<T>).actions -= action;
            }
        }
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="type">事件类型</param>
        public void EventTrigger(EventType type) {
            if (_events.ContainsKey(type)) {
                (_events[type] as EventCallBack).actions?.Invoke();
            }
        }
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="args">传递参数</param>
        public void EventTrigger<T>(EventType type, T args) {
            if (_events.ContainsKey(type)) {
                (_events[type] as EventCallBack<T>).actions?.Invoke(args);
            }
        }

        /// <summary>
        /// 清空事件列表
        /// </summary>
        public void Clear() {
            _events.Clear();
        }
    }

    /// <summary>
    /// 事件类型
    /// </summary>
    public enum EventType {
        LoginSuccess,
        LoginFail,
        RegisterSuccess,
        RegisterFail,
        PasswordSuccess,
        PasswordFail,
    }
}
