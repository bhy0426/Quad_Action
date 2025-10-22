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
            StopCoroutine("Swing"); // 지금 동작하는 코루틴 동작 중지
            StartCoroutine("Swing");
        }
    }

    IEnumerator Swing() // IEnumerator 열거형 함수 클래스
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    // 일반 함수 : 교차 실행
    // Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴

    // 코루틴 : 동시 실행
    // Use() 메인루틴 + Swing() 코루틴
}
