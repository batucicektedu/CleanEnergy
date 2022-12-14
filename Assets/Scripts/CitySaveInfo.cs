using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CitySaveInfo
{
    public List<int> stateLitUpObjectCounts;

    public CitySaveInfo(int def)
    {
        stateLitUpObjectCounts = new List<int>
        {
            0,
            0,
            0,
            0,
            0,
            0,
        };

        // stateLitUpObjectCounts= new List<int>
        // {
        //     0,0,0,0,0,0
        // };
    }

    public CitySaveInfo()
    {
        
    }
}
