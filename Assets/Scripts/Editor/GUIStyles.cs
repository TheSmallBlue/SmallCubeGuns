using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GUIStyles
{
    public static GUIStyle Title => TitleBuilder();
    static GUIStyle TitleBuilder()
    {
        var gui = new GUIStyle();

        gui.normal.textColor = Color.white;
        gui.alignment = TextAnchor.MiddleCenter;
        gui.fontStyle = FontStyle.Bold;
        gui.fontSize = 16;

        return gui;
    }

    public static GUIStyle Centered => CenteredBuilder();
    static GUIStyle CenteredBuilder()
    {
        var gui = new GUIStyle();

        gui.normal.textColor = Color.white;
        gui.alignment = TextAnchor.MiddleCenter;

        return gui;
    }

    public static GUIStyle Right => RightBuilder();
    static GUIStyle RightBuilder()
    {
        var gui = new GUIStyle();

        gui.normal.textColor = Color.white;
        gui.alignment = TextAnchor.MiddleRight;

        return gui;
    }
}
