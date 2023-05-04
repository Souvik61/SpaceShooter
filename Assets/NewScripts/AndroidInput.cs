using UnityEngine;
using Unity.Netcode;

public class AndroidInput : MonoBehaviour, IInputInterface
{
    public MyButton upBtn;
    public MyButton leftBtn;
    public MyButton rightBtn;
    public MyButton fireBtn;

    public bool GetKey(string s)
    {
        bool ret = false;

        switch (s)
        {
            case "Up":
                ret = upBtn.buttonPressed;
                break;
            case "Left":
                ret = leftBtn.buttonPressed;
                break;
            case "Right":
                ret = rightBtn.buttonPressed;
                break;
            case "Fire":
                ret = fireBtn.buttonPressed;
                break;
            default:
                break;
        }

        return ret;
    }

    public bool GetKeyDown(string s)
    {
        bool ret = false;
        switch (s)
        {
            case "Fire":
                ret = fireBtn.buttonJustPressed;
                break;
            default:
                break;
        }
        return ret;
    }
}
