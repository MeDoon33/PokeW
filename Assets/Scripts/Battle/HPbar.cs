using UnityEngine;
using System.Collections;

public class HPbar : MonoBehaviour
{
    [SerializeField] GameObject heatlh;


    public void SetHP(float hpNomalized)
    {

        heatlh.transform.localScale = new Vector3(hpNomalized, 1f);
    }
    public IEnumerator SetHPSmooth(float newHp)
    { 
     float curHp = heatlh.transform.localScale.x;
        float changeAmt = curHp - newHp;
        while (curHp - newHp > Mathf.Epsilon)
        { 
            curHp -= changeAmt * Time.deltaTime;
            heatlh.transform.localScale = new Vector3(curHp, 1f);
            yield return null;
        }
            heatlh.transform.localScale = new Vector3(newHp, 1f);
    }
}
