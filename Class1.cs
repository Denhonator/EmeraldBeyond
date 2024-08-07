﻿using EmeraldBeyond;
using Harmony;
using Il2Cpp;
using Il2CppBattle;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.Reflection;
using Il2CppSystem.Text;
using Il2CppUI.CutScene;
using Il2CppUI.Title;
using MelonLoader;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Il2Cpp.LaunchManager;
using static MelonLoader.MelonLogger;

//[HarmonyLib.HarmonyPatch(typeof(Il2CppMakimono.UITime), "GetDeltaTime_UI", new Type[] { typeof(Il2CppMakimono.AnimationDirector.ETimeScaleMode) })]
//class UITime
//{
//    private static void Postfix(ref float __result)
//    {
//        __result *= 10;
//    }
//}

[HarmonyLib.HarmonyPatch(typeof(Il2CppMakimono.AnimationDirector), "GetCurrentState", new Type[] { typeof(int) })]
class PlayTime
{
    static string lastresult = "";
    private static void Postfix(int layerindex, ref string __result, ref Il2CppMakimono.AnimationDirector __instance)
    {
        string layername = __instance.animationData.GetLayer(layerindex).name;
        double animTime = __instance.GetAnimTime(layerindex);
        double clipLen = __instance.GetClipLength(layername, __result);

        //__instance.SetTime(animTime);

        //MelonLogger.Msg($"state: {__result} animTime: {animTime} clipLen: {clipLen}");

        //if (lastresult == __result) return;

        //__instance.SetEndTime(layername, __result);

        //Il2CppSystem.Collections.Generic.List<AnimationClip> results= new Il2CppSystem.Collections.Generic.List<AnimationClip>();
        //__instance.GetAnimationClips(results);
        //foreach(AnimationClip ac in results)
        //{
        //    Il2CppReferenceArray<AnimationEvent> events = ac.events;
        //    for(int i = 0; i < events.Count; i++) 
        //    {
        //        MelonLogger.Msg($"eventtime: {events[i].time} clip: {ac.name} framerate: {ac.frameRate}");
        //    }
        //}

        //for(int i=0;i<__instance.layerstatuslist.Count;i++)
        //{
        //    Il2CppMakimono.AnimationDirector.LayerStatusInfo l = __instance.layerstatuslist[i];
        //    string callback = l.callback != null ? l.callback.method_info.Name : "";
        //    MelonLogger.Msg($"playstatus: {l.Status} nextstate: {l.nextstate} callback: {callback}");
        //}

        //foreach (Il2CppMakimono.UIAnimation.State s in __instance.animationData.GetStateList(layerindex))
        //{
        //    if (s.name == __result && __instance.animationData.GetNextState(s) != null)
        //    {
        //        //MelonLogger.Msg($"State: {s.name} NextState: {__instance.animationData.GetNextState(s).name} Animation: {__instance.animationData.name}");
        //        if (__result == lastresult)
        //        {
        //            __instance.SetState(layername, __instance.animationData.GetNextState(s).name);
        //            return;
        //        }
        //    }
        //}
        //lastresult = __result;
        __instance.UpdateSpecifiedAnim();
    }
}

//[HarmonyLib.HarmonyPatch(typeof(Il2Cpp.ScreenCutSpeechBubble), "OpenSpeechBubble", new Type[] { typeof(Il2CppUI.CutScene.SpeechBubbleCreateInfo), typeof(bool), typeof(Il2CppSystem.Action) })]
//class Bubble
//{
//    static void Postfix(Il2CppUI.CutScene.SpeechBubbleCreateInfo info, bool isFollowSelectBubble, Il2CppSystem.Action callback, ref Il2Cpp.ScreenCutSpeechBubble __instance)
//    {
//        MelonLogger.Msg(info.text);
//    }
//}

//[HarmonyLib.HarmonyPatch(typeof(Il2CppMakimono.Unit), "Transition", new Type[] { typeof(string), typeof(Il2CppMakimono.TransitionType), typeof(bool) })]
//class SetState
//{
//    static void Prefix(ref string unitname, ref Il2CppMakimono.TransitionType transition, ref bool playanimation, ref Il2CppMakimono.Unit __instance)
//    {
//        MelonLogger.Msg($"UnitTransition: {unitname} playanimation: {playanimation}");
//    }
//}
[HarmonyLib.HarmonyPatch(typeof(BattleInspiration), "GetArtsRate", new Type[] { typeof(BtArtsDataTableLabel) })]
class Inspiration
{
    static void Postfix(BtArtsDataTableLabel ArtsID, float __result)
    {
        Msg($"{ArtsID}: {__result}");
    }
}

[HarmonyLib.HarmonyPatch(typeof(BattleInspiration), "GetInspirationProbA")]
class InspirationA
{
    static void Postfix(int Difficulty, float ArtsRate, int HistoryBonus, float __result)
    {
        Msg($"Diff: {Difficulty} Rate: {ArtsRate} History: {HistoryBonus} Prob: {__result}%");
    }
}

[HarmonyLib.HarmonyPatch(typeof(BattleInspiration), "GetInspirationProbB")]
class InspirationB
{
    static void Postfix(int InspirationPoint, float __result)
    {
        Msg($"InspPoint: {InspirationPoint} Prob: {__result}%");
    }
}

[HarmonyLib.HarmonyPatch(typeof(CatheScript), "AftefFunc", new Type[] { typeof(CatheScript.FuncData), typeof(bool), typeof(CatheScript.Val) })]
class EventScriptFunc
{
    static void Postfix(CatheScript.FuncData data, bool needRet, CatheScript.Val returnVal, CatheScript __instance)
    {
        Msg($"{data.funcName} {data.GetArgNumStr()} \nReturn: {returnVal.valInt} {returnVal.valString}");
        MyMod.cs = __instance;
    }
}

[HarmonyLib.HarmonyPatch(typeof(CatheScript), "ReverseDictAdd")]
class EventScriptFunc2
{
    static void Postfix(Dictionary<string, CatheScript.Val> dict, string name, CatheScript __instance)
    {
        Msg($"{name}");
    }
}

//[HarmonyLib.HarmonyPatch(typeof(CatheScript), nameof(CatheScript.EvaluteImmediate))]
//class EventScriptFunc2
//{
//    unsafe static void Prefix(CatheScript.ILType type, IntPtr datas)
//    {
//        int len = (datas + 0x18).ToInt32();
//        Msg(len);
//        IntPtr ip = datas + 0x38;
//        byte* bytes = (byte*)ip;
//        //for(int i=0; i<len; i++)
//        //{
//        //    string s = Encoding.Unicode.GetString(bytes, 10);
//        //    Msg(s);
//        //    bytes += 0x10;
//        //}


//        //Msg(Il2CppSystem.String.Join("",datas));
//    }
//}

//[HarmonyLib.HarmonyPatch(typeof(CatheScript), nameof(CatheScript.ILParse))]
//class EventScriptFunc3
//{
//    static void Postfix(string str, CatheScript.ILType __result, CatheScript __instance)
//    {
//        Msg($"ILParse: {__instance.line} = {str}");
//    }
//}

[HarmonyLib.HarmonyPatch(typeof(BattleDamage), "CulcEffectSuccessRate", new Type[] { typeof(Effect), typeof(int) })]
class EffectChance
{
    static void Postfix(Effect effectType, int OverrideSuccessRate, float __result)
    {
        Msg($"Effect: {effectType} Chance: {__result} Override: {OverrideSuccessRate}");
    }
}

[HarmonyLib.HarmonyPatch(typeof(BattleOverAttackInfo), "GetOverDriveProb")]
class OverdriveChance
{
    static void Postfix(int __result)
    {
        Msg($"OverdriveChance: {__result}");
    }
}

[HarmonyLib.HarmonyPatch(typeof(BattleRank), "UpdatePlayerRank", new Type[] { typeof(int) })]
class BattleRankMonitor
{
    static void Postfix(int DeadNum, BattleRank __instance)
    {
        Msg($"Dead: {DeadNum} \nBaseEnemy: {__instance.m_BaseEnemyRank} " +
            $"\nEnemy: {__instance.m_EnemyRank}" +
            $"\nPlayer: {__instance.m_PlayerRank}" +
            $"\nPotential: {__instance.m_PotentialBattleRank}");
    }
}

[HarmonyLib.HarmonyPatch(typeof(PartsSpeechBubble), "Update")]
class Cutscene
{
    static Dictionary<string, int> timers = new Dictionary<string, int>();
    static void Postfix(PartsSpeechBubble __instance)
    {
        if(Input.GetKeyDown(KeyCode.F6))
        {
            Msg($"Text: {__instance.m_text.text}\n" +
                $"ActiveEnabled: {__instance.isActiveAndEnabled}\n" +
                $"Open: {__instance.IsOpen}\n" +
                $"OpenAnim: {__instance.m_isOpenAnimPlay}");
        }
        bool heldDown = Il2CppMakimono.Input.GetButton(Il2CppMakimono.Input.InputCategory.UI, Il2CppMakimono.Input.Button.Cancel);
        if (Il2CppMakimono.Input.GetButtonUp(Il2CppMakimono.Input.InputCategory.UI, Il2CppMakimono.Input.Button.Cancel))
            timers.Clear();
        if (__instance.m_isOpenAnimPlay && heldDown)
        {
            if (!timers.ContainsKey(__instance.m_text.text))
            {
                foreach (string key in timers.Keys)
                    timers[key] -= 1;
                timers.Add(__instance.m_text.text, 2);
            }
            if (timers[__instance.m_text.text] <= 0)
            {
                timers.Remove(__instance.m_text.text);
                __instance.m_isOpenAnimPlay = false;
            }
            if (timers[__instance.m_text.text] <= 2)
            {
                __instance.SetActive(__instance.m_isOpenAnimPlay);
                __instance.m_onActionCallback.Invoke(__instance.m_position);
                if (!__instance.IsOpen)
                {
                    __instance.Close();
                }
            }
        }
    }
}

//[HarmonyLib.HarmonyPatch(typeof(PartsSpeechBubble), "Open", new Type[] { typeof(string), typeof(SpeechBubbleTailType), typeof(Action) })]
//class Cutscene
//{
//    static void Postfix(string text, SpeechBubbleTailType type, Action callback, PartsSpeechBubble __instance)
//    {
//        Msg(text);
//    }
//}

[HarmonyLib.HarmonyPatch(typeof(TitleController.LogoController.Each), "OnUpdate", new Type[] {  })]
class Logos
{
    static void Prefix(TitleController.LogoController.Each __instance)
    {
        MelonLogger.Msg($"OnUpdate {__instance.m_step} {__instance.m_nMode}");
        __instance.m_step = TitleController.LogoController.Each.EStep.AllFinished;
        
    }
}

[HarmonyLib.HarmonyPatch(typeof(GameManager), "OnApplicationFocus", new Type[] { typeof(bool) })]
class Focus
{
    static bool Prefix(ref bool focusStatus, GameManager __instance)
    {
        focusStatus = true;
        return false;
    }
}

[HarmonyLib.HarmonyPatch(typeof(GameManager), "OnApplicationPause", new Type[] { typeof(bool) })]
class Pause
{
    static bool Prefix(ref bool pauseStatus, GameManager __instance)
    {
        pauseStatus = true;
        return false;
    }
}

[HarmonyLib.HarmonyPatch(typeof(GameManager), "Update", new Type[] {})]
class background
{
    static void Prefix(GameManager __instance)
    {
        Application.runInBackground = true;
    }
    static void Postfix(GameManager __instance)
    {
        Application.runInBackground = true;
    }
}

namespace EmeraldBeyond
{
    public class MyMod : MelonMod
    {
        public static CatheScript cs;
        List<Component> comps = new List<Component>();
        public static UnitCutScene? cutscene;
        public override void OnUpdate()
        {
            //DOTween.timeScale = 10;
            //DOTween.defaultTimeScaleIndependent = false;
            //LoggerInstance.Msg(DOTween.timeScale.ToString());

            if(Input.GetKeyDown(KeyCode.F1))
            {
                SoundController.BGM.Suspend(SoundController.BGM.ESuspendRequired.Start, true);
            }
            else if(Input.GetKeyDown(KeyCode.F2))
            {
                SoundController.BGM.Resume(SoundController.BGM.ESuspendRequired.Start, true);
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                BattleRank br = Singlton<BattleRank>.Instance;
                Msg($"BattleRank: {br.GetBattleRank()}" +
                $"\nEnemy: {br.m_EnemyRank}" +
                $"\nPlayer: {br.m_PlayerRank}" +
                $"\nPotential: {br.m_PotentialBattleRank}");
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                TitleControllerArgs args = new TitleControllerArgs(false);
                //GameManager.GameModeArgsBase argsBase = new GameManager.GameModeArgsBase(GameManager.EGameMode.Title);
                GameManager.ChangeGameMode(args);
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                foreach (string key in cs.m_valGlobalDict.Keys)
                {
                    Msg($"{key} = {cs.m_valGlobalDict[key].valInt}{cs.m_valGlobalDict[key].valBool}{cs.m_valGlobalDict[key].valString}");
                }
                foreach (string key in cs.m_valLocalDict.Keys)
                {
                    Msg($"{key} = {cs.m_valLocalDict[key].valInt}{cs.m_valLocalDict[key].valBool}{cs.m_valLocalDict[key].valString}");
                }
            }
            //if (Input.GetKeyDown(KeyCode.P))
            //{
            //    comps.Clear();
            //    foreach (GameObject g in SceneManager.GetActiveScene().GetRootGameObjects())
            //    {
            //        if (g.transform && g.transform.childCount > 0 && g.name.Contains("UI"))
            //        {
            //            RecursivePrint(g.transform, "-");
            //        }
            //    }
            //}
            //if(Input.GetKey(KeyCode.O))
            //{
            //    foreach(Component a in comps)
            //    {
            //        Il2CppReferenceArray<FieldInfo> finfos = a.GetIl2CppType().GetFields(BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public);
            //        Il2CppReferenceArray<MethodInfo> minfos = a.GetIl2CppType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            //        //FieldInfo finfo = a.GetIl2CppType().GetField("PlayBackTime");

            //        LoggerInstance.Msg(a.name);
            //        foreach (FieldInfo finfo in finfos)
            //        {
            //            if (finfo.GetValue(a) != null && finfo.Name == "playbackTime")
            //            {
            //                LoggerInstance.Msg($"{finfo.Name}, {finfo.FieldType.Name}: {finfo.GetValue(a).Unbox<float>()}");
            //            }
            //        }
            //        //foreach (MethodInfo minfo in minfos)
            //        //{
            //        //    string par = "";
            //        //    foreach(Il2CppSystem.Type t in minfo.GetParameterTypes())
            //        //    {
            //        //        par += t.Name;
            //        //    }
            //        //    LoggerInstance.Msg($"{minfo.Name}, {minfo.ReturnType.GetIl2CppType()}: {par}");
            //        //}
            //    }
            //}
            //if(Input.GetKey(KeyCode.T))
            //{
            //    Il2CppMakimono.UIManager.deltatime = Il2CppMakimono.UIManager.deltatime *= 5;
            //    Il2CppMakimono.UIManager.elapsedtime += Il2CppMakimono.UIManager.deltatime;
            //    LoggerInstance.Msg(Il2CppMakimono.UITime.GetDeltaTime_UI(Il2CppMakimono.AnimationDirector.ETimeScaleMode.Normal));
            //}
            //if(Input.GetKeyDown(KeyCode.O))
            //{
            //    foreach (GameObject g in SceneManager.GetActiveScene().GetRootGameObjects())
            //    {
            //        foreach(Makimono.AnimationDirector ad in g.GetComponentsInChildren<Makimono.AnimationDirector>())
            //        {
            //            ad.Stop();
            //            ad.StopAllCoroutines();
            //        }
            //    }
            //}
        }

        void RecursivePrint(Transform t, string pre)
        {
            Component[] components = t.GetComponents<Component>();
            string compnames = "";
            foreach (Component comp in components)
            {
                compnames += comp.GetIl2CppType().FullName+", ";
                if(comp.GetIl2CppType().FullName == "Makimono.AnimationDirector")
                {
                    comps.Add(comp);
                }
            }

            //LoggerInstance.Msg(pre + t.name +"\n"+comps);
            for(int i=0; i<t.childCount;i++)
            {
                if (pre.Length < 12)
                {
                    if(t.GetChild(i).gameObject.activeSelf)
                        RecursivePrint(t.GetChild(i), pre + "-");
                }
            }
        }

        //public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        //{
        //    LoggerInstance.Msg($"Scene {sceneName} with build index {buildIndex} has been loaded!");
        //    foreach(GameObject g in SceneManager.GetActiveScene().GetRootGameObjects())
        //    {
        //        if (g.transform && g.transform.childCount>0 && g.name.Contains("UI"))
        //        {
        //            RecursivePrint(g.transform, "-");
        //        }
        //    }
        //}
    }
}