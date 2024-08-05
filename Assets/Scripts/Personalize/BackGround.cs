using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BackGround : MonoBehaviour
{
    public static Texture2D t;
    public RawImage back;
    private void Start()
    {
        back= GetComponent<RawImage>();
        if(File.Exists(backgroundpath))
        {
            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(File.ReadAllBytes(backgroundpath));
            t = texture;
        }
        else
        {
            t = Resources.Load<Texture2D>("back");
        }
        back.texture = t;
    }
    public static string backgroundpath { get { return DocManager.basePath + "/" + "back.jpg"; } }
}
