using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DocLib : MonoBehaviour
{
    public string path;
    public string title;

    public TextMeshProUGUI libName;

    private void Start()
    {
        libName.text = title;
        
    }
    public void Use()
    {
        Program.instance.doc=File.ReadAllText(path);
        Program.instance.HashInit();
        Program.instance.SaveLib(title);
        DocManager.instance.SetLib(title);
    }
}
