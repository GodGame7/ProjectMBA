using UnityEngine;

public class Indicator : MonoBehaviour
{
    public LayerMask layerGround;
    public GameObject rangeCircle;
    public GameObject areaCircle;
    public GameObject line;
    public float range;
    public float area;

    private void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerGround))
        {
            if (rangeCircle.activeSelf)
            {

            }
            if (line.activeSelf)
            {
                Vector3 direction = hit.point - transform.position;
                float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                line.transform.rotation = Quaternion.Euler(90f, 0f, angle - 90f);
            }
            if (areaCircle.activeSelf)
            {
                float centerX = transform.position.x;
                float centerZ = transform.position.z;

                Vector3 hitPoint = hit.point;
                hitPoint.y = transform.position.y;

                Vector3 direction = hitPoint - transform.position;
                Vector3 clampedDirection = Vector3.ClampMagnitude(direction, range);

                Vector3 clampedPoint = transform.position + clampedDirection;

                areaCircle.transform.position = new Vector3(clampedPoint.x, 0.1f, clampedPoint.z);
            }
        }
    }
    public void OnIndicator(Unit unit)
    {
        OnRangeCircle(); range = unit.range;
    }
    public void OnIndicator(Skill skill)
    {
        if(skill.inputType == InputType.NonTarget)
        {
            if (skill.outputType == OutputType.AoE)
            {
                OnRangeCircle(); range = skill.range[skill.level]; rangeCircle.transform.localScale = new Vector2(range*2, range*2);
                OnAreaCircle(); area = skill.area[skill.level]; areaCircle.transform.localScale = new Vector2(area*2, area*2);
            }
            else
            {
                OnLine(); area = skill.area[skill.level];  range = skill.range[skill.level]; line.transform.localScale = new Vector2(area, range);
            }
        }       
        else if (skill.inputType == InputType.Target)
        {
            OnRangeCircle(); range = skill.range[skill.level];
        }
    }   
    public void OffIndicator()
    {
        rangeCircle.SetActive(false);
        areaCircle.SetActive(false);
        line.SetActive(false);
    }

    public void OnRangeCircle()
    {
        rangeCircle.SetActive(true);
    }
    public void OnAreaCircle()
    {
        areaCircle.SetActive(true);
    }
    public void OnLine()
    {
        line.SetActive(true);
    }
}
