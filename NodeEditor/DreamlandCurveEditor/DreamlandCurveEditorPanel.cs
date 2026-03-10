using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NodeEditor;
using Sirenix.OdinInspector;
using TableDR;
using UnityEngine;

[Serializable]
public class DreamlandCurveEditorPanel
{
    private enum LoadType
    {
        LT_Origin = 0,      // 原生序列化
        LT_Json,            // json序列化
    }
    private class CurvePropertyConfig
    {
        public int iIndex;                         // excel中的列的位置
        public string strPropertyName;                // 属性名称
        public string strConfigName;                  // excel表的名称
        public LoadType loadType = LoadType.LT_Origin;  // 序列化方式
        public Type propertyType = typeof(int);
    }

    private static readonly List<CurvePropertyConfig> curvePropertyConfigs = new List<CurvePropertyConfig>() {
        new CurvePropertyConfig() {iIndex = 1, strPropertyName = nameof(id), strConfigName = "ID",loadType = LoadType.LT_Origin ,propertyType = typeof(int)},
        new CurvePropertyConfig() {iIndex = 2, strPropertyName = nameof(strName), strConfigName = "Name",loadType = LoadType.LT_Origin ,propertyType = typeof(string)},

        new CurvePropertyConfig() {iIndex = 3, strPropertyName = nameof(fShakeFrequency_X), strConfigName = "Frequency_X",loadType = LoadType.LT_Origin,propertyType = typeof(float) },
        new CurvePropertyConfig() {iIndex = 4, strPropertyName = nameof(fShakeRange_X), strConfigName = "Range_X",loadType = LoadType.LT_Origin,propertyType = typeof(float) },
        new CurvePropertyConfig() {iIndex = 5, strPropertyName = nameof(iSamplings_X), strConfigName = "Samplings_X",loadType = LoadType.LT_Origin ,propertyType = typeof(int)},
        new CurvePropertyConfig() {iIndex = 6, strPropertyName = nameof(curve_X), strConfigName = "Curve_X",loadType = LoadType.LT_Json ,propertyType = typeof(AnimationCurve)},

        new CurvePropertyConfig() {iIndex = 7, strPropertyName = nameof(fShakeFrequency_Y), strConfigName = "Frequency_Y",loadType = LoadType.LT_Origin,propertyType = typeof(float) },
        new CurvePropertyConfig() {iIndex = 8, strPropertyName = nameof(fShakeRange_Y), strConfigName = "Range_Y",loadType = LoadType.LT_Origin,propertyType = typeof(float) },
        new CurvePropertyConfig() {iIndex = 9, strPropertyName = nameof(iSamplings_Y), strConfigName = "Samplings_Y",loadType = LoadType.LT_Origin ,propertyType = typeof(int)},
        new CurvePropertyConfig() {iIndex = 10, strPropertyName = nameof(curve_Y), strConfigName = "Curve_Y",loadType = LoadType.LT_Json ,propertyType = typeof(AnimationCurve)},

        new CurvePropertyConfig() {iIndex = 11, strPropertyName = nameof(fShakeFrequency_Z), strConfigName = "Frequency_Z",loadType = LoadType.LT_Origin,propertyType = typeof(float) },
        new CurvePropertyConfig() {iIndex = 12, strPropertyName = nameof(fShakeRange_Z), strConfigName = "Range_Z",loadType = LoadType.LT_Origin,propertyType = typeof(float) },
        new CurvePropertyConfig() {iIndex = 13, strPropertyName = nameof(iSamplings_Z), strConfigName = "Samplings_Z",loadType = LoadType.LT_Origin ,propertyType = typeof(int)},
        new CurvePropertyConfig() {iIndex = 14, strPropertyName = nameof(curve_Z), strConfigName = "Curve_Z",loadType = LoadType.LT_Json ,propertyType = typeof(AnimationCurve)},

        new CurvePropertyConfig() {iIndex = 15, strPropertyName = nameof(fShakeFrequency_Yaw), strConfigName = "Frequency_Yaw",loadType = LoadType.LT_Origin,propertyType = typeof(float) },
        new CurvePropertyConfig() {iIndex = 16, strPropertyName = nameof(fShakeRange_Yaw), strConfigName = "Range_Yaw",loadType = LoadType.LT_Origin,propertyType = typeof(float) },
        new CurvePropertyConfig() {iIndex = 17, strPropertyName = nameof(iSamplings_Yaw), strConfigName = "Samplings_Yaw",loadType = LoadType.LT_Origin ,propertyType = typeof(int)},
        new CurvePropertyConfig() {iIndex = 18, strPropertyName = nameof(curve_Yaw), strConfigName = "Curve_Yaw",loadType = LoadType.LT_Json ,propertyType = typeof(AnimationCurve)},

        new CurvePropertyConfig() {iIndex = 19, strPropertyName = nameof(fShakeFrequency_Pitch), strConfigName = "Frequency_Pitch",loadType = LoadType.LT_Origin,propertyType = typeof(float) },
        new CurvePropertyConfig() {iIndex = 20, strPropertyName = nameof(fShakeRange_Pitch), strConfigName = "Range_Pitch",loadType = LoadType.LT_Origin,propertyType = typeof(float) },
        new CurvePropertyConfig() {iIndex = 21, strPropertyName = nameof(iSamplings_Pitch), strConfigName = "Samplings_Pitch",loadType = LoadType.LT_Origin ,propertyType = typeof(int)},
        new CurvePropertyConfig() {iIndex = 22, strPropertyName = nameof(curve_Pitch), strConfigName = "Curve_Pitch",loadType = LoadType.LT_Json ,propertyType = typeof(AnimationCurve)},

        new CurvePropertyConfig() {iIndex = 23, strPropertyName = nameof(fShakeFrequency_Roll), strConfigName = "Frequency_Roll",loadType = LoadType.LT_Origin,propertyType = typeof(float) },
        new CurvePropertyConfig() {iIndex = 24, strPropertyName = nameof(fShakeRange_Roll), strConfigName = "Range_Roll",loadType = LoadType.LT_Origin,propertyType = typeof(float) },
        new CurvePropertyConfig() {iIndex = 25, strPropertyName = nameof(iSamplings_Roll), strConfigName = "Samplings_Roll",loadType = LoadType.LT_Origin ,propertyType = typeof(int)},
        new CurvePropertyConfig() {iIndex = 26, strPropertyName = nameof(curve_Roll), strConfigName = "Curve_Roll",loadType = LoadType.LT_Json ,propertyType = typeof(AnimationCurve)},
    };
    private bool bTrue = true;

    [LabelText("ID")]
    [DisableIf("bTrue")]
    [PropertyOrder(10)]
    public int id = 0;

    [LabelText("名称")]
    [PropertyOrder(20)]
    public string strName = "";

    [Range(0, 100f)]
    [LabelText("震动频率_X轴"),TitleGroup("X轴")]
    [PropertyOrder(30)]
    public float fShakeFrequency_X = 0.9f;

    [Range(0, 100f)]
    [LabelText("震动幅度_X轴"), TitleGroup("X轴")]
    [PropertyOrder(40)]
    public float fShakeRange_X = 20.0f;

    [Range(10, 100)]
    [LabelText("采样数量_X轴"), TitleGroup("X轴")]
    [PropertyOrder(50)]
    public int iSamplings_X = 20;

    [LabelText("曲线_X轴"), TitleGroup("X轴")]
    [PropertyOrder(60)]
    public AnimationCurve curve_X = new AnimationCurve();

    [Button("重新自动生成曲线_X轴", ButtonSizes.Medium), TitleGroup("X轴")]
    [PropertyOrder(70)]
    public void OnClickReGenerate_X()
    {
        int iRandomValue = UnityEngine.Random.Range(0, 100000);
        curve_X = new AnimationCurve();
        for (int i = 0; i < iSamplings_X; ++i)
        {
            float noiseFrequency = (iRandomValue + i);
            float fValue = Mathf.PerlinNoise(noiseFrequency, 0.5f);
            fValue = Mathf.Clamp(fValue, 0.0f, 1.0f);
            fValue = (fValue - 0.5f) * fShakeRange_X * 2;
            float fKey = 1.0f / iSamplings_X * i;
            curve_X.AddKey(fKey, fValue);
        }
    }

    [Range(0, 100f)]
    [LabelText("震动频率_Y轴"), TitleGroup("Y轴")]
    [PropertyOrder(80)]
    public float fShakeFrequency_Y = 0.9f;

    [Range(0, 100f)]
    [LabelText("震动幅度_Y轴"), TitleGroup("Y轴")]
    [PropertyOrder(90)]
    public float fShakeRange_Y = 20.0f;

    [Range(10, 100)]
    [PropertyOrder(100)]
    [LabelText("采样数量_Y轴"), TitleGroup("Y轴")]
    public int iSamplings_Y = 20;

    [LabelText("曲线_Y轴"), TitleGroup("Y轴")]
    [PropertyOrder(110)]
    public AnimationCurve curve_Y = new AnimationCurve();

    [Button("重新自动生成曲线_Y轴", ButtonSizes.Medium), TitleGroup("Y轴")]
    [PropertyOrder(120)]
    public void OnClickReGenerate_Y()
    {
        int iRandomValue = UnityEngine.Random.Range(0, 100000);
        curve_Y = new AnimationCurve();
        for (int i = 0; i < iSamplings_Y; ++i)
        {
            float noiseFrequency = (iRandomValue + i);
            float fValue = Mathf.PerlinNoise(noiseFrequency, 0.5f);
            fValue = Mathf.Clamp(fValue, 0.0f, 1.0f);
            fValue = (fValue - 0.5f) * fShakeRange_Y * 2;
            float fKey = 1.0f / iSamplings_Y * i;
            curve_Y.AddKey(fKey, fValue);
        }
    }

    [Range(0, 100f)]
    [LabelText("震动频率_Z轴"), TitleGroup("Z轴")]
    [PropertyOrder(130)]
    public float fShakeFrequency_Z = 0.9f;

    [Range(0, 100f)]
    [LabelText("震动幅度_Z轴"), TitleGroup("Z轴")]
    [PropertyOrder(140)]
    public float fShakeRange_Z = 20.0f;

    [Range(10, 100)]
    [LabelText("采样数量_Z轴"), TitleGroup("Z轴")]
    [PropertyOrder(150)]
    public int iSamplings_Z = 20;

    [LabelText("曲线_Z轴"), TitleGroup("Z轴")]
    [PropertyOrder(160)]
    public AnimationCurve curve_Z = new AnimationCurve();

    [Button("重新自动生成曲线_Z轴", ButtonSizes.Medium), TitleGroup("Z轴")]
    [PropertyOrder(170)]
    private void OnClickReGenerate_Z()
    {
        int iRandomValue = UnityEngine.Random.Range(0, 100000);
        curve_Z = new AnimationCurve();
        for (int i = 0; i < iSamplings_Z; ++i)
        {
            float noiseFrequency = (iRandomValue + i);
            float fValue = Mathf.PerlinNoise(noiseFrequency, 0.5f);
            fValue = Mathf.Clamp(fValue, 0.0f, 1.0f);
            fValue = (fValue - 0.5f) * fShakeRange_Z * 2;
            float fKey = 1.0f / iSamplings_Z * i;
            curve_Z.AddKey(fKey, fValue);
        }
    }

    [Range(0, 100f)]
    [LabelText("震动频率_Yaw"), TitleGroup("Yaw")]
    [PropertyOrder(180)]
    public float fShakeFrequency_Yaw = 0.9f;

    [Range(0, 100f)]
    [LabelText("震动幅度_Yaw"), TitleGroup("Yaw")]
    [PropertyOrder(190)]
    public float fShakeRange_Yaw = 20.0f;

    [Range(10, 100)]
    [LabelText("采样数量_Yaw"), TitleGroup("Yaw")]
    [PropertyOrder(200)]
    public int iSamplings_Yaw = 20;

    [LabelText("曲线_Yaw"), TitleGroup("Yaw")]
    [PropertyOrder(210)]
    public AnimationCurve curve_Yaw = new AnimationCurve();

    [Button("重新自动生成曲线_Yaw", ButtonSizes.Medium), TitleGroup("Yaw")]
    [PropertyOrder(220)]
    private void OnClickReGenerate_Yaw()
    {
        int iRandomValue = UnityEngine.Random.Range(0, 100000);
        curve_Yaw = new AnimationCurve();
        for (int i = 0; i < iSamplings_Yaw; ++i)
        {
            float noiseFrequency = (iRandomValue + i);
            float fValue = Mathf.PerlinNoise(noiseFrequency, 0.5f);
            fValue = Mathf.Clamp(fValue, 0.0f, 1.0f);
            fValue = (fValue - 0.5f) * fShakeRange_Yaw * 2;
            float fKey = 1.0f / iSamplings_Yaw * i;
            curve_Yaw.AddKey(fKey, fValue);
        }
    }

    [Range(0, 100f)]
    [LabelText("震动频率_Pitch"), TitleGroup("Pitch")]
    [PropertyOrder(230)]
    public float fShakeFrequency_Pitch = 0.9f;

    [Range(0, 100f)]
    [LabelText("震动幅度_Pitch"), TitleGroup("Pitch")]
    [PropertyOrder(240)]
    public float fShakeRange_Pitch = 20.0f;

    [Range(10, 100)]
    [LabelText("采样数量_Pitch"), TitleGroup("Pitch")]
    [PropertyOrder(250)]
    public int iSamplings_Pitch = 20;

    [LabelText("曲线_Pitch"), TitleGroup("Pitch")]
    [PropertyOrder(260)]
    public AnimationCurve curve_Pitch = new AnimationCurve();

    [Button("重新自动生成曲线_Pitch", ButtonSizes.Medium), TitleGroup("Pitch")]
    [PropertyOrder(270)]
    private void OnClickReGenerate_Pitch()
    {
        int iRandomValue = UnityEngine.Random.Range(0, 100000);
        curve_Pitch = new AnimationCurve();
        for (int i = 0; i < iSamplings_Pitch; ++i)
        {
            float noiseFrequency = (iRandomValue + i);
            float fValue = Mathf.PerlinNoise(noiseFrequency, 0.5f);
            fValue = Mathf.Clamp(fValue, 0.0f, 1.0f);
            fValue = (fValue - 0.5f) * fShakeRange_Pitch * 2;
            float fKey = 1.0f / iSamplings_Pitch * i;
            curve_Pitch.AddKey(fKey, fValue);
        }
    }

    [Range(0, 100f)]
    [PropertyOrder(280)]
    [LabelText("震动频率_Roll"), TitleGroup("Roll")]
    public float fShakeFrequency_Roll = 0.9f;

    [Range(0, 100f)]
    [LabelText("震动幅度_Roll"), TitleGroup("Roll")]
    [PropertyOrder(290)]
    public float fShakeRange_Roll = 20.0f;

    [Range(10, 100)]
    [LabelText("采样数量_Roll"), TitleGroup("Roll")]
    [PropertyOrder(300)]
    public int iSamplings_Roll = 20;

    [LabelText("曲线_Roll"), TitleGroup("Roll")]
    [PropertyOrder(310)]
    public AnimationCurve curve_Roll = new AnimationCurve();

    [Button("重新自动生成曲线_Roll", ButtonSizes.Medium), TitleGroup("Roll")]
    [PropertyOrder(320)]
    private void OnClickReGenerate_Roll()
    {
        int iRandomValue = UnityEngine.Random.Range(0, 100000);
        curve_Roll = new AnimationCurve();
        for (int i = 0; i < iSamplings_Roll; ++i)
        {
            float noiseFrequency = (iRandomValue + i);
            float fValue = Mathf.PerlinNoise(noiseFrequency, 0.5f);
            fValue = Mathf.Clamp(fValue, 0.0f, 1.0f);
            fValue = (fValue - 0.5f) * fShakeRange_Roll * 2;
            float fKey = 1.0f / iSamplings_Roll * i;
            curve_Roll.AddKey(fKey, fValue);
        }
    }

    public BattleCameraShakeConfig ConvertToConfig()
    {
        BattleCameraShakeConfig config = new BattleCameraShakeConfig();

        for (int i = 0; i < curvePropertyConfigs.Count; ++i)
        {
            CurvePropertyConfig curveProp = curvePropertyConfigs[i];
            object value = this.ExGetValueField(curveProp.strPropertyName);
            if (curveProp.loadType == LoadType.LT_Origin)
            {
                config.ExSetValue(curveProp.strConfigName, value);
            }
            else if (curveProp.loadType == LoadType.LT_Json)
            {
                config.ExSetValue(curveProp.strConfigName, JsonConvert.SerializeObject(value));
            }
        }

        return config;
    }

    public bool ReloadFromExcelData(object[,] excelData, int iCol)
    {
        for (int i = 0; i < curvePropertyConfigs.Count; ++i)
        {
            CurvePropertyConfig curveProp = curvePropertyConfigs[i];
            object configValue = GetExcelData(excelData, iCol, curveProp.iIndex);
            if (curveProp.loadType == LoadType.LT_Json)
            {
                string strDeserialize = configValue as string;
                if (strDeserialize != null && strDeserialize != "")
                {
                    object serializeObj = JsonConvert.DeserializeObject(strDeserialize, curveProp.propertyType);
                    if (serializeObj != null)
                    {
                        this.ExSetValueField(curveProp.strPropertyName, serializeObj);
                    }
                }
            }
            else if (curveProp.loadType == LoadType.LT_Origin)
            {
                if (curveProp.propertyType == typeof(int))
                {
                    string strValue = configValue as string;
                    if (int.TryParse(strValue, out int iValue))
                    {
                        this.ExSetValueField(curveProp.strPropertyName, iValue);
                    }
                }
                else if (curveProp.propertyType == typeof(float))
                {
                    string strValue = configValue as string;
                    if (float.TryParse(strValue, out float fValue))
                    {
                        this.ExSetValueField(curveProp.strPropertyName, fValue);
                    }
                }
                else if (curveProp.propertyType == typeof(string))
                {
                    string strValue = configValue as string;
                    this.ExSetValueField(curveProp.strPropertyName, strValue);
                }
            }
        }
        return true;
    }

    private string GetExcelData(object[,] excelData, int i, int j)
    {
        object data = excelData[i, j];
        if (data == null)
        {
            return "";
        }

        return data.ToString();
    }
}