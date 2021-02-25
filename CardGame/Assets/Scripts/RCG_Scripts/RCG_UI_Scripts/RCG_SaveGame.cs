using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RCG
{
    [UCL.Core.ATTR.EnableUCLEditor]
    public class RCG_SaveGame : MonoBehaviour
    {
        [UCL.Core.ATTR.UCL_RuntimeOnly]
        [UCL.Core.ATTR.UCL_FunctionButton]
        public void SaveGame()
        {
            RCG_DataService.ins.SaveGame();
        }
    }
}