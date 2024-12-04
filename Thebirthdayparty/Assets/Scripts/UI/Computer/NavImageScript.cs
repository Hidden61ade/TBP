using UnityEngine;
using UnityEngine.UI;

public class NavImageScript : MonoBehaviour
{
    public Sprite Morning;
    public Sprite Noon;
    public Sprite Eve;
    private void OnEnable()
    {
        var img = GetComponent<Image>();
        try
        {
            string temp = GameSaveManager.Instance.currentSave.currentPeriod;
            var a = GameManager.TimeFromString(temp);
            Debug.Log(a);
            img.sprite = temp switch
            {
                "morning" => Morning,
                "afternoon" => Noon,
                "evening" => Eve,
                _ => Morning,
            };
        }
        catch
        {
            //Ignored.   
        }
    }
}
