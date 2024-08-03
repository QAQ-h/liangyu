using System.Text;
using System;
using System.IO;
using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

public class Program:MonoBehaviour
{
    public int seed=1;
    public string doc;
    public Dictionary<char, int> dockey = new();

    public static Program instance;


    private void Start()
    {
        instance = this;

        //doc = Resources.Load<TextAsset>("简体中文").text;
        SetLib();

        SetSeed();
    }
    #region
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string AddEncry(string str)
    {
        string result = "";
        for(int i=0;i<str.Length; i++)
        {
            char c = str[i];
            string ch = c.ToString();

            //int x = doc.IndexOf(c);

            if (!dockey.ContainsKey(c))
            {

                //如果是字母数字
                if (IsNumOrCh(c))
                {
                    //ch = "\ufeff" + doc[c+seed];
                    ch = "\ufeff" + doc[c+seed];
                    if(i==0)
                    {
                        ch = ' ' + ch;
                    }
                }
            }
            else
            {
                int x = dockey[c];
                if (x >= 0 && x < dockey.Count - seed)
                {
                    x += seed;
                }
                else
                {
                    x -= dockey.Count - seed;
                }
                ch = doc[x].ToString();

            }

            result += ch;
        }
        return result;
    }
    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string RemoveEncry(string str)
    {
        string resukt = "";
        for(int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            string ch = c.ToString();
            if (!dockey.ContainsKey(c))
            {
                if(i==0&&c==' ')
                {
                    i += 2;
                    ch = RemoveNum(str[i]);
                }
                if(c=='\ufeff')
                {
                    ++i;
                    ch = RemoveNum(str[i]);
                }
            }
            else
            {
                int x = dockey[c];
                if (x >= 0 && x <seed)
                {
                    x=dockey.Count+x-seed;
                }
                else
                {
                    x -= seed;
                }
                ch = doc[x].ToString();
            }
            resukt += ch;
        }
        return resukt;
    }

    public string RemoveNum(char c)
    {
        if (dockey.ContainsKey(c))
        {
            int ind = dockey[c] - seed;
            if (IsNumOrCh(ind))
            {
                char h = Convert.ToChar(ind);
                return h.ToString();
            }
        }
        return c.ToString();
    }
    #endregion
    public void SetSeed()
    {
        if (File.Exists(DocManager.xmlPath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(DocManager.xmlPath);
            seed= Convert.ToInt32(xmlDoc.DocumentElement.SelectSingleNode("value").InnerText);
            return;
        }
        seed=1;
    }
    public void SaveSeed()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(DocManager.xmlPath);
        xmlDoc.DocumentElement.SelectSingleNode("value").InnerText = seed.ToString();
        xmlDoc.Save(DocManager.xmlPath);
    }
    public void SaveLib(string name)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(DocManager.xmlPath);
        xmlDoc.DocumentElement.SelectSingleNode("lib").InnerText =name;
        xmlDoc.Save(DocManager.xmlPath);
    }

    public void SetLib()
    {
        if (!File.Exists(DocManager.xmlPath))
        {
            doc = Resources.Load<TextAsset>("简体中文").text;
            HashInit(); 
            return;
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(DocManager.xmlPath);
        doc = File.ReadAllText(DocManager.libPath + "/" + xmlDoc.DocumentElement.SelectSingleNode("lib").InnerText + ".txtlib");
        HashInit();
    }

    public bool IsNumOrCh(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');
    }    


    public bool IsNumOrCh(int c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');
    }

    public void HashInit()
    {
        dockey.Clear();
        for(int i=0;i<doc.Length; i++)
        {
            dockey.Add(doc[i], i);
        }
    }
}