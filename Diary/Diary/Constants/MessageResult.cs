using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace Diary.Constants
{
   public class MessageResult
   {
       public const int NONE = 0;
       public const int OK = 1;
       public const int CANCEL = 2;
       public const int ABORT = 3;
       public const int RETRY = 4;
       public const int IGNORE = 5;
       public const int YES = 6;
       public const int NO = 7;
       public enum Messages
       {
           NONE = 0,
           OK = 1,
           CANCEL = 2,
           ABORT = 3,
           RETRY = 4,
           IGNORE = 5,
           YES = 6,
           NO = 7
       }
    }
}
