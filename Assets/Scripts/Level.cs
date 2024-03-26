using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// the data container for levels
// used to read the level json files
public class Level
{
    public int level_number;
    public int grid_width;
    public int grid_height;
    public int move_count;
    public string[] grid;
}
