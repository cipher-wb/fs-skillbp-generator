#if UNITY_EDITOR
using Funny.Base;
using Funny.Base.Logs;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TableDR
{
    public partial class BattleCameraShakeConfig
    {
        [HideInInspector, JsonIgnore]
        public ShakeInfo[] cacheShakeInfos = new ShakeInfo[6];

        public class ShakeInfo
        {
            public double frequency;

            public double range;

            public int samplings;

            public int time;
        }

        protected void SetCacheShakeInfo(bool isEnable, int index)
        {
            if (isEnable)
            {
                if (cacheShakeInfos[index] == default)
                {
                    return;
                }

                switch (index)
                {
                    case 0:
                        Frequency_X = cacheShakeInfos[index].frequency;
                        Range_X = cacheShakeInfos[index].range;
                        Samplings_X = cacheShakeInfos[index].samplings;
                        Time_X = cacheShakeInfos[index].time;
                        break;
                    case 1:
                        Frequency_Y = cacheShakeInfos[index].frequency;
                        Range_Y = cacheShakeInfos[index].range;
                        Samplings_Y = cacheShakeInfos[index].samplings;
                        Time_Y = cacheShakeInfos[index].time;
                        break;
                    case 2:
                        Frequency_Z = cacheShakeInfos[index].frequency;
                        Range_Z = cacheShakeInfos[index].range;
                        Samplings_Z = cacheShakeInfos[index].samplings;
                        Time_Z = cacheShakeInfos[index].time;
                        break;
                    case 3:
                        Frequency_Yaw = cacheShakeInfos[index].frequency;
                        Range_Yaw = cacheShakeInfos[index].range;
                        Samplings_Yaw = cacheShakeInfos[index].samplings;
                        Time_Yaw = cacheShakeInfos[index].time;
                        break;
                    case 4:
                        Frequency_Pitch = cacheShakeInfos[index].frequency;
                        Range_Pitch = cacheShakeInfos[index].range;
                        Samplings_Pitch = cacheShakeInfos[index].samplings;
                        Time_Pitch = cacheShakeInfos[index].time;
                        break;
                    case 5:
                        Frequency_Roll = cacheShakeInfos[index].frequency;
                        Range_Roll = cacheShakeInfos[index].range;
                        Samplings_Roll = cacheShakeInfos[index].samplings;
                        Time_Roll = cacheShakeInfos[index].time;
                        break;
                }
            }
            else
            {
                switch (index)
                {
                    case 0:
                        cacheShakeInfos[index] = new ShakeInfo
                        {
                            frequency = Frequency_X,
                            range = Range_X,
                            samplings = Samplings_X,
                            time = Time_X,
                        };

                        Frequency_X = 0;
                        Range_X = 0;
                        Samplings_X = 10;
                        Time_X = 0;
                        break;
                    case 1:
                        cacheShakeInfos[index] = new ShakeInfo
                        {
                            frequency = Frequency_Y,
                            range = Range_Y,
                            samplings = Samplings_Y,
                            time = Time_Y,
                        };

                        Frequency_Y = 0;
                        Range_Y = 0;
                        Samplings_Y = 10;
                        Time_Y = 0;
                        break;
                    case 2:
                        cacheShakeInfos[index] = new ShakeInfo
                        {
                            frequency = Frequency_Z,
                            range = Range_Z,
                            samplings = Samplings_Z,
                            time = Time_Z,
                        };

                        Frequency_Z = 0;
                        Range_Z = 0;
                        Samplings_Z = 10;
                        Time_Z = 0;
                        break;
                    case 3:
                        cacheShakeInfos[index] = new ShakeInfo
                        {
                            frequency = Frequency_Yaw,
                            range = Range_Yaw,
                            samplings = Samplings_Yaw,
                            time = Time_Yaw,
                        };

                        Frequency_Yaw = 0;
                        Range_Yaw = 0;
                        Samplings_Yaw = 10;
                        Time_Yaw = 0;
                        break;
                    case 4:
                        cacheShakeInfos[index] = new ShakeInfo
                        {
                            frequency = Frequency_Pitch,
                            range = Range_Pitch,
                            samplings = Samplings_Pitch,
                            time = Time_Pitch,
                        };

                        Frequency_Pitch = 0;
                        Range_Pitch = 0;
                        Samplings_Pitch = 10;
                        Time_Pitch = 0;
                        break;
                    case 5:
                        cacheShakeInfos[index] = new ShakeInfo
                        {
                            frequency = Frequency_Roll,
                            range = Range_Roll,
                            samplings = Samplings_Roll,
                            time = Time_Roll,
                        };

                        Frequency_Roll = 0;
                        Range_Roll = 0;
                        Samplings_Roll = 10;
                        Time_Roll = 0;
                        break;
                }
            }

            SaveCurveVal();
        }

        private void SetCacheShakeInfoX()
        {
            SetCacheShakeInfo(EnableX, 0);
        }

        private void SetCacheShakeInfoY()
        {
            SetCacheShakeInfo(EnableY, 1);
        }

        private void SetCacheShakeInfoZ()
        {
            SetCacheShakeInfo(EnableZ, 2);
        }

        private void SetCacheShakeInfoYaw()
        {
            SetCacheShakeInfo(EnableYaw, 3);
        }

        private void SetCacheShakeInfoPitch()
        {
            SetCacheShakeInfo(EnablePitch, 4);
        }

        private void SetCacheShakeInfoRoll()
        {
            SetCacheShakeInfo(EnableRoll, 5);
        }

        [ToggleGroupAttribute("EnableX", 9, "X轴"), OnValueChanged("SetCacheShakeInfoX")]
        public bool EnableX = true;
        [ToggleGroupAttribute("EnableY", 10, "Y轴"), OnValueChanged("SetCacheShakeInfoY")]
        public bool EnableY = true;
        [ToggleGroupAttribute("EnableZ", 11, "Z轴"), OnValueChanged("SetCacheShakeInfoZ")]
        public bool EnableZ = true;
        [ToggleGroupAttribute("EnableYaw", 12, "Yaw"), OnValueChanged("SetCacheShakeInfoYaw")]
        public bool EnableYaw = true;
        [ToggleGroupAttribute("EnablePitch", 13, "Pitch"), OnValueChanged("SetCacheShakeInfoPitch")]
        public bool EnablePitch = true;
        [ToggleGroupAttribute("EnableRoll", 14, "Roll"), OnValueChanged("SetCacheShakeInfoRoll")]
        public bool EnableRoll = true;


        [Button("重新自动生成曲线_X轴", ButtonSizes.Medium), ShowIf("EnableX"), BoxGroupAttribute("X轴", false, order: 9)]
        [PropertyOrder(70)]
        public void OnClickReGenerate_X()
        {
            int iRandomValue = UnityEngine.Random.Range(0, 100000);
            curve_X = new AnimationCurve();
            float time = (float)Time_X / 1000;
            for (int i = 0; i < Samplings_X; ++i)
            {
                float noiseFrequency = (iRandomValue + i);
                float fValue = Mathf.PerlinNoise(noiseFrequency, 0.5f);
                fValue = Mathf.Clamp(fValue, 0.0f, 1.0f);
                fValue = (fValue - 0.5f) * (float)Range_X * 2;
                float fKey = 1.0f / Samplings_X * i * time;
                if (i == Samplings_X - 1)
                {
                    fKey = time;
                }
                curve_X.AddKey(fKey, fValue);
            }

        }

        [System.ComponentModel.Description("曲线内容_X")]
        [ShowInInspector, DelayedProperty, LabelText("曲线内容_X"), Newtonsoft.Json.JsonProperty]
        [HideReferenceObjectPicker, ShowIf("EnableX"), BoxGroupAttribute("X轴", false, order: 9), OnValueChanged("SaveCurveVal"), JsonIgnore]
        public AnimationCurve curve_X;

        [Button("重新自动生成曲线_Y轴", ButtonSizes.Medium), ShowIf("EnableY"), BoxGroupAttribute("Y轴", false, order: 10)]
        [PropertyOrder(120)]
        public void OnClickReGenerate_Y()
        {
            int iRandomValue = UnityEngine.Random.Range(0, 100000);
            curve_Y = new AnimationCurve();
            float time = (float)Time_Y / 1000;
            for (int i = 0; i < Samplings_Y; ++i)
            {
                float noiseFrequency = (iRandomValue + i);
                float fValue = Mathf.PerlinNoise(noiseFrequency, 0.5f);
                fValue = Mathf.Clamp(fValue, 0.0f, 1.0f);
                fValue = (fValue - 0.5f) * (float)Range_Y * 2;
                float fKey = 1.0f / Samplings_Y * i * time;
                if (i == Samplings_Y - 1)
                {
                    fKey = time;
                }
                curve_Y.AddKey(fKey, fValue);
            }
        }

        [System.ComponentModel.Description("曲线内容_Y")]
        [ShowInInspector, DelayedProperty, LabelText("曲线内容_Y"), Newtonsoft.Json.JsonProperty]
        [HideReferenceObjectPicker, ShowIf("EnableY"), BoxGroupAttribute("Y轴", false, order: 10), OnValueChanged("SaveCurveVal"), JsonIgnore]
        public AnimationCurve curve_Y;

        [Button("重新自动生成曲线_Z轴", ButtonSizes.Medium), ShowIf("EnableZ"), BoxGroupAttribute("Z轴", false, order: 11)]
        [PropertyOrder(120)]
        public void OnClickReGenerate_Z()
        {
            int iRandomValue = UnityEngine.Random.Range(0, 100000);
            curve_Z = new AnimationCurve();
            float time = (float)Time_Z / 1000;
            for (int i = 0; i < Samplings_Z; ++i)
            {
                float noiseFrequency = (iRandomValue + i);
                float fValue = Mathf.PerlinNoise(noiseFrequency, 0.5f);
                fValue = Mathf.Clamp(fValue, 0.0f, 1.0f);
                fValue = (fValue - 0.5f) * (float)Range_Z * 2;
                float fKey = 1.0f / Samplings_Z * i * time;
                if (i == Samplings_Z - 1)
                {
                    fKey = time;
                }
                curve_Z.AddKey(fKey, fValue);
            }

        }

        [System.ComponentModel.Description("曲线内容_Z")]
        [ShowInInspector, DelayedProperty, LabelText("曲线内容_Z"), Newtonsoft.Json.JsonProperty]
        [HideReferenceObjectPicker, ShowIf("EnableZ"), BoxGroupAttribute("Z轴", false, order: 11), OnValueChanged("SaveCurveVal"), JsonIgnore]
        public AnimationCurve curve_Z;

        [Button("重新自动生成曲线_Yaw轴", ButtonSizes.Medium), ShowIf("EnableYaw"), BoxGroupAttribute("Yaw", false, order: 12)]
        [PropertyOrder(120)]
        public void OnClickReGenerate_Yaw()
        {
            int iRandomValue = UnityEngine.Random.Range(0, 100000);
            curve_Yaw = new AnimationCurve();
            float time = (float)Time_Yaw / 1000;
            for (int i = 0; i < Samplings_Yaw; ++i)
            {
                float noiseFrequency = (iRandomValue + i);
                float fValue = Mathf.PerlinNoise(noiseFrequency, 0.5f);
                fValue = Mathf.Clamp(fValue, 0.0f, 1.0f);
                fValue = (fValue - 0.5f) * (float)Range_Yaw * 2;
                float fKey = 1.0f / Samplings_Yaw * i * time;
                if (i == Samplings_Yaw - 1)
                {
                    fKey = time;
                }
                curve_Yaw.AddKey(fKey, fValue);
            }

        }

        [System.ComponentModel.Description("曲线内容_Yaw")]
        [ShowInInspector, DelayedProperty, LabelText("曲线内容_Yaw"), Newtonsoft.Json.JsonProperty]
        [HideReferenceObjectPicker, ShowIf("EnableYaw"), BoxGroupAttribute("Yaw", false, order: 12), OnValueChanged("SaveCurveVal"), JsonIgnore]
        public AnimationCurve curve_Yaw;

        [Button("重新自动生成曲线_Pitch轴", ButtonSizes.Medium), ShowIf("EnablePitch"), BoxGroupAttribute("Pitch", false, order: 13)]
        [PropertyOrder(120)]
        public void OnClickReGenerate_Pitch()
        {
            int iRandomValue = UnityEngine.Random.Range(0, 100000);
            curve_Pitch = new AnimationCurve();
            float time = (float)Time_Pitch / 1000;
            for (int i = 0; i < Samplings_Pitch; ++i)
            {
                float noiseFrequency = (iRandomValue + i);
                float fValue = Mathf.PerlinNoise(noiseFrequency, 0.5f);
                fValue = Mathf.Clamp(fValue, 0.0f, 1.0f);
                fValue = (fValue - 0.5f) * (float)Range_Pitch * 2;
                float fKey = 1.0f / Samplings_Pitch * i * time;
                if (i == Samplings_Pitch - 1)
                {
                    fKey = time;
                }
                curve_Pitch.AddKey(fKey, fValue);
            }
        }

        [System.ComponentModel.Description("曲线内容_Pitch")]
        [ShowInInspector, DelayedProperty, LabelText("曲线内容_Pitch"), Newtonsoft.Json.JsonProperty]
        [HideReferenceObjectPicker, ShowIf("EnablePitch"), BoxGroupAttribute("Pitch", false, order: 13), OnValueChanged("SaveCurveVal"), JsonIgnore]
        public AnimationCurve curve_Pitch;

        [Button("重新自动生成曲线_Roll轴", ButtonSizes.Medium), ShowIf("EnableRoll"), BoxGroupAttribute("Roll", false, order: 14)]
        [PropertyOrder(120)]
        public void OnClickReGenerate_Roll()
        {
            int iRandomValue = UnityEngine.Random.Range(0, 100000);
            curve_Roll = new AnimationCurve();
            float time = (float)Time_Roll / 1000;
            for (int i = 0; i < Samplings_Roll; ++i)
            {
                float noiseFrequency = (iRandomValue + i);
                float fValue = Mathf.PerlinNoise(noiseFrequency, 0.5f);
                fValue = Mathf.Clamp(fValue, 0.0f, 1.0f);
                fValue = (fValue - 0.5f) * (float)Range_Roll * 2;
                float fKey = 1.0f / Samplings_Roll * i * time;
                if (i == Samplings_Roll - 1)
                {
                    fKey = time;
                }
                curve_Roll.AddKey(fKey, fValue);
            }
        }

        [System.ComponentModel.Description("曲线内容_Roll")]
        [ShowInInspector, DelayedProperty, LabelText("曲线内容_Roll"), Newtonsoft.Json.JsonProperty]
        [HideReferenceObjectPicker, ShowIf("EnableRoll"), BoxGroupAttribute("Roll", false, order: 14), OnValueChanged("SaveCurveVal"), JsonIgnore]
        public AnimationCurve curve_Roll;

        public void SaveCurveVal()
        {
            try
            {
                if (EnableX && curve_X != null)
                    Curve_X = JsonConvert.SerializeObject(curve_X);
                else
                    Curve_X = string.Empty;
                if (EnableY && curve_Y != null)
                    Curve_Y = JsonConvert.SerializeObject(curve_Y);
                else
                    Curve_Y = string.Empty;
                if (EnableZ && curve_Z != null)
                    Curve_Z = JsonConvert.SerializeObject(curve_Z);
                else
                    Curve_Z = string.Empty;
                if (EnableYaw && curve_Yaw != null)
                    Curve_Yaw = JsonConvert.SerializeObject(curve_Yaw);
                else
                    Curve_Yaw = string.Empty;
                if (EnablePitch && curve_Pitch != null)
                    Curve_Pitch = JsonConvert.SerializeObject(curve_Pitch);
                else
                    Curve_Pitch = string.Empty;
                if (EnableRoll && curve_Roll != null)
                    Curve_Roll = JsonConvert.SerializeObject(curve_Roll);
                else
                    Curve_Roll = string.Empty;

                ResetInit();
            }
            catch (Exception)
            {
                Log.Error("BattleCameraShakeConfig SaveCurve Fail, AnimationCurve Is Null");
            }
        }

        public bool PostLoadAniCure()
        {
            try
            {
                if (EnableX)
                    curve_X = JsonConvert.DeserializeObject<AnimationCurve>(Curve_X);
                if (EnableY)
                    curve_Y = JsonConvert.DeserializeObject<AnimationCurve>(Curve_Y);
                if (EnableZ)
                    curve_Z = JsonConvert.DeserializeObject<AnimationCurve>(Curve_Z);
                if (EnableYaw)
                    curve_Yaw = JsonConvert.DeserializeObject<AnimationCurve>(Curve_Yaw);
                if (EnablePitch)
                    curve_Pitch = JsonConvert.DeserializeObject<AnimationCurve>(Curve_Pitch);
                if (EnableRoll)
                    curve_Roll = JsonConvert.DeserializeObject<AnimationCurve>(Curve_Roll);
                return true;
            }
            catch
            {
                Log.Warning("BattleCameraShakeConfig LoadCurve Fail, Curve Is Null");
            }
            return true;
        }
    }
}
#endif
