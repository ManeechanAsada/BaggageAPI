using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using System.Security.Principal;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace tikSystem.Web.Library
{
    public class LibraryBase : CollectionBase, IList, IEnumerable, IDisposable
    {
        protected string gServer = ConfigurationManager.AppSettings["ComServer"];

        public agentservice.TikAeroXMLwebservice objService = null;

        #region Impersonate Function
        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        public void InitializeImpersonateUser()
        {
            string strUserName = ConfigurationManager.AppSettings["ComUser"];
            string strPassword = ConfigurationManager.AppSettings["ComPassword"];
            string strDomain = ConfigurationManager.AppSettings["ComDomain"];

            if (string.IsNullOrEmpty(strUserName) == false && string.IsNullOrEmpty(strPassword) == false && string.IsNullOrEmpty(strDomain) == false)
            {
                impersonateValidUser(strUserName, strDomain, strPassword);
            }
        }
        private bool impersonateValidUser(String userName, String domain, String password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                    LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        if (impersonationContext != null)
                        {
                            using (impersonationContext = tempWindowsIdentity.Impersonate())
                            {
                                CloseHandle(token);
                                CloseHandle(tokenDuplicate);
                                return true;
                            }
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return false;
        }

        private void undoImpersonation()
        {
            if (impersonationContext != null)
            {
                using (impersonationContext)
                {
                    impersonationContext.Undo();
                }
            }
        }

        protected string CreateToken()
        {
            string token = System.Configuration.ConfigurationManager.AppSettings["AuthenUser"].ToString() +
                           System.Configuration.ConfigurationManager.AppSettings["AuthenPassword"].ToString();

            // Use input string to calculate MD5 hash
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(token);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }

            return sb.ToString();
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            string strUserName = ConfigurationManager.AppSettings["ComUser"];
            string strPassword = ConfigurationManager.AppSettings["ComPassword"];
            string strDomain = ConfigurationManager.AppSettings["ComDomain"];

            if (string.IsNullOrEmpty(strUserName) == false && string.IsNullOrEmpty(strPassword) == false && string.IsNullOrEmpty(strDomain) == false)
            {
                undoImpersonation();
            }
        }

        #endregion
    }
}
