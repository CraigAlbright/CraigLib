using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CraigLib.Data
{
    public static class Expr
    {
        public const string TempNamespace = "http://tempuri.org/";
        public const string AdjustUtcPattern = "(?<DATE>\\d{4}-\\d{2}-\\d{2})(?<TIME>T(\\d{2}:\\d{2}:\\d{2}.\\d{7}|\\d{2}:\\d{2}:\\d{2}))(?<TZ>[+-]\\d{2}:\\d{2})";
        public const string AdjustUtcReplacement = "${DATE}${TIME}";

        public static string Value(object value)
        {
            return !IsNull(value) ? (!(value is string) ? (!(value is DateTime) ? (!(value is Decimal) ? (!(value is double) ? value.ToString() : ((double)value).ToString(CultureInfo.InvariantCulture)) : ((Decimal)value).ToString(CultureInfo.InvariantCulture)) : "#" + ((DateTime)value).ToString("yyyy-MM-dd HH\\:mm\\:ss.fff") + "#") : "'" + ((string)value).Replace("'", "''") + "'") : "null";
        }

        public static string Value(string value, Type type)
        {
            return Value(value, type, true);
        }

        public static string Value(string value, Type type, bool emptyIsNull)
        {
            DateTime result;
            return string.IsNullOrEmpty(value) || value == "<null>" ? (emptyIsNull || !(value == string.Empty) || !(type == typeof(string)) ? "null" : "''") : (!(type == typeof(string)) ? (!(type == typeof(DateTime)) ? value : (!DateTime.TryParse(value, out result) ? value : "#" + result.ToString("yyyy-MM-dd HH\\:mm\\:ss.fff") + "#")) : "'" + value.Replace("'", "''") + "'");
        }

        public static string Format(string format, params object[] args)
        {
            var list = new List<string>();
            foreach (var obj in args)
            {
                if (obj is Array)
                {
                    var objArray = obj as object[];
                    if (objArray != null)
                    {
                        for (var index = 0; index < objArray.Length; ++index)
                            objArray[index] = Value(objArray[index]);
                        list.Add(string.Join(",", (string[])objArray));
                    }
                }
                else
                    list.Add(Value(obj));
            }
            // ReSharper disable once CoVariantArrayConversion
            return string.Format(format, list.ToArray());
        }

        public static string ToString(this object value, string formatString)
        {
            if (IsNull(value))
                return "";
            if (string.IsNullOrEmpty(formatString))
                return value.ToString();
            var s = value as string;
            if (s != null)
                return s;
            if (value is DateTime)
                return ((DateTime)value).ToString(formatString);
            if (IsNumericValue(value))
                return Convert.ToDouble(value).ToString(formatString);
            return value.ToString();
        }

        public static string ToDbValue(object value)
        {
            return !IsNull(value) ? (!(value is string) ? (!(value is DateTime) ? (!(value is Decimal) ? (!(value is double) ? value.ToString() : ((double)value).ToString(CultureInfo.InvariantCulture)) : ((Decimal)value).ToString(CultureInfo.InvariantCulture)) : ((DateTime)value).ToString("yyyy-MM-dd HH\\:mm\\:ss.fff")) : (string)value) : string.Empty;
        }

        public static string PascalCase(string value)
        {
            value = value.Trim();
            if (value.Length == 0)
                return value;
            return value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
        }

        public static string CamelCase(string value)
        {
            value = value.Trim();
            if (value.Length == 0)
                return value;
            return value.Substring(0, 1).ToLower() + value.Substring(1);
        }

        public static string Lower(string strParam)
        {
            return strParam.ToLower();
        }

        public static string Upper(string strParam)
        {
            return strParam.ToUpper();
        }

        public static string PCase(string strParam)
        {
            var str1 = strParam.Substring(0, 1).ToUpper();
            strParam = strParam.Substring(1).ToLower();
            var str2 = "";
            for (var startIndex = 0; startIndex < strParam.Length; ++startIndex)
            {
                if (startIndex > 1)
                    str2 = strParam.Substring(startIndex - 1, 1);
                str1 = str2.Equals(" ") || str2.Equals("\t") || (str2.Equals("\n") || str2.Equals(".")) ? str1 + strParam.Substring(startIndex, 1).ToUpper() : str1 + strParam.Substring(startIndex, 1);
            }
            return str1;
        }

        public static string Reverse(string strParam)
        {
            if (strParam.Length != 1)
                return Reverse(strParam.Substring(1)) + strParam.Substring(0, 1);
            return strParam;
        }

        public static bool IsPalindrome(string strParam)
        {
            var num1 = strParam.Length - 1;
            var num2 = num1 / 2;
            for (var startIndex = 0; startIndex <= num2; ++startIndex)
            {
                if (strParam.Substring(startIndex, 1) != strParam.Substring(num1 - startIndex, 1))
                    return false;
            }
            return true;
        }

        public static string Left(string strParam, int iLen)
        {
            if (iLen <= 0)
                return strParam;
            return strParam.Substring(0, iLen);
        }

        public static string Right(string strParam, int iLen)
        {
            if (iLen <= 0)
                return strParam;
            return strParam.Substring(strParam.Length - iLen, iLen);
        }

        public static int CharCount(string strSource, string strToCount)
        {
            var num = 0;
            for (var index = strSource.IndexOf(strToCount, StringComparison.Ordinal); index != -1; index = strSource.IndexOf(strToCount, StringComparison.Ordinal))
            {
                ++num;
                strSource = strSource.Substring(index + 1);
            }
            return num;
        }

        public static int CharCount(string strSource, string strToCount, bool ignoreCase)
        {
            return !ignoreCase ? CharCount(strSource, strToCount) : CharCount(strSource.ToLower(), strToCount.ToLower());
        }

        public static string ToSingleSpace(string strParam)
        {
            var length = strParam.IndexOf("  ", StringComparison.Ordinal);
            if (length != -1)
                return ToSingleSpace(strParam.Substring(0, length) + strParam.Substring(length + 1));
            return strParam;
        }

        public static string Replace(string strText, string strFind, string strReplace)
        {
            var length = strText.IndexOf(strFind, StringComparison.Ordinal);
            var str = "";
            for (; length != -1; length = strText.IndexOf(strFind, StringComparison.Ordinal))
            {
                str = str + strText.Substring(0, length) + strReplace;
                strText = strText.Substring(length + strFind.Length);
            }
            if (strText.Length > 0)
                str = str + strText;
            return str;
        }

        public static void ToLowerName(DataSet ds)
        {
            ds.DataSetName = ds.DataSetName.ToLower();
            foreach (DataTable dataTable in ds.Tables)
            {
                dataTable.TableName = dataTable.TableName.ToLower();
                foreach (DataColumn dataColumn in dataTable.Columns)
                    dataColumn.ColumnName = dataColumn.ColumnName.ToLower();
            }
        }

        public static bool IsNaturalNumber(string strNumber)
        {
            var regex1 = new Regex("[^0-9]");
            var regex2 = new Regex("0*[1-9][0-9]*");
            if (!regex1.IsMatch(strNumber))
                return regex2.IsMatch(strNumber);
            return false;
        }

        public static bool IsWholeNumber(string strNumber)
        {
            return !new Regex("[^0-9]").IsMatch(strNumber);
        }

        public static bool IsInteger(string strNumber)
        {
            var regex1 = new Regex("[^0-9-]");
            var regex2 = new Regex("^-[0-9]+$|^[0-9]+$");
            if (!regex1.IsMatch(strNumber))
                return regex2.IsMatch(strNumber);
            return false;
        }

        public static bool IsPositiveNumber(string strNumber)
        {
            var regex1 = new Regex("[^0-9.]");
            var regex2 = new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
            var regex3 = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            if (!regex1.IsMatch(strNumber) && regex2.IsMatch(strNumber))
                return !regex3.IsMatch(strNumber);
            return false;
        }

        public static bool IsNumber(string strNumber)
        {
            var regex1 = new Regex("[^0-9.-]");
            var regex2 = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            var regex3 = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            var regex4 = new Regex("(" + "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$" + ")|(" + "^([-]|[0-9])[0-9]*$" + ")");
            if (!regex1.IsMatch(strNumber) && !regex2.IsMatch(strNumber) && !regex3.IsMatch(strNumber))
                return regex4.IsMatch(strNumber);
            return false;
        }

        public static bool IsAlpha(string strToCheck)
        {
            return !new Regex("[^a-zA-Z]").IsMatch(strToCheck);
        }

        public static bool IsAlphaNumeric(string strToCheck)
        {
            return !new Regex("[^a-zA-Z0-9]").IsMatch(strToCheck);
        }

        public static bool IsNull(object o)
        {
            if (o != null)
                return o == DBNull.Value;
            return true;
        }

        public static bool IsEqual(object o1, object o2)
        {
            var flag1 = IsNull(o1);
            var flag2 = IsNull(o2);
            if (flag1 && flag2)
                return true;
            if (flag1 != flag2)
                return false;
            return o1.Equals(o2);
        }

        public static bool IsNullOrEmpty(object o)
        {
            if (!IsNull(o))
                return o.ToString().Trim().Length == 0;
            return true;
        }

        public static bool IsNullOrEmptyOrZero(object o)
        {
            if (IsNullOrEmpty(o))
                return true;
            if (IsNumericValue(o))
                return Math.Abs(Convert.ToDouble(o)) < Double.Epsilon;
            return false;
        }

        public static bool IsEqualWithNull(string s1, string s2)
        {
            var flag1 = IsNullOrEmpty(s1);
            var flag2 = IsNullOrEmpty(s2);
            if (flag1 && flag2)
                return true;
            if (flag1 != flag2)
                return false;
            return s1 == s2;
        }

        public static bool IsNumericValue(object o)
        {
            return IsNumericDataType(o.GetType());
        }

        public static bool IsNumericDataType(Type dataType)
        {
            if (!(dataType == typeof(int)) && !(dataType == typeof(long)) && !(dataType == typeof(Decimal)))
                return dataType == typeof(double);
            return true;
        }

        public static bool IsNullableType(Type theType)
        {
            if (theType.IsGenericType)
                return theType.GetGenericTypeDefinition() == typeof(Nullable<>);
            return false;
        }

        public static Type GetUnderlyingType(Type theType)
        {
            if (!IsNullableType(theType))
                return theType;
            return Nullable.GetUnderlyingType(theType);
        }

        public static string AdjustUtcXml(string xmlString)
        {
            return Regex.Replace(xmlString, "(?<DATE>\\d{4}-\\d{2}-\\d{2})(?<TIME>T(\\d{2}:\\d{2}:\\d{2}.\\d{7}|\\d{2}:\\d{2}:\\d{2}))(?<TZ>[+-]\\d{2}:\\d{2})", "${DATE}${TIME}");
        }

        public static StringDictionary ParseArguments(string[] args)
        {
            var stringDictionary = new StringDictionary();
            var regex1 = new Regex("^-{1,2}|^/|=", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var regex2 = new Regex("^['\"]?(.*?)['\"]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var key1 = (string)null;
            foreach (var input in args)
            {
                var strArray = regex1.Split(input, 3);
                switch (strArray.Length)
                {
                    case 1:
                        if (key1 != null)
                        {
                            if (!stringDictionary.ContainsKey(key1))
                            {
                                strArray[0] = regex2.Replace(strArray[0], "$1");
                                stringDictionary.Add(key1, strArray[0]);
                            }
                            key1 = null;
                        }
                        break;
                    case 2:
                        if (key1 != null && !stringDictionary.ContainsKey(key1))
                            stringDictionary.Add(key1, "true");
                        key1 = strArray[1];
                        break;
                    case 3:
                        if (key1 != null && !stringDictionary.ContainsKey(key1))
                            stringDictionary.Add(key1, "true");
                        var key2 = strArray[1];
                        if (!stringDictionary.ContainsKey(key2))
                        {
                            strArray[2] = regex2.Replace(strArray[2], "$1");
                            stringDictionary.Add(key2, strArray[2]);
                        }
                        key1 = null;
                        break;
                }
            }
            if (key1 != null && !stringDictionary.ContainsKey(key1))
                stringDictionary.Add(key1, "true");
            return stringDictionary;
        }

        public static StringDictionary ParseQueryString(string queryString)
        {
            var stringDictionary = new StringDictionary();
            queryString = queryString.TrimStart(' ', '?');
            if (!queryString.EndsWith("&"))
                queryString = queryString + "&";
            foreach (Match match in new Regex("(?<name>[^=&]+)=(?<value>[^&]+)&", RegexOptions.IgnoreCase | RegexOptions.Compiled).Matches(queryString))
            {
                var key = match.Result("${name}");
                var str = match.Result("${value}");
                stringDictionary.Add(key, str);
            }
            return stringDictionary;
        }

        public static string UriPathNoQuery(Uri uri)
        {
            var uriBuilder = new UriBuilder(uri);
            var list = new List<string>(uriBuilder.Uri.Segments);
            if (!list[list.Count - 1].EndsWith("/"))
                list[list.Count - 1] = "";
            uriBuilder.Path = string.Join("", list.ToArray());
            uriBuilder.Query = "";
            return uriBuilder.Uri.AbsoluteUri;
        }

        public static int ParseColumnClause(string columnClause, List<string> columnList)
        {
            columnList.Clear();
            var strArray = columnClause.Split(new[]
      {
        ','
      });
            var num = 0;
            var str1 = string.Empty;
            foreach (var str2 in strArray)
            {
                num = num + (str2.Split(new[]
        {
          '('
        }).Length - 1) - (str2.Split(new[]
        {
          ')'
        }).Length - 1);
                var str3 = str1 + str2.Trim();
                if (num > 0)
                {
                    str1 = str3 + ",";
                }
                else
                {
                    if (str3.Length > 0)
                        columnList.Add(str3);
                    num = 0;
                    str1 = string.Empty;
                }
            }
            return columnList.Count;
        }

        public static bool ParseSelectSql(string selectSql, ref string select, ref string from, ref string where, ref string groupBy, ref string having, ref string orderBy)
        {
            if (@select == null) throw new ArgumentNullException("select");
            selectSql = selectSql.Replace("\n", " ").Replace("\r", " ");
            select = from = where = groupBy = having = orderBy = string.Empty;
            var startIndex = selectSql.ToUpper().IndexOf("SELECT ", StringComparison.Ordinal);
            if (startIndex < 0)
                return false;
            selectSql = selectSql.Substring(startIndex).Trim();
            var num1 = selectSql.ToUpper().IndexOf(" ORDER BY ", StringComparison.Ordinal);
            if (num1 >= 0)
            {
                orderBy = selectSql.Substring(num1).Trim();
                selectSql = selectSql.Substring(0, num1).Trim();
            }
            var num2 = selectSql.ToUpper().IndexOf(" HAVING ", StringComparison.Ordinal);
            if (num2 >= 0)
            {
                having = selectSql.Substring(num2).Trim();
                selectSql = selectSql.Substring(0, num2).Trim();
            }
            var num3 = selectSql.ToUpper().IndexOf(" GROUP BY ", StringComparison.Ordinal);
            if (num3 >= 0)
            {
                groupBy = selectSql.Substring(num3).Trim();
                selectSql = selectSql.Substring(0, num3).Trim();
            }
            var num4 = selectSql.ToUpper().IndexOf(" WHERE ", StringComparison.Ordinal);
            if (num4 >= 0)
            {
                where = selectSql.Substring(num4).Trim();
                selectSql = selectSql.Substring(0, num4).Trim();
            }
            var num5 = selectSql.ToUpper().IndexOf(" FROM ", StringComparison.Ordinal);
            if (num5 >= 0)
            {
                from = selectSql.Substring(num5).Trim();
                select = selectSql.Substring(0, num5).Trim();
            }
            return true;
        }

        public static object ConvertToType(string value, Type type)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            DateTime result1;
            int result2;
            Decimal result3;
            double result4;
            return !(type == typeof(string)) ? (!(type == typeof(bool)) ? (!(type == typeof(DateTime)) ? (!(type == typeof(int)) ? (!(type == typeof(Decimal)) ? (!(type == typeof(double)) ? value : (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result4) ? null : (object)result4)) : (!Decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result3) ? null : (object)result3)) : (!int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result2) ? null : (object)result2)) : (!DateTime.TryParse(value, out result1) ? null : (object)result1)) : (value.Equals("1") || value.Equals("true") ? true : (object)false)) : value;
        }
    }
}
