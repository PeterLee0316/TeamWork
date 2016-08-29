using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using static LWDicer.Layers.DEF_Scanner;

namespace LWDicer.Layers
{
    public interface IMarkingScanner
    {
       // /*------------------------------------------------------------------------------------
       //* Date : 2016.02.24
       //* Author : HSLEE
       //* Function : GetValue(String Section, String Key, String iniPath)
       //* Description : Text File Data Load 처리
       //------------------------------------------------------------------------------------*/
       // public String GetValue(String Section, String Key, String iniPath)
       // {
       //     StringBuilder temp = new StringBuilder(255);
       //     int i = GetPrivateProfileString(Section, Key, "", temp, 255, iniPath);
       //     return temp.ToString();
       // }

       // /*------------------------------------------------------------------------------------
       //  * Date : 2016.02.24
       //  * Author : HSLEE
       //  * Function : SetValue(String Section, String Key, String Value, String iniPath)
       //  * Description : Text File Data Save 처리
       //  ------------------------------------------------------------------------------------*/
       // public bool SetValue(String Section, String Key, String Value, String iniPath)
       // {
       //     bool bRet = WritePrivateProfileString(Section, Key, Value, iniPath);
       //     return WritePrivateProfileString(Section, Key, Value, iniPath);
       // }


       // [DllImport("kernel32.dll")]
       // public static extern int GetPrivateProfileString(
       //                             String section,
       //                             String key,
       //                             String def,
       //                             StringBuilder retVal,
       //                             int size,
       //                             String filePath);

       // [DllImport("kernel32.dll")]
       // public static extern bool WritePrivateProfileString(
       //                             String section,
       //                             String key,
       //                             String val,
       //                             String filePath);

    }


    
}
