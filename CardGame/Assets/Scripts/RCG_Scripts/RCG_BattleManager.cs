using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UCL.Core.JsonLib;
using System.IO;

namespace RCG {
    public class RCG_BattleManager : MonoBehaviour {
        /// <summary>
        /// 是否在戰鬥中
        /// </summary>
        public bool IsInBattle
        {
            get { return m_Entered; }
        }
        public enum BattleState
        {
            None = 0,
            TrunInit,//回合開始 初始化階段
            PlayerAction,//玩家行動
            EnemyAction,//敵方行動
            TurnEnd,//回合結束
        }
        static public RCG_BattleManager Ins = null;
        public RCG_BattleField m_BattleField = null;
        public RCG_CardGame m_CardGame = null;
        public RCG_VFXManager m_VFXManager = null;
        public RCG_StatusManager m_StatusManager = null;
        public Button m_BackgroundButton;
        [SerializeField] GameObject m_ActionBlocker = null;
        bool m_Entered = false;
        BattleState m_State = BattleState.None;
        virtual public void Init() {
            Ins = this;
            m_BattleField.Init();
            m_CardGame.Init();
            m_VFXManager.Init();
            m_StatusManager.Init();
            m_BackgroundButton.onClick.AddListener(BackgroundClick);
            gameObject.SetActive(false);
            UnBlockAction();
        }
        /// <summary>
        /// 背景按鈕被按下
        /// </summary>
        virtual public void BackgroundClick()
        {
            m_CardGame.BackgroundClick();
        }
        /// <summary>
        /// 封鎖所有操作
        /// </summary>
        public void BlockAction()
        {
            m_ActionBlocker.SetActive(true);
        }
        /// <summary>
        /// 解鎖操作
        /// </summary>
        public void UnBlockAction()
        {
            m_ActionBlocker.SetActive(false);
        }
        public void SetState(BattleState iState)
        {
            m_State = iState;
        }
        /// <summary>
        /// 存檔
        /// </summary>
        /// <param name="iPath"></param>
        public void SaveData(string iPath)
        {
            if (!m_Entered || m_State != BattleState.PlayerAction)
            {
                return;
            }
            var aJson = SaveBattle();
            if(aJson == null)
            {
                Debug.LogError("SaveData aJson == null!!");
                return;
            }
            File.WriteAllText(iPath, aJson.ToJsonBeautify());
        }
        /// <summary>
        /// 讀取戰鬥存檔
        /// </summary>
        /// <param name="iPath"></param>
        public void LoadData(string iPath)
        {
            if (!File.Exists(iPath))
            {
                Debug.LogWarning("LoadData fail iPath:" + iPath + " ,Not Exist!!");
                return;
            }
            string aJsonStr = File.ReadAllText(iPath);
            JsonData aData = JsonData.ParseJson(aJsonStr);
            LoadBattle(aData);
        }
        /// <summary>
        /// 戰鬥存檔(Json)
        /// </summary>
        /// <returns></returns>
        public JsonData SaveBattle()
        {
            if (!m_Entered)
            {
                Debug.LogError("SaveBattle() Fail !m_Entered");
                return null;
            }
            if(m_State != BattleState.PlayerAction)
            {
                Debug.LogError("SaveBattle() Fail " + m_State.ToString() + " != BattleState.PlayerAction");
                return null;
            }
            JsonData aJson = new JsonData();
            aJson["Test"] = "BattleSave";
            return aJson;
        }
        /// <summary>
        /// 讀取戰鬥存檔(Json)
        /// </summary>
        /// <param name="iJson"></param>
        public void LoadBattle(JsonData iJson)
        {

        }
        /// <summary>
        /// 進入戰鬥
        /// </summary>
        /// <param name="iMonsterSet"></param>
        public void EnterBattle(RCG_MonsterSet iMonsterSet) {
            if(m_Entered) {
                Debug.LogError("EnterBattle() Fail!! Already Entered!!");
                return;
            }
            m_Entered = true;
            gameObject.SetActive(true);
            m_BattleField.EnterBattle();
            m_BattleField.SetMonsters(iMonsterSet);
            m_CardGame.EnterBattle();
            TurnInit();
        }
        /// <summary>
        /// 脫離戰鬥
        /// </summary>
        public void ExitBattle()
        {
            if (!m_Entered)
            {
                Debug.LogError("ExitBattle() Fail!! Not Entered!!");
                return;
            }
            m_Entered = false;
            m_BattleField.ExitBattle();
            m_CardGame.ExitBattle();

            gameObject.SetActive(false);
        }
        public void TurnInit()
        {
            SetState(BattleState.TrunInit);
            Debug.LogWarning("RCG_BattleManager TurnInit()");
            m_BattleField.TurnInit();
            m_CardGame.TurnInit();
            UnBlockAction();
            //進入玩家行動階段
            SetState(BattleState.PlayerAction);
        }
        /// <summary>
        /// 玩家行動結束
        /// </summary>
        public void PlayerTurnEnd()
        {
            BlockAction();
            SetState(BattleState.EnemyAction);
            m_BattleField.TurnStart();
        }
        /// <summary>
        /// 敵人行動結束
        /// </summary>
        public void EnemyTurnEnd()
        {
            TurnEnd();
        }
        public void TurnEnd()
        {
            SetState(BattleState.TurnEnd);
            RCG_Player.Ins.TurnEndPlayerAction(TurnInit);
            //TurnInit();
        }
        private void OnGUI() {
            if(m_Entered) {
                if(GUILayout.Button("ExitBattle()")) {
                    ExitBattle();
                }
            }

        }

    }
}

