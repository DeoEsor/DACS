using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vartumyan.Wpf.MVVM.Core.Events;

namespace Vartumyan.Wpf.MVVM.Core.Commands
{
    public class Command : ICommand
    {
        /// <summary>
        ///  Вызывается, когда меняется возможность выполнения команды
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Вызывается во время выполнения команды
        /// </summary>
        public event CancelCommandEventHandler Executing;

        /// <summary>
        /// Вызывается, когда команды выполнена
        /// </summary>
        public event CommandEventHandler Executed;

        /// <summary>
        /// Инициализация нового экземпляра класса без параметров <see cref="Command"/>.
        /// </summary>
        /// <param name="action">Действие.</param>
        /// <param name="canExecute">Если установлено в<c>true</c> [can execute] (выполнение разрешено).</param>
        public Command(Action action, bool canExecute = true)
        {
            //  Set the action.
            this.action = action;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Инициализация нового экземпляра класса с параметрами <see cref="Command"/> class.
        /// </summary>
        /// <param name="parameterizedAction">Параметризированное действие.</param>
        /// <param name="canExecute"> Если установлено в <c>true</c> [can execute](выполнение разрешено).</param>
        public Command(Action<object> parameterizedAction, bool canExecute = true)
        {
            //  Set the action.
            this.parameterizedAction = parameterizedAction;
            this.canExecute = canExecute;
        }
        /// <summary>
        /// Действие(или параметризованное действие) которое вызывается при активации команды.
        /// </summary>
        protected Action action = null;
        protected Action<object> parameterizedAction = null;

        /// <summary>
        /// Будевое значение, отвечающие за возможность выполнения команды.
        /// </summary>
        private bool canExecute = false;

        /// <summary>
        /// Установка /  получение значения, отвечающего за возможность выполнения команды
        /// </summary>
        /// <value>
        ///     <c>true</c> если выполнение разрешено; если запрещено - <c>false</c>.
        /// </value>
        public bool CanExecute
        {
            get { return canExecute; }
            set
            {
                if (canExecute != value)
                {
                    canExecute = value;
                    EventHandler canExecuteChanged = CanExecuteChanged;
                    if (canExecuteChanged != null)
                        canExecuteChanged(this, EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Определяем метод, определющий, что выполнение команды допускается в текущем состоянии
        /// </summary>
        /// <param name="parameter">Этот параметр используется командой.
        ///  Если команда вызывается без использования параметра,
        ///  то этот объект может быть установлен в  null.</param>
        /// <returns>
        /// > если выполнение команды разрешено; если запрещено - false.
        /// </returns>
        bool ICommand.CanExecute(object parameter)
        {
            return canExecute;
        }

        /// <summary>
        /// Задание метода, который будет вызван при активации команды.
        /// </summary>
        /// <param name="parameter"> Этот параметр используется командой.
        ///  Если команда вызывается без использования параметра,
        ///  то этот объект может быть установлен в  null.</param>
        void ICommand.Execute(object parameter)
        {
            this.DoExecute(parameter);

        }

        protected void InvokeAction(object param)
        {
            Action theAction = action;
            Action<object> theParameterizedAction = parameterizedAction;
            if (theAction != null)
                theAction();
            else if (theParameterizedAction != null)
                theParameterizedAction(param);
        }

        protected void InvokeExecuted(CommandEventArgs args)
        {
            CommandEventHandler executed = Executed;

            //  Вызвать все события
            if (executed != null)
                executed(this, args);
        }

        protected void InvokeExecuting(CancelCommandEventArgs args)
        {
            CancelCommandEventHandler executing = Executing;

            //  Call the executed event.
            if (executing != null)
                executing(this, args);
        }

        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="param">The param.</param>
        public virtual void DoExecute(object param)
        {
            //  Вызывает выполнении команды с возможностью отмены
            CancelCommandEventArgs args =
               new CancelCommandEventArgs() { Value = param, Cancel = false };
            InvokeExecuting(args);

            //  Если событие было отменено -  останавливаем.
            if (args.Cancel)
                return;

            //  Вызываем действие с / без параметров, в зависимости от того. Какое было устанвленно.
            InvokeAction(param);

            //  Call the executed function.
            InvokeExecuted(new CommandEventArgs() { Parameter = param });
        }
    }
}
