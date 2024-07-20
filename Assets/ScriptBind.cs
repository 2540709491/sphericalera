using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScriptBind : MonoBehaviour
{
    public GameObject scrView;
    public TMP_Dropdown dropdown;

    public List<CMDPanel> cmds;
    // Start is called before the first frame update



    public void addOption(Recorder.RecordInfo recordInfo,CMDPanel cmdPanel)
    {
        TMP_Dropdown.OptionData optition = new TMP_Dropdown.OptionData();
        optition.text = recordInfo.RecordName;
            
        dropdown.options.Add(optition);
        this.cmds.Add(cmdPanel);
    }

    public void removeOption(int index)
    {
        dropdown.options.RemoveAt(index);
    }

    public void rename(int index, string name)
    {
        dropdown.options[index].text = name;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
