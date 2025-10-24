using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public int maxAmmo;
    public int curAmmo;


    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing"); // ���� �����ϴ� �ڷ�ƾ ���� ����
            StartCoroutine("Swing");
        }
        else if (type == Type.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing() // IEnumerator ������ �Լ� Ŭ����
    {
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.4f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        // �Ѿ� �߻�
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;
        yield return null; // 1������ ���
        // ź�� ����
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletPos.forward * Random.Range(-4, -3) + Vector3.up *Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.forward * 10, ForceMode.Impulse); // ȸ���� �߰�
    }

    // �Ϲ� �Լ� : ���� ����
    // Use() ���η�ƾ -> Swing() �����ƾ -> Use() ���η�ƾ

    // �ڷ�ƾ : ���� ����
    // Use() ���η�ƾ + Swing() �ڷ�ƾ
}
