using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static string FormatCurrency(int amount)
    {
        if (amount >= 1000)
        {
            // Chuy?n ??i thành giá tr? có 'k' và ??nh d?ng v?i 2 ch? s? th?p phân
            float valueInK = amount / 1000f;
            return string.Format("{0:0.##}k", valueInK);
        }
        else
        {
            // Tr? v? giá tr? g?c d??i d?ng chu?i
            return amount.ToString();
        }
    }

    public static void RotateAround()
    {
         //lastMousePosition = input.mousepostition;

/*            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - lastMousePosition ;
            mouseDelta.z = 0f;

            float rotationX = -mouseDelta.y * 0.05f;
            float rotationY = mouseDelta.x * 0.05f;

            // Get the current rotation angles
            Vector3 currentRotation = transform.eulerAngles;

            // Clamp the X rotation
            currentRotation.x = Mathf.Clamp(NormalizeAngle(currentRotation.x + rotationX), -32.5f, 0);

            // Clamp the Y rotation
            currentRotation.y = Mathf.Clamp(NormalizeAngle(currentRotation.y + rotationY), -14.5f, 12f);

            // Apply the clamped rotation
            transform.eulerAngles = currentRotation;

            lastMousePosition = currentMousePosition;*/

        // maxY = 11 minY -13
    }

    public static void CountTime()
    {
/*        if (MaxTime < 0) return;

        TimeCountDown += Time.deltaTime;

        if (TimeCountDown > 1)
        {
            TimeCountDown -= 1;
            MaxTime -= 1;

            Second = MaxTime;

            if (MaxTime <= 0)
            {
                _isCanShowAdsInShop = true;
            }
        }*/
    }

    public static Vector3 GetMousePos3D()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 EndPos = ray.origin + ray.direction * 36f; // thay doi gia tri Z;

        return EndPos;
    }


}
