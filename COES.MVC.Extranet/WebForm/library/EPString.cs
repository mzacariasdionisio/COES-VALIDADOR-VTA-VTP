using System;
using System.Collections;
using System.Collections.Generic;

namespace WSIC2010
{
	/// <summary>
	/// Funtions for manipulate fields separate with pipes in a string
	/// </summary>
	public class EPString
	{
		public EPString()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		static public string EP_GetFirstString(string s)
		{
         if (s == null) return null;
			int j = 0;
			while ((j < s.Length) && !(Char.IsWhiteSpace(s, j) || Char.IsControl(s, j) || s[j] == ',')) j++;
			return s.Substring(0, j);
		}

        static public string EP_GetRestStringAfterFirst(string s, char ch)
        {
            if (s == null) return null;
            int j = 0;
            while ((j < s.Length) && s[j] != ch) j++;
            return s.Substring(j);
        }


      static public string EP_GetEliminateDoubleWhiteSpaceString(string s)
      {
         if (s == null) return null;
         int j = 0;
         while( s.IndexOf("  ") > 0)
         {
            s = s.Replace("  ", " ");
         }
         return s;
      }

      static public string EP_GetFirstString(string s, char ch)
      {
         if (s == null) return null;
         int j = 0;
         while ((j < s.Length) && s[j] != ch) j++;
         return s.Substring(0, j);
      }

		static public ArrayList EP_GetArrayStringSeparateWithPipes(string s)
		{
			ArrayList AL_temp = new ArrayList();
			int j = 0;
			int jlast = 0;
			while (j < s.Length)
			{
				while ((j < s.Length) && (s[j] != '|')) j++;
				AL_temp.Add(s.Substring(jlast,j-jlast));
				j++;
				jlast = j;
			}
			return AL_temp;
		}

		static public ArrayList EP_GetArrayStringSeparate(string s,char ch)
		{
			ArrayList AL_temp = new ArrayList();
			int j = 0;
			int jlast = 0;
			while (j < s.Length)
			{
				while ((j < s.Length) && (s[j] != ch)) j++;
				AL_temp.Add(s.Substring(jlast,j-jlast));
				j++;
				jlast = j;
			}
			return AL_temp;
		}

      static public List<string> EP_GetListStringSeparate(string s, char ch)
      {
         List<string> AL_temp = new List<string>();
         int j = 0;
         int jlast = 0;
         while (j < s.Length)
         {
            while ((j < s.Length) && (s[j] != ch)) j++;
            AL_temp.Add(s.Substring(jlast, j - jlast).Trim());
            //AL_temp.Add(s.Substring(jlast, j - jlast));
            j++;
            jlast = j;
         }
         return AL_temp;
      }

      static public List<string> EP_GetListStringSeparate(string s, char ch1, char ch2)
      {
         List<string> AL_temp = new List<string>();
         int j = 0;
         int jlast = 0;
         while (j < s.Length)
         {
            while ((j < s.Length) && (s[j] != ch1) && (s[j] != ch2)) j++;
            AL_temp.Add(s.Substring(jlast, j - jlast).Trim());
            //AL_temp.Add(s.Substring(jlast, j - jlast));
            j++;
            jlast = j;
         }
         return AL_temp;
      }


		static public string EP_GetItemStringSeparate(string s,char ch,int index)
		{
			int j = 0;
			int jlast = 0;
			int i_temp = 0;
			while (j < s.Length)
			{
				while ((j < s.Length) && (s[j] != ch)) j++;
				if (i_temp==index) return s.Substring(jlast,j-jlast);
				i_temp++;
				j++;
				jlast = j;
			}
			return "";
		}

	}
}
