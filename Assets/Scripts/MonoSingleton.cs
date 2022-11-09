using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton <T> : MonoBehaviour where T : Object
{
    private static object m_lock = new object();
    private static T m_instance;
    public static T Instance
    {
        get
        {
            lock (m_lock)
            {
                if (m_instance == null)
                    m_instance = (T)FindObjectOfType(typeof(T));
                
                if(m_instance == null)
                    Debug.LogWarning("No singleton instance of " + typeof(T).ToString());
            }

            return m_instance;
        }
    }
}
