using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

//사용자 설정 값의 저장 관리를 위한 클래스. 여기선 제일 쉬운 PlayerPrefs를 사용
public static class PreferenceData
{
    private static float mouseSensitivity;   //마우스 감도
    public static float MouseSensitivity
    {
        get { return mouseSensitivity; }
        set { PlayerPrefs.SetFloat(GetMemberName(() => mouseSensitivity), value); }
    }

    static PreferenceData()     //초기화. PlayerPrefs 내 값을 변수에 할당.
    {
        mouseSensitivity = PlayerPrefs.GetFloat(GetMemberName(() => mouseSensitivity), 2f);
    }

    private static string GetMemberName<T>(Expression<Func<T>> memberExpression)    //변수명을 string으로 리턴해주는 함수. 변수명을 그대로 key로 쓰기 위함. 
    {
        MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
        return expressionBody.Member.Name;
    }
}
