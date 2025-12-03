using Microsoft.AspNet.Identity;
using Org.Business.Methods;
using Org.Business.Objects;
using System.Collections.Generic;
using System.Web;
using System;
using ASE.Models;
using System.Linq;
using System.Security.Cryptography;

namespace ASE.Utility
{
    public class ListHelper
    {
        public static string ClearJSONString(string pString)
        {
            if (pString != null)
            {
                pString = pString.Replace("[", "");
                pString = pString.Replace("]", "");
                pString = pString.Replace("\"", "");
                pString = pString.Replace("\\n", "");
            }
            return pString;
        }

        public static string ClearJSONNumbers(string pString)
        {
            if (pString != null)
            {
                pString = pString.Replace("[", "");
                pString = pString.Replace("]", "");
                pString = pString.Replace("\"", "");
            }
            return pString;
        }
        public static string CommaSeperatedString(string[] pStrings)
        {
            string ReturnValue = "";

            if (pStrings != null)
            {
                foreach (string val in pStrings)
                {
                    ReturnValue += val.Trim() + ",";
                }

                if (ReturnValue != "")
                {
                    ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 1);
                }
            }

            return ReturnValue;
        }

        public static string CommaSeperatedValues(int[] pValues)
        {
            string ReturnValue = "";

            if (pValues != null)
            {
                foreach (int val in pValues)
                {
                    ReturnValue += val.ToString().Trim() + ",";
                }

                if (ReturnValue != "")
                {
                    ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 1);
                }
            }

            return ReturnValue;
        }
        public static string CommaSeperatedValues(long[] pValues)
        {
            string ReturnValue = "";

            if (pValues != null)
            {
                foreach (long val in pValues)
                {
                    ReturnValue += val.ToString().Trim() + ",";
                }

                if (ReturnValue != "")
                {
                    ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 1);
                }
            }

            return ReturnValue;
        }

        

        public static bool CompareArray(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                    return false;
            }
            return true;
        }
    }
}