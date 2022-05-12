using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Thoughts.UI.WPF.Infrastructure.Commands.Base;

namespace Thoughts.UI.WPF.Infrastructure.Commands
{
    internal class RelayCommand : Command
    {
        private readonly Func<object, bool>? _CanExecute;
        private readonly Action<object> _Execute;

        public override void Execute(object? parameter)
        {   if(parameter == null)
                return;

            if (CanExecute(parameter))
                _Execute(parameter);
        }
        public override bool CanExecute(object? parameter)
        {
            if (parameter == null)
                return true;

            return _CanExecute?.Invoke(parameter) ?? true;
        }

        /// <summary>
        /// Defines 
        /// </summary>
        /// <param name="Execute">Method to <c>.Invoke()</c>.</param>
        /// <param name="CanExecute">Defines can be Command executed.</param>
        public RelayCommand(Action<object> Execute, Func<object, bool>? CanExecute = null)
        {
            _Execute = Execute;
            _CanExecute = CanExecute;
        }
    }
}
