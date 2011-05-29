namespace Interop.BugTraqProvider
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("90D24818-31FB-4658-9F5C-19693123D9A6")]
    internal interface IBugTraqProvider2String : IBugTraqProvider2
    {
        /// <remarks>
        /// <code>
        /// HRESULT	OnCommitFinished (
        /// 		[in] HWND hParentWnd,					// Parent window for any (error) UI that needs to be displayed.
        /// 		[in] BSTR commonRoot,					// The common root of all paths that got committed.
        /// 		[in] SAFEARRAY(BSTR) pathList,			// All the paths that got committed.
        /// 		[in] BSTR logMessage,					// The text already present in the commit message.
        /// 		[in] BSTR revision,			    		// The revision of the commit.
        /// 		[out, retval] BSTR * error				// An error to show to the user if this function returns something else than S_OK
        /// 		);
        /// </code>
        /// </remarks>

        [return: MarshalAs(UnmanagedType.BStr)]
        string OnCommitFinished(
            IntPtr hParentWnd,
            [MarshalAs(UnmanagedType.BStr)] string commonRoot,
            [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)] string[] pathList,
            [MarshalAs(UnmanagedType.BStr)] string logMessage,
            [MarshalAs(UnmanagedType.BStr)] string revision);
    }
}
