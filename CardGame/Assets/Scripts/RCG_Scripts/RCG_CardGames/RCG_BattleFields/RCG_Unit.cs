using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UCL.Core.UI;

namespace RCG {
    /// <summary>
    /// 角色技能組 決定能使用的卡牌
    /// </summary>
    public enum UnitSkill
    {
        /// <summary>
        /// 基礎魔法
        /// </summary>
        Magic = 0,

        /// <summary>
        /// 基礎弓箭技能
        /// </summary>
        Archery,

        /// <summary>
        /// 基礎近戰
        /// </summary>
        Melee,
    }
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_Unit : MonoBehaviour {
        public int Hp {
            get{return m_Hp;}
            set{
                m_Hp = value;
                m_unit_HUD.UpdateHUD();
            }
        }
        public int MaxHp {
            get{return m_MaxHp;}
            set{
                m_MaxHp = value;
                m_unit_HUD.UpdateHUD();
            }
        }
        public bool IsEnemy
        {
            get; protected set;
        }
        public bool m_is_dead = false;
        public HashSet<UnitSkill> m_SkillSets = new HashSet<UnitSkill>();
        public List<UnitSkill> m_Skills = new List<UnitSkill>();
        public List<RCG_Status> m_status_list;
        public Transform m_UnitDisplay = null;

        [SerializeField] protected int m_MaxHp = 0;
        [SerializeField] protected RCG_UnitUI m_UnitUI = null;
        protected int m_Hp = 0;

        private Queue<RCG_UnitAction> m_action_queue;
        private RCG_UnitAction m_action = null;
        private RCG_UnitHUD m_unit_HUD;

        virtual public void Init(bool _IsEnemy)
        {
            Hp = MaxHp;
            IsEnemy = _IsEnemy;
            if (IsEnemy)
            {
                m_UnitDisplay.rotation = Quaternion.Euler(0, 180, 0);
            }
            m_UnitUI.Init();
            foreach(var aSkill in m_Skills)
            {
                m_SkillSets.Add(aSkill);
            }
        }
        private void Awake() {
            m_action = null;
            if(!m_unit_HUD){
                m_unit_HUD = gameObject.GetComponent<RCG_UnitHUD>();
                //QWQ
            }
        }
        virtual public void SelectUnit()
        {
            m_UnitUI.m_SelectedItem.SetActive(true);
        }
        virtual public void DeselectUnit()
        {
            m_UnitUI.m_SelectedItem.SetActive(false);
        }
        public int RestoreHP(int amount){
            Hp += amount;
            return 0;
        }

        public int DamageHP(int amount){
            Hp -= amount;
            if(Hp <= 0){
                Die();
            }
            return 0;
        }

        public void Die(){
            m_is_dead = true;
            m_action_queue.Enqueue(new RCG_UnitAction(UnitActionType.Die, 0, null, 1.1));
        }

        public void EndTurn(){
            for(int i = 0; i < m_status_list.Count; i++) {
                if(m_status_list[i] != null){
                    m_status_list[i].StatusTurnEnd();
                }
                else{
                    m_status_list.RemoveAt(i);
                }
            }
        }

        [UCL.Core.ATTR.UCL_FunctionButton]
        public void Test(){
            DamageHP(1);
        }

        public void AddAction(RCG_UnitAction action){
            m_action_queue.Enqueue(action);
        }

        public void TakeAction(){
            m_action.TakeAction(this);
            if(m_action.m_type == UnitActionType.Die){
                gameObject.GetComponent<UCL_RectTransformCollider>().enabled = false;
                gameObject.GetComponent<Outline>().enabled = false;
            }
        }

        public void Start()
        {
            m_action_queue = new Queue<RCG_UnitAction>();
        }

        public void Update(){
            if(m_action_queue.Count <= 0 && m_action == null){
                return;
            }
            else if(m_action == null){
                m_action = m_action_queue.Dequeue();
                Debug.Log(m_action.m_type);
                TakeAction();
            }
            else{
                m_action.m_duration -= Time.deltaTime;
                if(m_action.m_type == UnitActionType.Die){
                    gameObject.GetComponent<UCL_Image>().color = new Color(1, 1, 1, (float)m_action.m_duration/1.1F);
                    foreach (Image img in gameObject.GetComponentsInChildren<Image>())
                    {
                        img.color = new Color(1, 1, 1, (float)m_action.m_duration/1.1F);
                    }
                    // gameObject.GetComponent<UCL_Image>().color = new Color(1, 1, 1, 1);
                }
                if(m_action.m_duration < 0){
                    if(m_action.m_type == UnitActionType.Die){
                        Destroy(gameObject);
                        Debug.Log("Destroy");
                    }
                    m_action = null;
                }
            }
        }

    }
}