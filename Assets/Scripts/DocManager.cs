using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DocManager : MonoBehaviour
{

    public TMP_InputField value;

    public TextMeshProUGUI now;

    public GameObject lib;
    public Transform parent;

    public TMP_InputField libpath;
    
    public void SetValue()
    {
        Program.instance.seed=Convert.ToInt32(value.text);
        Program.instance.SaveSeed();
    }

    public void SetLibPath()
    {
        string path=libpath.text;
        if(!Directory.Exists(path))
        {
            libpath.text = libPath;
            return;
        }
        libPath = path;
        XmlDocument doc= new XmlDocument();
        doc.Load(xmlPath);
        doc.DocumentElement.SelectSingleNode("libpath").InnerText = path;
        doc.Save(xmlPath);
        SceneManager.LoadScene(0);
    }
    private void Start()
    {
        instance = this;
        Init();
        LibPathInit();
    }

    private void LibPathInit()
    {
        if (File.Exists(xmlPath))
        {


            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            if(doc.DocumentElement.HasAttribute("libpath"))
            {
                libPath = doc.DocumentElement.SelectSingleNode("libpath").InnerText;
                return;
            }
            XmlElement e= doc.CreateElement("libpath");
            e.InnerText = libPath;
            doc.DocumentElement.AppendChild(e);
            doc.Save(xmlPath) ;
        }
    }
    /// <summary>
    /// 第一次打开程序
    /// </summary>
    private void Init()
    {
#if UNITY_EDITOR_WIN
        datapath=Application.persistentDataPath;
#endif
        //检查的是是否存在basepath
        if(!Directory.Exists(basePath))
        {
            DirectoryInfo basedir = Directory.CreateDirectory(basePath);
        }
        if(!Directory.Exists(libPath))
        {
            //字库文件夹
            DirectoryInfo libdir = Directory.CreateDirectory(libPath);
            //将默认字库添加进字库文件夹
            string defaultlib = libPath + "/baselib.txtlib";

            Program.instance.doc = Resources.Load<TextAsset>("简体中文").text;

            File.WriteAllText(defaultlib, Program.instance.doc);
            Program.instance.HashInit();
        }
        if(!File.Exists(xmlPath))
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("root");
            XmlElement value = doc.CreateElement("value");
            value.InnerText = Program.instance.seed.ToString();
            XmlElement lib = doc.CreateElement("lib");
            XmlElement libpath= doc.CreateElement("libpath");
            libpath.InnerText = libPath;
            root.AppendChild(libpath);
            root.AppendChild(lib);
            root.AppendChild(value);
            doc.AppendChild(root);
            doc.Save(xmlPath);
        }
    }
    public void SettingInit()
    {
        libpath.text = libPath;
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
    private static string datapath = "/storage/emulated/0/Download/Liangyu";
    public static string basePath { get { return  Application.persistentDataPath+ "/Info"; } }
    public static string libPath { get { return datapath + "/libs"; } set { datapath = value; } }
    public static string xmlPath { get { return basePath + "/Options.xml"; } }
}
