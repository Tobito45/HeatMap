using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatMapController : MonoBehaviour
{
    public GameObject prefab;
    public float distanceBetweenMaps = 2;
    List<HeatMapClass> heatmaps;


    // Start is called before the first frame update
    void Start()
    {
       
        heatmaps = getJsonFiles("heatmap1");

        for(int i = 0; i < heatmaps.Count; i ++)
        {
            createHeatMap(i);
        } 
        
    }

    void createHeatMap(int indexInList)
    {
        GameObject newHeatMap = Instantiate(prefab, new Vector3(indexInList * distanceBetweenMaps, 0, 0), Quaternion.identity);
        for(int i = 0; i < heatmaps[indexInList].coordinates.Length; i++)
        {
            HeatMapClass heatMap = heatmaps[indexInList];
            Coord coord = heatmaps[indexInList].coordinates[i];
            newHeatMap.GetComponent<ShowOnMap>().createCircle(detectColor(heatMap.colorName), coord.x, coord.y, coord.size);

        }
    }

    Color detectColor(string col)
    {
        switch(col)
        {
            case "green":
                return Color.green;
            case "orange":
                return new Color(1,0.5f,0,1);
            case "red":
                return Color.red;
            default:
                Debug.Log("Cant detect color");
                return Color.white;

        }
    }


    public List<HeatMapClass> getJsonFiles(string name_src)
    {
        TextAsset[] asset = Resources.LoadAll<TextAsset>(name_src);
        List<HeatMapClass> assets = new List<HeatMapClass>();
        for(int i=0;i<asset.Length;i++)
            assets.Add(JsonUtility.FromJson<HeatMapClass>(asset[i].ToString()));
        return assets;
    }
    [System.Serializable]
    public class HeatMapClass{
        public string colorName;
        public Coord[] coordinates;
         
        //ColorUtility.TryParseHtmlString(colorName.ToString(), out color); 
    }
    [System.Serializable]
    public class Coord
    {
        public float x;
        public float y;
        public float size;
    }
}
