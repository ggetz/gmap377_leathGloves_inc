﻿using UnityEngine;
public class Powerup
{
    public bool IsActive;
    public float Timer, SetTime;
    public GameObject GuiBar;
	public GameObject guiIcon;
    
    public Powerup(float setTime, GameObject guiBar, GameObject guiIcon)
    {
        this.IsActive = false;
        this.Timer = 0f;
        this.SetTime = setTime;
        this.GuiBar = guiBar;
		this.guiIcon = guiIcon;
        
    }
	public Powerup(float setTime, GameObject guiBar)
	{
		this.IsActive = false;
		this.Timer = 0f;
		this.SetTime = setTime;
		this.GuiBar = guiBar;		
	}
}
