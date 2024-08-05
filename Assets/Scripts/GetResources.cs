using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class GetResources: MonoBehaviour
{
    public string path;
    public ResourcesCallBack callBack;
    //用Button事件监听该函数执行
    public void GetPhoto()
    {
        path = null;
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        //AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //jo.Call("TakePhoto", Application.persistentDataPath);
        using (AndroidJavaObject javaobj = new AndroidJavaObject("com.defaultcompany.galgamemaker.mylibrary.CallAndroid"))
        {
            javaobj.Call("TakePhoto", Application.persistentDataPath);
        }
    }
    public void GetVideo()
    {
        path = null;
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        using (AndroidJavaObject javaobj = new AndroidJavaObject("com.defaultcompany.galgamemaker.mylibrary.CallAndroid"))
        {
            javaobj.Call("TakeVideo", Application.persistentDataPath);
        }
    }
    public void GetAudio()
    {
        path = null;
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        using (AndroidJavaObject javaobj = new AndroidJavaObject("com.defaultcompany.galgamemaker.mylibrary.CallAndroid"))
        {
            javaobj.Call("TakeAudio", Application.persistentDataPath);
        }
    }
    public void GetHtml()
    {
        path = null;
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        using (AndroidJavaObject javaobj = new AndroidJavaObject("com.defaultcompany.galgamemaker.mylibrary.CallAndroid"))
        {
            javaobj.Call("TakeHtml", Application.persistentDataPath);
        }
    }
    public void GetText()
    {
        path = null;
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        using (AndroidJavaObject javaobj = new AndroidJavaObject("com.defaultcompany.galgamemaker.mylibrary.CallAndroid"))
        {
            javaobj.Call("TakeText", Application.persistentDataPath);
        }
    }
    public void Get()
    {
        path = null;
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        using (AndroidJavaObject javaobj = new AndroidJavaObject("com.defaultcompany.galgamemaker.mylibrary.CallAndroid"))
        {
            javaobj.Call("TakeAny", Application.persistentDataPath);
        }
    }
    //Java获取path传递给unity
    public void GetPath(string path)
    {
        this.path=path;
        callBack(path);
    }

    private void Start()
    {
        instance = this;
    }

    public delegate void ResourcesCallBack(string path);

    public static GetResources instance;
}
