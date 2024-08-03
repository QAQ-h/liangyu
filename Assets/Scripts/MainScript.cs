using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using static UnityEngine.Random;

public class MainScript : MonoBehaviour
{

    public TMP_InputField before;
    public TMP_InputField after;

    public GameObject setting;

    public void AddEncry()
    {
        string str = before.text;
        str = Program.instance.AddEncry(str);
        after.text = str;
    }

    public void RemoveEncry()
    {
        string str = after.text;
        str = Program.instance.RemoveEncry(str);
        before.text = str;
    }

    public void CreatTxtLib()
    {

        string basepath = DocManager.libPath + "/" + "baselib.txtlib";

        string doc = File.ReadAllText(basepath);

        bool[] used = new bool[doc.Length];
        for (int i = 0; i < doc.Length; i++)
        {
            used[i] = false;
        }

        string newdoc = "";

        int x = 0;
        while (x < doc.Length)
        {
            int val = Range(0, doc.Length);
            if (!used[val])
            {
                used[val] = true;
                newdoc += doc[val];
                ++x;

            }
        }
        string libName = "";
        for(int i=0;i<5;++i)
        {
            libName += newdoc[i];
        }
        libName += "_¿â.txtlib";
        File.WriteAllText(DocManager.libPath+"/"+libName, newdoc);

        DocManager.instance.SettingInit();
    }


    public void AutoSetting()
    {
        setting.SetActive(!setting.activeSelf);
        DocManager.instance.SettingInit();
    }
}
