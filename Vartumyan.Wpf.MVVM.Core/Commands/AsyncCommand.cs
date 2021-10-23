using System;
using System.ComponentModel;
using Vartumyan.Wpf.MVVM.Core.Events;

using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Collections;

namespace Vartumyan.Wpf.MVVM.Core.Commands
{
    public class AsyncCommand : Command, INotifyPropertyChanged
    {


        /// <summary>
        /// Флаг, отображающий, что команда в процессе выполнения.
        /// </summary>
        private bool isExecuting = false;
        private bool isCancellationRequested;

        /// <summary>
        /// Команда отмены
        /// </summary>
        private Command cancelCommand;
        protected AsyncOperation asyncop;

        /// <summary>
        /// Получение команды отмены.
        /// </summary>
        public Command CancelCommand
        {
            get { return cancelCommand; }
        }

        /// <summary>
        /// Получить/Установить значение, указывающее, поступила ли команда отмены
        /// </summary>
        /// <value>
        ///     <c>true</c> если есть запрос на отмену; запроса нет -  <c>false</c>.
        /// </value>
        public bool IsCancellationRequested
        {
            get
            {
                return isCancellationRequested;
            }
            set
            {
                if (isCancellationRequested != value)
                {
                    isCancellationRequested = value;
                    OnPropertyChanged("IsCancellationRequested");
                }
            }
        }

        /// <summary>
        /// Инициализация нового экземпляра класса без параметров <see cref="AsynchronousCommand"/>.
        /// </summary>
        /// <param name="action">Действие.</param>
        /// <param name="canExecute"> Если установлено в 
        ///  <c>true</c> команда может выполняться.</param>
        public AsyncCommand(Action action, bool canExecute = true)
          : base(action, canExecute)
        {
            Initialise();
        }

        /// <summary>
        /// Инициализация нового экземпляра класса с параметрами<see cref="AsynchronousCommand"/>.
        /// </summary>
        /// <param name="parameterizedAction">Параметризированное действие.</param>
        /// <param name="canExecute"> Если установлено в <c>true</c> [can execute] (может выполняться).</param>
        public AsyncCommand(Action<object> parameterizedAction, bool canExecute = true)
          : base(parameterizedAction, canExecute)
        {
            Initialise();
        }

        /// <summary>
        /// Инициализация экземпляра
        /// </summary>
        private void Initialise()
        {
            //  Конструктор команды отмены
            cancelCommand = new Command(
              () =>
              {
                  IsCancellationRequested = true;
              }, true);
        }

        /// <summary>
        /// Получение/Установка флага, который показывает, что команда в процессе выполнения..
        /// </summary>
        /// <value>
        ///     <c>true</c> если в процессе выполнения; иначе <c>false</c>.
        /// </value>
        public bool IsExecuting
        {
            get
            {
                return isExecuting;
            }
            set
            {
                if (isExecuting != value)
                {
                    isExecuting = value;
                    OnPropertyChanged("IsExecuting");
                }
            }
        }

        // <summary>
        /// The property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Возникает, когда команда отменена.
        /// </summary>
        public event CommandEventHandler Cancelled;


        /// <summary>
        /// Выполнение команды.
        /// </summary>
        /// <param name="param">Параметр.</param>
        public override void DoExecute(object param)
        {
            //  Если уже в процессе выполнения, тоне продолжаем.
            if (IsExecuting)
                return;

            //  Вызов выподняющейся команды, что позволяет отменить ее выполнение.
            CancelCommandEventArgs args =
               new CancelCommandEventArgs() { Value = param, Cancel = false };
            InvokeExecuting(args);

            //  Если отмена -  прерываем.
            if (args.Cancel)
                return;

            //  В процессе выполнения.
            IsExecuting = true;
        }
        /// <summary>
        /// Reports progress on the thread which invoked the command.
        /// </summary>
        /// <param name="action">The action.</param>
        public IEnumerator ReportProgress(Action action)
        {
            yield return null;
            //TODO
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
