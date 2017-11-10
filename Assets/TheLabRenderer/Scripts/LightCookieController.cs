using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LightCookieController : MonoBehaviour {


    

    public Texture2D[] CookieList;
    [Range(16,8192)]   
    public int MasterCookieResolution = 256;

    [SerializeField]
    static public Texture2DArray CookieArray;

  //  public RenderTexture TempRT;
      //  public Texture2D tempTex;


    public void OnValidate()
    {

        if (FindObjectsOfType<LightCookieController>().Length > 1)
        {
            Debug.LogError("Found Another Instance of cookie Controller in scene");         
        }


        if (CookieList != null)
        {
            MakeArray();
        }

    }


    public void Start()
    {
        MakeArray();
    }



    public void MakeArray()
    {

       // Debug.Log("make it");
        
        //Make Cookie Texture Array

        CookieArray = new Texture2DArray(MasterCookieResolution, MasterCookieResolution, CookieList.Length, TextureFormat.ARGB32, true, true);
        Texture2D tempTex = new Texture2D(MasterCookieResolution, MasterCookieResolution, TextureFormat.ARGB32, true);
        RenderTexture TempRT = new RenderTexture(MasterCookieResolution, MasterCookieResolution, 16, RenderTextureFormat.ARGB32);

        TempRT.Create();
        //Casting Array to RT to normalize texture sizes and avoid setting restrictions

        for (int i = 0; i < CookieList.Length; i++)
        {
            Graphics.Blit(CookieList[i], TempRT);

            //Move RT to tex2D to get pixels
            RenderTexture.active = TempRT;
            tempTex.ReadPixels(new Rect(0, 0, TempRT.width, TempRT.height), 0, 0);
            tempTex.Apply();

            //Set Pixels to array
             CookieArray.SetPixels32(tempTex.GetPixels32(0), i, 0);
        }

        CookieArray.Apply();

        //clear from memory

        RenderTexture.active = null;
        TempRT.Release();
        TempRT.DiscardContents();
        DestroyImmediate(TempRT);
        DestroyImmediate (tempTex);



        ApplyArray();

    }



    public void ApplyArray()
    {
        Shader.SetGlobalTexture("g_tVrLightCookieTexture", CookieArray);               
    }

}


#if UNITY_EDITOR

[CustomEditor(typeof(LightCookieController))]
public class LightCookieControllerGUI : Editor
{

    LightCookieController LCC;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LCC = (LightCookieController)target;

        if (GUILayout.Button("Make Array"))
        {
            LCC.MakeArray();

        }


    }
}


#endif
