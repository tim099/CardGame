using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UCL.TweenLib;
namespace RCG
{
    public class RCG_VFX_HP : RCG_VFX
    {
        [SerializeField] protected Text m_AlterHPText = null;
        [SerializeField] protected Transform m_MoveOffset = null;
        protected UCL_Tweener m_Tweener = null;
        protected Color m_Col;
        int m_AlterHP = 0;
        public override void Init()
        {
            base.Init();
            m_Col = m_AlterHPText.color;
        }
        public void SetAlterHP(int iAlterHP, Vector3 iPos, bool IsEnemy)
        {
            if (m_Tweener != null)
            {
                m_Tweener.Kill(true);
                m_Tweener = null;
            }
            m_AlterHPText.color = m_Col;
            transform.position = iPos;
            transform.localScale = Vector3.one;
            m_AlterHP = iAlterHP;
            m_AlterHPText.text = m_AlterHP.ToString();
            m_Tweener = LibTween.Tweener(0.8f);
            m_Tweener.SetEase(EaseType.OutSin);
            float aOffset = m_MoveOffset.position.x - transform.position.x;
            if (m_AlterHP < 0)
            {
                m_Tweener.AddComponent(transform.TC_Jump(iPos + new Vector3(IsEnemy ? aOffset : -aOffset, 0, 0), 1, Vector3.up, 100, 1.5f));
            }
            else//Heal
            {
                m_Tweener.AddComponent(transform.TC_Move(iPos + new Vector3(0, 40, 0)));
            }
            
            m_Tweener.AddComponent(transform.TC_Scale(1.5f,1.5f,1f));
            m_Tweener.OnUpdate((y) =>
            {
                if (y > 0.75f)
                {
                    m_AlterHPText.color = new Color(m_Col.r, m_Col.g, m_Col.b, 4f - 4f * y);
                }
            });
            m_Tweener.OnComplete(DeleteVFX);
            m_Tweener.Start();
        }

    }
}