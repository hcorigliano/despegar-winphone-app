using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Despegar.WP.UI.Model
{
    public interface IBugTracker
    {
        void LeaveBreadcrumb(string breadcrumb);

        void LogException(Exception exception);

        void LogException(Exception exception, object extrasExtraDataList);

        void SetExtraData(string key, string value);

        void LogEvent(string eventName);
    }
}