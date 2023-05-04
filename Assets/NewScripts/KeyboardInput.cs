using UnityEngine;
using Unity.Netcode;

public class KeyboardInput : MonoBehaviour, IInputInterface
{
    public bool GetKey(string s)
    {

        bool ret = false;

        switch (s)
        {
            case "Up":
                ret = Input.GetKey(KeyCode.UpArrow);
                break;
            case "Left":
                ret = Input.GetKey(KeyCode.LeftArrow);
                break;
            case "Right":
                ret = Input.GetKey(KeyCode.RightArrow);
                break;
            case "Fire":
                ret = Input.GetKey(KeyCode.Space);
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
                ret = Input.GetKeyDown(KeyCode.Space);
                break;
            default:
                break;
        }
        return ret;
    }
}
