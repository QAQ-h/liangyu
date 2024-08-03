using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TMPro;
using UnityEngine;

public class DocManager : MonoBehaviour
{
    /// <summary>
    /// 所有字库的路径
    /// </summary>
    public List<string> libs=new List<string>();

    public TMP_InputField value;

    public TextMeshProUGUI now;

    public GameObject lib;
    public Transform parent;
    public void SetValue()
    {
        Program.instance.seed=Convert.ToInt32(value.text);
        Program.instance.SaveSeed();
    }
    private void Start()
    {
        instance = this;
        Init();
    }
    /// <summary>
    /// 第一次打开程序
    /// </summary>
    private void Init()
    {

        if(!Directory.Exists(basePath))
        {
            DirectoryInfo basedir = Directory.CreateDirectory(basePath);
            //字库文件夹
            DirectoryInfo libdir = Directory.CreateDirectory(libPath);
            //将默认字库添加进字库文件夹
            string defaultlib = libPath + "/baselib.txtlib";

            Program.instance.doc = Resources.Load<TextAsset>("简体中文").text;

            File.WriteAllText(defaultlib, Program.instance.doc);
            Program.instance.HashInit();

            libs.Add(defaultlib);

            XmlDocument doc = new XmlDocument();
            XmlElement root=doc.CreateElement("root");
            XmlElement value=doc.CreateElement("value");
            value.InnerText=Program.instance.seed.ToString();
            XmlElement lib = doc.CreateElement("lib");
            root.AppendChild(lib);
            root.AppendChild(value);
            doc.AppendChild(root);
            doc.Save(xmlPath);
        }

    }
    public void SettingInit()
    {
        value.text = Program.instance.seed.ToString();

        for(int i=0;i<parent.childCount;i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }

        DirectoryInfo libdir = new DirectoryInfo(libPath);
        foreach(FileInfo file in libdir.GetFiles())
        {
            if(file.FullName.EndsWith(".txtlib"))
            {
                string fileName = file.FullName;
                string libName=Path.GetFileNameWithoutExtension(fileName);

                GameObject g = Instantiate(lib);
                DocLib doclib=g.GetComponent<DocLib>();

                doclib.path = fileName;
                doclib.title = libName;

                doclib.transform.SetParent(parent,false);
            }
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlPath);
        SetLib(xmlDoc.DocumentElement.SelectSingleNode("lib").InnerText);
    }
    public void SetLib(string libname)
    {
        now.text = "正在使用:" +libname;    
    }
    public static DocManager instance;
    public static string basePath { get { return Application.persistentDataPath + "/Info"; } }
    public static string libPath { get { return basePath + "/libs"; } }
    public static string xmlPath { get { return basePath + "/Options.xml"; } }
}
