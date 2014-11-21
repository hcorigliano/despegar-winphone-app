using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Common
{
    public interface IPopupContent
    {
        /// <summary>
        /// Runs the Enter animation of the content
        /// </summary>
        void Enter();
        /// <summary>
        /// Runs the Leave transition of the content
        /// </summary>
        void Leave();
    }
}
