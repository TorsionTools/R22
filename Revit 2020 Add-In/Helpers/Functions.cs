using Autodesk.Revit.DB;
using System;
using System.Diagnostics;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace Revit_2020_Add_In.Helpers
{
    class Functions
    {

    }
    
    //This class passes the handle (window / program) that Own a modeless dialog. 
    public class JtWindowHandle : IWin32Window
    {
        IntPtr _hwnd;

        public JtWindowHandle(IntPtr h)
        {
            Debug.Assert(IntPtr.Zero != h,
              "expected non-null window handle");

            _hwnd = h;
        }

        public IntPtr Handle
        {
            get
            {
                return _hwnd;
            }
        }
    }

    //This is a helper class and method to control the Duplicate Elements Warning when copying elements between models.
    //For example, it is used in the ViewLegendCopyForm code to  help minimize prompts
    internal class CopyHandler : IDuplicateTypeNamesHandler
    {
        public DuplicateTypeAction OnDuplicateTypeNamesFound(DuplicateTypeNamesHandlerArgs args)
        {
            return DuplicateTypeAction.UseDestinationTypes;
        }
    }
}
