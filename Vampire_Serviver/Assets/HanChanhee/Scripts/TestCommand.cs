using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCommand : MonoBehaviour
{
    public static TestCommand Instance { get; private set; }    
    
    public Dictionary<string, UpgradeBase> Upgrades = new Dictionary<string, UpgradeBase>();

    public List<UpgradeBase> UpgradesList = new List<UpgradeBase>();
    
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        RegisterCommand();
    }

    void RegisterCommand()
    {
        Upgrades.Add("ArcherNode1", UpgradesList[0]);
        Upgrades.Add("ArcherNode2", UpgradesList[0]);
        Upgrades.Add("ArcherNode3", UpgradesList[0]);
        Upgrades.Add("ArcherNode4", UpgradesList[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Command(string cmd)
    {
        string[] args = cmd.Split('/');
        string log = "Command:";
        UpgradeBase upgrade = (args[0] == "Skill" && args[1] == "Upgrade") ? Upgrades[args[2] + args[3]] : null;
        if (upgrade == null)
        {
            Debug.LogError("Error: Not Found Command");
            return;
        }
        upgrade.Upgrade();
        
        //switch(args[0])
        //{
        //    case "Skill":
        //        log += "Skill ";
        //        switch(args[1])
        //        {
        //            case "Upgrade":
        //                log += "Upgrade ";
        //                switch(args[2])
        //                {
        //                    case "Archer":
        //                        log += "Archer ";
                                
        //                        UpgradeBase upgrade = Upgrades[args[2] + args[3]];
                                
        //                        break;
        //                    case "Magic":
        //                        log += "Magic ";
        //                        break;
        //                    default:
        //                        log = "Error: Not Found Command in arg-2";
        //                        break;
        //                }
        //                break;
        //            default:
        //                log = "Error: Not Found Command in arg-1";
        //                break;
        //        }
        //        break;
        //    default:
        //        log = "Error: Not Found Command";
        //        break;
        //}
        //if(log.Contains("Error"))
        //{
        //    Debug.LogError(log);
        //} else
        //{
        //     Debug.Log(log);

        //}

    }
}
