using System;

namespace HomeBasePlugin
{
//    enum PluginTypes
//    {
//            
//    }
    public interface IHomePlugin
    {
        /// <summary>
        /// Установка события
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="eventAction">Функция, которая вызывается при событии. object[] - параметры, которые передаются в mqtt</param>
        void SetEventHendler(string eventName, Action<object[]> eventAction);

        /// <summary>
        /// Начало работы
        /// </summary>
        void Start();

        /// <summary>
        /// Вызывается поле пробуждения компьютера
        /// </summary>
        void ResumeFromSleep();
    }
}
