using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    
    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing"); // ���� �����ϴ� �ڷ�ƾ ���� ����
            StartCoroutine("Swing");
        }
    }

    IEnumerator Swing() // IEnumerator ������ �Լ� Ŭ����
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    // �Ϲ� �Լ� : ���� ����
    // Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ

    // �ڷ�ƾ : ���� ����
    // Use() ���η�ƾ + Swing() �ڷ�ƾ
}
