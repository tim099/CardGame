using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCL.TweenLib;
using System.IO;

namespace RCG {
    public class RCG_Map : MonoBehaviour {
        public Transform m_NodesRoot = null;
        public List<RCG_MapNode> m_MapNodes = new List<RCG_MapNode>();
        public RCG_MapNodeContent m_MapNodeContentTmp = null;
        public RCG_MapNode m_StartCity = null;
        public RCG_MapItems m_MapItems = null;

        public RCG_MapNode m_CurCity = null;
        public List<RCG_MonsterSet> m_MonsterSets = new List<RCG_MonsterSet>();
        UCL_Tweener m_Tweener = null;
        virtual public void Init(RCG_MapItems _MapItems) {
            m_MapItems = _MapItems;
            m_MapItems.transform.SetParent(transform);
            m_MapItems.transform.position = transform.position;
            
            m_MapNodeContentTmp.gameObject.SetActive(false);
            m_MapNodes.Clear();
            UCL.Core.GameObjectLib.SearchChild(m_NodesRoot, m_MapNodes);
            foreach(var node in m_MapNodes) {
                node.InitMapNode(this, m_MapNodeContentTmp);
            }
            //m_MapItems.m_PlayerIcon.transform.position = m_StartCity.transform.position;
            PlayerMoveTo(m_StartCity, true);
            LoadMapData();
        }
        virtual public bool IsMoving() { return m_Tweener != null; }
        virtual public void PlayerMoveTo(RCG_MapNode _node, bool init_move = false) {
            m_CurCity = _node;


            if(init_move) {
                var parent_pos = transform.parent.position;
                transform.position += parent_pos - m_CurCity.transform.position;
                m_MapItems.m_PlayerIcon.transform.position = m_CurCity.transform.position;
                foreach(var node in m_MapNodes) {
                    node.UpdateSelectNode(m_CurCity);
                }
            } else {
                foreach(var node in m_MapNodes) {//disable select mode until move end!!
                    node.UpdateSelectNode(null);
                }
                //var pos_o = m_MapItems.m_PlayerIcon.transform.position;
                var pos_o = transform.position;
                var pos_e = transform.parent.position + pos_o - m_CurCity.transform.position;
                //Debug.LogWarning("Pos o:" + pos_o + ",pos_c:"+ pos_c + ",pos_e:" + pos_e);
                //UCL_TweenManager.Instance.KillAllOnTransform(m_MapItems.m_PlayerIcon.transform);
                if(m_Tweener != null) m_Tweener.Kill(true);
                m_Tweener = m_MapItems.m_PlayerIcon.transform.UCL_LocalMove(0.5f, m_CurCity.transform).SetEase(EaseType.InSin);
                m_Tweener.OnUpdate(delegate (float pos) {
                    //var del = m_MapItems.m_PlayerIcon.transform.position - pos_o;
                    transform.position = UCL.Core.MathLib.Lib.Lerp(pos_o, pos_e, pos);
                });
                m_Tweener.OnComplete(delegate () {
                    foreach(var node in m_MapNodes) {
                        node.UpdateSelectNode(m_CurCity);
                    }
                    m_CurCity.Selected();
                    m_Tweener = null;
                });
                m_Tweener.Start();
                //transform.position = pos_e;
            }

        }

        public static string MapDataPath
        {
            get
            {
                return Application.streamingAssetsPath + "/MapDatas";
            }
        }
        virtual public void LoadMapData()
        {
            var aFiles = UCL.Core.FileLib.Lib.GetFiles(Path.Combine(MapDataPath, gameObject.name));
            Debug.Log(Path.Combine(MapDataPath, gameObject.name));
            foreach (var aFile in aFiles)
            {
                Debug.Log(aFile);
                if (!aFile.Contains(".json")) continue;
                if (aFile.Contains(".meta")) continue; //ignore meta files

                var aData = UCL.Core.JsonLib.JsonData.ParseJson(System.IO.File.ReadAllText(aFile));
                for(int i=0; i < aData["MonsterSets"].Count; i++)
                {
                    RCG_MonsterSet iMonsterSet = new RCG_MonsterSet();
                    var monsterSetJson = aData["MonsterSets"][i];
                    for (int j = 0; j < monsterSetJson["Monsters"].Count; j++)
                    {
                        var monster = monsterSetJson["Monsters"][j];
                        iMonsterSet.m_Monsters.Add(new RCG_MonsterTuple(monster["Name"], monster["Pos"]));
                    }
                    m_MonsterSets.Add(iMonsterSet);
                }
            }
        }
    }
}