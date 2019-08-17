using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Turret))]
public class TurretEditor : Editor
{
  /*  public override void OnInspectorGUI()
    {
        Turret turret = target as Turret;

        turret.needAwakening = EditorGUILayout.Toggle("Need Awakening", turret.needAwakening);

        using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(turret.needAwakening)))
        {
            if (group.visible == true)
            {
                EditorGUI.indentLevel++;
                turret.awakeRange = EditorGUILayout.Slider("Awakening Range", turret.awakeRange, 0,20f); //add turretpart and materials
                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.Space();
        turret.shootRange = EditorGUILayout.Slider("Shoot Range", turret.shootRange, 0, 15f);
        turret.bulletSpeed = EditorGUILayout.FloatField("Bullet Speed", turret.bulletSpeed);
        turret.bulletLifeTime = EditorGUILayout.FloatField("Bullet Life Time", turret.bulletLifeTime);
        turret.bulletDamage = EditorGUILayout.IntField("Bullet Damage", turret.bulletDamage);
        turret.shootCooldown = EditorGUILayout.Slider("Shoot Cooldown", turret.shootCooldown, 0, 2f);
        turret.bulletPrefab = (GameObject)EditorGUILayout.ObjectField("Bullet Prefab", turret.bulletPrefab, typeof(GameObject), false);
        EditorGUILayout.Space();
        turret.shootPoint = (GameObject)EditorGUILayout.ObjectField("Shoot Point", turret.shootPoint, typeof(GameObject), true);
        
    }*/
}
